
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Captures Android screen frames until the configured visual stability
/// policy succeeds or its time budget expires.
/// </summary>
public sealed class AndroidScreenStabilityGuard
    : IAndroidScreenStabilityGuard
{
    private readonly IScreenshotService _screenshotService;
    private readonly IScreenFrameComparer _comparer;
    private readonly ILoggerFactoryBaseUtilityService _loggerFactory;
    private readonly TimeProvider _timeProvider;
    private readonly Action<int, ReadOnlyMemory<byte>>? _diagnosticFrameSink;

    /// <summary>
    /// Initializes the guard with screenshot, comparison, logging,
    /// time, and optional diagnostic dependencies.
    /// </summary>
    public AndroidScreenStabilityGuard(
        IScreenshotService screenshots,
        IScreenFrameComparer comparer,
        ILoggerFactoryBaseUtilityService loggerFactoryBaseUtilityService,
        TimeProvider? timeProvider = null,
        Action<int, ReadOnlyMemory<byte>>? diagnosticFrameSink = null)
    {
        _screenshotService = screenshots
            ?? throw new ArgumentNullException(nameof(screenshots));

        _comparer = comparer
            ?? throw new ArgumentNullException(nameof(comparer));

        _loggerFactory = loggerFactoryBaseUtilityService
            ?? throw new ArgumentNullException(
                nameof(loggerFactoryBaseUtilityService));

        _timeProvider = timeProvider ?? TimeProvider.System;
        _diagnosticFrameSink = diagnosticFrameSink;
    }

    /// <summary>
    /// Samples screenshots until enough consecutive frame pairs match,
    /// the timeout expires, or cancellation is requested.
    /// </summary>
    public async Task<ScreenStabilityResult> WaitForStabilityAsync(
        ScreenStabilityOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        cancellationToken.ThrowIfCancellationRequested();

        ILogger logger = _loggerFactory.Logger;

        long started = _timeProvider.GetTimestamp();

        byte[]? previous = null;
        int matches = 0;
        int samples = 0;

        logger.LogInformation(
            "Screen stability started. TimeoutMs={TimeoutMs}, " +
            "SampleIntervalMs={SampleIntervalMs}, " +
            "RequiredMatches={RequiredMatches}, " +
            "MaximumDifferenceRatio={MaximumDifferenceRatio}",
            options.Timeout.TotalMilliseconds,
            options.SampleInterval.TotalMilliseconds,
            options.RequiredConsecutiveMatches,
            options.MaximumDifferenceRatio);

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            TimeSpan beforeCapture =
                _timeProvider.GetElapsedTime(started);

            if (beforeCapture >= options.Timeout)
            {
                return Complete(
                    logger, false, true, samples, matches,
                    started);
            }

            long captureStarted = _timeProvider.GetTimestamp();

            byte[] current;

            try
            {
                current = _screenshotService.CaptureScreenshotBytes(null);
            }
            catch (Exception ex) when (
                ex is not OperationCanceledException)
            {
                logger.LogError(
                    ex,
                    "Screen stability screenshot failed. " +
                    "NextSampleIndex={SampleIndex}, ElapsedMs={ElapsedMs}",
                    samples + 1,
                    _timeProvider
                        .GetElapsedTime(started)
                        .TotalMilliseconds);

                throw;
            }

            TimeSpan captureDuration =
                _timeProvider.GetElapsedTime(captureStarted);

            samples++;

            cancellationToken.ThrowIfCancellationRequested();

            bool hasImage = current is { Length: > 0 };
            bool? isSimilar = null;

            if (!hasImage)
            {
                matches = 0;
                previous = null;
            }
            else
            {
                SaveDiagnosticFrame(
                    logger,
                    samples,
                    current);

                if (previous is not null)
                {
                    try
                    {
                        isSimilar = _comparer.AreSimilar(
                            previous,
                            current,
                            options.MaximumDifferenceRatio);
                    }
                    catch (Exception ex) when (
                        ex is not OperationCanceledException)
                    {
                        logger.LogError(
                            ex,
                            "Screen frame comparison failed. " +
                            "SampleIndex={SampleIndex}",
                            samples);

                        throw;
                    }

                    matches = isSimilar.Value
                        ? matches + 1
                        : 0;
                }
                else
                {
                    matches = 0;
                }

                // Protect against providers reusing their buffers.
                previous = current.ToArray();
            }

            TimeSpan elapsed =
                _timeProvider.GetElapsedTime(started);

            TimeSpan remaining = options.Timeout - elapsed;

            bool stable =
                matches >= options.RequiredConsecutiveMatches;

            logger.LogInformation(
                "Screen stability sample. " +
                "SampleIndex={SampleIndex}, " +
                "HasImage={HasImage}, " +
                "IsSimilar={IsSimilar}, " +
                "ConsecutiveMatches={ConsecutiveMatches}, " +
                "RequiredMatches={RequiredMatches}, " +
                "EncodedBytes={EncodedBytes}, " +
                "CaptureDurationMs={CaptureDurationMs}, " +
                "ElapsedMs={ElapsedMs}, " +
                "RemainingMs={RemainingMs}",
                samples,
                hasImage,
                isSimilar,
                matches,
                options.RequiredConsecutiveMatches,
                current?.Length ?? 0,
                captureDuration.TotalMilliseconds,
                elapsed.TotalMilliseconds,
                Math.Max(0, remaining.TotalMilliseconds));

            // Preserve the existing success-first boundary.
            if (stable)
            {
                return Complete(
                    logger, true, false, samples, matches,
                    started);
            }

            if (remaining <= TimeSpan.Zero)
            {
                return Complete(
                    logger, false, true, samples, matches,
                    started);
            }

            TimeSpan delay =
                remaining < options.SampleInterval
                    ? remaining
                    : options.SampleInterval;

            await Task.Delay(
                    delay,
                    _timeProvider,
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Publishes a diagnostic frame without allowing diagnostic
    /// failures to affect the stability decision.
    /// </summary>
    private void SaveDiagnosticFrame(
        ILogger logger,
        int sampleIndex,
        byte[] imageBytes)
    {
        if (_diagnosticFrameSink is null)
            return;

        try
        {
            _diagnosticFrameSink(
                sampleIndex,
                imageBytes);
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                ex,
                "Unable to save diagnostic frame. " +
                "SampleIndex={SampleIndex}",
                sampleIndex);
        }
    }

    /// <summary>
    /// Records the final stability decision and creates its result.
    /// </summary>
    private ScreenStabilityResult Complete(
        ILogger logger,
        bool stable,
        bool timedOut,
        int samples,
        int matches,
        long started)
    {
        TimeSpan elapsed =
            _timeProvider.GetElapsedTime(started);

        if (timedOut)
        {
            logger.LogWarning(
                "Screen stability timed out. " +
                "Samples={Samples}, " +
                "ConsecutiveMatches={ConsecutiveMatches}, " +
                "ElapsedMs={ElapsedMs}",
                samples,
                matches,
                elapsed.TotalMilliseconds);
        }
        else
        {
            logger.LogInformation(
                "Screen stability completed. " +
                "IsStable={IsStable}, " +
                "Samples={Samples}, " +
                "ConsecutiveMatches={ConsecutiveMatches}, " +
                "ElapsedMs={ElapsedMs}",
                stable,
                samples,
                matches,
                elapsed.TotalMilliseconds);
        }

        return new ScreenStabilityResult(
            stable,
            timedOut,
            samples);
    }
}
