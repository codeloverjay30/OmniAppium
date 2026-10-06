using LoggerFactoryUtilityServices;
using Microsoft.Extensions.DependencyInjection;
using OmniAppium.EngineUtilityService.Services.Screenshots;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Provides dependency injection registrations for runtime-owned
/// OmniAppium services.
/// </summary>
public static class OmniAppiumRuntimeServiceCollectionExtensions
{
    /// <summary>
    /// Registers runtime-owned OmniAppium service instances.
    /// </summary>
    /// <param name="services">
    /// The service collection to which the runtime services are registered.
    /// </param>
    /// <param name="loggerFactoryService">
    /// The application logging service created by the composition root.
    /// </param>
    /// <param name="screenshotService">
    /// The screenshot service bound to the active Appium driver.
    /// </param>
    /// <returns>
    /// The same service collection for registration chaining.
    /// </returns>
    public static IServiceCollection AddOmniAppiumRuntime(
        this IServiceCollection services,
        ILoggerFactoryBaseUtilityService loggerFactoryService,
        IScreenshotService screenshotService)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(loggerFactoryService);
        ArgumentNullException.ThrowIfNull(screenshotService);

        services.AddSingleton(
            loggerFactoryService);

        services.AddSingleton(
            screenshotService);

        return services;
    }
}