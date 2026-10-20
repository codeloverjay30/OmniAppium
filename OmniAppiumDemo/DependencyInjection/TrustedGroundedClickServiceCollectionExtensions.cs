using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Registers the signed-review-only grounded-click device input boundary.
/// </summary>
public static class TrustedGroundedClickServiceCollectionExtensions
{
    /// <summary>
    /// Registers a trusted authorization pipeline using externally provisioned reviewer keys.
    /// </summary>
    public static IServiceCollection AddTrustedGroundedClick(
        this IServiceCollection services, IEnumerable<TrustedReviewerKey> trustedKeys)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(trustedKeys);
        TrustedReviewerKey[] keys = trustedKeys.ToArray();
        if (keys.Length == 0 || keys.Any(k => k is null ||
            string.IsNullOrWhiteSpace(k.ReviewerId) ||
            string.IsNullOrWhiteSpace(k.KeyId) ||
            string.IsNullOrWhiteSpace(k.PublicKeyPem)))
            throw new InvalidOperationException("Trusted reviewer key configuration is invalid.");

        services.AddSingleton<ITrustedReviewIntegrityAuthorizationService>(sp =>
            TrustedGroundedClickAuthorizationFactory.Create(
                sp.GetRequiredService<IFileSystem>(), keys,
                sp.GetService<TimeProvider>()));
        services.AddSingleton<ITrustedGroundedClickService, TrustedGroundedClickService>();
        return services;
    }
}
