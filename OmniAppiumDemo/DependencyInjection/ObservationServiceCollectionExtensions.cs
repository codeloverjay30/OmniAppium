using Microsoft.Extensions.DependencyInjection;
using OmniAppium.EngineUtilityServices.Services.Observation;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Provides dependency injection registrations for Android screen observation services.
/// </summary>
public static class ObservationServiceCollectionExtensions
{
    /// <summary>
    /// Registers Android screen observation services.
    /// </summary>
    /// <param name="services">
    /// The service collection to which the services are registered.
    /// </param>
    /// <returns>
    /// The same service collection for registration chaining.
    /// </returns>
    public static IServiceCollection AddAndroidScreenObservation(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<
            IAndroidScreenObservationService,
            AndroidScreenObservationService>();

        return services;
    }
}