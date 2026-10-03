using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Creates OCR-grounded snapshots from OCR results.
/// </summary>
public sealed class OcrGroundedSnapshotFactory
    : IOcrGroundedSnapshotFactory
{
    /// <inheritdoc/>
    public OcrGroundedSnapshot Create(
        string snapshotId,
        OcrResult ocrResult)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(snapshotId);
        ArgumentNullException.ThrowIfNull(ocrResult);

        if (ocrResult.Lines is null)
        {
            throw new ArgumentException(
                "The OCR result must contain a non-null line collection.",
                nameof(ocrResult));
        }

        OcrGroundedTarget[] targets =
            ocrResult.Lines
                .Select(
                    (line, index) =>
                        new OcrGroundedTarget(
                            CreateTargetId(index),
                            line.Text,
                            line.Bounds))
                .ToArray();

        return new OcrGroundedSnapshot(
            snapshotId,
            targets);
    }

    /// <summary>
    /// Creates a stable target identifier within a single OCR snapshot.
    /// </summary>
    /// <param name="index">
    /// The zero-based OCR target index.
    /// </param>
    /// <returns>
    /// The target identifier.
    /// </returns>
    private static string CreateTargetId(int index)
    {
        return $"ocr-{index}";
    }
}
