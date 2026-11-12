using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Produces read-only diagnostics for verified badge and OCR rectangle association.
/// </summary>
public static class NotificationAssociationDiagnostics
{
    /// <summary>
    /// Inspects the closest OCR rectangles without changing association evidence or authorizing actions.
    /// </summary>
    public static IReadOnlyList<NotificationBadgeDiagnostic> Inspect(
        IReadOnlyList<RedDotEvidence> badges,
        IReadOnlyList<OcrGroundedTarget> targets,
        NotificationAssociationOptions options,
        int imageWidth,
        int imageHeight,
        int nearestCount = 3)
    {
        ArgumentNullException.ThrowIfNull(badges);
        ArgumentNullException.ThrowIfNull(targets);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        if (imageWidth <= 0 || imageHeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(imageWidth), "Image dimensions must be positive.");
        if (nearestCount is < 1 or > 10)
            throw new ArgumentOutOfRangeException(nameof(nearestCount), "Nearest target count must be between 1 and 10.");

        var result = new List<NotificationBadgeDiagnostic>(badges.Count);
        foreach (RedDotEvidence badge in badges)
        {
            if (badge is null || badge.CandidateNodeIds is null)
                throw new ArgumentException("Badge evidence or candidate identifiers cannot be null.", nameof(badges));

            var b = badge.Bounds;
            bool validBadge = IsValid(b);
            double bx = b.TopLeft.X + b.Width / 2d;
            double by = b.TopLeft.Y + b.Height / 2d;
            bool badgeInside = validBadge && b.TopLeft.X >= 0 && b.TopLeft.Y >= 0 &&
                b.TopLeft.X + b.Width <= imageWidth && b.TopLeft.Y + b.Height <= imageHeight;

            var candidates = targets.Where(t => t is not null && !string.IsNullOrWhiteSpace(t.Id))
                .Select(t =>
                {
                    var r = t.Bounds;
                    bool valid = IsValid(r);
                    double right = r.TopLeft.X + r.Width;
                    double top = r.TopLeft.Y;
                    double dx = bx - right;
                    double dy = by - top;
                    double horizontal = Math.Min(options.MaxHorizontalGapPx, r.Width * options.MaxHorizontalGapRatio);
                    double vertical = Math.Min(options.MaxVerticalGapPx, r.Height * options.MaxVerticalGapRatio);
                    bool inside = valid && r.TopLeft.X >= 0 && r.TopLeft.Y >= 0 &&
                        right <= imageWidth && r.TopLeft.Y + r.Height <= imageHeight;
                    bool topRightAccepted = options.Enabled && validBadge && valid &&
                        dx >= -options.MaxInwardOverlapPx && dx <= horizontal &&
                        dy >= -vertical && dy <= options.MaxInwardOverlapPx;
                    double aboveLabelDistance = top - by;
                    bool iconAboveLabelAccepted = options.Enabled && options.EnableIconAboveLabel &&
                        validBadge && valid &&
                        Math.Abs(dx) <= options.IconAboveLabelRightAlignmentPx &&
                        aboveLabelDistance >= options.IconAboveLabelMinVerticalPx &&
                        aboveLabelDistance <= options.IconAboveLabelMaxVerticalPx;
                    bool accepted = topRightAccepted || iconAboveLabelAccepted;
                    string reason = !validBadge ? "InvalidBadgeBounds"
                        : !badgeInside ? "BadgeOutsideImage"
                        : !valid ? "InvalidTargetBounds"
                        : !inside ? "TargetOutsideImage"
                        : !options.Enabled ? "GeometryDisabled"
                        : iconAboveLabelAccepted ? "AcceptedIconAboveLabel"
                        : dx < -options.MaxInwardOverlapPx ? "TooFarInsideLeft"
                        : dx > horizontal ? "TooFarRight"
                        : dy < -vertical ? "TooFarAbove"
                        : dy > options.MaxInwardOverlapPx ? "TooFarBelow"
                        : "Accepted";
                    double distance = valid && validBadge
                        ? Math.Sqrt(dx * dx + dy * dy) : double.PositiveInfinity;
                    return new NotificationTargetDiagnostic(t.Id, t.Text, r.TopLeft.X,
                        r.TopLeft.Y, r.Width, r.Height, dx, dy, horizontal, vertical,
                        distance, accepted, reason);
                })
                .OrderBy(x => x.Distance)
                .ThenBy(x => x.TargetId, StringComparer.Ordinal)
                .Take(nearestCount)
                .ToArray();

            result.Add(new NotificationBadgeDiagnostic(b.TopLeft.X, b.TopLeft.Y,
                b.Width, b.Height, bx, by, badgeInside,
                badge.CandidateNodeIds.ToArray(), candidates));
        }
        return result.AsReadOnly();
    }

    /// <summary>Validates finite positive rectangle dimensions.</summary>
    private static bool IsValid(CoordinateUtilityServices.Rectangle r) =>
        double.IsFinite(r.TopLeft.X) && double.IsFinite(r.TopLeft.Y) &&
        double.IsFinite(r.Width) && double.IsFinite(r.Height) &&
        r.Width > 0 && r.Height > 0;
}

/// <summary>Describes one verified badge and nearby OCR rectangles.</summary>
public sealed record NotificationBadgeDiagnostic(
    double X, double Y, double Width, double Height,
    double CenterX, double CenterY, bool IsInsideImage,
    IReadOnlyList<string> CandidateNodeIds,
    IReadOnlyList<NotificationTargetDiagnostic> NearestTargets);

/// <summary>Describes a candidate OCR rectangle and its geometric rejection reason.</summary>
public sealed record NotificationTargetDiagnostic(
    string TargetId, string Text, double X, double Y,
    double Width, double Height, double DeltaX, double DeltaY,
    double AllowedHorizontal, double AllowedVertical,
    double Distance, bool Accepted, string Reason);