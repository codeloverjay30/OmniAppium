using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Stability;

/// <summary>
/// Regression tests for the existing screen-observation boundary.
/// These tests compile against the interfaces present in the supplied snapshot.
/// </summary>
public sealed class ScreenObservationBoundaryTests
{
    [Fact]
    public void AndroidScreenObservation_WhenSourceBufferChanges_ShouldRetainOriginalImage()
    {
        byte[] source = [1, 2, 3];
        var ocr = new OcrResult(string.Empty, Array.Empty<OcrTextLine>());
        var sut = new AndroidScreenObservation(source, ocr);

        source[0] = 99;

        sut.ImageBytes.ToArray().Should().Equal(1, 2, 3);
        sut.OcrResult.Should().BeSameAs(ocr);
    }

    [Fact]
    public void AndroidScreenObservation_WhenImageIsEmpty_ShouldRejectIt()
    {
        var ocr = new OcrResult(string.Empty, Array.Empty<OcrTextLine>());
        Action act = () => new AndroidScreenObservation([], ocr);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Screen observation image data cannot be empty*");
    }

    [Fact]
    public async Task ObserveAsync_WhenCaptureSucceeds_ShouldRunOcrOnTheSameImage()
    {
        byte[] captured = [1, 2, 3];
        var expectedOcr = new OcrResult("text", Array.Empty<OcrTextLine>());
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        screenshot.Setup(x => x.CaptureScreenshotBytes(null)).Returns(captured);
        ocr.Setup(x => x.RecognizeAsync(
                It.Is<ReadOnlyMemory<byte>>(bytes => bytes.Length == 3),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedOcr);

        var sut = new AndroidScreenObservationService(screenshot.Object, ocr.Object);
        IAndroidScreenObservation result = await sut.ObserveAsync();

        result.ImageBytes.ToArray().Should().Equal(captured);
        result.OcrResult.Should().BeSameAs(expectedOcr);
        screenshot.Verify(x => x.CaptureScreenshotBytes(null), Times.Once);
        ocr.Verify(x => x.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObserveAsync_WhenScreenshotIsEmpty_ShouldNotInvokeOcr()
    {
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        screenshot.Setup(x => x.CaptureScreenshotBytes(null)).Returns([]);
        var sut = new AndroidScreenObservationService(screenshot.Object, ocr.Object);

        Func<Task> act = () => sut.ObserveAsync();

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("The captured Android screen contains no image data.");
        ocr.Verify(x => x.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ObserveAsync_WhenCanceledBeforeCapture_ShouldNotCapture()
    {
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var sut = new AndroidScreenObservationService(screenshot.Object, ocr.Object);
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        Func<Task> act = () => sut.ObserveAsync(cts.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
        screenshot.Verify(x => x.CaptureScreenshotBytes(null), Times.Never);
    }

    [Fact]
    public async Task ObserveAsync_WhenCaptureFails_ShouldPreserveOriginalException()
    {
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        screenshot.Setup(x => x.CaptureScreenshotBytes(null))
            .Throws(new InvalidOperationException("capture failed"));
        var sut = new AndroidScreenObservationService(screenshot.Object, ocr.Object);

        Func<Task> act = () => sut.ObserveAsync();

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("capture failed");
        ocr.Verify(x => x.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
}
