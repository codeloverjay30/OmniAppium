using System.ComponentModel.DataAnnotations;
using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Models;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Configs;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using CommonModels;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityService.Utilities;

/// <summary>
/// Handles Gemini automation jobs by acquiring a single atomic Android screen
/// observation and executing the Gemini workflow with the captured image and
/// OCR context.
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

    private readonly IOcrGroundedPlannerExecutionScopeFactory _plannerExecutionScopeFactory;

    public GeminiJobHandler(
        AiExecutionSettings aiExecutionSettings,
        IGeminiToolRegistry registry,
        GeminiToolConverter converter,
        IGeminiSessionManager sessionManager,
        IAndroidScreenObservationService screenObservationService,
        IOcrGroundedPlannerExecutionScopeFactory plannerExecutionScopeFactory,
        IProgress<TProgress> progressBar)
    {
        ArgumentNullException.ThrowIfNull(aiExecutionSettings);
        ArgumentNullException.ThrowIfNull(registry);
        ArgumentNullException.ThrowIfNull(converter);
        ArgumentNullException.ThrowIfNull(sessionManager);
        ArgumentNullException.ThrowIfNull(screenObservationService);
        ArgumentNullException.ThrowIfNull(plannerExecutionScopeFactory);
        ArgumentNullException.ThrowIfNull(progressBar);

        ValidateExecutionSettings(aiExecutionSettings);

        _aiExecutionSettings = aiExecutionSettings;
        _registry = registry;
        _converter = converter;
        _sessionManager = sessionManager;
        _screenObservationService = screenObservationService;
        _plannerExecutionScopeFactory =
            plannerExecutionScopeFactory;
        _progressBar = progressBar;
    }


    /// <summary>
    /// Updates the AI execution settings used by subsequent Gemini jobs.
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
    /// The automation job to inspect.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the job is a <see cref="GeminiJob"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool CanHandle(Job job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return job is GeminiJob;
    }

    /// <summary>
    /// Executes the specified automation job when it is a Gemini job.
    /// </summary>
    /// <param name="job">
    /// The automation job to execute.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="job"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="job"/> is not a <see cref="GeminiJob"/>.
    /// </exception>
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
    /// Executes a Gemini job against a single immutable Android screen observation.
    /// The same observation is used for both Gemini visual context and the
    /// OCR-grounded planner execution scope.
    /// </summary>
    /// <param name="gJob">
    /// The Gemini job containing the prompt and user task to execute.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="gJob"/> is null or when a required job string
    /// is null, empty, or consists only of white-space characters.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the configured tool execution timeout is not greater than zero.
    /// </exception>
    public async Task AutoExecuteAsync(GeminiJob gJob)
    {
        ArgumentNullException.ThrowIfNull(gJob);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
            _aiExecutionSettings.ToolExecutionTimeout.TotalMilliseconds,
            nameof(_aiExecutionSettings.ToolExecutionTimeout));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            gJob.UserTask,
            nameof(GeminiJob.UserTask));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            gJob.Prompt,
            nameof(GeminiJob.Prompt));

        /*
         * Slice 5 boundary:
         * Acquire exactly one observation for this execution.
         *
         * Its ImageBytes and OcrResult belong to the same physical screen
         * capture, preventing Gemini and the grounded planner from reasoning
         * about different screen states.
         */
        IAndroidScreenObservation observation =
            await _screenObservationService
                .ObserveAsync()
                .ConfigureAwait(false);

        /*
         * The planner scope must be created from the exact observation that
         * Gemini will receive below.
         *
         * Keep this scope alive for the complete Gemini tool-execution lifetime.
         * using guarantees cleanup on success, exception, and cancellation.
         */
        using IOcrGroundedPlannerExecutionScope plannerExecutionScope =
            await _plannerExecutionScopeFactory
                .CreateAsync(observation)
                .ConfigureAwait(false);

        /*
         * Convert all currently registered services into Gemini tool
         * declarations.
         */
        List<GeminiToolDeclaration> tools =
            _registry
                .GetAllTools()
                .Select(metadata =>
                    _converter.ToToolDeclaration(metadata))
                .ToList();

        /*
         * Clone the shared request template so this execution owns all mutable
         * request state.
         */
        GeminiGenerateRequest request =
            DefaultRequest.DeepClone();

        request.SetPrompt(gJob.Prompt);

        /*
         * IMPORTANT:
         * Use the image from the already-acquired observation.
         * Do not capture another screenshot here.
         */
        request.AddUserMessage(
            request.RawPrompt,
            observation.ImageBytes);

        request.Tools =
        [
            new GeminiGenerateRequest.GeminiToolDeclarationWrapper
            {
                FunctionDeclarations = tools
            }
        ];

        /*
         * plannerExecutionScope intentionally remains alive during this await.
         * Gemini tool calls therefore resolve against the planner state derived
         * from the same observation used above.
         */
        await _sessionManager
            .ExecuteWithToolSupportAsync<TProgress>(
                request,
                gJob.UserTask.AsMemory(),
                _aiExecutionSettings,
                default,
                _progressBar)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds OCR text from the supplied screen observation to the Gemini request
    /// when meaningful OCR text is available.
    /// </summary>
    /// <param name="request">
    /// The Gemini request receiving the OCR context.
    /// </param>
    /// <param name="observation">
    /// The atomic Android screen observation containing the OCR result.
    /// </param>
    private static void AddOcrContext(
        GeminiGenerateRequest request,
        IAndroidScreenObservation observation)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(observation);

        string ocrText =
            observation.OcrResult.Text;

        if (string.IsNullOrWhiteSpace(ocrText))
        {
            return;
        }

        request.AddUserMessage(
            ocrText.AsMemory());
    }

    /// <summary>
    /// Validates the supplied AI execution settings against its declared
    /// data-annotation constraints and handler-specific runtime requirements.
    /// </summary>
    /// <param name="settings">
    /// The AI execution settings to validate.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="settings"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when an execution setting violates its supported range.
    /// </exception>
    private static void ValidateExecutionSettings(
        AiExecutionSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        List<ValidationResult> validationResults = [];

        bool isValid =
            Validator.TryValidateObject(
                settings,
                new ValidationContext(settings),
                validationResults,
                validateAllProperties: true);

        if (!isValid)
        {
            ValidationResult validationResult =
                validationResults[0];

            string memberName =
                validationResult.MemberNames.FirstOrDefault()
                ?? nameof(settings);

            object? actualValue =
                memberName switch
                {
                    nameof(AiExecutionSettings.MaxSteps) =>
                        settings.MaxSteps,

                    nameof(AiExecutionSettings.Threshold) =>
                        settings.Threshold,

                    nameof(AiExecutionSettings.LastTokenCountNeededToBeKept) =>
                        settings.LastTokenCountNeededToBeKept,

                    nameof(AiExecutionSettings.ToolExecutionTimeout) =>
                        settings.ToolExecutionTimeout,

                    _ => null
                };

            throw new ArgumentOutOfRangeException(
                memberName,
                actualValue,
                validationResult.ErrorMessage
                    ?? "The AI execution settings are invalid.");
        }

        if (settings.MaxSteps < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(settings.MaxSteps),
                settings.MaxSteps,
                "MaxSteps must be non-negative.");
        }

        if (settings.ToolExecutionTimeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(settings.ToolExecutionTimeout),
                settings.ToolExecutionTimeout,
                "ToolExecutionTimeout must be greater than zero.");
        }
    }
}