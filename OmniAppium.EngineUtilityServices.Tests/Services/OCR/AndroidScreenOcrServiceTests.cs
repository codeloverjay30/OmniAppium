using FluentAssertions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Services.OCR;
using System.Drawing.Imaging;

namespace OmniAppium.EngineUtilityServices.Tests.Services.OCR;

public sealed class AndroidScreenOcrServiceTests
{
    private readonly Mock<IScreenshotService> _screenshots = new(MockBehavior.Strict);
    private readonly Mock<IOCRUtilityService> _ocr = new(MockBehavior.Strict);
    private readonly Mock<IOcrDiagnosticImageWriter> _diagnosticImageWriter = new(MockBehavior.Strict);
    private readonly Mock<ILoggerFactoryBaseUtilityService> _loggerFactoryServiceMock;
    private readonly Mock<ILogger> _loggerMock;

    public AndroidScreenOcrServiceTests()
    {
                _loggerFactoryServiceMock =
            new Mock<ILoggerFactoryBaseUtilityService>(MockBehavior.Strict);

        _loggerMock =
            new Mock<ILogger>(MockBehavior.Loose);

        _loggerFactoryServiceMock
            .SetupGet(service => service.Logger)
            .Returns(_loggerMock.Object);
    }
    [Fact]
    public async Task RecognizeCurrentScreenAsync_CapturesFreshBytesAndForwardsLanguageAndCancellation()
    {
        byte[] bytes = [1, 2, 3];
        using var cancellation = new CancellationTokenSource();
        var expected = new OcrResult("任務", []);
        var sequence = new MockSequence();
        _screenshots.InSequence(sequence).Setup(service => service.TakeScreenshot());
        _screenshots.InSequence(sequence)
            .Setup(service => service.GetBytesOfCachedScreenshotBytes(null)).Returns(bytes);
        _ocr.InSequence(sequence)
            .Setup(service => service.RecognizeAsync(bytes, "zh-TW", cancellation.Token))
            .ReturnsAsync(expected);
        _diagnosticImageWriter
            .Setup(writer =>
                writer.WriteAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<string?>(),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var sut = new AndroidScreenOcrService(
            _screenshots.Object,
            _ocr.Object,
            _loggerFactoryServiceMock.Object,
            _diagnosticImageWriter.Object
        );

        var actual = await sut.RecognizeCurrentScreenAsync(cancellation.Token);

        actual.Should().BeSameAs(expected);
        _screenshots.VerifyAll();
        _ocr.VerifyAll();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_RejectsEmptyScreenshotBeforeRecognition()
    {
        _screenshots.Setup(service => service.TakeScreenshot());
        _screenshots.Setup(service => service.GetBytesOfCachedScreenshotBytes(null)).Returns([]);
                var sut = new AndroidScreenOcrService(
            _screenshots.Object,
            _ocr.Object,
            _loggerFactoryServiceMock.Object,
            _diagnosticImageWriter.Object
        );

        Action act = () => sut.RecognizeCurrentScreenAsync().GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The Android screenshot contains no image data.");
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_HonorsCancellationBeforeCapture()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var sut = new AndroidScreenOcrService(
            _screenshots.Object,
            _ocr.Object,
            _loggerFactoryServiceMock.Object,
            _diagnosticImageWriter.Object
        );
        Action act = () => sut.RecognizeCurrentScreenAsync(cancellation.Token).GetAwaiter().GetResult();

        act.Should().Throw<OperationCanceledException>()
            .WithMessage("The operation was canceled.");
        _screenshots.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_HonorsCancellationAfterCapture()
    {
        using var cancellation = new CancellationTokenSource();
        _screenshots.Setup(service => service.TakeScreenshot()).Callback(cancellation.Cancel);
        _screenshots.Setup(service => service.GetBytesOfCachedScreenshotBytes(null)).Returns([1]);
        var sut = new AndroidScreenOcrService(
            _screenshots.Object,
            _ocr.Object,
            _loggerFactoryServiceMock.Object,
            _diagnosticImageWriter.Object
        );
        Action act = () => sut.RecognizeCurrentScreenAsync(cancellation.Token).GetAwaiter().GetResult();

        act.Should().Throw<OperationCanceledException>()
            .WithMessage("The operation was canceled.");
        _screenshots.VerifyAll();
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_PropagatesScreenshotFailure()
    {
        _screenshots.Setup(service => service.TakeScreenshot())
            .Throws(new InvalidOperationException("Screenshot capture failed."));
        var sut = new AndroidScreenOcrService(
            _screenshots.Object,
            _ocr.Object,
            _loggerFactoryServiceMock.Object,
            _diagnosticImageWriter.Object
        );
        Action act = () => sut.RecognizeCurrentScreenAsync().GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>().WithMessage("Screenshot capture failed.");
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_PropagatesRecognitionFailure()
    {
        byte[] bytes = [1];
        _screenshots.Setup(service => service.TakeScreenshot());
        _screenshots.Setup(service => service.GetBytesOfCachedScreenshotBytes(null)).Returns(bytes);
        _ocr.Setup(service => service.RecognizeAsync(bytes, "zh-TW", CancellationToken.None))
            .ThrowsAsync(new InvalidOperationException("OCR recognition failed."));

        _diagnosticImageWriter
            .Setup(writer =>
                writer.WriteAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<string?>(),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var sut = new AndroidScreenOcrService(
            _screenshots.Object,
            _ocr.Object,
            _loggerFactoryServiceMock.Object,
            _diagnosticImageWriter.Object
        );
        Action act = () => sut.RecognizeCurrentScreenAsync().GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>().WithMessage("OCR recognition failed.");
    }

    [Fact]
    public async Task RecognizeCurrentScreenAsync_WhenCalledMultipleTimes_ShouldUseUniqueDiagnosticPaths()
    {
        // Arrange
        byte[] firstImageBuffer = [1, 2, 3];
        byte[] secondImageBuffer = [4, 5, 6];

        OcrResult firstResult = new(
            Text: "first",
            Lines: Array.Empty<OcrTextLine>());

        OcrResult secondResult = new(
            Text: "second",
            Lines: Array.Empty<OcrTextLine>());

        var screenshotServiceMock =
            new Mock<IScreenshotService>(MockBehavior.Strict);

        var ocrUtilityServiceMock =
            new Mock<IOCRUtilityService>(MockBehavior.Strict);

        var diagnosticImageWriterMock =
            new Mock<IOcrDiagnosticImageWriter>(MockBehavior.Strict);

        screenshotServiceMock
            .Setup(service => service.TakeScreenshot());

        screenshotServiceMock
            .SetupSequence(service =>
                service.GetBytesOfCachedScreenshotBytes())
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
                    path.Should().NotBeNullOrWhiteSpace();
                    diagnosticPaths.Add(path!);
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

        var sut = new AndroidScreenOcrService(
            screenshotServiceMock.Object,
            ocrUtilityServiceMock.Object,
            _loggerFactoryServiceMock.Object,
            diagnosticImageWriterMock.Object);

        // Act
        Func<Task> act = async () =>
        {
            await sut.RecognizeCurrentScreenAsync();
            await sut.RecognizeCurrentScreenAsync();
        };

        // Assert
        await act.Should().NotThrowAsync();

        diagnosticPaths.Should().HaveCount(2);
        diagnosticPaths.Should().OnlyHaveUniqueItems();

        diagnosticPaths.Should().AllSatisfy(
            path =>
                Path.GetExtension(path)
                    .Should()
                    .Be(".png"));

        screenshotServiceMock.Verify(
            service => service.TakeScreenshot(),
            Times.Exactly(2));

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
    }

}
