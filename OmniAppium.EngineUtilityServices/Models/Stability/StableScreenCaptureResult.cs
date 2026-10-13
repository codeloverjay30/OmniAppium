namespace OmniAppium.EngineUtilityServices.Models.Stability;

/// <summary>
/// Represents a stable screenshot capture and its stability decision.
/// The validated image is copied on construction and on retrieval so callers
/// cannot mutate the owned snapshot through a shared array.
/// </summary>
public sealed class StableScreenCaptureResult
{
    private readonly byte[]? _image;

    /// <summary>
    /// Gets the stability decision for this capture.
    /// </summary>
    public ScreenStabilityResult Stability { get; }

    /// <summary>
    /// Initializes the result with an independently owned image snapshot.
    /// </summary>
    public StableScreenCaptureResult(
        ScreenStabilityResult stability,
        byte[]? validatedImage)
    {
        Stability = stability ?? throw new ArgumentNullException(nameof(stability));
        if (stability.IsStable && !stability.TimedOut &&
            validatedImage is not { Length: > 0 })
        {
            throw new ArgumentException(
                "A successful stability result requires a non-empty validated image.",
                nameof(validatedImage));
        }

        if ((!stability.IsStable || stability.TimedOut) && validatedImage is not null)
        {
            throw new ArgumentException(
                "An unsuccessful stability result must not contain an image.",
                nameof(validatedImage));
        }

        _image = validatedImage?.ToArray();
    }

    /// <summary>
    /// Returns a defensive copy of the validated image.
    /// </summary>
    public byte[] GetImageBytes()
    {
        if (_image is null)
        {
            throw new InvalidOperationException(
                "No validated image is available for an unsuccessful stability result.");
        }

        return _image.ToArray();
    }
}
