using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppiumDemo.DependencyInjection;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue48ProductionCompositionRootIntegrationTests
{
    [Fact]
    public void ProductionComposition_ShouldReplaceLegacyGroundedClick()
    {
        var services = new ServiceCollection();

        services.AddSingleton<
            IOcrGroundedClickService,
            OcrGroundedClickService>();

        services.AddProductionClickSecurity();

        using var provider = services.BuildServiceProvider();

        var registered = provider
            .GetServices<IOcrGroundedClickService>()
            .ToArray();

        registered.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<RejectingProductionGroundedClickService>();
    }

    [Fact]
    public void ProductionComposition_ShouldRejectLegacyOcrClick()
    {
        var services = new ServiceCollection();

        services.AddAndroidOcrServices();
        services.AddProductionClickSecurity();

        using var provider = services.BuildServiceProvider();

        Action act = () =>
            provider.GetRequiredService<IOcrClickService>();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Signed trusted-review authorization is required*");
    }

    [Fact]
    public void ProductionPlanner_ShouldRejectUnsignedClick()
    {
        var services = new ServiceCollection();

        services.AddProductionClickSecurity();

        using var provider = services.BuildServiceProvider();

        var planner = provider.GetRequiredService<
            IOcrGroundedPlannerActionExecutor>();

        var snapshot = new OcrGroundedSnapshot("snapshot", []);

        Action act = () => planner.Click(snapshot, "target");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Trusted signed review approval is required*");
    }

    [Fact]
    public void ProductionComposition_ShouldNotExposeLegacyGroundedClick()
    {
        var services = new ServiceCollection();

        services.AddProductionClickSecurity();

        using var provider = services.BuildServiceProvider();

        provider.GetServices<IOcrGroundedClickService>()
            .Should()
            .NotContain(service =>
                service is OcrGroundedClickService);
    }
}