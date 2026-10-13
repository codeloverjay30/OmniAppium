using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Observation;

public sealed class AndroidScreenObservationServiceRoiTests
{
    private static readonly NormalizedOcrTextLine Target = new("任務", 0.25, 0.75);

    private static ScreenObservationStabilityOptions Options(int matches = 2) => new()
    {
        Region = new NormalizedScreenRegion(0.2, 0.7, 0.2, 0.2),
        TargetText = "任務",
        RequiredConsecutiveMatches = matches,
        SampleInterval = TimeSpan.Zero,
        Timeout = TimeSpan.FromMilliseconds(150)
    };

    private static OcrResult Result(string text = "任務") =>
        new(text, Array.Empty<OcrTextLine>());

    private static AndroidScreenObservationService Create(
        Mock<IScreenshotService> screenshot,
        Mock<IAndroidScreenOcrService> ocr,
        Mock<IScreenFrameDifferenceAnalyzer> analyzer,
        Mock<IOcrObservationLineNormalizer> normalizer,
        ScreenObservationStabilityOptions options)
    {
        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        return new AndroidScreenObservationService(
            screenshot.Object,
            ocr.Object,
            guard.Object,
            new ScreenStabilityOptions(),
            analyzer.Object,
            new OcrTargetStabilityEvaluatorFactory(),
            normalizer.Object,
            options);
    }

    private static void SetupOcr(
        Mock<IAndroidScreenOcrService> ocr,
        Mock<IOcrObservationLineNormalizer> normalizer,
        OcrResult result)
    {
        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        normalizer.Setup(x => x.Normalize(
                result, It.IsAny<ReadOnlyMemory<byte>>()))
            .Returns([Target]);
    }

    [Fact]
    public async Task ObserveAsync_WhenRoiAndOcrStable_ShouldReturnMatchingFinalFrame()
    {
        byte[] first = [1], second = [2], third = [3];
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var analyzer = new Mock<IScreenFrameDifferenceAnalyzer>(MockBehavior.Strict);
        var normalizer = new Mock<IOcrObservationLineNormalizer>(MockBehavior.Strict);
        OcrResult result = Result();

        screenshot.SetupSequence(x => x.CaptureScreenshotBytes(null))
            .Returns(first).Returns(second).Returns(third);

        SetupOcr(ocr, normalizer, result);
        analyzer.Setup(x => x.Analyze(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<NormalizedScreenRegion>(),
                It.IsAny<byte>()))
            .Returns(new ScreenFrameDifference(0, 0, 100, true));

        var sut = Create(screenshot, ocr, analyzer, normalizer, Options());
        IAndroidScreenObservation observation =
            await sut.ObserveAsync(TestContext.Current.CancellationToken);

        observation.ImageBytes.ToArray().Should().Equal(third);
        observation.OcrResult.Should().BeSameAs(result);
        screenshot.Verify(x => x.CaptureScreenshotBytes(null), Times.Exactly(3));
        ocr.Verify(x => x.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<CancellationToken>()), Times.Exactly(3));
        analyzer.Verify(x => x.Analyze(
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<NormalizedScreenRegion>(),
            It.IsAny<byte>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ObserveAsync_WhenScreenshotEmpty_ShouldThrowWithoutCallingOcr()
    {
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var analyzer = new Mock<IScreenFrameDifferenceAnalyzer>(MockBehavior.Strict);
        var normalizer = new Mock<IOcrObservationLineNormalizer>(MockBehavior.Strict);

        screenshot.Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(Array.Empty<byte>());

        var sut = Create(screenshot, ocr, analyzer, normalizer, Options());
        Func<Task> act = () => sut.ObserveAsync(TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("The captured Android screen contains no image data.");

        ocr.VerifyNoOtherCalls();
        analyzer.VerifyNoOtherCalls();
        normalizer.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ObserveAsync_WhenRoiAlwaysChanges_ShouldTimeoutEvenIfOcrStable()
    {
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var analyzer = new Mock<IScreenFrameDifferenceAnalyzer>(MockBehavior.Strict);
        var normalizer = new Mock<IOcrObservationLineNormalizer>(MockBehavior.Strict);
        OcrResult result = Result();

        screenshot.Setup(x => x.CaptureScreenshotBytes(null)).Returns([1]);
        SetupOcr(ocr, normalizer, result);
        analyzer.Setup(x => x.Analyze(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<NormalizedScreenRegion>(),
                It.IsAny<byte>()))
            .Returns(new ScreenFrameDifference(1, 100, 100, true));

        var sut = Create(screenshot, ocr, analyzer, normalizer, Options());
        Func<Task> act = () => sut.ObserveAsync(TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<TimeoutException>()
            .WithMessage("*ROI and OCR stability timeout*Target='任務'*RegionMatches=0*");
    }

    [Fact]
    public async Task ObserveAsync_WhenOcrTargetMissing_ShouldTimeoutEvenIfRoiStable()
    {
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var analyzer = new Mock<IScreenFrameDifferenceAnalyzer>(MockBehavior.Strict);
        var normalizer = new Mock<IOcrObservationLineNormalizer>(MockBehavior.Strict);
        OcrResult result = Result("");

        screenshot.Setup(x => x.CaptureScreenshotBytes(null)).Returns([1]);
        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        normalizer.Setup(x => x.Normalize(result, It.IsAny<ReadOnlyMemory<byte>>()))
            .Returns(Array.Empty<NormalizedOcrTextLine>());
        analyzer.Setup(x => x.Analyze(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<NormalizedScreenRegion>(),
                It.IsAny<byte>()))
            .Returns(new ScreenFrameDifference(0, 0, 100, true));

        var sut = Create(screenshot, ocr, analyzer, normalizer, Options());
        Func<Task> act = () => sut.ObserveAsync(TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<TimeoutException>()
            .WithMessage("*ROI and OCR stability timeout*Target='任務'*");
    }

    [Fact]
    public async Task ObserveAsync_WhenAlreadyCancelled_ShouldNotAccessDependencies()
    {
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var analyzer = new Mock<IScreenFrameDifferenceAnalyzer>(MockBehavior.Strict);
        var normalizer = new Mock<IOcrObservationLineNormalizer>(MockBehavior.Strict);
        var sut = Create(screenshot, ocr, analyzer, normalizer, Options());

        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Func<Task> act = () => sut.ObserveAsync(cancellation.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
        screenshot.VerifyNoOtherCalls();
        ocr.VerifyNoOtherCalls();
        analyzer.VerifyNoOtherCalls();
        normalizer.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ObserveAsync_WhenOcrThrows_ShouldPropagateOriginalException()
    {
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var analyzer = new Mock<IScreenFrameDifferenceAnalyzer>(MockBehavior.Strict);
        var normalizer = new Mock<IOcrObservationLineNormalizer>(MockBehavior.Strict);

        screenshot.Setup(x => x.CaptureScreenshotBytes(null)).Returns([1]);
        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("OCR backend unavailable."));

        var sut = Create(screenshot, ocr, analyzer, normalizer, Options());
        Func<Task> act = () => sut.ObserveAsync(TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("OCR backend unavailable.");
        normalizer.VerifyNoOtherCalls();
    }
}
