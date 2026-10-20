using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.AiSecurity;

public sealed class AiClickExecutionPolicyTests
{
    [Fact]
    public void ProtectedProduction_ShouldBeAllowed()
    {
        Action act = () => AiClickExecutionPolicy.Validate(
            AiClickExecutionMode.Protected, isProduction: true);
        act.Should().NotThrow();
    }

    [Fact]
    public void DirectProduction_ShouldBeRejected()
    {
        Action act = () => AiClickExecutionPolicy.Validate(
            AiClickExecutionMode.Direct, isProduction: true);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*prohibited in Production*");
    }

    [Fact]
    public void UnsupportedMode_ShouldBeRejected()
    {
        Action act = () => AiClickExecutionPolicy.Validate(
            (AiClickExecutionMode)12345, isProduction: false);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Unsupported AI click execution mode*");
    }

    [Fact]
    public void ExplicitDirectDevelopment_ShouldBeAllowed()
    {
        Action act = () => AiClickExecutionPolicy.Validate(
            AiClickExecutionMode.Direct, isProduction: false);
        act.Should().NotThrow();
    }
}
