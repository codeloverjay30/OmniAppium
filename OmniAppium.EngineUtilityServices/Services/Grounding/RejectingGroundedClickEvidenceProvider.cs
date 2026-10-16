using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Rejects click authorization when no reviewed evidence source is configured.
/// </summary>
public sealed class RejectingGroundedClickEvidenceProvider : IGroundedClickEvidenceProvider
{
    /// <summary>
    /// Rejects OCR observations without reviewed evidence.
    /// </summary>
    public ReviewedGroundTruth Resolve(OcrResult observation, string targetText)
        => throw new InvalidOperationException("Reviewed OCR click evidence is unavailable.");

    /// <summary>
    /// Rejects confidence claims without reviewed evidence.
    /// </summary>
    public float ResolveConfidence(OcrResult observation, OcrTextMatch match)
        => throw new InvalidOperationException("Reviewed OCR click confidence is unavailable.");

    /// <summary>
    /// Rejects grounded snapshots without reviewed evidence.
    /// </summary>
    public ReviewedGroundTruth Resolve(OcrGroundedSnapshot snapshot, OcrGroundedTarget target)
        => throw new InvalidOperationException("Reviewed grounded click evidence is unavailable.");

    /// <summary>
    /// Rejects confidence claims without reviewed evidence.
    /// </summary>
    public float ResolveConfidence(OcrGroundedSnapshot snapshot, OcrGroundedTarget target)
        => throw new InvalidOperationException("Reviewed grounded click confidence is unavailable.");
}
