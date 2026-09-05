namespace OmniAppium.ConfigUtilityService.Models;

/// <summary>
/// Defines how a configured screen coordinate should be interpreted.
/// </summary>
public enum CoordinateMode
{
    /// <summary>
    /// Uses the configured coordinate directly in the current device viewport.
    /// </summary>
    Absolute = 0,

    /// <summary>
    /// Transforms the configured coordinate from the reference resolution
    /// to the current device resolution.
    /// </summary>
    ReferenceResolution = 1
}