using FluentAssertions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Services.OCR;

namespace OmniAppium.EngineUtilityServices.Tests.Services.OCR;

public sealed class AndroidScreenOcrServiceTests
{
    private readonly Mock<IScreenshotService> _screenshots =
        new(MockBehavior.Strict);

    private readonly Mock<IOCRUtilityService> _ocr =
        new(MockBehavior.Strict);

    private readonly Mock<IOcrDiagnosticImageWriter> _diagnosticImageWriter =
        new(MockBehavior.Strict);

    private readonly Mock<ILoggerFactoryBaseUtilityService>
        _loggerFactoryServiceMock;

    private readonly Mock<ILogger> _loggerMock;

    public AndroidScreenOcrServiceTests()
    {
        _loggerFactoryServiceMock =
            new Mock<ILoggerFactoryBaseUtilityService>(
                MockBehavior.Strict);

        _loggerMock =
            new Mock<ILogger>(
                MockBehavior.Loose);

        _loggerFactoryServiceMock
            .SetupGet(service => service.Logger)
            .Returns(_loggerMock.Object);
    }

    [Fact]
    public async Task RecognizeCurrentScreenAsync_CapturesFreshBytesAndForwardsLanguageAndCancellation()
    {
        byte[] bytes = [1, 2, 3];

        using var cancellation =
            new CancellationTokenSource();

        OcrResult expected =
            new(
                Text: "任務",
                Lines: Array.Empty<OcrTextLine>());

        _screenshots
            .Setup(service =>
                service.CaptureScreenshotBytes(null))
            .Returns(bytes);

        _diagnosticImageWriter
            .Setup(writer =>
                writer.WriteAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<string?>(),
                    cancellation.Token))
            .Returns(Task.CompletedTask);

        _ocr
            .Setup(service =>
                service.RecognizeAsync(
                    bytes,
                    "zh-TW",
                    cancellation.Token))
            .ReturnsAsync(expected);

        var sut =
            CreateSut();

        OcrResult actual =
            await sut.RecognizeCurrentScreenAsync(
                cancellation.Token);

        actual.Should()
            .BeSameAs(expected);

        _screenshots.Verify(
            service =>
                service.CaptureScreenshotBytes(null),
            Times.Once);

        _screenshots.Verify(
            service =>
                service.TakeScreenshot(),
            Times.Never);

        _screenshots.Verify(
            service =>
                service.GetBytesOfCachedScreenshotBytes(null),
            Times.Never);

        _diagnosticImageWriter.Verify(
            writer =>
                writer.WriteAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<string?>(),
                    cancellation.Token),
            Times.Once);

        _ocr.Verify(
            service =>
                service.RecognizeAsync(
                    bytes,
                    "zh-TW",
                    cancellation.Token),
            Times.Once);

        _screenshots.VerifyNoOtherCalls();
        _diagnosticImageWriter.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_RejectsEmptyScreenshotBeforeRecognition()
    {
        _screenshots
            .Setup(service =>
                service.CaptureScreenshotBytes(null))
            .Returns([]);

        var sut =
            CreateSut();

        Action act =
            () => sut
                .RecognizeCurrentScreenAsync()
                .GetAwaiter()
                .GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "The Android screenshot contains no image data.");

        _screenshots.Verify(
            service =>
                service.CaptureScreenshotBytes(null),
            Times.Once);

        _screenshots.Verify(
            service =>
                service.TakeScreenshot(),
            Times.Never);

        _screenshots.Verify(
            service =>
                service.GetBytesOfCachedScreenshotBytes(null),
            Times.Never);

        _diagnosticImageWriter.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
        _screenshots.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_HonorsCancellationBeforeCapture()
    {
        using var cancellation =
            new CancellationTokenSource();

        cancellation.Cancel();

        var sut =
            CreateSut();

        Action act =
            () => sut
                .RecognizeCurrentScreenAsync(
                    cancellation.Token)
                .GetAwaiter()
                .GetResult();

        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage(
                "The operation was canceled.");

        _screenshots.VerifyNoOtherCalls();
        _diagnosticImageWriter.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_HonorsCancellationAfterCapture()
    {
        byte[] bytes = [1];

        using var cancellation =
            new CancellationTokenSource();

        _screenshots
            .Setup(service =>
                service.CaptureScreenshotBytes(null))
            .Callback(cancellation.Cancel)
            .Returns(bytes);

        var sut =
            CreateSut();

        Action act =
            () => sut
                .RecognizeCurrentScreenAsync(
                    cancellation.Token)
                .GetAwaiter()
                .GetResult();

        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage(
                "The operation was canceled.");

        _screenshots.Verify(
            service =>
                service.CaptureScreenshotBytes(null),
            Times.Once);

        _screenshots.Verify(
            service =>
                service.TakeScreenshot(),
            Times.Never);

        _screenshots.Verify(
            service =>
                service.GetBytesOfCachedScreenshotBytes(null),
            Times.Never);

        _diagnosticImageWriter.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
        _screenshots.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_PropagatesScreenshotFailure()
    {
        _screenshots
            .Setup(service =>
                service.CaptureScreenshotBytes(null))
            .Throws(
                new InvalidOperationException(
                    "Screenshot capture failed."));

        var sut =
            CreateSut();

        Action act =
            () => sut
                .RecognizeCurrentScreenAsync()
                .GetAwaiter()
                .GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "Screenshot capture failed.");

        _screenshots.Verify(
            service =>
                service.CaptureScreenshotBytes(null),
            Times.Once);

        _diagnosticImageWriter.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
        _screenshots.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_PropagatesRecognitionFailure()
    {
        byte[] bytes = [1];

        _screenshots
            .Setup(service =>
                service.CaptureScreenshotBytes(null))
            .Returns(bytes);

        _diagnosticImageWriter
            .Setup(writer =>
                writer.WriteAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<string?>(),
                    CancellationToken.None))
            .Returns(Task.CompletedTask);

        _ocr
            .Setup(service =>
                service.RecognizeAsync(
                    bytes,
                    "zh-TW",
                    CancellationToken.None))
            .ThrowsAsync(
                new InvalidOperationException(
                    "OCR recognition failed."));

        var sut =
            CreateSut();

        Action act =
            () => sut
                .RecognizeCurrentScreenAsync()
                .GetAwaiter()
                .GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "OCR recognition failed.");

        _screenshots.Verify(
            service =>
                service.CaptureScreenshotBytes(null),
            Times.Once);

        _diagnosticImageWriter.Verify(
            writer =>
                writer.WriteAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<string?>(),
                    CancellationToken.None),
            Times.Once);

        _ocr.Verify(
            service =>
                service.RecognizeAsync(
                    bytes,
                    "zh-TW",
                    CancellationToken.None),
            Times.Once);

        _screenshots.VerifyNoOtherCalls();
        _diagnosticImageWriter.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RecognizeCurrentScreenAsync_WhenCalledMultipleTimes_ShouldUseUniqueDiagnosticPaths()
    {
        byte[] firstImageBuffer =
            [1, 2, 3];

        byte[] secondImageBuffer =
            [4, 5, 6];

        OcrResult firstResult =
            new(
                Text: "first",
                Lines: Array.Empty<OcrTextLine>());

        OcrResult secondResult =
            new(
                Text: "second",
                Lines: Array.Empty<OcrTextLine>());

        var screenshotServiceMock =
            new Mock<IScreenshotService>(
                MockBehavior.Strict);

        var ocrUtilityServiceMock =
            new Mock<IOCRUtilityService>(
                MockBehavior.Strict);

        var diagnosticImageWriterMock =
            new Mock<IOcrDiagnosticImageWriter>(
                MockBehavior.Strict);

        screenshotServiceMock
            .SetupSequence(service =>
                service.CaptureScreenshotBytes(null))
            .Returns(firstImageBuffer)
            .Returns(secondImageBuffer);

        List<string> diagnosticPaths = [];

        diagnosticImageWriterMock
            .Setup(writer =>
                writer.WriteAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<string?>(),
                    It.IsAny<CancellationToken>()))
            .Callback<
                ReadOnlyMemory<byte>,
                string?,
                CancellationToken>(
                (_, path, _) =>
                {
                    path.Should()
                        .NotBeNullOrWhiteSpace();

                    diagnosticPaths.Add(
                        path!);
                })
            .Returns(Task.CompletedTask);

        ocrUtilityServiceMock
            .Setup(service =>
                service.RecognizeAsync(
                    firstImageBuffer,
                    "zh-TW",
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(firstResult);

        ocrUtilityServiceMock
            .Setup(service =>
                service.RecognizeAsync(
                    secondImageBuffer,
                    "zh-TW",
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(secondResult);

        var sut =
            new AndroidScreenOcrService(
                screenshotServiceMock.Object,
                ocrUtilityServiceMock.Object,
                _loggerFactoryServiceMock.Object,
                diagnosticImageWriterMock.Object);

        Func<Task> act =
            async () =>
            {
                await sut
                    .RecognizeCurrentScreenAsync();

                await sut
                    .RecognizeCurrentScreenAsync();
            };

        await act.Should()
            .NotThrowAsync();

        diagnosticPaths.Should()
            .HaveCount(2);

        diagnosticPaths.Should()
            .OnlyHaveUniqueItems();

        diagnosticPaths.Should()
            .AllSatisfy(
                path =>
                    Path.GetExtension(path)
                        .Should()
                        .Be(".png"));

        screenshotServiceMock.Verify(
            service =>
                service.CaptureScreenshotBytes(null),
            Times.Exactly(2));

        screenshotServiceMock.Verify(
            service =>
                service.TakeScreenshot(),
            Times.Never);

        screenshotServiceMock.Verify(
            service =>
                service.GetBytesOfCachedScreenshotBytes(null),
            Times.Never);

        diagnosticImageWriterMock.Verify(
            writer =>
                writer.WriteAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<string?>(),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(2));

        ocrUtilityServiceMock.Verify(
            service =>
                service.RecognizeAsync(
                    It.IsAny<byte[]>(),
                    "zh-TW",
                    It.IsAny<CancellationToken>()),
            Times.Exactly(2));

        screenshotServiceMock.VerifyNoOtherCalls();
        diagnosticImageWriterMock.VerifyNoOtherCalls();
        ocrUtilityServiceMock.VerifyNoOtherCalls();
    }

    private AndroidScreenOcrService CreateSut()
    {
        return new AndroidScreenOcrService(
            _screenshots.Object,
            _ocr.Object,
            _loggerFactoryServiceMock.Object,
            _diagnosticImageWriter.Object);
    }
}