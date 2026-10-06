using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;
using System.Drawing.Imaging;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Observation;

public sealed class AndroidScreenObservationServiceTests
{
    private readonly Mock<IScreenshotService> _screenshotServiceMock =
        new(MockBehavior.Strict);

    private readonly Mock<IAndroidScreenOcrService> _ocrServiceMock =
        new(MockBehavior.Strict);

    [Fact]
    public async Task ObserveAsync_ShouldUseAtomicCaptureAndSupplyExactCapturedBytesToOcr()
    {
        byte[] capturedImageBytes = [1, 2, 3, 4];

        OcrResult expectedOcrResult =
            new(
                Text: "任務",
                Lines: Array.Empty<OcrTextLine>());

        ReadOnlyMemory<byte> suppliedOcrImage = default;

        _screenshotServiceMock
            .Setup(service =>
                service.CaptureScreenshotBytes(null))
            .Returns(capturedImageBytes);

        _ocrServiceMock
            .Setup(service =>
                service.RecognizeAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    CancellationToken.None))
            .Callback<ReadOnlyMemory<byte>, CancellationToken>(
                (imageBuffer, _) =>
                {
                    suppliedOcrImage = imageBuffer;
                })
            .ReturnsAsync(expectedOcrResult);

        var sut =
            new AndroidScreenObservationService(
                _screenshotServiceMock.Object,
                _ocrServiceMock.Object);

        IAndroidScreenObservation observation =
            await sut.ObserveAsync();

        suppliedOcrImage
            .ToArray()
            .Should()
            .Equal(capturedImageBytes);

        observation.ImageBytes
            .ToArray()
            .Should()
            .Equal(capturedImageBytes);

        observation.OcrResult
            .Should()
            .BeSameAs(expectedOcrResult);

        _screenshotServiceMock.Verify(
            service =>
                service.CaptureScreenshotBytes(null),
            Times.Once);

        _screenshotServiceMock.Verify(
            service =>
                service.TakeScreenshot(),
            Times.Never);

        _screenshotServiceMock.Verify(
            service =>
                service.GetBytesOfCachedScreenshotBytes(
                    It.IsAny<ImageFormat?>()),
            Times.Never);

        _ocrServiceMock.Verify(
            service =>
                service.RecognizeAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    CancellationToken.None),
            Times.Once);

        _screenshotServiceMock.VerifyNoOtherCalls();
        _ocrServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_ShouldRejectEmptyAtomicCaptureBeforeOcr()
    {
        _screenshotServiceMock
            .Setup(service =>
                service.CaptureScreenshotBytes(null))
            .Returns([]);

        var sut =
            new AndroidScreenObservationService(
                _screenshotServiceMock.Object,
                _ocrServiceMock.Object);

        Action act =
            () => sut
                .ObserveAsync()
                .GetAwaiter()
                .GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "The captured Android screen contains no image data.");

        _screenshotServiceMock.Verify(
            service =>
                service.CaptureScreenshotBytes(null),
            Times.Once);

        _ocrServiceMock.VerifyNoOtherCalls();
        _screenshotServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_ShouldHonorCancellationBeforeCapture()
    {
        using var cancellation =
            new CancellationTokenSource();

        cancellation.Cancel();

        var sut =
            new AndroidScreenObservationService(
                _screenshotServiceMock.Object,
                _ocrServiceMock.Object);

        Action act =
            () => sut
                .ObserveAsync(cancellation.Token)
                .GetAwaiter()
                .GetResult();

        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage("The operation was canceled.");

        _screenshotServiceMock.VerifyNoOtherCalls();
        _ocrServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_ShouldHonorCancellationAfterCaptureBeforeOcr()
    {
        byte[] capturedImageBytes = [1, 2, 3];

        using var cancellation =
            new CancellationTokenSource();

        _screenshotServiceMock
            .Setup(service =>
                service.CaptureScreenshotBytes(null))
            .Callback(cancellation.Cancel)
            .Returns(capturedImageBytes);

        var sut =
            new AndroidScreenObservationService(
                _screenshotServiceMock.Object,
                _ocrServiceMock.Object);

        Action act =
            () => sut
                .ObserveAsync(cancellation.Token)
                .GetAwaiter()
                .GetResult();

        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage("The operation was canceled.");

        _screenshotServiceMock.Verify(
            service =>
                service.CaptureScreenshotBytes(null),
            Times.Once);

        _ocrServiceMock.VerifyNoOtherCalls();
        _screenshotServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_ShouldPropagateAtomicCaptureFailure()
    {
        _screenshotServiceMock
            .Setup(service =>
                service.CaptureScreenshotBytes(null))
            .Throws(
                new InvalidOperationException(
                    "Screenshot capture failed."));

        var sut =
            new AndroidScreenObservationService(
                _screenshotServiceMock.Object,
                _ocrServiceMock.Object);

        Action act =
            () => sut
                .ObserveAsync()
                .GetAwaiter()
                .GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Screenshot capture failed.");

        _screenshotServiceMock.Verify(
            service =>
                service.CaptureScreenshotBytes(null),
            Times.Once);

        _ocrServiceMock.VerifyNoOtherCalls();
        _screenshotServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_ShouldPropagateOcrFailure()
    {
        byte[] capturedImageBytes = [1, 2, 3];

        _screenshotServiceMock
            .Setup(service =>
                service.CaptureScreenshotBytes(null))
            .Returns(capturedImageBytes);

        _ocrServiceMock
     .Setup(service =>
         service.RecognizeAsync(
             It.IsAny<ReadOnlyMemory<byte>>(),
             CancellationToken.None))
     .Callback<ReadOnlyMemory<byte>, CancellationToken>(
         (imageBuffer, _) =>
         {
             imageBuffer
                 .ToArray()
                 .Should()
                 .Equal(capturedImageBytes);
         })
     .ThrowsAsync(
         new InvalidOperationException(
             "OCR recognition failed."));
            

        var sut =
            new AndroidScreenObservationService(
                _screenshotServiceMock.Object,
                _ocrServiceMock.Object);

        Action act =
            () => sut
                .ObserveAsync()
                .GetAwaiter()
                .GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("OCR recognition failed.");

        _screenshotServiceMock.VerifyAll();
        _ocrServiceMock.VerifyAll();
    }
}