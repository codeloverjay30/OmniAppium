using AiUtility.GeminiKits.Abstractions;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppiumDemo.DependencyInjection;

namespace OmniAppium.EngineUtilityServices.Tests.Services.AiSecurity;

public sealed class GeminiClickToolRegistryIntegrationTests
{
    [Fact]
    public void Protected_WithoutTrustedPlannerConfiguration_ShouldNotRegisterAnyClickTool()
    {
        var registry = new Mock<IGeminiToolRegistry>(MockBehavior.Strict);

        Action act = () => registry.Object.RegisterAiClickTools(
            AiClickExecutionMode.Protected,
            isProduction: true,
            () => throw new InvalidOperationException("Direct click factory must not be invoked."),
            () => throw new InvalidOperationException("Protected click factory must not be invoked."),
            isTrustedPlannerConfigured: false);

        act.Should().NotThrow();
        registry.Verify(
            x => x.Register<OcrGroundedPlannerClickTool>(
                It.IsAny<Func<OcrGroundedPlannerClickTool>>()),
            Times.Never);
        registry.Verify(
            x => x.Register<ClickService>(
                It.IsAny<Func<ClickService>>()),
            Times.Never);
        registry.VerifyNoOtherCalls();
    }

    [Fact]
    public void Protected_RegistersPlannerOnly()
    {
        var registry = new Mock<IGeminiToolRegistry>(MockBehavior.Loose);

        registry.Object.RegisterAiClickTools(
            AiClickExecutionMode.Protected,
            isProduction: true,
            () => null!,
            () => null!);

        registry.Verify(
            x => x.Register<OcrGroundedPlannerClickTool>(
                It.IsAny<Func<OcrGroundedPlannerClickTool>>()),
            Times.Once);
        registry.Verify(
            x => x.Register<ClickService>(It.IsAny<Func<ClickService>>()),
            Times.Never);
    }

    [Fact]
    public void ProductionDirect_RejectsBeforeAnyRegistration()
    {
        var registry = new Mock<IGeminiToolRegistry>(MockBehavior.Loose);

        Action act = () => registry.Object.RegisterAiClickTools(
            AiClickExecutionMode.Direct,
            isProduction: true,
            () => null!,
            () => null!);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*prohibited in Production*");

        registry.Verify(
            x => x.Register<ClickService>(It.IsAny<Func<ClickService>>()),
            Times.Never);
        registry.Verify(
            x => x.Register<OcrGroundedPlannerClickTool>(
                It.IsAny<Func<OcrGroundedPlannerClickTool>>()),
            Times.Never);
    }

    [Fact]
    public void DevelopmentDirect_RegistersLegacyClickOnly()
    {
        var registry = new Mock<IGeminiToolRegistry>(MockBehavior.Loose);

        registry.Object.RegisterAiClickTools(
            AiClickExecutionMode.Direct,
            isProduction: false,
            () => null!,
            () => null!);

        registry.Verify(
            x => x.Register<ClickService>(It.IsAny<Func<ClickService>>()),
            Times.Once);
        registry.Verify(
            x => x.Register<OcrGroundedPlannerClickTool>(
                It.IsAny<Func<OcrGroundedPlannerClickTool>>()),
            Times.Never);
    }
}