
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

    [Fact]
    public async Task ObserveAsync_ShouldWaitForStabilityBeforeScreenshotAndOcr()
    {
        var sequence = new MockSequence();

        var guard =
            new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);

        var screenshots =
            new Mock<IScreenshotService>(MockBehavior.Strict);

        var ocr =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        byte[] image = [10, 20, 30];
        OcrResult result = CreateOcrResult();

        guard.InSequence(sequence)
            .Setup(x => x.WaitForStabilityAsync(
                It.IsAny<ScreenStabilityOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new ScreenStabilityResult(true, false, 3));

        screenshots.InSequence(sequence)
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(image);

        ocr.InSequence(sequence)
            .Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var sut = CreateSut(screenshots, ocr, guard);

        var observation = await sut.ObserveAsync();

        observation.ImageBytes.ToArray().Should().Equal(image);
        observation.OcrResult.Should().BeSameAs(result);

        guard.VerifyAll();
        screenshots.VerifyAll();
        ocr.VerifyAll();
    }

    [Fact]
    public void ObserveAsync_WhenGuardTimesOut_ShouldNeverCrossOcrBoundary()
    {
        var guard =
            new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);

        var screenshots =
            new Mock<IScreenshotService>(MockBehavior.Strict);

        var ocr =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        guard.Setup(x => x.WaitForStabilityAsync(
                It.IsAny<ScreenStabilityOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new ScreenStabilityResult(false, true, 2));

        var sut = CreateSut(screenshots, ocr, guard);

        Action act = () =>
            sut.ObserveAsync().GetAwaiter().GetResult();

        act.Should()
            .Throw<TimeoutException>()
            .WithMessage(
                "Android screen stability timeout after 2 samples.");

        guard.Verify(x => x.WaitForStabilityAsync(
            It.IsAny<ScreenStabilityOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);

        screenshots.VerifyNoOtherCalls();
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ObserveAsync_ShouldPassExactScreenshotToOcr()
    {
        var guard =
            new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);

        var screenshots =
            new Mock<IScreenshotService>(MockBehavior.Strict);

        var ocr =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        byte[] capturedImage = [1, 3, 5, 7];
        byte[]? receivedImage = null;

        OcrResult result = CreateOcrResult();

        guard.Setup(x => x.WaitForStabilityAsync(
                It.IsAny<ScreenStabilityOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new ScreenStabilityResult(true, false, 3));

        screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(capturedImage);

        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<CancellationToken>()))
            .Callback<ReadOnlyMemory<byte>, CancellationToken>(
                (buffer, _) =>
                {
                    receivedImage = buffer.ToArray();
                })
            .ReturnsAsync(result);

        var sut = CreateSut(screenshots, ocr, guard);

        var observation = await sut.ObserveAsync();

        receivedImage.Should().NotBeNull();
        receivedImage.Should().Equal(capturedImage);

        observation.ImageBytes.ToArray()
            .Should().Equal(capturedImage);

        observation.OcrResult.Should().BeSameAs(result);

        screenshots.Verify(
            x => x.CaptureScreenshotBytes(null),
            Times.Once);

        ocr.Verify(x => x.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void ObserveAsync_WhenCancelledAfterGuard_ShouldNotCaptureOrRunOcr()
    {
        var guard =
            new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);

        var screenshots =
            new Mock<IScreenshotService>(MockBehavior.Strict);

        var ocr =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        using var cts = new CancellationTokenSource();

        guard.Setup(x => x.WaitForStabilityAsync(
                It.IsAny<ScreenStabilityOptions>(),
                cts.Token))
            .Returns(() =>
            {
                cts.Cancel();

                return Task.FromResult(
                    new ScreenStabilityResult(true, false, 3));
            });

        var sut = CreateSut(screenshots, ocr, guard);

        Action act = () =>
            sut.ObserveAsync(cts.Token).GetAwaiter().GetResult();

        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage("*canceled*");

        guard.Verify(x => x.WaitForStabilityAsync(
            It.IsAny<ScreenStabilityOptions>(),
            cts.Token), Times.Once);

        screenshots.VerifyNoOtherCalls();
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenCancelledAfterScreenshot_ShouldNotRunOcr()
    {
        var guard =
            new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);

        var screenshots =
            new Mock<IScreenshotService>(MockBehavior.Strict);

        var ocr =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        using var cts = new CancellationTokenSource();

        guard.Setup(x => x.WaitForStabilityAsync(
                It.IsAny<ScreenStabilityOptions>(),
                cts.Token))
            .ReturnsAsync(
                new ScreenStabilityResult(true, false, 3));

        screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(() =>
            {
                cts.Cancel();
                return new byte[] { 1, 2, 3 };
            });

        var sut = CreateSut(screenshots, ocr, guard);

        Action act = () =>
            sut.ObserveAsync(cts.Token).GetAwaiter().GetResult();

        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage("*canceled*");

        screenshots.Verify(
            x => x.CaptureScreenshotBytes(null),
            Times.Once);

        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenCancelledDuringOcr_ShouldNotReturnObservation()
    {
        var guard =
            new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);

        var screenshots =
            new Mock<IScreenshotService>(MockBehavior.Strict);

        var ocr =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        using var cts = new CancellationTokenSource();

        guard.Setup(x => x.WaitForStabilityAsync(
                It.IsAny<ScreenStabilityOptions>(),
                cts.Token))
            .ReturnsAsync(
                new ScreenStabilityResult(true, false, 3));

        screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns([1, 2, 3]);

        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                cts.Token))
            .Returns(() =>
            {
                cts.Cancel();
                return Task.FromResult(CreateOcrResult());
            });

        var sut = CreateSut(screenshots, ocr, guard);

        Action act = () =>
            sut.ObserveAsync(cts.Token).GetAwaiter().GetResult();

        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage("*canceled*");

        ocr.Verify(x => x.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(),
            cts.Token), Times.Once);
    }
}