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

namespace OmniAppium.EngineUtilityServices.Tests.Stability;

public sealed class ScreenObservationRoiBoundaryTests
{
    [Fact]
    public async Task ObserveAsync_WhenRoiBecomesUnstable_ShouldResetEvaluatorBeforeNextObservation()
    {
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var analyzer = new Mock<IScreenFrameDifferenceAnalyzer>(MockBehavior.Strict);
        var normalizer = new Mock<IOcrObservationLineNormalizer>(MockBehavior.Strict);
        var evaluator = new Mock<IOcrTargetStabilityEvaluator>(MockBehavior.Strict);
        var factory = new Mock<IOcrTargetStabilityEvaluatorFactory>(MockBehavior.Strict);
        var events = new List<string>();
        OcrResult result = new("任務", Array.Empty<OcrTextLine>());
        var lines = new[] { new NormalizedOcrTextLine("任務", 0.25, 0.75) };

        var options = new ScreenObservationStabilityOptions
        {
            Region = new NormalizedScreenRegion(0.2, 0.7, 0.2, 0.2),
            TargetText = "任務",
            RequiredConsecutiveMatches = 1,
            SampleInterval = TimeSpan.Zero,
            Timeout = TimeSpan.FromMilliseconds(150)
        };

        screenshot.SetupSequence(x => x.CaptureScreenshotBytes(null))
            .Returns([1]).Returns([2]).Returns([3]);

        ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        normalizer.Setup(x => x.Normalize(result, It.IsAny<ReadOnlyMemory<byte>>()))
            .Returns(lines);

        analyzer.SetupSequence(x => x.Analyze(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<NormalizedScreenRegion>(),
                It.IsAny<byte>()))
            .Returns(new ScreenFrameDifference(1, 100, 100, true))
            .Returns(new ScreenFrameDifference(0, 0, 100, true));

        factory.Setup(x => x.Create("任務", options.MaximumCenterMovement))
            .Returns(evaluator.Object);

        evaluator.Setup(x => x.Reset())
            .Callback(() => events.Add("reset"));

        evaluator.SetupSequence(x => x.Observe(
                It.IsAny<IEnumerable<NormalizedOcrTextLine>>(), 1))
            .Returns(false)
            .Returns(false)
            .Returns(true);

        var guard = new Mock<IAndroidScreenStabilityGuard>(MockBehavior.Strict);
        var sut = new AndroidScreenObservationService(
            screenshot.Object,
            ocr.Object,
            guard.Object,
            new ScreenStabilityOptions(),
            analyzer.Object,
            factory.Object,
            normalizer.Object,
            options);

        IAndroidScreenObservation observation =
            await sut.ObserveAsync(TestContext.Current.CancellationToken);

        observation.ImageBytes.ToArray().Should().Equal([3]);
        events.Should().Equal("reset", "reset");
        evaluator.Verify(x => x.Observe(
            It.IsAny<IEnumerable<NormalizedOcrTextLine>>(), 1), Times.Exactly(3));
        evaluator.Verify(x => x.Reset(), Times.Exactly(2));
        guard.VerifyNoOtherCalls();
    }
}
