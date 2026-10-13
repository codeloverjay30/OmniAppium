using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Observation;

public sealed class AndroidScreenObservationServiceTests
{
    private static readonly byte[] ValidatedFrame = [1, 2, 3];

    private static AndroidScreenObservationService CreateSut(
        Mock<IScreenshotService> screenshots,
        Mock<IAndroidScreenOcrService> ocr,
        Mock<IAndroidScreenStabilityGuard> guard)
    {
        return new AndroidScreenObservationService(
            screenshots.Object, ocr.Object, guard.Object, new ScreenStabilityOptions());
    }

    private static StableScreenCaptureResult Stable(byte[] frame)
    {
        return new StableScreenCaptureResult(
            new ScreenStabilityResult(true, false, 3), frame);
    }

    private static OcrResult Ocr()
    {
        return new OcrResult("Recognized text", Array.Empty<OcrTextLine>());
    }

    [Fact]
    public async Task ObserveAsync_WhenStable_ShouldReturnObservation()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        byte[]? received = null;
        OcrResult recognized = Ocr();

        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Stable(ValidatedFrame));
        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
            .Callback<ReadOnlyMemory<byte>, CancellationToken>(
                (image, _) => received = image.ToArray())
            .ReturnsAsync(recognized);

        var sut = CreateSut(screenshots, ocr, guard);
        var observation = await sut.ObserveAsync(TestContext.Current.CancellationToken);

        observation.ImageBytes.ToArray().Should().Equal(ValidatedFrame);
        observation.OcrResult.Should().BeSameAs(recognized);
        received.Should().Equal(ValidatedFrame);
        guard.Verify(x => x.CaptureStableAsync(
            It.IsAny<ScreenStabilityOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        ocr.Verify(x => x.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()), Times.Once);
        screenshots.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenGuardTimesOut_ShouldNotRunOcr()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StableScreenCaptureResult(
                new ScreenStabilityResult(false, true, 2), null));

        var sut = CreateSut(screenshots, ocr, guard);
        Action act = () => sut.ObserveAsync(TestContext.Current.CancellationToken)
            .GetAwaiter().GetResult();

        act.Should().Throw<TimeoutException>()
            .WithMessage("Android screen stability timeout after 2 samples.");
        guard.VerifyAll();
        screenshots.VerifyNoOtherCalls();
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenScreenshotIsEmpty_ShouldNotRunOcr()
    {
        // An empty successful frame is rejected by the capture-result contract.
        Action act = () => new StableScreenCaptureResult(
            new ScreenStabilityResult(true, false, 1), []);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*image*");
    }

    [Fact]
    public void ObserveAsync_WhenScreenshotThrows_ShouldPropagateException()
    {
        // Screenshot capture belongs to the stability guard. Model the propagated
        // capture failure at the observation service's guard boundary.
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        const string message = "Screenshot capture failed.";

        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException(message));

        var sut = CreateSut(screenshots, ocr, guard);
        Action act = () => sut.ObserveAsync(TestContext.Current.CancellationToken)
            .GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>().WithMessage(message);
        guard.VerifyAll();
        screenshots.VerifyNoOtherCalls();
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenGuardThrows_ShouldPropagateException()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        const string message = "Stability guard failed.";

        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException(message));

        var sut = CreateSut(screenshots, ocr, guard);
        Action act = () => sut.ObserveAsync(TestContext.Current.CancellationToken)
            .GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>().WithMessage(message);
        guard.VerifyAll();
        screenshots.VerifyNoOtherCalls();
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenOcrThrows_ShouldPropagateException()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        const string message = "OCR recognition failed.";

        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Stable(ValidatedFrame));
        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException(message));

        var sut = CreateSut(screenshots, ocr, guard);
        Action act = () => sut.ObserveAsync(TestContext.Current.CancellationToken)
            .GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>().WithMessage(message);
        guard.VerifyAll();
        ocr.VerifyAll();
        screenshots.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenCancelledAfterGuard_ShouldNotRunOcr()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        using var cancellation = new CancellationTokenSource();

        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(), cancellation.Token))
            .Returns(() =>
            {
                cancellation.Cancel();
                return Task.FromResult(Stable(ValidatedFrame));
            });

        var sut = CreateSut(screenshots, ocr, guard);
        Action act = () => sut.ObserveAsync(cancellation.Token)
            .GetAwaiter().GetResult();

        act.Should().Throw<OperationCanceledException>()
            .WithMessage("*canceled*");
        guard.VerifyAll();
        ocr.VerifyNoOtherCalls();
        screenshots.VerifyNoOtherCalls();
    }
}