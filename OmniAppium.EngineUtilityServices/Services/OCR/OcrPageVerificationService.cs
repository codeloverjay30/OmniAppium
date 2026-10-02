using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;

namespace OmniAppium.EngineUtilityServices.Services.OCR;

/// <summary>
/// Verifies the current Android screen state by repeatedly recognizing OCR
/// text until the expected screen state is reached or the operation times out.
/// </summary>
public sealed class OcrPageVerificationService(
    IAndroidScreenOcrService screenOcrService,
    IOcrTextMatcher ocrTextMatcher,
    TimeProvider timeProvider,
    ILoggerFactoryBaseUtilityService loggerFactoryBaseUtilityService)
    : IOcrPageVerificationService
{
    private ILogger _logger => loggerFactoryBaseUtilityService.Logger;
    private static readonly TimeSpan RetryInterval =
        TimeSpan.FromSeconds(2);

    private readonly TimeProvider _timeProvider =
        timeProvider ??
        throw new ArgumentNullException(nameof(timeProvider));

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="expectedText"/> is null, empty, or consists
    /// only of white-space characters.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="timeout"/> is not greater than zero.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Thrown when the expected text cannot be found before the timeout expires.
    /// </exception>
    public async Task WaitForTextAsync(
        string expectedText,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(expectedText);

        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeout),
                timeout,
                "OCR page verification timeout must be greater than zero.");
        }

        using var timeoutSource =
            new CancellationTokenSource(
                timeout,
                _timeProvider);

        using var linkedSource =
            CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                timeoutSource.Token);

        try
        {
            while (true)
            {
                linkedSource.Token.ThrowIfCancellationRequested();

                OcrResult result =
                    await screenOcrService
                        .RecognizeCurrentScreenAsync(
                            linkedSource.Token)
                        .ConfigureAwait(false);

                try
                {
                    _ = ocrTextMatcher.FindUnique(
                        result,
                        expectedText,
                        OcrTextMatchMode.NormalizedContains);

                    return;
                }
                catch (InvalidOperationException ex)
                    when (ex.Message ==
                          $"OCR target '{expectedText}' was not found.")
                {
                    await Task.Delay(
                        RetryInterval,
                        _timeProvider,
                        linkedSource.Token)
                    .ConfigureAwait(false);
                }
            }
        }
        catch (OperationCanceledException)
            when (!cancellationToken.IsCancellationRequested &&
                  timeoutSource.IsCancellationRequested)
        {
            throw new TimeoutException(
                $"OCR target '{expectedText}' was not found within {timeout}.");
        }
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="expectedTexts"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when no expected text is provided, or when any expected text is
    /// null, empty, or consists only of white-space characters.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="timeout"/> is not greater than zero.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Thrown when all expected texts cannot be found on the same screen before
    /// the timeout expires.
    /// </exception>
    public async Task WaitForAllTextAsync(
        IReadOnlyCollection<string> expectedTexts,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expectedTexts);

        if (expectedTexts.Count == 0)
        {
            throw new ArgumentException(
                "At least one OCR page verification text is required.",
                nameof(expectedTexts));
        }

        foreach (string expectedText in expectedTexts)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(expectedText);
        }

        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeout),
                timeout,
                "OCR page verification timeout must be greater than zero.");
        }

        using var timeoutSource =
            new CancellationTokenSource(
                timeout,
                _timeProvider);

        using var linkedSource =
            CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                timeoutSource.Token);

        try
        {
            while (true)
            {
                linkedSource.Token.ThrowIfCancellationRequested();

                OcrResult result =
                    await screenOcrService
                        .RecognizeCurrentScreenAsync(
                            linkedSource.Token)
                        .ConfigureAwait(false);

                OcrVerificationAttemptResult attemptResult =
                    EvaluateTexts(
                        result,
                        expectedTexts);

                LogVerificationAttempt(
                    expectedTexts,
                    attemptResult);

                if (attemptResult.AllTextsFound)
                {
                    return;
                }


                await Task.Delay(
                        RetryInterval,
                        _timeProvider,
                        linkedSource.Token)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
            when (!cancellationToken.IsCancellationRequested &&
                  timeoutSource.IsCancellationRequested)
        {
            string markers =
                string.Join(", ", expectedTexts);

            throw new TimeoutException(
                $"OCR page markers [{markers}] were not all found within {timeout}.");
        }
    }

    /// <summary>
    /// Evaluates all expected OCR markers against a single screen snapshot.
    /// </summary>
    /// <param name="result">
    /// The OCR result representing a single screen state.
    /// </param>
    /// <param name="expectedTexts">
    /// The OCR markers expected to be present in the screen snapshot.
    /// </param>
    /// <returns>
    /// The marker evaluation result for the current OCR attempt.
    /// </returns>
    private OcrVerificationAttemptResult EvaluateTexts(
        OcrResult result,
        IReadOnlyCollection<string> expectedTexts)
    {
        List<string> matchedTexts =
            new(expectedTexts.Count);

        List<string> missingTexts =
            new(expectedTexts.Count);

        foreach (string expectedText in expectedTexts)
        {
            try
            {
                _ = ocrTextMatcher.FindUnique(
                    result,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains);

                matchedTexts.Add(expectedText);
            }
            catch (InvalidOperationException ex)
                when (ex.Message ==
                      $"OCR target '{expectedText}' was not found.")
            {
                missingTexts.Add(expectedText);
            }
        }

        return new OcrVerificationAttemptResult(
            matchedTexts,
            missingTexts);
    }

    /// <summary>
    /// Logs the OCR marker evaluation result for a single verification attempt.
    /// </summary>
    /// <param name="expectedTexts">
    /// The complete set of expected OCR markers.
    /// </param>
    /// <param name="attemptResult">
    /// The matched and missing markers for the current attempt.
    /// </param>
    private void LogVerificationAttempt(
        IReadOnlyCollection<string> expectedTexts,
        OcrVerificationAttemptResult attemptResult)
    {
        _logger.LogInformation(
            "OCR verification attempt. Expected=[{Expected}] Matched=[{Matched}] Missing=[{Missing}]",
            string.Join(", ", expectedTexts),
            string.Join(", ", attemptResult.MatchedTexts),
            string.Join(", ", attemptResult.MissingTexts));
    }

}