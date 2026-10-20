using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OmniAppium.ConfigUtilityServices.Models;
using OmniAppium.EngineUtilityService.Utilities;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppiumDemo.DependencyInjection;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue48ClickBypassBoundaryTests
{
    [Fact]
    public async Task TrustedConfigurationClickJob_ShouldReachDeviceInput()
    {
        var deviceInput = new Mock<IClickService>(MockBehavior.Strict);
        var clickJob = new ClickJob
        {
            Position = new Point { X = 100, Y = 200 }
        };

        deviceInput
            .Setup(service => service.Click(clickJob));

        var handler = new ClickJobHandler(deviceInput.Object);

        handler.CanHandle(clickJob).Should().BeTrue();

        await handler.AutoExecuteAsync(clickJob);

        deviceInput.Verify(
            service => service.Click(clickJob),
            Times.Once);

        deviceInput.VerifyNoOtherCalls();
    }

    [Fact]
    public void ProductionPlannerClick_ShouldRejectBeforeDeviceInput()
    {
        var deviceInput = new Mock<IClickService>(MockBehavior.Strict);
        var services = new ServiceCollection();

        services.AddSingleton(deviceInput.Object);
        services.AddProductionClickSecurity();

        using var provider = services.BuildServiceProvider(
            validateScopes: true);

        var planner = provider.GetRequiredService<
            IOcrGroundedPlannerActionExecutor>();

        var snapshot = new OcrGroundedSnapshot(
            "untrusted-snapshot",
            Array.Empty<OcrGroundedTarget>());

        Action act = () => planner.Click(snapshot, "untrusted-target");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "Trusted signed review approval is required for production grounded clicks.");

        deviceInput.VerifyNoOtherCalls();
    }

    [Fact]
    public void ProductionLegacyOcrClick_ShouldRejectBeforeDeviceInput()
    {
        var deviceInput = new Mock<IClickService>(MockBehavior.Strict);
        var services = new ServiceCollection();

        services.AddSingleton(deviceInput.Object);
        services.AddAndroidOcrServices();
        services.AddProductionClickSecurity();

        using var provider = services.BuildServiceProvider(
            validateScopes: true);

        Action act = () =>
            provider.GetRequiredService<IOcrClickService>();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "Legacy OCR click is disabled in production. Signed trusted-review authorization is required.");

        deviceInput.VerifyNoOtherCalls();
    }

    [Fact]
    public void ProductionGroundedClickRegistrations_ShouldContainOnlyRejectingService()
    {
        var services = new ServiceCollection();

        services.AddProductionClickSecurity();

        using var provider = services.BuildServiceProvider(
            validateScopes: true);

        provider.GetServices<IOcrGroundedClickService>()
            .Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<RejectingProductionGroundedClickService>();
    }
}
