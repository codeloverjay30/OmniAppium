using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Waits until Android screenshots satisfy a visual stability policy.
/// </summary>
public interface IAndroidScreenStabilityGuard
{
    /// <summary>
    /// Captures the final validated frame with independent buffer ownership.
    /// </summary>
    Task<StableScreenCaptureResult> CaptureStableAsync(
        ScreenStabilityOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Samples screenshots until stability, timeout, or cancellation.
    /// </summary>
    Task<ScreenStabilityResult> WaitForStabilityAsync(
        ScreenStabilityOptions options,
        CancellationToken cancellationToken = default);
}
