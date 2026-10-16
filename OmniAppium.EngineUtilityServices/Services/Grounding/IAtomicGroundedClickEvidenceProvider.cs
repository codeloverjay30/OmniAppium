using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Provides atomic single-use evidence and confidence retrieval.
/// </summary>
public interface IAtomicGroundedClickEvidenceProvider : IGroundedClickEvidenceProvider
{
    /// <summary>
    /// Atomically consumes the evidence associated with the exact OCR match.
    /// </summary>
    ConsumedGroundedClickEvidence Consume(
        OcrResult observation, string targetText, OcrTextMatch match);

    /// <summary>
    /// Atomically consumes the evidence associated with the exact snapshot target.
    /// </summary>
    ConsumedGroundedClickEvidence Consume(
        OcrGroundedSnapshot snapshot, OcrGroundedTarget target);
}
