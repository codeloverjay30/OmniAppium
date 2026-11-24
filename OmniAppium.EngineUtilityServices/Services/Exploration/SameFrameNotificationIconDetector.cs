using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using CoordinateUtilityServices;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Detects circular icon boundaries from one screenshot and links them only to uniquely aligned OCR labels.
/// </summary>
public sealed class SameFrameNotificationIconDetector
{
    /// <summary>
    /// Returns conservative icon-to-OCR links; ambiguous or missing OCR labels are never synthesized.
    /// </summary>
    public IReadOnlyList<NotificationIconTarget> Detect(
        ReadOnlyMemory<byte> screenshot,
        IReadOnlyList<OcrGroundedTarget> targets)
    {
        if (screenshot.IsEmpty)
            throw new ArgumentException("Screenshot bytes are required.", nameof(screenshot));
        ArgumentNullException.ThrowIfNull(targets);
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

        // These bounds are intentionally conservative: small badges and large scene decorations are excluded.
        CircleSegment[] circles = Cv2.HoughCircles(
            blurred, HoughModes.Gradient, dp: 1.2, minDist: 55,
            param1: 110, param2: 38, minRadius: 28, maxRadius: 82);

        var candidates = new List<(string Id, CoordinateUtilityServices.Rectangle Bounds)>();
        foreach (CircleSegment circle in circles)
        {
            double radius = circle.Radius;
            double x = circle.Center.X - radius;
            double y = circle.Center.Y - radius;
            double diameter = radius * 2;
            if (x < 0 || y < 0 || x + diameter > image.Width || y + diameter > image.Height)
                continue;

            // The label must be directly below the icon, not merely the nearest OCR string.
            var matching = targets.Where(t =>
            {
                if (!NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(t.Text))
                    return false;
                var r = t.Bounds;
                if (r.Width <= 0 || r.Height <= 0 ||
                    !double.IsFinite(r.TopLeft.X) || !double.IsFinite(r.TopLeft.Y))
                    return false;
                double labelCenter = r.TopLeft.X + r.Width / 2;
                double labelGap = r.TopLeft.Y - (y + diameter);
                return Math.Abs(labelCenter - circle.Center.X) <= Math.Min(22, radius * 0.35) &&
                       labelGap >= -Math.Max(10.0, radius * 0.75) && labelGap <= 36 &&
                       r.Width <= diameter * 1.8;
            }).ToArray();

            if (matching.Length == 1)
                candidates.Add((matching[0].Id,
                    CoordinateUtilityServices.Rectangle.FromXYWH(x, y, diameter, diameter)));
        }

        // If multiple circles claim one label, reject the entire label rather than selecting by score.
        return candidates.GroupBy(c => c.Id, StringComparer.Ordinal)
            .Where(g => g.Count() == 1)
            .Select(g => new NotificationIconTarget(g.Key, g.Single().Bounds))
            .ToArray();
    }
}