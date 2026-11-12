using CoordinateUtilityServices;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Applies fail-closed spatial and temporal checks to red-dot evidence without image decoding.
/// </summary>
public sealed class NotificationEvidenceFilter : INotificationEvidenceFilter
{
    /// <summary>
    /// Retains valid baseline candidates present in every supplied subsequent frame.
    /// </summary>
    public IReadOnlyList<RedDotEvidence> Filter(
        IReadOnlyList<RedDotEvidence> baseline,
        IReadOnlyList<IReadOnlyList<RedDotEvidence>> subsequentFrames,
        int imageWidth,
        int imageHeight,
        NotificationEvidenceFilterOptions options)
    {
        ArgumentNullException.ThrowIfNull(baseline);
        ArgumentNullException.ThrowIfNull(subsequentFrames);
        ArgumentNullException.ThrowIfNull(options);
        if (imageWidth <= 0 || imageHeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(imageWidth), "Image dimensions must be positive.");
        if (!double.IsFinite(options.CenterTolerancePixels) || options.CenterTolerancePixels < 0 ||
            !double.IsFinite(options.SizeTolerancePixels) || options.SizeTolerancePixels < 0)
            throw new ArgumentOutOfRangeException(nameof(options), "Tolerances must be finite and non-negative.");
        if (options.MinimumSubsequentFrames < 1)
            throw new ArgumentOutOfRangeException(nameof(options), "Minimum subsequent frames must be positive.");
        if (subsequentFrames.Count < options.MinimumSubsequentFrames)
            throw new ArgumentException("Insufficient subsequent frames for temporal validation.", nameof(subsequentFrames));
        if (subsequentFrames.Any(frame => frame is null))
            throw new ArgumentException("Subsequent frames cannot contain null.", nameof(subsequentFrames));
        if (options.AllowedRegions is null)
            throw new ArgumentException("Allowed regions cannot be null.", nameof(options));

        // Convert normalized regions once; geometry validation belongs to the coordinate library.
        Rectangle[] allowedRectangles = options.AllowedRegions
            .Select(region => region.ToRectangle(imageWidth, imageHeight))
            .ToArray();

        var accepted = new List<RedDotEvidence>();
        double toleranceSquared = options.CenterTolerancePixels * options.CenterTolerancePixels;
        foreach (RedDotEvidence candidate in baseline)
        {
            if (!TryCenter(candidate, out double x, out double y))
                continue;
            if (x < 0 || y < 0 || x >= imageWidth || y >= imageHeight)
                continue;
            if (allowedRectangles.Length > 0 &&
                !allowedRectangles.Any(region => region.Contains(new Point(x, y))))
                continue;

            bool stable = subsequentFrames.All(frame => frame.Any(other =>
            {
                if (!TryCenter(other, out double ox, out double oy))
                    return false;
                double dx = ox - x;
                double dy = oy - y;
                return dx * dx + dy * dy <= toleranceSquared &&
                    Math.Abs(other.Bounds.Width - candidate.Bounds.Width) <= options.SizeTolerancePixels &&
                    Math.Abs(other.Bounds.Height - candidate.Bounds.Height) <= options.SizeTolerancePixels;
            }));
            if (stable)
                accepted.Add(candidate);
        }
        return accepted.AsReadOnly();
    }

    /// <summary>
    /// Extracts a finite center point from valid evidence.
    /// </summary>
    private static bool TryCenter(RedDotEvidence? evidence, out double x, out double y)
    {
        x = y = 0;
        if (evidence is null)
            return false;
        var bounds = evidence.Bounds;
        if (!double.IsFinite(bounds.TopLeft.X) || !double.IsFinite(bounds.TopLeft.Y) ||
            !double.IsFinite(bounds.Width) || !double.IsFinite(bounds.Height) ||
            bounds.Width <= 0 || bounds.Height <= 0)
            return false;
        x = bounds.TopLeft.X + bounds.Width / 2d;
        y = bounds.TopLeft.Y + bounds.Height / 2d;
        return double.IsFinite(x) && double.IsFinite(y);
    }
}