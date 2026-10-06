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
    /// Executes the specified Gemini automation job using one atomic Android
    /// screen observation for both the screenshot and OCR context.
    /// </summary>
    /// <param name="gJob">
    /// The Gemini automation job to execute.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous AI workflow.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="gJob"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the user task or prompt is empty or consists only of
    /// white-space characters.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the Gemini workflow does not complete successfully.
    /// </exception>
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

        CancellationToken cancellationToken =
            cts.Token;

        IAndroidScreenObservation observation =
            await _screenObservationService
                .ObserveAsync(cancellationToken)
                .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        GeminiGenerateRequest request =
            DefaultRequest.DeepClone();

        request.SetPrompt(gJob.Prompt);

        /*
         * The screenshot and OCR context intentionally originate from the
         * exact same observation instance. Do not reacquire the screen or
         * invoke OCR independently in this handler.
         */
        request.AddUserMessage(
            request.Prompt,
            observation.ImageBytes.ToArray(),
            "image/jpeg");

        AddOcrContext(
            request,
            observation);

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
    /// Validates the supplied AI execution settings.
    /// </summary>
    /// <param name="settings">
    /// The AI execution settings.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="settings"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the tool execution timeout is not greater than zero.
    /// </exception>
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