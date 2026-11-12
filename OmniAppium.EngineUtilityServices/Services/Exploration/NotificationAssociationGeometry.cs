using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Resolves possible OCR owners of verified notification badges without ranking ambiguous matches.
/// </summary>
public static class NotificationAssociationGeometry
{
    /// <summary>Returns all plausible owner identifiers, preserving existing candidates.</summary>
    public static IReadOnlyList<string> Resolve(
        RedDotEvidence badge,
        IReadOnlyList<OcrGroundedTarget> targets,
        NotificationAssociationOptions options)
    {
        ArgumentNullException.ThrowIfNull(badge);
        ArgumentNullException.ThrowIfNull(targets);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        if (badge.CandidateNodeIds is null)
            throw new ArgumentException("Candidate identifiers cannot be null.", nameof(badge));

        var eligibleIds = targets.Where(t => t is not null &&
            NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(t.Text))
            .Select(t => t.Id)
            .ToHashSet(StringComparer.Ordinal);
        var owners = new HashSet<string>(badge.CandidateNodeIds.Where(eligibleIds.Contains), StringComparer.Ordinal);
        if (!options.Enabled) 
            return owners.ToArray();

        var b = badge.Bounds;
        if (!Valid(b)) 
            return owners.ToArray();
        double bx = b.TopLeft.X + b.Width / 2d;
        double by = b.TopLeft.Y + b.Height / 2d;
        foreach (OcrGroundedTarget target in targets)
        {
            if (target is null ||
                string.IsNullOrWhiteSpace(target.Id) ||
                !NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(target.Text) ||
                !Valid(target.Bounds))
                continue;
            var t = target.Bounds;
            double right = t.TopLeft.X + t.Width;
            double top = t.TopLeft.Y;
            double horizontal = Math.Min(options.MaxHorizontalGapPx, t.Width * options.MaxHorizontalGapRatio);
            double vertical = Math.Min(options.MaxVerticalGapPx, t.Height * options.MaxVerticalGapRatio);
            bool topRight = bx >= right - options.MaxInwardOverlapPx &&
                bx <= right + horizontal &&
                by >= top - vertical &&
                by <= top + options.MaxInwardOverlapPx;
            double aboveLabelDistance = top - by;
            bool iconAboveLabel = options.EnableIconAboveLabel &&
                Math.Abs(bx - right) <= options.IconAboveLabelRightAlignmentPx &&
                aboveLabelDistance >= options.IconAboveLabelMinVerticalPx &&
                aboveLabelDistance <= options.IconAboveLabelMaxVerticalPx;
            if (topRight || iconAboveLabel)
                owners.Add(target.Id);
        }
        return owners.ToArray();
    }

    /// <summary>
    /// Resolves badge ownership through independently supplied icon bounds, never through a missing OCR label.
    /// </summary>
    public static IReadOnlyList<string> ResolveViaIcons(
        RedDotEvidence badge,
        IReadOnlyList<OcrGroundedTarget> targets,
        IReadOnlyList<NotificationIconTarget> icons,
        NotificationAssociationOptions options)
    {
        ArgumentNullException.ThrowIfNull(badge);
        ArgumentNullException.ThrowIfNull(targets);
        ArgumentNullException.ThrowIfNull(icons);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        if (badge.CandidateNodeIds is null)
            throw new ArgumentException("Candidate identifiers cannot be null.", nameof(badge));
        var owners = new HashSet<string>(Resolve(badge, targets, options), StringComparer.Ordinal);
        if (!options.Enabled || !Valid(badge.Bounds))
            return owners.ToArray();

        var targetIds = targets.Where(t => t is not null &&
            !string.IsNullOrWhiteSpace(t.Id) &&
            NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(t.Text))
            .Select(t => t.Id)
            .ToHashSet(StringComparer.Ordinal);
        double bx = badge.Bounds.TopLeft.X + badge.Bounds.Width / 2d;
        double by = badge.Bounds.TopLeft.Y + badge.Bounds.Height / 2d;
        foreach (NotificationIconTarget icon in icons)
        {
            if (icon is null || !targetIds.Contains(icon.TargetId) || !Valid(icon.IconBounds))
                continue;
            var r = icon.IconBounds;
            double right = r.TopLeft.X + r.Width;
            double top = r.TopLeft.Y;
            double tolerance = Math.Min(40, Math.Max(badge.Bounds.Width, badge.Bounds.Height) * 1.5);
            if (Math.Abs(bx - right) <= tolerance && Math.Abs(by - top) <= tolerance)
                owners.Add(icon.TargetId);
        }
        return owners.ToArray();
    }

    /// <summary>Checks rectangle geometry before computing its center.</summary>
    private static bool Valid(CoordinateUtilityServices.Rectangle bounds) =>
        double.IsFinite(bounds.TopLeft.X) && double.IsFinite(bounds.TopLeft.Y) &&
        double.IsFinite(bounds.Width) && double.IsFinite(bounds.Height) &&
        bounds.Width > 0 && bounds.Height > 0;
}