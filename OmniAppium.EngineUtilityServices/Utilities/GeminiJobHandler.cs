using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Configs;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using CommonModels;
using OmniAppium.ConfigUtilityService.Models;
using System.Drawing.Imaging;

namespace OmniAppium.EngineUtilityService.Utilities;

/// <summary>
/// Handles Gemini-powered automation jobs.
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
    private readonly IScreenshotService _screenshotService;
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
    /// <param name="screenshotService">
    /// The screenshot service used to capture the current screen state.
    /// </param>
    /// <param name="progressBar">
    /// The progress reporter.
    /// </param>
    public GeminiJobHandler(
        AiExecutionSettings aiExecutionSettings,
        IGeminiToolRegistry registry,
        GeminiToolConverter converter,
        IGeminiSessionManager sessionManager,
        IScreenshotService screenshotService,
        IProgress<TProgress> progressBar)
    {
        ArgumentNullException.ThrowIfNull(aiExecutionSettings);
        ArgumentNullException.ThrowIfNull(registry);
        ArgumentNullException.ThrowIfNull(converter);
        ArgumentNullException.ThrowIfNull(sessionManager);
        ArgumentNullException.ThrowIfNull(screenshotService);
        ArgumentNullException.ThrowIfNull(progressBar);

        ValidateExecutionSettings(aiExecutionSettings);

        _aiExecutionSettings = aiExecutionSettings;
        _registry = registry;
        _converter = converter;
        _sessionManager = sessionManager;
        _screenshotService = screenshotService;
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
    /// <see langword="true"/> when the job is a <see cref="GeminiJob"/>;
    /// otherwise, <see langword="false"/>.
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
    /// Executes the specified Gemini automation job.
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

        ValidateExecutionSettings(
            _aiExecutionSettings);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            gJob.UserTask,
            nameof(gJob.UserTask));

        ArgumentException.ThrowIfNullOrWhiteSpace(
            gJob.Prompt,
            nameof(gJob.Prompt));

        var tools = _registry
            .GetAllTools()
            .Select(_converter.ToToolDeclaration)
            .ToList();

        /*
         * Capture a fresh screenshot before AI reasoning.
         * GetBytesOfCachedScreenshotBytes() alone may return stale state.
         */
        _screenshotService.TakeScreenshot();

        var imageBytes =
            _screenshotService.GetBytesOfCachedScreenshotBytes(
                ImageFormat.Png);

        if (imageBytes.Length == 0)
        {
            throw new InvalidOperationException(
                "The current screen capture produced an empty image buffer.");
        }

        var request =
            DefaultRequest.Clone();

        request.SetPrompt(
            gJob.Prompt);

        request.AddUserMessage(
            request.Prompt,
            imageBytes);

        /*
         * Keep tool declarations on the request only when the existing
         * request contract requires them explicitly.
         *
         * The Gemini session manager may also obtain tool metadata through
         * its tool service. Do not duplicate the tool registration lifecycle.
         */

        using var cts =
            new CancellationTokenSource(
                _aiExecutionSettings.ToolExecutionTimeout);

        await _sessionManager.ExecuteWithToolSupportAsync<TProgress>(
            request: request,
            userTask: gJob.UserTask,
            settings: _aiExecutionSettings,
            ct: cts.Token,
            progress: _progressBar);
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