using FluentAssertions;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Models.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Models.Observation;

public sealed class ScreenObservationStabilityOptionsTests
{
    private static ScreenObservationStabilityOptions Valid() => new()
    {
        Region = new NormalizedScreenRegion(0.2, 0.3, 0.4, 0.5),
        TargetText = "任務"
    };

    [Fact]
    public void Validate_WithDefaults_ShouldSucceed()
    {
        Action act = () => Valid().Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithRegionTouchingBottomRightEdge_ShouldSucceed()
    {
        var options = Valid() with
        {
            Region = new NormalizedScreenRegion(0.2, 0.3, 0.8, 0.7)
        };
        Action act = () => options.Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithNullRegion_ShouldThrow()
    {
        var options = Valid() with { Region = null! };
        Action act = () => options.Validate();
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Region*");
    }

    [Theory]
    [InlineData(-0.1, 0.1, 0.2, 0.2)]
    [InlineData(0.1, -0.1, 0.2, 0.2)]
    [InlineData(0.1, 0.1, 0.0, 0.2)]
    [InlineData(0.1, 0.1, 0.2, 0.0)]
    [InlineData(0.9, 0.1, 0.2, 0.2)]
    [InlineData(0.1, 0.9, 0.2, 0.2)]
    [InlineData(double.NaN, 0.1, 0.2, 0.2)]
    [InlineData(0.1, double.PositiveInfinity, 0.2, 0.2)]
    public void Validate_WithInvalidRegion_ShouldThrow(
        double x, double y, double width, double height)
    {
        var options = Valid() with
        {
            Region = new NormalizedScreenRegion(x, y, width, height)
        };
        Action act = () => options.Validate();
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*normalized screen region must be nonempty and within [0, 1]*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithMissingTarget_ShouldThrow(string? target)
    {
        var options = Valid() with { TargetText = target! };
        Action act = () => options.Validate();
        act.Should().Throw<ArgumentException>()
            .WithMessage("*TargetText*");
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Validate_WithInvalidDifferenceRatio_ShouldThrow(double ratio)
    {
        var options = Valid() with { MaximumDifferenceRatio = ratio };
        Action act = () => options.Validate();
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Maximum difference ratio must be between 0 and 1*");
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Validate_WithInvalidCenterMovement_ShouldThrow(double movement)
    {
        var options = Valid() with { MaximumCenterMovement = movement };
        Action act = () => options.Validate();
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Maximum center movement must be between 0 and 1*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithNonPositiveRequiredMatches_ShouldThrow(int count)
    {
        var options = Valid() with { RequiredConsecutiveMatches = count };
        Action act = () => options.Validate();
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Required consecutive matches must be positive*");
    }

    [Fact]
    public void Validate_WithNegativeSampleInterval_ShouldThrow()
    {
        var options = Valid() with { SampleInterval = TimeSpan.FromTicks(-1) };
        Action act = () => options.Validate();
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Sample interval cannot be negative*");
    }

    [Fact]
    public void Validate_WithZeroSampleInterval_ShouldSucceed()
    {
        var options = Valid() with { SampleInterval = TimeSpan.Zero };
        Action act = () => options.Validate();
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithNonPositiveTimeout_ShouldThrow(long ticks)
    {
        var options = Valid() with { Timeout = TimeSpan.FromTicks(ticks) };
        Action act = () => options.Validate();
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Observation timeout must be positive*");
    }

    [Fact]
    public void Validate_WithBoundaryToleranceValues_ShouldSucceed()
    {
        var options = Valid() with
        {
            MaximumDifferenceRatio = 1,
            MaximumCenterMovement = 0,
            RequiredConsecutiveMatches = 1
        };
        Action act = () => options.Validate();
        act.Should().NotThrow();
    }
}
