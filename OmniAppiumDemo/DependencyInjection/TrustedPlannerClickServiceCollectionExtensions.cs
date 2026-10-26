using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Registers the trusted planner adapter only when external approval provisioning is configured.
/// </summary>
public static class TrustedPlannerClickServiceCollectionExtensions
{
    /// <summary>
    /// Registers the externally provisioned request provider and trusted click pipeline.
    /// The caller must separately register IFileSystem, IClickService, and the snapshot freshness guard.
    /// </summary>
    public static IServiceCollection AddTrustedPlannerClick(
        this IServiceCollection services,
        IEnumerable<TrustedReviewerKey> trustedKeys,
        string claimDirectory,
        Func<IServiceProvider, IPlannerTrustedClickRequestProvider> providerFactory)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(trustedKeys);
        ArgumentException.ThrowIfNullOrWhiteSpace(claimDirectory);
        ArgumentNullException.ThrowIfNull(providerFactory);

        TrustedReviewerKey[] keys = trustedKeys.ToArray();
        if (keys.Length == 0)
            throw new InvalidOperationException("Trusted reviewer keys are required.");

        // Preserve the existing trusted verification and single-use claim gate.
        services.TryAddSingleton(new Issue54ClaimDirectory(claimDirectory));
        services.AddTrustedGroundedClick(keys);

        // Never silently replace the caller's independently provisioned approval source.
        if (services.Any(d => d.ServiceType == typeof(IPlannerTrustedClickRequestProvider)))
            throw new InvalidOperationException("Planner approval provider is already registered.");

        services.AddSingleton<IPlannerTrustedClickRequestProvider>(providerFactory);
        services.AddTransient<TrustedPlannerClickExecutor>();
        return services;
    }
}
