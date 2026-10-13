namespace OmniAppium.EngineUtilityServices.Models.Stability;

/// <summary>
/// Defines a rectangular screen region in normalized coordinates.
/// </summary>
public sealed record NormalizedScreenRegion(double X, double Y, double Width, double Height)
{
    /// <summary>
    /// Validates that the region is nonempty and fits inside the normalized screen.
    /// </summary>
    public void Validate()
    {
        if (!double.IsFinite(X) || !double.IsFinite(Y) ||
            !double.IsFinite(Width) || !double.IsFinite(Height) ||
            X < 0 || Y < 0 || Width <= 0 || Height <= 0 ||
            X >= 1 || Y >= 1 || Width > 1 - X || Height > 1 - Y)
            throw new ArgumentOutOfRangeException(nameof(Width),
                "The normalized screen region must be nonempty and within [0, 1].");
    }
}
