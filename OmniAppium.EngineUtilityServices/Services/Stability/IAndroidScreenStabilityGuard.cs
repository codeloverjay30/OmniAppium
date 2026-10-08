using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Waits until Android screenshots satisfy a visual stability policy.
/// </summary>
public interface IAndroidScreenStabilityGuard
{
    /// <summary>
    /// Samples screenshots until stability, timeout, or cancellation.
    /// </summary>
    Task<ScreenStabilityResult> WaitForStabilityAsync(
        ScreenStabilityOptions options,
        CancellationToken cancellationToken = default);
}
