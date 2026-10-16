using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppiumDemo.DependencyInjection;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class GroundedClickEvidenceProviderBoundaryTests
{
    [Fact]
    public void RejectingProvider_WhenGroundedEvidenceIsMissing_ShouldFailClosed()
    {
        var provider = new RejectingGroundedClickEvidenceProvider();
        var target = new OcrGroundedTarget(
            "target-1", "Play", CoordinateUtilityServices.Rectangle.FromXYWH(10, 20, 30, 40));
        var snapshot = new OcrGroundedSnapshot("frame-1", [target]);

        Action act = () => provider.Resolve(snapshot, target);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Reviewed grounded click evidence is unavailable*");
    }

    [Fact]
    public void AddOcrUtilityServices_WhenEvidenceProviderIsNotConfigured_ShouldRegisterFailClosedProvider()
    {
        var services = new ServiceCollection();
        services.AddOcrUtilityServices();

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IGroundedClickEvidenceProvider>()
            .Should().BeOfType<RejectingGroundedClickEvidenceProvider>();
    }

    [Fact]
    public void AddOcrUtilityServices_WhenEvidenceProviderIsAlreadyConfigured_ShouldPreserveIt()
    {
        var evidence = new Mock<IGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var services = new ServiceCollection();
        services.AddSingleton(evidence.Object);
        services.AddOcrUtilityServices();

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<IGroundedClickEvidenceProvider>()
            .Should().BeSameAs(evidence.Object);
    }
}
