using System.Reflection;
using AiUtility.GeminiKits.Security;
using FluentAssertions;
using OmniAppium.EngineUtilityService.Services.Wait;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppiumDemo.DependencyInjection;

namespace OmniAppium.EngineUtilityServices.Tests.Services.AiSecurity;

public sealed class GeminiClickExecutionPolicyFactoryTests
{
    [Fact]
    public void Create_ProductionProtected_ShouldAuthorizePlannerClick()
    {
        GeminiToolExecutionPolicy policy = GeminiClickExecutionPolicyFactory.Create(AiClickExecutionMode.Protected, true);
        MethodInfo method = typeof(OcrGroundedPlannerClickTool).GetMethod(nameof(OcrGroundedPlannerClickTool.Click))!;

        Action act = () => policy.EnsureAllowed(method);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(nameof(ClickService.ClickAbsolute))]
    [InlineData(nameof(ClickService.ClickScaled))]
    public void Create_ProductionProtected_ShouldRejectDirectClickBeforeInvocation(string methodName)
    {
        GeminiToolExecutionPolicy policy = GeminiClickExecutionPolicyFactory.Create(AiClickExecutionMode.Protected, true);
        MethodInfo method = typeof(ClickService).GetMethods().First(m => m.Name == methodName);

        Action act = () => policy.EnsureAllowed(method);

        act.Should().Throw<InvalidOperationException>().WithMessage("*Protected policy*");
    }

    [Fact]
    public void Create_ProductionProtected_ShouldAllowRegisteredWaitMethod()
    {
        GeminiToolExecutionPolicy policy = GeminiClickExecutionPolicyFactory.Create(AiClickExecutionMode.Protected, true);
        MethodInfo method = typeof(WaitService).GetMethod(nameof(WaitService.Wait), new[] { typeof(int) })!;

        Action act = () => policy.EnsureAllowed(method);

        act.Should().NotThrow();
    }

    [Fact]
    public void Create_ProductionDirect_ShouldRejectStartup()
    {
        Action act = () => GeminiClickExecutionPolicyFactory.Create(AiClickExecutionMode.Direct, true);

        act.Should().Throw<InvalidOperationException>().WithMessage("*Production*");
    }
}
