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
    /// Gets or sets the target element to click.
    /// </summary>
    public Target? Target { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the configured position
    /// should be transformed from the reference resolution to the
    /// current device resolution.
    /// </summary>
    public bool ScalePosition { get; set; } = false;
}