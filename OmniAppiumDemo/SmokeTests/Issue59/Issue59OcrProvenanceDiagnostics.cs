using CoordinateUtilityServices;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppiumDemo.SmokeTests.Issue59;

/// <summary>
/// Compares raw OCR lines with grounded targets in a bounded diagnostic region.
/// </summary>
public static class Issue59OcrProvenanceDiagnostics
{
    /// <summary>
    /// Describes one OCR line and whether the target factory retained it.
    /// </summary>
    public sealed record Entry(
        int RawIndex,
        string Text,
        Rectangle Bounds,
        bool Retained,
        string Reason);

    /// <summary>
    /// Inspects raw OCR lines without modifying OCR data or target associations.
    /// </summary>
    public static IReadOnlyList<Entry> Inspect(
        IReadOnlyList<(string? Text, Rectangle Bounds)> rawLines,
        IReadOnlyList<OcrGroundedTarget> targets,
        Rectangle region)
    {
        ArgumentNullException.ThrowIfNull(rawLines);
        ArgumentNullException.ThrowIfNull(targets);

        if (region.Width <= 0 || region.Height <= 0)
            throw new ArgumentOutOfRangeException(nameof(region),
                "Diagnostic region must have positive dimensions.");

        var targetIds = new HashSet<string>(
            targets.Select(target => target.Id), StringComparer.Ordinal);

        var entries = new List<Entry>();
        for (int index = 0; index < rawLines.Count; index++)
        {
            (string? rawText, Rectangle bounds) = rawLines[index];
            double left = Math.Max(bounds.TopLeft.X, region.TopLeft.X);
            double top = Math.Max(bounds.TopLeft.Y, region.TopLeft.Y);
            double right = Math.Min(bounds.BottomRight.X, region.BottomRight.X);
            double bottom = Math.Min(bounds.BottomRight.Y, region.BottomRight.Y);
            if (!(right > left && bottom > top))
                continue;

            string text = rawText?.Trim() ?? string.Empty;
            bool retained = targetIds.Contains($"issue59-ocr-{index:D6}");
            string reason = text.Length == 0
                ? "BlankRawText"
                : retained ? "Retained" : "NonBlankRawLineNotRetained";

            entries.Add(new Entry(index, text, bounds, retained, reason));
        }

        return entries.AsReadOnly();
    }
}