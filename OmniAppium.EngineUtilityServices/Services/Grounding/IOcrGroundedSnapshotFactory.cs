using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Defines operations for creating OCR-grounded snapshots.
/// </summary>
public interface IOcrGroundedSnapshotFactory
{
    /// <summary>
    /// Creates an OCR-grounded snapshot from an OCR result.
    /// </summary>
    /// <param name="snapshotId">
    /// The identifier of the captured screen snapshot.
    /// </param>
    /// <param name="ocrResult">
    /// The OCR result produced from the captured screen.
    /// </param>
    /// <returns>
    /// An OCR-grounded snapshot containing the observable OCR targets.
    /// </returns>
    OcrGroundedSnapshot Create(
        string snapshotId,
        OcrResult ocrResult);
}
