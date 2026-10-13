using FluentAssertions;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Stability;

public sealed class OcrTargetStabilityEvaluatorTests
{
    private static NormalizedOcrTextLine Target(double x = 0.25, double y = 0.75) =>
        new("任務", x, y);

    [Fact]
    public void Observe_ShouldRequireBaselineAndTwoMatchingComparisons()
    {
        var sut = new OcrTargetStabilityEvaluator("任務", 0.02);

        sut.Observe([Target()], 2).Should().BeFalse();
        sut.Observe([Target(0.26)], 2).Should().BeFalse();
        sut.Observe([Target(0.27)], 2).Should().BeTrue();
    }

    [Fact]
    public void Observe_WhenTargetDisappears_ShouldDiscardAccumulatedMatches()
    {
        var sut = new OcrTargetStabilityEvaluator("任務");

        sut.Observe([Target()], 2).Should().BeFalse();
        sut.Observe([Target()], 2).Should().BeFalse();
        sut.Observe([], 2).Should().BeFalse();
        sut.Observe([Target()], 2).Should().BeFalse();
        sut.Observe([Target()], 2).Should().BeFalse();
        sut.Observe([Target()], 2).Should().BeTrue();
    }

    [Fact]
    public void Observe_WhenTargetIsAmbiguous_ShouldReset()
    {
        var sut = new OcrTargetStabilityEvaluator("任務");

        sut.Observe([Target()], 1).Should().BeFalse();
        sut.Observe([Target(), Target(0.6)], 1).Should().BeFalse();
        sut.Observe([Target()], 1).Should().BeFalse();
        sut.Observe([Target()], 1).Should().BeTrue();
    }

    [Fact]
    public void Observe_WhenTargetMovesBeyondTolerance_ShouldRestartMatching()
    {
        var sut = new OcrTargetStabilityEvaluator("任務", 0.02);

        sut.Observe([Target()], 1).Should().BeFalse();
        sut.Observe([Target(0.40)], 1).Should().BeFalse();
        sut.Observe([Target(0.40)], 1).Should().BeTrue();
    }

    [Fact]
    public void Reset_ShouldRemovePreviousBaseline()
    {
        var sut = new OcrTargetStabilityEvaluator("任務");

        sut.Observe([Target()], 1).Should().BeFalse();
        sut.Reset();
        sut.Observe([Target()], 1).Should().BeFalse();
        sut.Observe([Target()], 1).Should().BeTrue();
    }

    [Fact]
    public void Factory_ShouldCreateIndependentEvaluators()
    {
        var factory = new OcrTargetStabilityEvaluatorFactory();
        IOcrTargetStabilityEvaluator first = factory.Create("任務");
        IOcrTargetStabilityEvaluator second = factory.Create("任務");

        first.Should().NotBeSameAs(second);
        first.Observe([Target()], 1).Should().BeFalse();
        first.Observe([Target()], 1).Should().BeTrue();
        second.Observe([Target()], 1).Should().BeFalse();
    }

    [Fact]
    public void Observe_WithInvalidMatchCount_ShouldThrowExpectedMessage()
    {
        var sut = new OcrTargetStabilityEvaluator("任務");
        Action act = () => sut.Observe([Target()], 0);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Required consecutive matches must be positive.*");
    }

    [Fact]
    public void Constructor_WithInvalidTolerance_ShouldThrowExpectedMessage()
    {
        Action act = () => new OcrTargetStabilityEvaluator("任務", double.NaN);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Maximum center movement must be between 0 and 1.*");
    }
}
