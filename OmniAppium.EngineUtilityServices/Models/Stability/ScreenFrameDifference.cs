namespace OmniAppium.EngineUtilityServices.Models.Stability;

/// <summary>
/// Describes the measured difference between two decoded screen frames.
/// </summary>
public sealed record ScreenFrameDifference(
    double DifferenceRatio,
    long DifferentPixels,
    long TotalPixels,
    bool DimensionsMatch);
