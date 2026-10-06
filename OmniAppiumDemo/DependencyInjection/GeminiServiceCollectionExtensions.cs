using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using CommonModels;
using Microsoft.Extensions.DependencyInjection;
using OmniAppium.EngineUtilityService.Utilities;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Provides dependency injection registrations for Gemini automation services.
/// </summary>
public static class GeminiServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Gemini automation dependencies required by
    /// <see cref="GeminiJobHandler{TProgress}"/>.
    /// </summary>
    /// <param name="services">
    /// The service collection to configure.
    /// </param>
    /// <param name="aiExecutionSettings">
    /// The AI execution settings used by the Gemini job handler.
    /// </param>
    /// <param name="toolRegistry">
    /// The Gemini tool registry used by the automation runtime.
    /// </param>
    /// <param name="toolConverter">
    /// The converter used to create Gemini tool declarations.
    /// </param>
    /// <param name="sessionManager">
    /// The Gemini session manager used to execute Gemini requests.
    /// </param>
    /// <param name="progress">
    /// The progress reporter used by the Gemini workflow.
    /// </param>
    /// <returns>
    /// The same service collection for registration chaining.
    /// </returns>
    public static IServiceCollection AddGeminiAutomation(
        this IServiceCollection services,
        AiExecutionSettings aiExecutionSettings,
        IGeminiToolRegistry toolRegistry,
        GeminiToolConverter toolConverter,
        IGeminiSessionManager sessionManager,
        IProgress<WorkflowProgress> progress)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(aiExecutionSettings);
        ArgumentNullException.ThrowIfNull(toolRegistry);
        ArgumentNullException.ThrowIfNull(toolConverter);
        ArgumentNullException.ThrowIfNull(sessionManager);
        ArgumentNullException.ThrowIfNull(progress);

        services.AddSingleton(aiExecutionSettings);

        services.AddSingleton(
            toolRegistry);

        services.AddSingleton(
            toolConverter);

        services.AddSingleton(
            sessionManager);

        services.AddSingleton(
            progress);

        services.AddSingleton<
            IGeminiJobHandler,
            GeminiJobHandler<WorkflowProgress>>();

        return services;
    }
}