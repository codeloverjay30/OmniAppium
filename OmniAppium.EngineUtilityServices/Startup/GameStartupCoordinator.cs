using Microsoft.Extensions.Logging;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Interfaces;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Workflows;

namespace OmniAppium.EngineUtilityServices.Startup;

/// <summary>
/// Coordinates the transition from a game's login screen
/// to its main screen using OCR-based verification.
/// </summary>
public sealed class GameStartupCoordinator : IGameStartupCoordinator
{
    private readonly IAndroidScreenOcrService _ocr;
    private readonly IGameWorkflowStepExecutor _workflow;
    private readonly IGameStartupProfile _profile;
    private readonly ILogger<GameStartupCoordinator> _logger;

    /// <summary>
    /// Initializes a new instance of the game startup coordinator.
    /// </summary>
    /// <param name="ocr">The screen OCR service.</param>
    /// <param name="workflow">The verified workflow executor.</param>
    /// <param name="profile">The game-specific startup profile.</param>
    /// <param name="logger">The diagnostic logger.</param>
    public GameStartupCoordinator(
        IAndroidScreenOcrService ocr,
        IGameWorkflowStepExecutor workflow,
        IGameStartupProfile profile,
        ILogger<GameStartupCoordinator> logger)
    {
        _ocr = ocr ?? throw new ArgumentNullException(nameof(ocr));
        _workflow = workflow ?? throw new ArgumentNullException(nameof(workflow));
        _profile = profile ?? throw new ArgumentNullException(nameof(profile));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        ArgumentException.ThrowIfNullOrWhiteSpace(_profile.LoginButtonText);
        ArgumentException.ThrowIfNullOrWhiteSpace(_profile.MainScreenReadyText);

        if (string.Equals(
            _profile.LoginButtonText,
            _profile.MainScreenReadyText,
            StringComparison.Ordinal))
        {
            throw new ArgumentException(
                "Login and main-screen targets must be different.",
                nameof(profile));
        }
    }

    /// <inheritdoc />
    public async Task EnsureMainScreenReadyAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Startup screens may contain neither marker while the app loads.
        // A monotonic deadline bounds all retries, including OCR latency.
        TimeSpan startupTimeout = TimeSpan.FromMinutes(2);
        TimeSpan pollInterval = TimeSpan.FromSeconds(2);
        long started = System.Diagnostics.Stopwatch.GetTimestamp();
        bool loginDetected = false;
        int attempts = 0;

        while (System.Diagnostics.Stopwatch.GetElapsedTime(started) < startupTimeout)
        {
            cancellationToken.ThrowIfCancellationRequested();
            attempts++;

            OcrResult current = await RecognizeScreenAsync(
                cancellationToken).ConfigureAwait(false);

            if (ContainsText(current, _profile.MainScreenReadyText))
            {
                _logger.LogInformation(
                    "Game main screen is ready after {Attempts} observations. Target: {Target}",
                    attempts,
                    _profile.MainScreenReadyText);
                return;
            }

            if (ContainsText(current, _profile.LoginButtonText))
            {
                loginDetected = true;
                break;
            }

            _logger.LogInformation(
                "Game startup screen is not yet identified. Observation={Attempt}; elapsed={Elapsed}.",
                attempts,
                System.Diagnostics.Stopwatch.GetElapsedTime(started));

            TimeSpan remaining = startupTimeout -
                System.Diagnostics.Stopwatch.GetElapsedTime(started);
            if (remaining <= TimeSpan.Zero)
            {
                break;
            }

            await Task.Delay(
                remaining < pollInterval ? remaining : pollInterval,
                cancellationToken).ConfigureAwait(false);
        }

        if (!loginDetected)
        {
            throw new TimeoutException(
                $"Game startup timed out after {startupTimeout}. Neither login target " +
                $"'{_profile.LoginButtonText}' nor main-screen target " +
                $"'{_profile.MainScreenReadyText}' was recognized.");
        }

        _logger.LogInformation(
            "Login screen detected. Login target: {Target}",
            _profile.LoginButtonText);

        cancellationToken.ThrowIfCancellationRequested();

        // Execute only one verified login action.
        // The workflow owns click positioning and post-click verification.
        await _workflow.ExecuteAsync(
            new GameWorkflowStep(
                ClickText: _profile.LoginButtonText,
                VerificationTexts: [_profile.MainScreenReadyText],
                ClickTimeout: TimeSpan.FromSeconds(30),
                VerificationTimeout: TimeSpan.FromSeconds(60)))
            .ConfigureAwait(false);

        // The workflow already verifies the main-screen marker.
        // A second immediate OCR capture can race with game animations.
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Game main screen verified successfully. Target: {Target}",
            _profile.MainScreenReadyText);
    }

    /// <summary>
    /// Recognizes the current screen while honoring cancellation
    /// before and after the underlying OCR operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The current screen OCR result.</returns>
    private async Task<OcrResult> RecognizeScreenAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        OcrResult result = await _ocr
            .RecognizeCurrentScreenAsync(cancellationToken)
            .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        if (result is null)
        {
            throw new InvalidOperationException(
                "Screen OCR returned a null result.");
        }

        _logger.LogInformation(
            "Startup OCR completed. Recognized line count: {LineCount}.",
            result.Lines?.Count ?? 0);

        if (result.Lines is not null)
        {
            int lineIndex = 0;

            foreach (OcrTextLine line in result.Lines)
            {
                _logger.LogInformation(
                    "Startup OCR line [{LineIndex}]: '{RecognizedText}'",
                    lineIndex,
                    line.Text ?? string.Empty);

                lineIndex++;
            }
        }
        return result;

    }

    /// <summary>
    /// Determines whether an OCR result contains the specified text,
    /// ignoring whitespace in recognized lines.
    /// </summary>
    /// <param name="result">The OCR result.</param>
    /// <param name="target">The expected text.</param>
    /// <returns>True when the expected text is found.</returns>
    private static bool ContainsText(
        OcrResult result,
        string target)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentException.ThrowIfNullOrWhiteSpace(target);

        if (result.Lines is null)
        {
            return false;
        }

        ReadOnlySpan<char> expected = target.AsSpan();

        foreach (OcrTextLine line in result.Lines)
        {
            if (string.IsNullOrWhiteSpace(line.Text))
            {
                continue;
            }

            ReadOnlySpan<char> source = line.Text.AsSpan();

            Span<char> buffer = source.Length <= 256
                ? stackalloc char[source.Length]
                : new char[source.Length];

            int length = 0;

            foreach (char character in source)
            {
                if (!char.IsWhiteSpace(character))
                {
                    buffer[length++] = character;
                }
            }

            if (buffer[..length].Contains(
                expected,
                StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}
