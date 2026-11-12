using OpenCvSharp;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Rejects template lookalikes that lack the expected white exclamation glyph on a red badge.
/// </summary>
public static class NotificationExclamationVisualGate
{
    /// <summary>
    /// Validates a bounded candidate against the actual screenshot pixels without inferring an owner.
    /// </summary>
    public static bool IsExclamation(Mat source, NotificationTemplateMatch match)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(match);
        if (source.Empty() || source.Type() != MatType.CV_8UC3 ||
            match.Width < 12 || match.Height < 12 ||
            match.X < 0 || match.Y < 0 ||
            (long)match.X + match.Width > source.Width ||
            (long)match.Y + match.Height > source.Height)
            return false;

        int centerLeft = match.X + match.Width * 35 / 100;
        int centerRight = match.X + match.Width * 65 / 100;
        int upperStart = match.Y + match.Height * 25 / 100;
        int upperEnd = match.Y + match.Height * 55 / 100;
        int gapStart = match.Y + match.Height * 58 / 100;
        int gapEnd = match.Y + match.Height * 67 / 100;
        int dotStart = match.Y + match.Height * 69 / 100;
        int dotEnd = match.Y + match.Height * 80 / 100;

        int upperRows = 0, gapRows = 0, dotRows = 0;
        for (int y = upperStart; y < upperEnd; y++)
            if (WhiteCount(y) >= 4) upperRows++;
        for (int y = gapStart; y < gapEnd; y++)
            if (WhiteCount(y) >= 2) gapRows++;
        for (int y = dotStart; y < dotEnd; y++)
            if (WhiteCount(y) >= 2) dotRows++;

        int red = 0, sampled = 0;
        for (int y = match.Y + match.Height / 4; y < match.Y + match.Height * 3 / 4; y += 2)
        for (int x = match.X + match.Width / 5; x < match.X + match.Width * 4 / 5; x += 2)
        {
            Vec3b pixel = source.Get<Vec3b>(y, x);
            sampled++;
            if (pixel.Item2 >= 125 && pixel.Item2 > pixel.Item1 + 45 &&
                pixel.Item2 > pixel.Item0 + 45)
                red++;
        }

        return upperRows >= Math.Max(2, (upperEnd - upperStart) / 2) &&
            gapRows <= 1 && dotRows >= 1 &&
            sampled > 0 && red * 100 >= sampled * 30;

        int WhiteCount(int y)
        {
            int count = 0;
            for (int x = centerLeft; x <= centerRight; x++)
            {
                Vec3b pixel = source.Get<Vec3b>(y, x);
                int min = Math.Min(pixel.Item0, Math.Min(pixel.Item1, pixel.Item2));
                int max = Math.Max(pixel.Item0, Math.Max(pixel.Item1, pixel.Item2));
                if (min >= 205 && max - min <= 50) count++;
            }
            return count;
        }
    }
}
