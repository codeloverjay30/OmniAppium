using CoordinateUtilityServices;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Configures conservative notification evidence filtering without scene-specific assumptions.
/// </summary>
public sealed record NotificationEvidenceFilterOptions
{
    /// <summary>Gets the maximum allowed center displacement in pixels.</summary>
    public double CenterTolerancePixels { get; init; } = 10d;

    /// <summary>Gets the maximum allowed width or height difference in pixels.</summary>
    public double SizeTolerancePixels { get; init; } = 8d;

    /// <summary>Gets the minimum number of subsequent observations required.</summary>
    public int MinimumSubsequentFrames { get; init; } = 2;

    /// <summary>Gets the allowed screen regions in normalized coordinates; empty means unrestricted.</summary>
    public IReadOnlyList<NormalizedRectangle> AllowedRegions { get; init; } =
        Array.Empty<NormalizedRectangle>();
}