using FluentAssertions;
using OmniAppium.EngineUtilityServices.Models.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Stability;

public sealed class ScreenStabilityOptionsTests
{
    [Fact]
    public void Validate_WhenValuesAreValid_ShouldNotThrow()
    {
        var options = Valid();
        Action act = options.Validate;
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenRequiredMatchesNotPositive_ShouldThrow(int count)
    {
        var options = Valid() with { RequiredConsecutiveMatches = count };
        Action act = options.Validate;
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*RequiredConsecutiveMatches*");
    }

    [Fact]
    public void Validate_WhenIntervalIsZero_ShouldThrow()
    {
        var options = Valid() with { SampleInterval = TimeSpan.Zero };
        Action act = options.Validate;
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*SampleInterval*");
    }

    [Fact]
    public void Validate_WhenTimeoutIsNegative_ShouldThrow()
    {
        var options = Valid() with { Timeout = TimeSpan.FromSeconds(-1) };
        Action act = options.Validate;
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Timeout*");
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(double.NaN)]
    [InlineData(double.NegativeInfinity)]
    public void Validate_WhenDifferenceRatioInvalid_ShouldThrow(double ratio)
    {
        var options = Valid() with { MaximumDifferenceRatio = ratio };
        Action act = options.Validate;
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*MaximumDifferenceRatio*");
    }

    private static ScreenStabilityOptions Valid() => new()
    {
        RequiredConsecutiveMatches = 2,
        SampleInterval = TimeSpan.FromMilliseconds(20),
        Timeout = TimeSpan.FromSeconds(2),
        MaximumDifferenceRatio = 0.01
    };
}
