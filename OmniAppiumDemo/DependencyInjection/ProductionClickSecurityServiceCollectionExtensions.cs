using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Configures the production fail-closed boundary for legacy OCR clicks.
/// </summary>
public static class ProductionClickSecurityServiceCollectionExtensions
{
    /// <summary>
    /// Replaces legacy OCR click registrations with production-safe registrations.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddProductionClickSecurity(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.RemoveAll<IOcrGroundedClickService>();
        services.AddSingleton<
            IOcrGroundedClickService,
            RejectingProductionGroundedClickService>();

        services.RemoveAll<IOcrClickService>();
        services.AddSingleton<IOcrClickService>(_ =>
            throw new InvalidOperationException(
                "Legacy OCR click is disabled in production. " +
                "Signed trusted-review authorization is required."));

        services.RemoveAll<IOcrGroundedPlannerActionExecutor>();
        services.AddSingleton<
            IOcrGroundedPlannerActionExecutor,
            OcrGroundedPlannerActionExecutor>();

        return services;
    }
}