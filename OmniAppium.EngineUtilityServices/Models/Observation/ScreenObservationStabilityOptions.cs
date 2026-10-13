using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Models.Observation;

/// <summary>
/// Defines the region and OCR requirements for a stable screen observation.
/// </summary>
public sealed record ScreenObservationStabilityOptions
{
    /// <summary>
    /// Gets the normalized region used for pixel stability.
    /// </summary>
    public required NormalizedScreenRegion Region { get; init; }

    /// <summary>
    /// Gets the required OCR target text.
    /// </summary>
    public required string TargetText { get; init; }

    /// <summary>
    /// Gets the maximum allowed pixel difference ratio.
    /// </summary>
    public double MaximumDifferenceRatio { get; init; } = 0.03;

    /// <summary>
    /// Gets the maximum absolute difference permitted in each decoded color channel (0-255).
    /// </summary>
    public byte ChannelTolerance { get; init; } = 8;

    /// <summary>
    /// Gets whether ROI difference masks are saved for each frame comparison.
    /// </summary>
    public bool SaveRoiDifferenceMasks { get; init; } = true;

    /// <summary>
    /// Gets an optional output directory for ROI difference masks.
    /// </summary>
    public string? RoiDifferenceMaskDirectory { get; init; }

    /// <summary>
    /// Gets the maximum allowed normalized target movement.
    /// </summary>
    public double MaximumCenterMovement { get; init; } = 0.02;

    /// <summary>
    /// Gets the required number of consecutive matching comparisons.
    /// </summary>
    public int RequiredConsecutiveMatches { get; init; } = 2;

    /// <summary>
    /// Gets the interval between completed observation attempts.
    /// </summary>
    public TimeSpan SampleInterval { get; init; } =
        TimeSpan.FromMilliseconds(250);

    /// <summary>
    /// Gets the overall observation timeout.
    /// </summary>
    public TimeSpan Timeout { get; init; } =
        TimeSpan.FromSeconds(45);

    /// <summary>
    /// Validates the configured observation requirements.
    /// </summary>
    public void Validate()
    {
        ArgumentNullException.ThrowIfNull(Region);
        Region.Validate();

        ArgumentException.ThrowIfNullOrWhiteSpace(TargetText);

        if (!double.IsFinite(MaximumDifferenceRatio) ||
            MaximumDifferenceRatio is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(MaximumDifferenceRatio),
                "Maximum difference ratio must be between 0 and 1.");
        }

        if (!double.IsFinite(MaximumCenterMovement) ||
            MaximumCenterMovement is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(MaximumCenterMovement),
                "Maximum center movement must be between 0 and 1.");
        }

        if (RequiredConsecutiveMatches <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(RequiredConsecutiveMatches),
                "Required consecutive matches must be positive.");
        }

        if (SampleInterval < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(SampleInterval),
                "Sample interval cannot be negative.");
        }

        if (Timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(Timeout),
                "Observation timeout must be positive.");
        }
    }
}