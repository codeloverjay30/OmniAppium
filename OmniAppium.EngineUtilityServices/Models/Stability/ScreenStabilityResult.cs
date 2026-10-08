namespace OmniAppium.EngineUtilityServices.Models.Stability;

/// <summary>
/// Describes the outcome of a screen stability wait.
/// </summary>
/// <param name="IsStable">Whether the required number of matching frame pairs was observed.</param>
/// <param name="TimedOut">Whether the wait reached its time limit.</param>
/// <param name="SampleCount">The number of screenshots captured.</param>
public sealed record ScreenStabilityResult(bool IsStable, bool TimedOut, int SampleCount);
