namespace OmniAppium.ConfigUtilityService.Models;

/// <summary>
/// Represents an automation job that performs a click operation.
/// </summary>
public sealed class ClickJob : Job
{
    /// <summary>
    /// Gets or sets the screen position to click.
    /// </summary>
    public Point? Position { get; set; }

    /// <summary>
    /// Gets or sets the UI target to click.
    /// </summary>
    public Target? Target { get; set; }

    /// <summary>
    /// Gets or sets how the configured position should be interpreted.
    /// </summary>
    public CoordinateMode CoordinateMode { get; set; }
        = CoordinateMode.Absolute;
}