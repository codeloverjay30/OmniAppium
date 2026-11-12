namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Configures conservative geometric association for verified notification badges.
/// </summary>
public sealed class NotificationAssociationOptions
{
    /// <summary>Gets or sets whether geometric association is enabled.</summary>
    public bool Enabled { get; set; } = true;
    /// <summary>Gets or sets the maximum horizontal distance in pixels.</summary>
    public double MaxHorizontalGapPx { get; set; } = 48;
    /// <summary>Gets or sets the maximum vertical distance in pixels.</summary>
    public double MaxVerticalGapPx { get; set; } = 32;
    /// <summary>Gets or sets the maximum horizontal distance relative to OCR width.</summary>
    public double MaxHorizontalGapRatio { get; set; } = 1;
    /// <summary>Gets or sets the maximum vertical distance relative to OCR height.</summary>
    public double MaxVerticalGapRatio { get; set; } = 1;
    /// <summary>Gets or sets the allowed inward overlap in pixels.</summary>
    public double MaxInwardOverlapPx { get; set; } = 8;

    /// <summary>Enables association of badges above icon labels.</summary>
    public bool EnableIconAboveLabel { get; set; } = true;
    /// <summary>Minimum distance from badge center to label top.</summary>
    public double IconAboveLabelMinVerticalPx { get; set; } = 70;
    /// <summary>Maximum distance from badge center to label top.</summary>
    public double IconAboveLabelMaxVerticalPx { get; set; } = 120;
    /// <summary>Maximum absolute distance from badge center to label right edge.</summary>
    public double IconAboveLabelRightAlignmentPx { get; set; } = 12;

    /// <summary>Rejects invalid or unbounded association settings.</summary>
    public void Validate()
    {
        Check(MaxHorizontalGapPx, 0, 256, nameof(MaxHorizontalGapPx));
        Check(MaxVerticalGapPx, 0, 256, nameof(MaxVerticalGapPx));
        Check(MaxHorizontalGapRatio, 0, 4, nameof(MaxHorizontalGapRatio));
        Check(MaxVerticalGapRatio, 0, 4, nameof(MaxVerticalGapRatio));
        Check(MaxInwardOverlapPx, 0, 64, nameof(MaxInwardOverlapPx));
        Check(IconAboveLabelMinVerticalPx, 0, 256, nameof(IconAboveLabelMinVerticalPx));
        Check(IconAboveLabelMaxVerticalPx, 0, 256, nameof(IconAboveLabelMaxVerticalPx));
        Check(IconAboveLabelRightAlignmentPx, 0, 64, nameof(IconAboveLabelRightAlignmentPx));
        if (IconAboveLabelMinVerticalPx > IconAboveLabelMaxVerticalPx)
            throw new ArgumentOutOfRangeException(nameof(IconAboveLabelMinVerticalPx),
                "Icon-above-label vertical band must have minimum <= maximum.");
    }

    /// <summary>Checks one finite configuration value.</summary>
    private static void Check(double value, double minimum, double maximum, string name)
    {
        if (!double.IsFinite(value) || value < minimum || value > maximum)
            throw new ArgumentOutOfRangeException(name, "Association setting must be finite and within its safe range.");
    }
}
