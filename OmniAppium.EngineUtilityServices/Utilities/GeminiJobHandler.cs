using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Configs;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using CommonModels;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Observation;

namespace OmniAppium.EngineUtilityService.Utilities;

/// <summary>
/// Handles Gemini-powered automation jobs by consuming an atomic Android
/// screen observation.
/// </summary>
/// <typeparam name="TProgress">
/// The workflow progress model used to report AI execution progress.
/// </typeparam>
public sealed class GeminiJobHandler<TProgress> : IGeminiJobHandler
    where TProgress : WorkflowProgress, new()
{
    private static readonly GeminiGenerateRequest DefaultRequest =
        new GeminiConfig().DefaultRequestConfig;

    private readonly IGeminiToolRegistry _registry;
    private readonly GeminiToolConverter _converter;
    private readonly IGeminiSessionManager _sessionManager;
    private readonly IAndroidScreenObservationService _screenObservationService;
    private readonly IProgress<TProgress> _progressBar;

    private AiExecutionSettings _aiExecutionSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiJobHandler{TProgress}"/> class.
    /// </summary>
    /// <param name="aiExecutionSettings">
    /// The AI execution settings.
    /// </param>
    /// <param name="registry">
    /// The Gemini tool registry.
    /// </param>
    /// <param name="converter">
    /// The Gemini tool declaration converter.
    /// </param>
    /// <param name="sessionManager">
    /// The Gemini session manager.
    /// </param>
    /// <param name="screenObservationService">
    /// The service used to acquire an atomic Android screen observation.
    /// </param>
    /// <param name="progressBar">
    /// The progress reporter.
    /// </param>
    public GeminiJobHandler(
        AiExecutionSettings aiExecutionSettings,
        IGeminiToolRegistry registry,
        GeminiToolConverter converter,
        IGeminiSessionManager sessionManager,
        IAndroidScreenObservationService screenObservationService,
        IProgress<TProgress> progressBar)
    {
        ArgumentNullException.ThrowIfNull(aiExecutionSettings);
        ArgumentNullException.ThrowIfNull(registry);
        ArgumentNullException.ThrowIfNull(converter);
        ArgumentNullException.ThrowIfNull(sessionManager);
        ArgumentNullException.ThrowIfNull(screenObservationService);
        ArgumentNullException.ThrowIfNull(progressBar);

        ValidateExecutionSettings(aiExecutionSettings);

        _aiExecutionSettings = aiExecutionSettings;
        _registry = registry;
        _converter = converter;
        _sessionManager = sessionManager;
        _screenObservationService = screenObservationService;
        _progressBar = progressBar;
    }

    /// <summary>
    /// Updates the AI execution settings.
    /// </summary>
    /// <param name="aiExecutionSettings">
    /// The new AI execution settings.
    /// </param>
    public void SetExecutionSettings(
        AiExecutionSettings aiExecutionSettings)
    {
        ArgumentNullException.ThrowIfNull(aiExecutionSettings);

        ValidateExecutionSettings(aiExecutionSettings);

        _aiExecutionSettings = aiExecutionSettings;
    }

    /// <summary>
    /// Determines whether this handler can execute the specified job.
    /// </summary>
    /// <param name="job">
    /// The automation job.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the job is a <see cref="GeminiJob"/>.
    /// </returns>
    public bool CanHandle(Job job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return job is GeminiJob;
    }

    /// <summary>
    /// Executes the specified automation job.
    /// </summary>
    /// <param name="job">
    /// The automation job.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    public Task AutoExecuteAsync(Job job)
    {
        ArgumentNullException.ThrowIfNull(job);

        if (job is not GeminiJob geminiJob)
        {
            throw new ArgumentException(
                $"Expected {nameof(GeminiJob)}, but received {job.GetType().Name}.",
                nameof(job));
        }

        return AutoExecuteAsync(geminiJob);
    }

    /// <summary>
    /// Executes the specified Gemini automation job using a single atomic
    /// Android screen observation.
    /// </summary>
    /// <param name="gJob">
    /// The Gemini automation job.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous AI workflow.
    /// </returns>
    public async Task AutoExecuteAsync(
        GeminiJob gJob)
    {
        ArgumentNullException.ThrowIfNull(gJob);

        ValidateExecutionSettings(_aiExecutionSettings);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            gJob.UserTask,
            nameof(gJob.UserTask));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            gJob.Prompt,
            nameof(gJob.Prompt));

        _ = _registry
            .GetAllTools()
            .Select(_converter.ToToolDeclaration)
            .ToList();

        using CancellationTokenSource cts =
            new(_aiExecutionSettings.ToolExecutionTimeout);

        CancellationToken cancellationToken = cts.Token;

        IAndroidScreenObservation observation =
            await _screenObservationService
                .ObserveAsync(cancellationToken)
                .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        var request = DefaultRequest.DeepClone();

        request.SetPrompt(gJob.Prompt);

        request.AddUserMessage(
            request.Prompt,
            observation.ImageBytes.ToArray(),
            "image/jpeg");

        /*
         * Slice 3 boundary:
         *
         * Both screenshot and OCR context MUST originate from this exact
         * observation instance.
         *
         * Do not acquire another screenshot or invoke OCR directly here.
         *
         * OCR context injection must use the existing Gemini request contract.
         * Do not invent a new Planner/ExecutionScope/AiUtility contract here.
         */

        var executionResult =
            await _sessionManager
                .ExecuteWithToolSupportAsync<TProgress>(
                    request: request,
                    userTask: gJob.UserTask,
                    settings: _aiExecutionSettings,
                    ct: cancellationToken,
                    progress: _progressBar)
                .ConfigureAwait(false);

        if (!executionResult.IsAllSuccess)
        {
            throw new InvalidOperationException(
                "The Gemini workflow did not complete successfully.");
        }
    }

    /// <summary>
    /// Validates the supplied AI execution settings.
    /// </summary>
    /// <param name="settings">
    /// The AI execution settings.
    /// </param>
    private static void ValidateExecutionSettings(
        AiExecutionSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        if (settings.ToolExecutionTimeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(settings),
                settings.ToolExecutionTimeout,
                "ToolExecutionTimeout must be greater than zero.");
        }
    }
}