
using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Stability;

public sealed class ScreenObservationBoundaryTests
{
    private static AndroidScreenObservationService CreateSut(
        Mock<IScreenshotService> screenshots,
        Mock<IAndroidScreenOcrService> ocr,
        Mock<IAndroidScreenStabilityGuard> guard,
        ScreenStabilityOptions? options = null)
    {
        return new AndroidScreenObservationService(
            screenshots.Object,
            ocr.Object,
            guard.Object,
            options ?? new ScreenStabilityOptions());
    }

    private static OcrResult CreateOcrResult()
    {
        return new OcrResult(
            Text: "Boundary OCR text",
            Lines: Array.Empty<OcrTextLine>());
    }

    private static bool MatchesFrame(ReadOnlyMemory<byte> actual, byte[] expected)
    {
        return actual.Span.SequenceEqual(expected);
    }

    private static StableScreenCaptureResult Stable(byte[] image)
    {
        return new StableScreenCaptureResult(
            new ScreenStabilityResult(true, false, 3), image);
    }

    [Fact]
    public async Task ObserveAsync_ShouldWaitForStabilityBeforeScreenshotAndOcr()
    {
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        byte[] validatedFrame = [1, 2, 3];
        OcrResult result = CreateOcrResult();
        var calls = new List<string>();

        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback(() => calls.Add("guard"))
            .ReturnsAsync(Stable(validatedFrame));

        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<CancellationToken>()))
            .Callback<ReadOnlyMemory<byte>, CancellationToken>((buffer, _) =>
            {
                buffer.ToArray().Should().Equal(validatedFrame);
                calls.Add("ocr");
            })
            .ReturnsAsync(result);

        var sut = CreateSut(screenshots, ocr, guard);
        var observation = await sut.ObserveAsync(TestContext.Current.CancellationToken);

        observation.ImageBytes.ToArray().Should().Equal(validatedFrame);
        observation.OcrResult.Should().BeSameAs(result);
        calls.Should().Equal("guard", "ocr");
        guard.VerifyAll();
        ocr.VerifyAll();
        screenshots.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenGuardTimesOut_ShouldNeverCrossOcrBoundary()
    {
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StableScreenCaptureResult(
                new ScreenStabilityResult(false, true, 2), null));

        var sut = CreateSut(screenshots, ocr, guard);
        Action act = () => sut.ObserveAsync(
            TestContext.Current.CancellationToken).GetAwaiter().GetResult();

        act.Should().Throw<TimeoutException>()
            .WithMessage("Android screen stability timeout after 2 samples.");

        guard.Verify(x => x.CaptureStableAsync(
            It.IsAny<ScreenStabilityOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
        screenshots.VerifyNoOtherCalls();
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ObserveAsync_ShouldPassExactScreenshotToOcr()
    {
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        byte[] validatedFrame = [1, 3, 5, 7];
        byte[]? receivedImage = null;
        OcrResult result = CreateOcrResult();

        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Stable(validatedFrame));

        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<CancellationToken>()))
            .Callback<ReadOnlyMemory<byte>, CancellationToken>(
                (buffer, _) => receivedImage = buffer.ToArray())
            .ReturnsAsync(result);

        var sut = CreateSut(screenshots, ocr, guard);
        var observation = await sut.ObserveAsync(TestContext.Current.CancellationToken);

        receivedImage.Should().NotBeNull();
        receivedImage.Should().Equal(validatedFrame);
        observation.ImageBytes.ToArray().Should().Equal(validatedFrame);
        observation.OcrResult.Should().BeSameAs(result);
        screenshots.VerifyNoOtherCalls();
        ocr.Verify(x => x.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void ObserveAsync_WhenCancelledAfterGuard_ShouldNotCaptureOrRunOcr()
    {
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        using var cts = new CancellationTokenSource();

        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(), cts.Token))
            .Returns(() =>
            {
                cts.Cancel();
                return Task.FromResult(Stable([1, 2, 3]));
            });

        var sut = CreateSut(screenshots, ocr, guard);
        Action act = () => sut.ObserveAsync(cts.Token).GetAwaiter().GetResult();

        act.Should().Throw<OperationCanceledException>().WithMessage("*canceled*");
        guard.Verify(x => x.CaptureStableAsync(
            It.IsAny<ScreenStabilityOptions>(), cts.Token), Times.Once);
        screenshots.VerifyNoOtherCalls();
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenCancelledAfterScreenshot_ShouldNotRunOcr()
    {
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        using var cts = new CancellationTokenSource();

        // Screenshot capture now occurs inside the guard, not in the observation service.
        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(), cts.Token))
            .Returns(() =>
            {
                cts.Cancel();
                return Task.FromResult(Stable([1, 2, 3]));
            });

        var sut = CreateSut(screenshots, ocr, guard);
        Action act = () => sut.ObserveAsync(cts.Token).GetAwaiter().GetResult();

        act.Should().Throw<OperationCanceledException>().WithMessage("*canceled*");
        screenshots.VerifyNoOtherCalls();
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenCancelledDuringOcr_ShouldNotReturnObservation()
    {
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        using var cts = new CancellationTokenSource();

        guard.Setup(x => x.CaptureStableAsync(
                It.IsAny<ScreenStabilityOptions>(), cts.Token))
            .ReturnsAsync(Stable([1, 2, 3]));

        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(), cts.Token))
            .Returns(() =>
            {
                cts.Cancel();
                return Task.FromResult(CreateOcrResult());
            });

        var sut = CreateSut(screenshots, ocr, guard);
        Action act = () => sut.ObserveAsync(cts.Token).GetAwaiter().GetResult();

        act.Should().Throw<OperationCanceledException>().WithMessage("*canceled*");
        ocr.Verify(x => x.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(), cts.Token), Times.Once);
        screenshots.VerifyNoOtherCalls();
    }
}