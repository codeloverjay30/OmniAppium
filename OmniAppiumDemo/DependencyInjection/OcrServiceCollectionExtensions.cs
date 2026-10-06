using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.OCR;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Provides dependency injection registrations for OCR services.
/// </summary>
public static class OcrServiceCollectionExtensions
{
    /// Registers the core OCR utility services required by the application.
    /// </summary>
    /// <param name="services">
    /// The service collection to which the OCR services are registered.
    /// </param>
    /// <returns>
    /// The same service collection for registration chaining.
    /// </returns>
    public static IServiceCollection AddOcrUtilityServices(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<
            IOCRUtilityService,
            OCRUtilityService>();

        services.AddSingleton<
            IOcrTextMatcher,
            OcrTextMatcher>();

        return services;
    }
}