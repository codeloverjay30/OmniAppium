using System.IO.Abstractions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.DependencyInjection;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Workflows;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Provides dependency injection registrations for Android OCR
/// workflow services.
/// </summary>
public static class AndroidOcrServiceCollectionExtensions
{
    /// <summary>
    /// Registers Android OCR and OCR-driven workflow services.
    /// </summary>
    /// <param name="services">
    /// The service collection to configure.
    /// </param>
    /// <returns>
    /// The configured service collection.
    /// </returns>
    public static IServiceCollection AddAndroidOcrServices(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IFileSystem, FileSystem>();

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

        services.AddSingleton<TimeProvider>(
            TimeProvider.System);

        services.AddSingleton<
            IGameWorkflowStepExecutor,
            GameWorkflowStepExecutor>();

        return services;
    }
}