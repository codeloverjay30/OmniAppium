using OpenCvSharp;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>Performs bounded multi-scale OpenCV template matching with overlap suppression.</summary>
public sealed class OpenCvNotificationTemplateMatcher : INotificationTemplateMatcher
{
    private const int MaxEncodedBytes = 64 * 1024 * 1024;
    private const long MaxPixels = 16_000_000;

    /// <summary>Finds visual badges without granting permission to click them.</summary>
    public IReadOnlyList<NotificationTemplateMatch> Match(
        ReadOnlyMemory<byte> screenshot,
        ReadOnlyMemory<byte> template,
        NotificationTemplateMatchOptions? options = null)
    {
        options ??= new NotificationTemplateMatchOptions();
        if (!double.IsFinite(options.MinimumSimilarity) || options.MinimumSimilarity <= 0 ||
            options.MinimumSimilarity > 1 || !double.IsFinite(options.MinimumScale) ||
            !double.IsFinite(options.MaximumScale) || !double.IsFinite(options.ScaleStep) ||
            options.MinimumScale <= 0 || options.MaximumScale < options.MinimumScale ||
            options.MaximumScale > 4 || options.ScaleStep <= 0 ||
            (options.MaximumScale - options.MinimumScale) / options.ScaleStep > 100 ||
            !double.IsFinite(options.MaximumOverlap) || options.MaximumOverlap < 0 ||
            options.MaximumOverlap >= 1 || options.MaximumMatches is < 1 or > 1024)
            throw new ArgumentOutOfRangeException(nameof(options), "Invalid notification template matching options.");

        ValidateImage(screenshot, nameof(screenshot));
        ValidateImage(template, nameof(template));
        try
        {
            using Mat source = Cv2.ImDecode(screenshot.ToArray(), ImreadModes.Color);
            using Mat sample = Cv2.ImDecode(template.ToArray(), ImreadModes.Unchanged);
            if (source.Empty() || sample.Empty() || source.Width <= 0 || sample.Width <= 0 ||
                (long)source.Width * source.Height > MaxPixels ||
                (long)sample.Width * sample.Height > MaxPixels)
                throw new ArgumentException("Notification image could not be decoded or exceeds dimension limits.");

            // Alpha transparency is supported through the TM_CCORR_NORMED mask path.
            using Mat color = new();
            using Mat mask = new();
            bool hasAlpha = sample.Channels() == 4;
            if (hasAlpha)
            {
                Cv2.CvtColor(sample, color, ColorConversionCodes.BGRA2BGR);
                Cv2.ExtractChannel(sample, mask, 3);
                if (Cv2.CountNonZero(mask) < 9)
                    throw new ArgumentException("Notification template alpha mask is empty.");
            }
            else if (sample.Channels() == 3)
                sample.CopyTo(color);
            else if (sample.Channels() == 1)
                Cv2.CvtColor(sample, color, ColorConversionCodes.GRAY2BGR);
            else
                throw new ArgumentException("Notification template channels are unsupported.");

            var candidates = new List<NotificationTemplateMatch>();
            int steps = (int)Math.Floor((options.MaximumScale - options.MinimumScale) / options.ScaleStep + 1e-8);
            for (int step = 0; step <= steps + 1; step++)
            {
                double scale = Math.Min(options.MaximumScale, options.MinimumScale + step * options.ScaleStep);
                int width = (int)Math.Round(color.Width * scale);
                int height = (int)Math.Round(color.Height * scale);
                if (width < 3 || height < 3 || width > source.Width || height > source.Height)
                    continue;

                using Mat resized = new();
                Cv2.Resize(color, resized, new Size(width, height));
                using Mat resizedMask = new();
                if (hasAlpha)
                    Cv2.Resize(mask, resizedMask, new Size(width, height), interpolation: InterpolationFlags.Nearest);

                using Mat scores = new();
                if (hasAlpha)
                {
                    Cv2.MatchTemplate(
                        source,
                        resized,
                        scores,
                        TemplateMatchModes.CCorrNormed,
                        resizedMask);
                }
                else
                {
                    Cv2.MatchTemplate(
                        source,
                        resized,
                        scores,
                        TemplateMatchModes.CCoeffNormed);
                }

                // Suppress a small area around each maximum before looking for the next peak.
                int examined = 0;
                while (examined++ < options.MaximumMatches * 8)
                {
                    Cv2.MinMaxLoc(scores, out _, out double maximum, out _, out Point location);
                    if (!double.IsFinite(maximum) || maximum < options.MinimumSimilarity)
                        break;
                    candidates.Add(new NotificationTemplateMatch(location.X, location.Y, width, height, maximum));
                    int radiusX = Math.Max(1, width / 2);
                    int radiusY = Math.Max(1, height / 2);
                    int x = Math.Max(0, location.X - radiusX);
                    int y = Math.Max(0, location.Y - radiusY);
                    var area = new Rect(x, y,
                        Math.Min(scores.Width - x, radiusX * 2 + 1),
                        Math.Min(scores.Height - y, radiusY * 2 + 1));
                    using Mat roi = new(scores, area);
                    roi.SetTo(Scalar.All(-1));
                }
            }

            var accepted = new List<NotificationTemplateMatch>();
            foreach (NotificationTemplateMatch candidate in candidates.OrderByDescending(m => m.Similarity))
            {
                if (accepted.Any(existing => IntersectionOverUnion(existing, candidate) > options.MaximumOverlap))
                    continue;
                if (options.RequireExclamationGlyph &&
                    !NotificationExclamationVisualGate.IsExclamation(source, candidate))
                    continue;
                accepted.Add(candidate);
                if (accepted.Count >= options.MaximumMatches)
                    break;
            }
            return accepted;
        }
        catch (OpenCVException ex)
        {
            throw new InvalidOperationException("OpenCV notification template matching failed.", ex);
        }
    }

    /// <summary>Validates bounded PNG/JPEG input without allocating decoded pixel buffers.</summary>
    private static void ValidateImage(ReadOnlyMemory<byte> image, string name)
    {
        ReadOnlySpan<byte> bytes = image.Span;
        bool png = bytes.Length >= 8 && bytes[..8].SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });
        bool jpeg = bytes.Length >= 3 && bytes[0] == 0xff && bytes[1] == 0xd8 && bytes[2] == 0xff;
        if (bytes.Length > MaxEncodedBytes || (!png && !jpeg))
            throw new ArgumentException("Notification image must be a bounded PNG or JPEG.", name);
    }

    /// <summary>Calculates spatial overlap between two template matches.</summary>
    private static double IntersectionOverUnion(NotificationTemplateMatch a, NotificationTemplateMatch b)
    {
        int left = Math.Max(a.X, b.X);
        int top = Math.Max(a.Y, b.Y);
        int right = Math.Min(a.X + a.Width, b.X + b.Width);
        int bottom = Math.Min(a.Y + a.Height, b.Y + b.Height);
        long intersection = (long)Math.Max(0, right - left) * Math.Max(0, bottom - top);
        long union = (long)a.Width * a.Height + (long)b.Width * b.Height - intersection;
        return union == 0 ? 0 : (double)intersection / union;
    }
}