using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Workflows;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Provides dependency injection registrations for OmniAppium OCR services.
/// </summary>
public static class OmniAppiumOcrServiceCollectionExtensions
{
    /// <summary>
    /// Registers the OmniAppium OCR services required by the Android
    /// automation runtime.
    /// </summary>
    /// <param name="services">
    /// The service collection to which the OmniAppium OCR services
    /// are registered.
    /// </param>
    /// <returns>
    /// The same service collection for registration chaining.
    /// </returns>
    public static IServiceCollection AddOmniAppiumOcrServices(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<
            IFileSystem,
            FileSystem>();

        services.AddSingleton(
            TimeProvider.System);

        services.AddSingleton<
            IOcrDiagnosticImageWriter,
            OcrDiagnosticImageWriter>();

        services.AddSingleton<
            IAndroidScreenOcrService,
            AndroidScreenOcrService>();

        services.AddSingleton<
            IOcrClickService,
            OcrClickService>();

        services.AddSingleton<
            IOcrPageVerificationService,
            OcrPageVerificationService>();

        services.AddSingleton<
            IGameWorkflowStepExecutor,
            GameWorkflowStepExecutor>();

        return services;
    }
}