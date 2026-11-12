using CoordinateUtilityServices;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Detects compact red notification indicators without authorizing device actions.
/// </summary>
public sealed class RedDotDetector
{
    private const int MaxPixels = 16_000_000;

    /// <summary>
    /// Finds red connected components and associates them with nearby UI node upper-right corners.
    /// </summary>
    public IReadOnlyList<RedDotEvidence> Detect(
        RedDotFrame frame,
        IReadOnlyList<RedDotNodeBounds> nodes,
        int minimumDiameter = 3,
        int maximumDiameter = 32)
    {
        ArgumentNullException.ThrowIfNull(frame);
        ArgumentNullException.ThrowIfNull(nodes);
        if (frame.Width <= 0 || frame.Height <= 0 ||
            (long)frame.Width * frame.Height > MaxPixels ||
            (long)frame.Width * frame.Height * 3 != frame.Rgb24.Length)
            throw new ArgumentException("Invalid RGB24 frame dimensions or buffer length.", nameof(frame));
        if (minimumDiameter < 1 || maximumDiameter < minimumDiameter)
            throw new ArgumentOutOfRangeException(nameof(minimumDiameter), "Invalid red-dot diameter range.");

        int width = frame.Width;
        int height = frame.Height;
        int count = checked(width * height);
        ReadOnlySpan<byte> rgb = frame.Rgb24.Span;
        var mask = new bool[count];
        var visited = new bool[count];
        for (int i = 0; i < count; i++)
        {
            int offset = i * 3;
            int r = rgb[offset], g = rgb[offset + 1], b = rgb[offset + 2];
            mask[i] = r >= 155 && r >= g + 65 && r >= b + 65 && g <= 155 && b <= 155;
        }

        var results = new List<RedDotEvidence>();
        var queue = new Queue<int>();
        for (int start = 0; start < count; start++)
        {
            if (!mask[start] || visited[start]) continue;
            visited[start] = true;
            queue.Enqueue(start);
            int minX = width, minY = height, maxX = -1, maxY = -1, pixels = 0;
            while (queue.Count > 0)
            {
                int index = queue.Dequeue();
                int x = index % width, y = index / width;
                minX = Math.Min(minX, x); maxX = Math.Max(maxX, x);
                minY = Math.Min(minY, y); maxY = Math.Max(maxY, y);
                pixels++;
                // Four-neighbor connectivity deliberately avoids merging diagonal decorations.
                if (x > 0) Enqueue(index - 1);
                if (x + 1 < width) Enqueue(index + 1);
                if (y > 0) Enqueue(index - width);
                if (y + 1 < height) Enqueue(index + width);
            }

            int boxWidth = maxX - minX + 1, boxHeight = maxY - minY + 1;
            if (boxWidth < minimumDiameter || boxHeight < minimumDiameter ||
                boxWidth > maximumDiameter || boxHeight > maximumDiameter) continue;
            double aspect = (double)Math.Min(boxWidth, boxHeight) / Math.Max(boxWidth, boxHeight);
            double fill = (double)pixels / (boxWidth * boxHeight);
            if (aspect < 0.7 || fill < 0.55 || fill > 0.95) continue;

            var bounds = Rectangle.FromXYWH(minX, minY, boxWidth, boxHeight);
            double centerX = minX + (boxWidth - 1) / 2.0;
            double centerY = minY + (boxHeight - 1) / 2.0;
            var owners = new List<string>();
            foreach (RedDotNodeBounds node in nodes)
            {
                if (node is null || string.IsNullOrWhiteSpace(node.Id) ||
                    node.Bounds.Width <= 0 || node.Bounds.Height <= 0) continue;
                Rectangle rect = node.Bounds;
                // Notification badges typically overlap the top-right corner of an element.
                double tolerance = Math.Max(boxWidth, boxHeight) * 1.5;
                if (Math.Abs(centerX - rect.BottomRight.X) <= tolerance &&
                    Math.Abs(centerY - rect.TopLeft.Y) <= tolerance)
                {
                    owners.Add(node.Id);
                }
            }

            results.Add(new RedDotEvidence(bounds, Math.Min(1, aspect * fill), owners.AsReadOnly()));

            void Enqueue(int neighbor)
            {
                if (!mask[neighbor] || visited[neighbor]) return;
                visited[neighbor] = true;
                queue.Enqueue(neighbor);
            }
        }

        return results.AsReadOnly();
    }
}