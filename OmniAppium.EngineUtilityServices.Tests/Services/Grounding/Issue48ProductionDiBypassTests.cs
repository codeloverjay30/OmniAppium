using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue48ProductionDiBypassTests
{
    [Fact]
    public void ProductionGroundedClick_ShouldUseRejectingService()
    {
        var services = new ServiceCollection();

        services.AddSingleton<
            IOcrGroundedClickService,
            RejectingProductionGroundedClickService>();

        using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IOcrGroundedClickService>()
            .Should()
            .BeOfType<RejectingProductionGroundedClickService>();
    }

    [Fact]
    public void LegacyOcrClick_ShouldFailClosed()
    {
        var services = new ServiceCollection();

        services.RemoveAll<IOcrClickService>();

        services.AddSingleton<IOcrClickService>(_ =>
            throw new InvalidOperationException(
                "Legacy OCR click is disabled in production. " +
                "Signed trusted-review authorization is required."));

        using var provider = services.BuildServiceProvider();

        Action act = () =>
            provider.GetRequiredService<IOcrClickService>();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Signed trusted-review authorization is required*");
    }
}