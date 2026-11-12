using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Describes OCR evidence near a notification badge without inferring ownership or character bounds.
/// </summary>
public sealed record NotificationOcrSegmentationFinding(
    int BadgeIndex,
    string TargetId,
    string Text,
    bool IsPrimaryLabelCandidate,
    bool ContainsMultipleWhitespaceSeparatedTokens,
    double DistancePx);

/// <summary>
/// Inspects same-frame OCR evidence around badges for read-only segmentation review.
/// </summary>
public static class NotificationOcrSegmentationDiagnostics
{
    /// <summary>
    /// Returns nearby OCR evidence in stable distance order without modifying targets or assigning owners.
    /// </summary>
    public static IReadOnlyList<NotificationOcrSegmentationFinding> Inspect(
        IReadOnlyList<RedDotEvidence> badges,
        IReadOnlyList<OcrGroundedTarget> targets,
        double maxDistancePx = 240)
    {
        ArgumentNullException.ThrowIfNull(badges);
        ArgumentNullException.ThrowIfNull(targets);
        if (!double.IsFinite(maxDistancePx) || maxDistancePx <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxDistancePx),
                "Maximum OCR review distance must be finite and positive.");

        var findings = new List<NotificationOcrSegmentationFinding>();
        for (int index = 0; index < badges.Count; index++)
        {
            RedDotEvidence badge = badges[index]
                ?? throw new ArgumentException("Badge entries cannot be null.", nameof(badges));
            if (!IsValid(badge.Bounds))
                continue;

            double centerX = badge.Bounds.TopLeft.X + badge.Bounds.Width / 2;
            double centerY = badge.Bounds.TopLeft.Y + badge.Bounds.Height / 2;
            foreach (OcrGroundedTarget target in targets)
            {
                if (target is null || string.IsNullOrWhiteSpace(target.Id) ||
                    !IsValid(target.Bounds))
                    continue;

                // Measure the shortest distance to the actual OCR rectangle, not its center.
                double left = target.Bounds.TopLeft.X;
                double top = target.Bounds.TopLeft.Y;
                double dx = Math.Max(left - centerX,
                    Math.Max(0, centerX - (left + target.Bounds.Width)));
                double dy = Math.Max(top - centerY,
                    Math.Max(0, centerY - (top + target.Bounds.Height)));
                double distance = Math.Sqrt(dx * dx + dy * dy);
                if (distance > maxDistancePx)
                    continue;

                ReadOnlySpan<char> text = (target.Text ?? string.Empty).AsSpan();
                bool multipleTokens = false;
                bool seenToken = false;
                bool inToken = false;
                foreach (char c in text)
                {
                    if (char.IsWhiteSpace(c))
                    {
                        inToken = false;
                    }
                    else if (!inToken)
                    {
                        if (seenToken)
                            multipleTokens = true;
                        seenToken = true;
                        inToken = true;
                    }
                }

                findings.Add(new NotificationOcrSegmentationFinding(
                    index, target.Id, target.Text ?? string.Empty,
                    NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(target.Text),
                    multipleTokens, distance));
            }
        }

        return findings.OrderBy(f => f.BadgeIndex)
            .ThenBy(f => f.DistancePx)
            .ThenBy(f => f.TargetId, StringComparer.Ordinal)
            .ToArray();
    }

    /// <summary>
    /// Validates finite and nonempty image-space rectangles.
    /// </summary>
    private static bool IsValid(CoordinateUtilityServices.Rectangle bounds) =>
        double.IsFinite(bounds.TopLeft.X) &&
        double.IsFinite(bounds.TopLeft.Y) &&
        double.IsFinite(bounds.Width) &&
        double.IsFinite(bounds.Height) &&
        bounds.Width > 0 && bounds.Height > 0;
}