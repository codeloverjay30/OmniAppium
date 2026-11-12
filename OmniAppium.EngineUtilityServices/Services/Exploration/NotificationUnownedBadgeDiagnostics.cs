using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Classifies unowned notification badges for observation-only investigation.
/// </summary>
public static class NotificationUnownedBadgeDiagnostics
{
    /// <summary>
    /// Produces diagnostic hints without changing badge ownership or authorizing an action.
    /// </summary>
    public static IReadOnlyList<NotificationUnownedBadgeDiagnostic> Inspect(
        IReadOnlyList<RedDotEvidence> badges,
        IReadOnlyList<OcrGroundedTarget> targets,
        NotificationAssociationOptions options,
        int imageWidth,
        int imageHeight)
    {
        ArgumentNullException.ThrowIfNull(badges);
        ArgumentNullException.ThrowIfNull(targets);
        ArgumentNullException.ThrowIfNull(options);

        IReadOnlyList<NotificationBadgeDiagnostic> inspected =
            NotificationAssociationDiagnostics.Inspect(
                badges, targets, options, imageWidth, imageHeight, nearestCount: 10);

        var result = new List<NotificationUnownedBadgeDiagnostic>();
        for (int index = 0; index < badges.Count; index++)
        {
            RedDotEvidence badge = badges[index];
            IReadOnlyList<string> owners = NotificationAssociationGeometry.Resolve(
                badge, targets, options);
            if (owners.Count != 0)
                continue;

            NotificationBadgeDiagnostic diagnostic = inspected[index];
            string region = ResolveRegion(diagnostic.CenterX, diagnostic.CenterY,
                imageWidth, imageHeight);
            NotificationTargetDiagnostic? nearest = diagnostic.NearestTargets.FirstOrDefault();
            string hint = !diagnostic.IsInsideImage
                ? "OutOfFrame"
                : nearest is null
                    ? "NoOcrTargets"
                    : nearest.Reason == "Accepted" ||
                      nearest.Reason == "AcceptedIconAboveLabel"
                        ? "InspectGeometryConsistency"
                        : "GeometryRejectedOrMissingIconBounds";

            NotificationNearbyOcrDiagnostic[] nearby = diagnostic.NearestTargets
                .Select(target => new NotificationNearbyOcrDiagnostic(
                    target.TargetId,
                    target.Text,
                    target.X,
                    target.Y,
                    target.Width,
                    target.Height,
                    target.DeltaX,
                    target.DeltaY,
                    target.Distance,
                    target.Reason,
                    target.Width / imageWidth,
                    IsPossiblyMerged(target.Width, imageWidth)))
                .ToArray();

            result.Add(new NotificationUnownedBadgeDiagnostic(
                index, diagnostic.CenterX, diagnostic.CenterY,
                region, hint, nearest?.TargetId, nearest?.Text,
                nearest?.Reason, nearest?.DeltaX, nearest?.DeltaY,
                nearest?.Distance)
            {
                NearbyTargets = Array.AsReadOnly(nearby),
                OcrTargetWidthRatio = nearest is null ? null : nearest.Width / imageWidth,
                PossibleMergedOcr = nearby.Any(target => target.PossibleMergedOcr),
                CandidateVisualType = "Unknown",
                SuggestedLayout = SuggestLayout(nearest, options),
                GroundTruthStatus = "Unreviewed",
                RequiresManualReview = true
            });
        }

        return result.AsReadOnly();
    }

    /// <summary>
    /// Flags unusually wide OCR rectangles for manual segmentation review.
    /// </summary>
    private static bool IsPossiblyMerged(double width, int imageWidth) =>
        double.IsFinite(width) && width >= 250d &&
        width / imageWidth >= 0.12d;

    /// <summary>
    /// Suggests a spatial layout without claiming a UI owner.
    /// </summary>
    private static string SuggestLayout(
        NotificationTargetDiagnostic? target,
        NotificationAssociationOptions options)
    {
        if (target is null || !double.IsFinite(target.DeltaY) ||
            !double.IsFinite(target.DeltaX))
            return "Unknown";

        if (Math.Abs(target.DeltaX) > options.MaxHorizontalGapPx)
            return "Unknown";

        if (target.DeltaY < 0)
            return "IconAboveLabel";

        if (target.DeltaY > 0)
            return "LabelAboveIcon";

        return "Unknown";
    }

    /// <summary>
    /// Returns a coarse screen region for diagnostics only.
    /// </summary>
    private static string ResolveRegion(
        double x, double y, int imageWidth, int imageHeight)
    {
        if (x < 0 || y < 0 || x >= imageWidth || y >= imageHeight)
            return "OutsideImage";
        string vertical = y < imageHeight / 3d ? "Top"
            : y >= imageHeight * 2d / 3d ? "Bottom" : "Middle";
        string horizontal = x < imageWidth / 3d ? "Left"
            : x >= imageWidth * 2d / 3d ? "Right" : "Center";
        return vertical + horizontal;
    }
}

/// <summary>
/// Describes an unowned badge and the nearest OCR target without claiming ownership.
/// </summary>
public sealed record NotificationUnownedBadgeDiagnostic(
    int BadgeIndex,
    double CenterX,
    double CenterY,
    string Region,
    string Hint,
    string? NearestTargetId,
    string? NearestTargetText,
    string? NearestRejectionReason,
    double? DeltaX,
    double? DeltaY,
    double? Distance)
{
    /// <summary>Gets the nearest OCR candidates without implying ownership.</summary>
    public IReadOnlyList<NotificationNearbyOcrDiagnostic> NearbyTargets { get; init; } =
        Array.Empty<NotificationNearbyOcrDiagnostic>();

    /// <summary>Gets the nearest OCR rectangle width divided by screenshot width.</summary>
    public double? OcrTargetWidthRatio { get; init; }

    /// <summary>Gets whether any nearby OCR rectangle may merge multiple labels.</summary>
    public bool PossibleMergedOcr { get; init; }

    /// <summary>Gets the human-annotated visual type, or Unknown.</summary>
    public string CandidateVisualType { get; init; } = "Unknown";

    /// <summary>Gets a non-authoritative spatial layout hint.</summary>
    public string SuggestedLayout { get; init; } = "Unknown";

    /// <summary>Gets the manual ground-truth review state.</summary>
    public string GroundTruthStatus { get; init; } = "Unreviewed";

    /// <summary>Gets whether human inspection is still required.</summary>
    public bool RequiresManualReview { get; init; } = true;
}

/// <summary>
/// Captures one OCR candidate for read-only badge investigation.
/// </summary>
public sealed record NotificationNearbyOcrDiagnostic(
    string TargetId,
    string Text,
    double X,
    double Y,
    double Width,
    double Height,
    double DeltaX,
    double DeltaY,
    double Distance,
    string RejectionReason,
    double OcrTargetWidthRatio,
    bool PossibleMergedOcr);
