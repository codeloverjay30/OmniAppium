namespace OmniAppium.EngineUtilityServices.Models.Stability;

/// <summary>
/// Configures the Android screen stability sampling policy.
/// </summary>
public sealed record ScreenStabilityOptions
{
    /// <summary>Gets the number of consecutive matching frame pairs required.</summary>
    public int RequiredConsecutiveMatches { get; init; } = 2;

    /// <summary>Gets the interval between captures.</summary>
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromMilliseconds(100);

    /// <summary>Gets the maximum time allowed for stability detection.</summary>
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(5);

    /// <summary>Gets the maximum ratio of different pixels, from zero to one.</summary>
    public double MaximumDifferenceRatio { get; init; } = 0.01;

    /// <summary>
    /// Validates the stability policy.
    /// </summary>
    public void Validate()
    {
        if (RequiredConsecutiveMatches < 1)
            throw new ArgumentOutOfRangeException(nameof(RequiredConsecutiveMatches),
                "RequiredConsecutiveMatches must be positive.");
        if (SampleInterval <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(SampleInterval),
                "SampleInterval must be positive.");
        if (Timeout <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(Timeout),
                "Timeout must be positive.");
        if (!double.IsFinite(MaximumDifferenceRatio) ||
            MaximumDifferenceRatio is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(MaximumDifferenceRatio),
                "MaximumDifferenceRatio must be between zero and one.");
    }
}
