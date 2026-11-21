using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.IO.Abstractions;
using OCRUtilityServices.Paddle.Python;
using OCRUtilityServices.Services;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>Enables the Python OCR implementation only when explicitly selected.</summary>
public static class PythonOcrServiceCollectionExtensions
{
    /// <summary>Replaces the OCR implementation while retaining existing click security registrations.</summary>
    public static IServiceCollection UseIssue59PythonOcr(
        this IServiceCollection services, PythonOcrProcessOptions options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);
        services.TryAddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton(options);
        services.AddSingleton<IPythonOcrProcessRunner, PythonOcrProcessRunner>();
        services.Replace(ServiceDescriptor.Singleton<IOCRUtilityService, PythonOcrUtilityService>());
        return services;
    }
}
