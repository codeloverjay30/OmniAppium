using OCRUtilityServices.Services;
using FluentAssertions;
using Moq;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using AiUtility.ToolKits.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppiumDemo.DependencyInjection;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests;

public sealed class Issue58PlannerCompositionCandidateTests
{
    [Fact]
    public void BuildWithoutApproval_ShouldNotRegisterOptionalPlannerGraph()
    {
        var services = new ServiceCollection();

        using var provider = services.BuildServiceProvider(
            new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        provider.GetService<IPlannerTrustedClickRequestProvider>().Should().BeNull();
        provider.GetService<TrustedPlannerClickExecutor>().Should().BeNull();
        provider.GetService<OcrGroundedPlannerClickTool>().Should().BeNull();
    }

    [Fact]
    public void AddTrustedPlannerClick_WithValidConfiguration_ShouldResolveCompletePlannerDependencyGraph()
    {
        var services = new ServiceCollection();
        var fileSystem = new MockFileSystem();
        var providerCreated = false;
        var claimDirectory = fileSystem.Path.Combine(
            fileSystem.Path.GetTempPath(), "issue58-claims");
        fileSystem.Directory.CreateDirectory(claimDirectory);

        services.AddSingleton<IFileSystem>(fileSystem);
        services.AddSingleton(new Mock<IClickService>(MockBehavior.Strict).Object);
        services.AddSingleton(
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict).Object);
        services.AddSingleton(
            new Mock<IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict).Object);

        var keys = new[]
        {
            new TrustedReviewerKey(
                ReviewerId: "issue58-test-reviewer",
                KeyId: "issue58-test-key",
                PublicKeyPem: "test-only-not-used-for-cryptographic-verification")
        };

        services.AddTrustedPlannerClick(
            keys,
            claimDirectory,
            sp =>
            {
                providerCreated = true;
                return new JsonPlannerTrustedClickRequestProvider(
                    sp.GetRequiredService<IFileSystem>(),
                    fileSystem.Path.Combine(fileSystem.Path.GetTempPath(), "issue58-approval.json"));
            });

        using var provider = services.BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

        provider.GetRequiredService<IPlannerTrustedClickRequestProvider>()
            .Should().BeOfType<JsonPlannerTrustedClickRequestProvider>();
        provider.GetRequiredService<TrustedPlannerClickExecutor>()
            .Should().NotBeNull();
        provider.GetRequiredService<OcrGroundedPlannerClickTool>()
            .Should().NotBeNull();
        providerCreated.Should().BeTrue();
    }

    [Fact]
    public void AddTrustedPlannerClick_WithNoKeys_ShouldRejectConfiguration()
    {
        var services = new ServiceCollection();

        Action act = () => services.AddTrustedPlannerClick(
            Array.Empty<TrustedReviewerKey>(),
            "claims",
            _ => throw new InvalidOperationException("Provider must not be invoked."));

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Trusted reviewer keys are required*");
    }

    [Fact]
    public void AddTrustedPlannerClick_WithExistingProvider_ShouldRejectDuplicateProvisioning()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IPlannerTrustedClickRequestProvider>(
            _ => throw new InvalidOperationException("Existing provider must not be invoked."));

        var keys = new[]
        {
            new TrustedReviewerKey(
                ReviewerId: "issue58-test-reviewer",
                KeyId: "issue58-test-key",
                PublicKeyPem: "test-only-not-used-for-cryptographic-verification")
        };

        Action act = () => services.AddTrustedPlannerClick(
            keys,
            "claims",
            _ => throw new InvalidOperationException("New provider must not be invoked."));

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Planner approval provider is already registered*");

        services.Count(x => x.ServiceType == typeof(IPlannerTrustedClickRequestProvider))
            .Should().Be(1);
    }
}
