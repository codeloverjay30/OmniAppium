using System.Diagnostics;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityService.Utilities;

namespace OmniAppium.EngineUtilityServices.Services.OCR;

/// <summary>
/// Coordinates OCR text recognition and absolute Appium click operations.
/// </summary>
public sealed class OcrClickService(
    IAndroidScreenOcrService screenOcrService,
    IOcrTextMatcher ocrTextMatcher,
    IClickService clickService,
    ILoggerFactoryBaseUtilityService loggerFactoryService)
    : IOcrClickService
{
    private readonly ILogger _logger = loggerFactoryService.Logger;
    private static readonly TimeSpan RetryInterval = TimeSpan.FromSeconds(2);

    /// <summary>
    /// Waits for a unique OCR target and clicks its source-image center.
    /// </summary>
    /// <param name="targetText">The text to locate.</param>
    /// <param name="timeout">The maximum time allowed for recognition.</param>
    /// <param name="cancellationToken">The operation cancellation token.</param>
    /// <returns>A task representing the click operation.</returns>
    public async Task ClickTextAsync(
        string targetText,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetText);

        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeout),
                timeout,
                "OCR click timeout must be greater than zero.");
        }

        OcrTextMatch targetMatch = await WaitForTextAsync(
            targetText,
            timeout,
            cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        double x = targetMatch.Bounds.Center.X;
        double y = targetMatch.Bounds.Center.Y;

        ValidateCoordinates(x, y, targetText);

        _logger.LogInformation(
            "OCR click target: {TargetText}. " +
            "Bounds: TopLeft=({Left}, {Top}), BottomRight=({Right}, {Bottom}), " +
            "Width={Width}, Height={Height}. CalculatedCenter=({CenterX}, {CenterY})",
            targetText,
            targetMatch.Bounds.TopLeft.X,
            targetMatch.Bounds.TopLeft.Y,
            targetMatch.Bounds.BottomRight.X,
            targetMatch.Bounds.BottomRight.Y,
            targetMatch.Bounds.Width,
            targetMatch.Bounds.Height,
            x,
            y);

        clickService.ClickAbsolute(x, y);
    }

    /// <summary>
    /// Waits for a uniquely recognized OCR target with safe source-image geometry.
    /// Uses line geometry only when the entire normalized line exactly matches
    /// the target and word-level geometry cannot be resolved.
    /// </summary>
    /// <param name="targetText">The OCR target text.</param>
    /// <param name="timeout">The maximum recognition duration.</param>
    /// <param name="cancellationToken">The caller cancellation token.</param>
    /// <returns>The matched target with validated source-image bounds.</returns>
    /// <exception cref="TimeoutException">
    /// Thrown when no usable target geometry is obtained before timeout.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when OCR results are ambiguous or contain unsafe geometry.
    /// </exception>
    private async Task<OcrTextMatch> WaitForTextAsync(
        string targetText,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        using var timeoutSource = new CancellationTokenSource(timeout);
        using var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            timeoutSource.Token);

        string lastFailure = "The OCR target has not been observed.";
        var stopwatch = Stopwatch.StartNew();
        int attempt = 0;

        try
        {
            while (true)
            {
                linkedSource.Token.ThrowIfCancellationRequested();

                attempt++;
                OcrResult result = await screenOcrService
                    .RecognizeCurrentScreenAsync(linkedSource.Token)
                    .ConfigureAwait(false);

                try
                {
                    OcrTextMatch match = ocrTextMatcher.FindUniqueMatch(
                        result,
                        targetText,
                        OcrTextMatchMode.NormalizedContains);

                    ValidateMatchBounds(match, targetText);
                    LogAttempt(attempt, stopwatch.ElapsedMilliseconds, result,
                        targetText, "MATCHED", null);
                    return match;
                }
                catch (InvalidOperationException ex)
                    when (ex.Message ==
                          $"OCR target '{targetText}' was not found.")
                {
                    lastFailure = ex.Message;
                    LogAttempt(attempt, stopwatch.ElapsedMilliseconds, result,
                        targetText, "NOT_FOUND", lastFailure);
                }
                catch (InvalidOperationException ex)
                    when (IsWordGeometryFailure(ex, targetText))
                {
                    // The matcher found a unique line, but word geometry is
                    // missing or cannot reproduce the target's exact bounds.
                    // Never fall back for a substring of a longer OCR line.
                    OcrTextLine line = ocrTextMatcher.FindUnique(
                        result,
                        targetText,
                        OcrTextMatchMode.NormalizedContains);

                    LogWordGeometryFailure(targetText, line, ex);

                    if (NormalizedEquals(line.Text, targetText))
                    {
                        var fallback = new OcrTextMatch(targetText.Trim(), line.Bounds);
                        ValidateMatchBounds(fallback, targetText);

                        LogAttempt(attempt, stopwatch.ElapsedMilliseconds, result,
                            targetText, "EXACT_LINE_FALLBACK", ex.Message);

                        return fallback;
                    }

                    lastFailure =
                        $"OCR target '{targetText}' was recognized, but its " +
                        "word-level geometry was unavailable and the full line " +
                        "did not exactly match the target.";

                    LogAttempt(attempt, stopwatch.ElapsedMilliseconds, result,
                        targetText, "SUBSTRING_GEOMETRY_REJECTED", lastFailure);
                }

                catch (InvalidOperationException ex)
                {
                    LogAttempt(attempt, stopwatch.ElapsedMilliseconds, result,
                        targetText, "MATCH_REJECTED", ex.Message);
                    throw;
                }

                await Task.Delay(RetryInterval, linkedSource.Token)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException ex)
            when (!cancellationToken.IsCancellationRequested &&
                  timeoutSource.IsCancellationRequested)
        {
            _logger.LogWarning(
                "OCR click timed out. TargetText={TargetText}; Attempts={Attempt}; " +
                "ElapsedMs={ElapsedMs}; MatcherOutcome=TIMEOUT; LastFailure={LastFailure}",
                targetText, attempt, stopwatch.ElapsedMilliseconds, lastFailure);
            throw new TimeoutException(
                $"OCR target '{targetText}' could not be safely located " +
                $"within {timeout}. Last observation: {lastFailure}",
                ex);
        }
    }

    /// <summary>
    /// Emits a bounded structured diagnostic for one OCR matching attempt.
    /// </summary>
    /// <param name="attempt">The one-based attempt number.</param>
    /// <param name="elapsedMs">Elapsed milliseconds since the first attempt.</param>
    /// <param name="result">The recognized OCR result.</param>
    /// <param name="targetText">The requested target.</param>
    /// <param name="outcome">The matcher outcome.</param>
    /// <param name="reason">The last observed failure, if any.</param>
    private void LogAttempt(
        int attempt,
        long elapsedMs,
        OcrResult result,
        string targetText,
        string outcome,
        string? reason)
    {
        // Limit text emitted into logs to prevent unbounded OCR payloads.
        const int maxLoggedCharacters = 256;
        string normalized = NormalizeForDiagnostics(result.Text);
        if (normalized.Length > maxLoggedCharacters)
        {
            normalized = normalized[..maxLoggedCharacters];
        }

        _logger.LogInformation(
            "OCR click attempt. TargetText={TargetText}; Attempt={Attempt}; " +
            "ElapsedMs={ElapsedMs}; LineCount={LineCount}; " +
            "NormalizedText={NormalizedText}; MatcherOutcome={MatcherOutcome}; " +
            "Reason={Reason}",
            targetText, attempt, elapsedMs, result.Lines.Count,
            normalized, outcome, reason);
    }

    /// <summary>
    /// Normalizes OCR diagnostic text without allocating intermediate arrays.
    /// </summary>
    /// <param name="text">The recognized OCR text.</param>
    /// <returns>OCR text with whitespace removed.</returns>
    private static string NormalizeForDiagnostics(string text)
    {
        ReadOnlySpan<char> input = text.AsSpan();
        Span<char> buffer = stackalloc char[256];
        int written = 0;
        foreach (char character in input)
        {
            if (!char.IsWhiteSpace(character))
            {
                if (written == buffer.Length)
                {
                    break;
                }

                buffer[written++] = character;
            }
        }

        return new string(buffer[..written]);
    }

    /// <summary>
    /// Identifies the matcher's documented word-level geometry failures.
    /// </summary>
    /// <param name="exception">The matching exception.</param>
    /// <param name="targetText">The requested OCR text.</param>
    /// <returns>Whether a guarded line-level fallback may be evaluated.</returns>
    private static bool IsWordGeometryFailure(
        InvalidOperationException exception,
        string targetText)
    {
        string prefix = $"OCR target '{targetText}'";
        return exception.Message.StartsWith(
                   prefix + " was found, but its word-level geometry is unavailable.",
                   StringComparison.Ordinal)
               || exception.Message.StartsWith(
                   prefix + " was found in a text line, but its word-level geometry could not be resolved.",
                   StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether two OCR texts are identical after removing whitespace.
    /// </summary>
    /// <param name="actual">The recognized text.</param>
    /// <param name="expected">The requested text.</param>
    /// <returns>Whether their non-whitespace characters match exactly.</returns>
    private static bool NormalizedEquals(string actual, string expected)
    {
        ArgumentNullException.ThrowIfNull(actual);
        ArgumentNullException.ThrowIfNull(expected);

        ReadOnlySpan<char> actualSpan = actual.AsSpan();
        ReadOnlySpan<char> expectedSpan = expected.AsSpan();

        int actualIndex = 0;
        int expectedIndex = 0;

        while (true)
        {
            while (actualIndex < actualSpan.Length &&
                   char.IsWhiteSpace(actualSpan[actualIndex]))
            {
                actualIndex++;
            }

            while (expectedIndex < expectedSpan.Length &&
                   char.IsWhiteSpace(expectedSpan[expectedIndex]))
            {
                expectedIndex++;
            }

            if (actualIndex == actualSpan.Length ||
                expectedIndex == expectedSpan.Length)
            {
                return actualIndex == actualSpan.Length &&
                       expectedIndex == expectedSpan.Length;
            }

            if (actualSpan[actualIndex] != expectedSpan[expectedIndex])
            {
                return false;
            }

            actualIndex++;
            expectedIndex++;
        }
    }

    /// <summary>
    /// Logs OCR word segmentation without swallowing the original failure.
    /// </summary>
    /// <param name="targetText">The OCR target.</param>
    /// <param name="line">The uniquely matched OCR line.</param>
    /// <param name="exception">The original word-geometry failure.</param>
    private void LogWordGeometryFailure(
        string targetText,
        OcrTextLine line,
        InvalidOperationException exception)
    {
        _logger.LogWarning(
            exception,
            "OCR word geometry unavailable for {TargetText}. " +
            "LineText={LineText}; LineBounds={LineBounds}; WordCount={WordCount}",
            targetText,
            line.Text,
            line.Bounds,
            line.Words.Count);

        for (int index = 0; index < line.Words.Count; index++)
        {
            OcrTextWord word = line.Words[index];
            _logger.LogDebug(
                "OCR word {Index}: Text={WordText}; Bounds={WordBounds}",
                index,
                word.Text,
                word.Bounds);
        }
    }

    /// <summary>
    /// Ensures that OCR geometry has finite coordinates and positive dimensions.
    /// </summary>
    /// <param name="match">The OCR match to validate.</param>
    /// <param name="targetText">The associated target text.</param>
    private static void ValidateMatchBounds(
        OcrTextMatch match,
        string targetText)
    {
        var bounds = match.Bounds;

        if (!double.IsFinite(bounds.TopLeft.X) ||
            !double.IsFinite(bounds.TopLeft.Y) ||
            !double.IsFinite(bounds.BottomRight.X) ||
            !double.IsFinite(bounds.BottomRight.Y) ||
            !double.IsFinite(bounds.Width) ||
            !double.IsFinite(bounds.Height) ||
            bounds.Width <= 0 ||
            bounds.Height <= 0 ||
            bounds.BottomRight.X <= bounds.TopLeft.X ||
            bounds.BottomRight.Y <= bounds.TopLeft.Y)
        {
            throw new InvalidOperationException(
                $"OCR target '{targetText}' produced invalid bounding geometry.");
        }

        ValidateCoordinates(bounds.Center.X, bounds.Center.Y, targetText);
    }

    /// <summary>
    /// Validates an OCR coordinate before it is forwarded to Appium.
    /// </summary>
    /// <param name="x">The absolute OCR X coordinate.</param>
    /// <param name="y">The absolute OCR Y coordinate.</param>
    /// <param name="targetText">The OCR target associated with the coordinate.</param>
    private static void ValidateCoordinates(
        double x,
        double y,
        string targetText)
    {
        if (!double.IsFinite(x) ||
            !double.IsFinite(y) ||
            x < 0 ||
            y < 0)
        {
            throw new InvalidOperationException(
                $"OCR target '{targetText}' produced an invalid click " +
                $"coordinate ({x}, {y}).");
        }
    }
}