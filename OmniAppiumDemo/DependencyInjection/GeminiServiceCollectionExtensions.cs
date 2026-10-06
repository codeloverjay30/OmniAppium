using AiUtility.AiBaseUtilityServices.Models;
using Microsoft.Extensions.DependencyInjection;
using OmniAppium.EngineUtilityService.Utilities;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Provides dependency injection registrations for Gemini automation services.
/// </summary>
public static class GeminiServiceCollectionExtensions
{
    /// <summary>
    /// Registers Gemini automation services.
    /// </summary>
    /// <param name="services">
    /// The service collection to which the services are registered.
    /// </param>
    /// <returns>
    /// The same service collection for registration chaining.
    /// </returns>
    public static IServiceCollection AddGeminiAutomation(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Gemini registrations...

        services.AddSingleton<
            IGeminiJobHandler,
            GeminiJobHandler<WorkflowProgress>>();

        return services;
    }
}