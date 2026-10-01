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
    private static readonly TimeSpan RetryInterval =
        TimeSpan.FromSeconds(2);

    /// <summary>
    /// Waits for the specified text to appear on the current Android screen
    /// and clicks the center of the uniquely matched OCR text region.
    /// </summary>
    /// <param name="targetText">
    /// The text to locate on the current Android screen.
    /// </param>
    /// <param name="timeout">
    /// The maximum amount of time to wait for the target text.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous OCR and click operation.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="targetText"/> is empty or consists only of
    /// white-space characters.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="timeout"/> is not greater than zero.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Thrown when the target text cannot be found before the timeout expires.
    /// </exception>
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

        OcrTextMatch targetMatch =
            await WaitForTextAsync(
                    targetText,
                    timeout,
                    cancellationToken)
                .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        double x = targetMatch.Bounds.Center.X;
        double y = targetMatch.Bounds.Center.Y;

        ValidateCoordinates(
            x,
            y,
            targetText);

        _logger.LogInformation(
            "OCR click target: {TargetText}. " +
            "Bounds: TopLeft=({Left}, {Top}), BottomRight=({Right}, {Bottom}), " +
            "Width={Width}, Height={Height}. " +
            "CalculatedCenter=({CenterX}, {CenterY})",
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
    /// Waits until the specified text is uniquely recognized with
    /// target-specific source-image geometry, or the operation reaches its timeout.
    /// </summary>
    /// <param name="targetText">
    /// The text to locate.
    /// </param>
    /// <param name="timeout">
    /// The maximum amount of time to wait.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The uniquely matched OCR target and its source-image bounds.
    /// </returns>
    /// <exception cref="TimeoutException">
    /// Thrown when the target text cannot be found before the timeout expires.
    /// </exception>
    private async Task<OcrTextMatch> WaitForTextAsync(
        string targetText,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        using var timeoutSource =
            new CancellationTokenSource(timeout);

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
                    return ocrTextMatcher.FindUniqueMatch(
                        result,
                        targetText,
                        OcrTextMatchMode.NormalizedContains);
                }
                catch (InvalidOperationException ex)
                    when (ex.Message ==
                          $"OCR target '{targetText}' was not found.")
                {
                    await Task.Delay(
                            RetryInterval,
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
                $"OCR target '{targetText}' was not found within {timeout}.");
        }
    }


    /// <summary>
    /// Validates an OCR coordinate before it is forwarded to Appium.
    /// </summary>
    /// <param name="x">The absolute OCR X coordinate.</param>
    /// <param name="y">The absolute OCR Y coordinate.</param>
    /// <param name="targetText">
    /// The OCR target associated with the coordinate.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when an OCR coordinate is not finite or is negative.
    /// </exception>
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