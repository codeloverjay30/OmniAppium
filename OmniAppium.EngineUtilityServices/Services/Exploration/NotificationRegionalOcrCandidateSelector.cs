using CoordinateUtilityServices;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Selects an isolated primary OCR label from a known source-image region without granting ownership.
/// </summary>
public static class NotificationRegionalOcrCandidateSelector
{
    /// <summary>
    /// Returns a single source-coordinate label candidate or no candidate when evidence is uncertain.
    /// </summary>
    public static OcrGroundedTarget? Select(
        OcrResult result,
        Rectangle region,
        int imageWidth,
        int imageHeight,
        string expectedLabel,
        string candidateId)
    {
        return Diagnose(result, region, imageWidth, imageHeight, expectedLabel, candidateId).Candidate;
    }

    /// <summary>
    /// Diagnoses regional OCR evidence without relaxing exact-label matching or ownership rules.
    /// </summary>
    public static NotificationRegionalOcrSelectionDiagnostic Diagnose(
        OcrResult result,
        Rectangle region,
        int imageWidth,
        int imageHeight,
        string expectedLabel,
        string candidateId)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentException.ThrowIfNullOrWhiteSpace(expectedLabel);
        ArgumentException.ThrowIfNullOrWhiteSpace(candidateId);

        if (imageWidth <= 0 || imageHeight <= 0 ||
            !IsWithin(region, imageWidth, imageHeight))
            return new(null, NotificationRegionalOcrRejectionReason.InvalidRegion, null);

        string expected = Compact(expectedLabel);
        if (expected.Length == 0 ||
            !NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(expected))
            return new(null, NotificationRegionalOcrRejectionReason.InvalidExpectedLabel, null);

        if (result.Lines is null || result.Lines.Count == 0 ||
            result.Lines.All(line => line is null || string.IsNullOrWhiteSpace(line.Text)))
            return new(null, NotificationRegionalOcrRejectionReason.NoTextDetected, null);

        OcrTextLine[] matchingText = result.Lines
            .Where(line => line is not null &&
                string.Equals(Compact(line.Text), expected, StringComparison.Ordinal) &&
                NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(line.Text))
            .ToArray()!;

        if (matchingText.Length == 0)
        {
            string? observed = result.Lines
                .Where(line => line is not null && !string.IsNullOrWhiteSpace(line.Text) &&
                    NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(line.Text))
                .Select(line => Compact(line!.Text))
                .FirstOrDefault();
            return new(null, NotificationRegionalOcrRejectionReason.LabelMismatch, observed);
        }

        if (matchingText.Length > 1)
            return new(null, NotificationRegionalOcrRejectionReason.DuplicateLabel, expected);

        OcrTextLine match = matchingText[0];
        if (!IsWithin(match.Bounds, imageWidth, imageHeight))
            return new(null, NotificationRegionalOcrRejectionReason.InvalidBounds, expected);

        if (!IsContained(match.Bounds, region))
            return new(null, NotificationRegionalOcrRejectionReason.OutOfRegion, expected);

        return new(
            new OcrGroundedTarget(candidateId, match.Text, match.Bounds),
            NotificationRegionalOcrRejectionReason.None,
            expected);
    }

    /// <summary>
    /// Removes whitespace without changing OCR characters or guessing missing words.
    /// </summary>
    private static string Compact(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;
        ReadOnlySpan<char> input = value.AsSpan();
        Span<char> buffer = input.Length <= 256
            ? stackalloc char[input.Length]
            : new char[input.Length];
        int length = 0;
        foreach (char c in input)
            if (!char.IsWhiteSpace(c))
                buffer[length++] = c;
        return new string(buffer[..length]);
    }

    /// <summary>
    /// Validates a finite positive rectangle within an image without integer overflow.
    /// </summary>
    private static bool IsWithin(Rectangle bounds, int width, int height)
    {
        double x = bounds.TopLeft.X;
        double y = bounds.TopLeft.Y;
        double w = bounds.Width;
        double h = bounds.Height;
        return double.IsFinite(x) && double.IsFinite(y) &&
            double.IsFinite(w) && double.IsFinite(h) &&
            x >= 0 && y >= 0 && w > 0 && h > 0 &&
            x + w <= width && y + h <= height;
    }

    /// <summary>
    /// Ensures OCR bounds were translated to and remain inside the requested source-image region.
    /// </summary>
    private static bool IsContained(Rectangle child, Rectangle parent) =>
        child.TopLeft.X >= parent.TopLeft.X &&
        child.TopLeft.Y >= parent.TopLeft.Y &&
        child.TopLeft.X + child.Width <= parent.TopLeft.X + parent.Width &&
        child.TopLeft.Y + child.Height <= parent.TopLeft.Y + parent.Height;
}