using OmniAppium.ConfigUtilityService.Models;

namespace OmniAppium.EngineUtilityService.Utilities;

/// <summary>
/// Defines click operations for the automation engine.
/// </summary>
public interface IClickService
{
    /// <summary>
    /// Executes the specified click job.
    /// </summary>
    /// <param name="clickJob">The click job to execute.</param>
    void Click(ClickJob clickJob);

    /// <summary>
    /// Clicks an absolute position in the current device viewport.
    /// </summary>
    /// <param name="x">The absolute X coordinate.</param>
    /// <param name="y">The absolute Y coordinate.</param>
    void ClickAbsolute(double x, double y);

    /// <summary>
    /// Clicks a position defined in the reference-resolution coordinate space.
    /// </summary>
    /// <param name="x">The reference-resolution X coordinate.</param>
    /// <param name="y">The reference-resolution Y coordinate.</param>
    void ClickScaled(double x, double y);

    /// <summary>
    /// Performs an Appium tap at the specified absolute coordinate.
    /// </summary>
    /// <param name="x">The absolute X coordinate.</param>
    /// <param name="y">The absolute Y coordinate.</param>
    /// <param name="duration">The optional press duration.</param>
    void TapAt(
        int x,
        int y,
        TimeSpan duration = default);
}