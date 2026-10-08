using System.Diagnostics;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Polls fresh screenshots without invoking OCR or sharing sampling state between calls.
/// </summary>
public sealed class AndroidScreenStabilityGuard : IAndroidScreenStabilityGuard
{
    private readonly IScreenshotService _screenshots;
    private readonly IScreenFrameComparer _comparer;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Creates a screen stability guard.
    /// </summary>
    public AndroidScreenStabilityGuard(
        IScreenshotService screenshots,
        IScreenFrameComparer comparer,
        TimeProvider? timeProvider = null)
    {
        _screenshots = screenshots ?? throw new ArgumentNullException(nameof(screenshots));
        _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    /// <summary>
    /// Captures and compares consecutive frames until the policy is satisfied or times out.
    /// </summary>
    public async Task<ScreenStabilityResult> WaitForStabilityAsync(
        ScreenStabilityOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        cancellationToken.ThrowIfCancellationRequested();

        long started = _timeProvider.GetTimestamp();
        byte[]? previous = null;
        int matches = 0;
        int samples = 0;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_timeProvider.GetElapsedTime(started) >= options.Timeout)
                return new ScreenStabilityResult(false, true, samples);

            byte[] current = _screenshots.CaptureScreenshotBytes(null);
            samples++;
            if (current is null || current.Length == 0)
            {
                matches = 0;
                previous = null;
            }
            else
            {
                if (previous is not null &&
                    _comparer.AreSimilar(previous, current, options.MaximumDifferenceRatio))
                {
                    matches++;
                    if (matches >= options.RequiredConsecutiveMatches)
                        return new ScreenStabilityResult(true, false, samples);
                }
                else
                {
                    matches = 0;
                }

                // Capture providers may reuse their buffers; keep a private snapshot.
                previous = current.ToArray();
            }

            TimeSpan remaining = options.Timeout - _timeProvider.GetElapsedTime(started);
            if (remaining <= TimeSpan.Zero)
                return new ScreenStabilityResult(false, true, samples);
            TimeSpan delay = remaining < options.SampleInterval ? remaining : options.SampleInterval;
            await Task.Delay(delay, _timeProvider, cancellationToken).ConfigureAwait(false);
        }
    }
}
