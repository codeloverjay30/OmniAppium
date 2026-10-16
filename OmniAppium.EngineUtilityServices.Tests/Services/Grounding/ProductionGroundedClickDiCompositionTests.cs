using FluentAssertions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppiumDemo.DependencyInjection;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class ProductionGroundedClickDiCompositionTests
{
    [Fact]
    public void ProductionRegistration_ShouldResolveReviewedGeometryAuthorizationAndBothClickServices()
    {
        using ServiceProvider provider = CreateProvider();

        provider.GetRequiredService<ReviewedPaddedTextGeometryPolicy>()
            .Should().BeOfType<ReviewedPaddedTextGeometryPolicy>();
        provider.GetRequiredService<IGroundedClickValidator>()
            .Should().BeOfType<GroundedClickValidator>();
        provider.GetRequiredService<IGroundedClickAuthorizationService>()
            .Should().BeOfType<GroundedClickAuthorizationService>();
        provider.GetRequiredService<IOcrClickService>()
            .Should().BeOfType<OcrClickService>();
        provider.GetRequiredService<IOcrGroundedClickService>()
            .Should().BeOfType<OcrGroundedClickService>();
    }

    [Fact]
    public void ProductionRegistration_WithoutReviewedEvidence_ShouldUseRejectingProvider()
    {
        using ServiceProvider provider = CreateProvider();

        IGroundedClickEvidenceProvider evidence =
            provider.GetRequiredService<IGroundedClickEvidenceProvider>();

        evidence.Should().BeOfType<RejectingGroundedClickEvidenceProvider>();
        (evidence is IAtomicGroundedClickEvidenceProvider).Should().BeFalse();
    }

    [Fact]
    public void ProductionRegistration_WithExplicitAtomicEvidence_ShouldPreserveReviewedProvider()
    {
        var evidence = new Mock<IAtomicGroundedClickEvidenceProvider>(MockBehavior.Strict);
        using ServiceProvider provider = CreateProvider(evidence.Object);

        provider.GetRequiredService<IGroundedClickEvidenceProvider>()
            .Should().BeSameAs(evidence.Object);
        provider.GetRequiredService<IOcrClickService>()
            .Should().BeOfType<OcrClickService>();
        provider.GetRequiredService<IOcrGroundedClickService>()
            .Should().BeOfType<OcrGroundedClickService>();
    }

    private static ServiceProvider CreateProvider(
        IAtomicGroundedClickEvidenceProvider? evidence = null)
    {
        var services = new ServiceCollection();

        if (evidence is not null)
        {
            services.AddSingleton<IGroundedClickEvidenceProvider>(evidence);
        }

        services.AddOcrUtilityServices();
        services.AddAndroidOcrServices();

        services.AddSingleton<IScreenshotService>(
            new Mock<IScreenshotService>(MockBehavior.Strict).Object);

        services.AddSingleton(
            new Mock<IClickService>(MockBehavior.Strict).Object);
        services.AddSingleton(
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict).Object);

        var loggerFactory = new Mock<ILoggerFactoryBaseUtilityService>(MockBehavior.Strict);
        loggerFactory.SetupGet(factory => factory.Logger)
            .Returns(new Mock<ILogger>(MockBehavior.Loose).Object);
        services.AddSingleton(loggerFactory.Object);

        services.AddSingleton<IOcrGroundedClickService, OcrGroundedClickService>();

        return services.BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });
    }
}
