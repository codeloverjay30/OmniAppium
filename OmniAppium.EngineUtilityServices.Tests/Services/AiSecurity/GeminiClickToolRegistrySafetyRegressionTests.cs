using AiUtility.GeminiKits.Abstractions;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppiumDemo.DependencyInjection;

namespace OmniAppium.EngineUtilityServices.Tests.Services.AiSecurity;

public sealed class GeminiClickToolRegistrySafetyRegressionTests
{
    [Fact]
    public void Protected_WithTrustedPlanner_RegistersOnlyProtectedClickTool()
    {
        var registry = new Mock<IGeminiToolRegistry>(MockBehavior.Strict);
        registry.Setup(x => x.Register<OcrGroundedPlannerClickTool>(
            It.IsAny<Func<OcrGroundedPlannerClickTool>>()));

        Action act = () => registry.Object.RegisterAiClickTools(
            AiClickExecutionMode.Protected,
            isProduction: true,
            () => throw new InvalidOperationException("Direct click factory invoked."),
            () => throw new InvalidOperationException("Protected click factory invoked."),
            isTrustedPlannerConfigured: true);

        act.Should().NotThrow();
        registry.Verify(x => x.Register<OcrGroundedPlannerClickTool>(
            It.IsAny<Func<OcrGroundedPlannerClickTool>>()), Times.Once);
        registry.Verify(x => x.Register<ClickService>(
            It.IsAny<Func<ClickService>>()), Times.Never);
        registry.VerifyNoOtherCalls();
    }

    [Fact]
    public void Protected_WithoutTrustedPlanner_DoesNotRegisterClickToolsOrInvokeFactories()
    {
        var registry = new Mock<IGeminiToolRegistry>(MockBehavior.Strict);
        var directFactoryInvocations = 0;
        var protectedFactoryInvocations = 0;

        Action act = () => registry.Object.RegisterAiClickTools(
            AiClickExecutionMode.Protected,
            isProduction: true,
            () =>
            {
                directFactoryInvocations++;
                throw new InvalidOperationException("Direct click factory invoked.");
            },
            () =>
            {
                protectedFactoryInvocations++;
                throw new InvalidOperationException("Protected click factory invoked.");
            },
            isTrustedPlannerConfigured: false);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*requires a trusted planner configuration*");
        directFactoryInvocations.Should().Be(0);
        protectedFactoryInvocations.Should().Be(0);
        registry.VerifyNoOtherCalls();
    }

    [Fact]
    public void ProductionDirect_ThrowsBeforeRegisteringAnyClickTool()
    {
        var registry = new Mock<IGeminiToolRegistry>(MockBehavior.Strict);

        Action act = () => registry.Object.RegisterAiClickTools(
            AiClickExecutionMode.Direct,
            isProduction: true,
            () => throw new InvalidOperationException("Direct click factory invoked."),
            () => throw new InvalidOperationException("Protected click factory invoked."),
            isTrustedPlannerConfigured: true);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*prohibited in Production*");
        registry.VerifyNoOtherCalls();
    }

    [Fact]
    public void UnsupportedExecutionMode_ThrowsBeforeRegisteringAnyClickTool()
    {
        var registry = new Mock<IGeminiToolRegistry>(MockBehavior.Strict);
        var unsupportedMode = (AiClickExecutionMode)int.MaxValue;

        Action act = () => registry.Object.RegisterAiClickTools(
            unsupportedMode,
            isProduction: false,
            () => throw new InvalidOperationException("Direct click factory invoked."),
            () => throw new InvalidOperationException("Protected click factory invoked."),
            isTrustedPlannerConfigured: true);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Unsupported AI click execution mode*");
        registry.VerifyNoOtherCalls();
    }

    [Fact]
    public void NullRegistry_ThrowsArgumentNullException()
    {
        IGeminiToolRegistry registry = null!;

        Action act = () => registry.RegisterAiClickTools(
            AiClickExecutionMode.Protected,
            isProduction: true,
            () => null!,
            () => null!,
            isTrustedPlannerConfigured: true);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*registry*");
    }
}