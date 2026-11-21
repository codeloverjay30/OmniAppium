using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Provides read-only, same-frame Hough-circle and OCR alignment diagnostics without changing ownership.
/// </summary>
public sealed class SameFrameNotificationIconDiagnostics
{
    /// <summary>
    /// Inspects circles near a badge and explains why their OCR labels cannot form a unique icon link.
    /// </summary>
    public IReadOnlyList<NotificationIconDiagnostic> Diagnose(
        ReadOnlyMemory<byte> screenshot,
        IReadOnlyList<OcrGroundedTarget> targets,
        double badgeCenterX,
        double badgeCenterY,
        double searchRadius = 160)
    {
        if (screenshot.IsEmpty)
            throw new ArgumentException("Screenshot bytes are required.", nameof(screenshot));
        ArgumentNullException.ThrowIfNull(targets);
        if (!double.IsFinite(badgeCenterX) || !double.IsFinite(badgeCenterY) ||
            !double.IsFinite(searchRadius) || searchRadius <= 0)
            throw new ArgumentOutOfRangeException(nameof(searchRadius),
                "Badge coordinates and search radius must be finite and the radius positive.");
        if (targets.Any(t => t is null || string.IsNullOrWhiteSpace(t.Id)) ||
            targets.GroupBy(t => t.Id, StringComparer.Ordinal).Any(g => g.Count() != 1))
            throw new ArgumentException("OCR targets must have unique non-empty identifiers.", nameof(targets));

        using Mat image = Cv2.ImDecode(screenshot.ToArray(), ImreadModes.Color);
        if (image.Empty())
            throw new InvalidOperationException("Cannot decode same-frame icon screenshot.");
        using Mat gray = new();
        using Mat blurred = new();
        Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
        Cv2.GaussianBlur(gray, blurred, new Size(7, 7), 1.5);

        CircleSegment[] circles = Cv2.HoughCircles(
            blurred, HoughModes.Gradient, dp: 1.2, minDist: 55,
            param1: 110, param2: 38, minRadius: 28, maxRadius: 82);

        var result = new List<NotificationIconDiagnostic>();
        foreach (CircleSegment circle in circles)
        {
            double cx = circle.Center.X;
            double cy = circle.Center.Y;
            if (Math.Abs(cx - badgeCenterX) > searchRadius ||
                Math.Abs(cy - badgeCenterY) > searchRadius)
                continue;

            double radius = circle.Radius;
            double left = cx - radius;
            double top = cy - radius;
            double diameter = radius * 2;
            if (left < 0 || top < 0 ||
                left + diameter > image.Width || top + diameter > image.Height)
            {
                result.Add(new NotificationIconDiagnostic(cx, cy, radius,
                    "CircleOutsideFrame", null, null, 0));
                continue;
            }

            var evaluated = targets.Select(t =>
            {
                var r = t.Bounds;
                bool valid = r.Width > 0 && r.Height > 0 &&
                    double.IsFinite(r.TopLeft.X) && double.IsFinite(r.TopLeft.Y) &&
                    double.IsFinite(r.Width) && double.IsFinite(r.Height);
                double dx = valid ? Math.Abs(r.TopLeft.X + r.Width / 2 - cx) : double.PositiveInfinity;
                double gap = valid ? r.TopLeft.Y - (top + diameter) : double.PositiveInfinity;
                string reason = !valid ? "InvalidLabelBounds" :
                    !NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(t.Text) ? "SecondaryOrInvalidLabel" :
                    dx > Math.Min(22, radius * 0.35) ? "LabelCenterMisaligned" :
                    gap < -10 || gap > 36 ? "LabelVerticalGapRejected" :
                    r.Width > diameter * 1.8 ? "LabelTooWide" : "Aligned";
                return (Target: t, Reason: reason, Distance: dx + Math.Abs(gap));
            }).OrderBy(x => x.Distance).ToArray();

            var aligned = evaluated.Where(x => x.Reason == "Aligned").ToArray();
            string reasonCode = aligned.Length switch
            {
                0 => "NoAlignedLabel",
                1 => "UniqueAlignedLabelDiagnosticOnly",
                _ => "CompetingAlignedLabels"
            };
            var nearest = evaluated.FirstOrDefault();
            result.Add(new NotificationIconDiagnostic(cx, cy, radius,
                reasonCode,
                nearest.Target?.Id,
                nearest.Target?.Text,
                aligned.Length,
                nearest.Target is null ? "NoOcrTargets" : nearest.Reason));
        }

        if (result.Count == 0)
            result.Add(new NotificationIconDiagnostic(
                badgeCenterX, badgeCenterY, 0,
                "NoHoughCircleNearBadge", null, null, 0));

        return result;
    }
}

/// <summary>
/// Describes a single read-only icon detection diagnostic without granting ownership.
/// </summary>
public sealed record NotificationIconDiagnostic(
    double CenterX,
    double CenterY,
    double Radius,
    string Reason,
    string? NearestTargetId,
    string? NearestText,
    int AlignedLabelCount,
    string? NearestLabelReason = null);
