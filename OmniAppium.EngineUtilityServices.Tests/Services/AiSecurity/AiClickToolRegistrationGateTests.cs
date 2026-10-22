using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityServices.Tests.Services.AiSecurity;

public sealed class AiClickToolRegistrationGateTests
{
    [Theory]
    [InlineData(AiClickExecutionMode.Protected, typeof(ClickService), "Direct AI click tool*")]
    [InlineData(AiClickExecutionMode.Direct, typeof(OcrGroundedPlannerClickTool), "Protected AI planner tool*")]
    public void EnsureAllowed_RejectsCrossModeTool(AiClickExecutionMode mode, Type type, string message)
    {
        var gate = new AiClickToolRegistrationGate(mode, false);
        Action act = () => gate.EnsureAllowed(type);
        act.Should().Throw<InvalidOperationException>().WithMessage(message);
    }

    [Fact]
    public void Constructor_RejectsProductionDirect()
    {
        Action act = () => new AiClickToolRegistrationGate(AiClickExecutionMode.Direct, true);
        act.Should().Throw<InvalidOperationException>().WithMessage("*prohibited in Production*");
    }

    [Fact]
    public void EnsureAllowed_AllowsProtectedPlanner()
    {
        var gate = new AiClickToolRegistrationGate(AiClickExecutionMode.Protected, true);
        Action act = () => gate.EnsureAllowed(typeof(OcrGroundedPlannerClickTool));
        act.Should().NotThrow();
    }
}
