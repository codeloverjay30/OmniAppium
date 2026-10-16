using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Resolves reviewed evidence only when it is bound to the exact OCR observation.
/// </summary>
public interface IGroundedClickEvidenceProvider
{
    /// <summary>
    /// Returns approved evidence for the specified OCR result, or throws when unavailable.
    /// </summary>
    ReviewedGroundTruth Resolve(OcrResult observation, string targetText);

    /// <summary>
    /// Returns the confidence recorded for this exact observation and match.
    /// </summary>
    float ResolveConfidence(OcrResult observation, OcrTextMatch match);

    /// <summary>
    /// Returns approved evidence for the specified grounded snapshot, or throws when unavailable.
    /// </summary>
    ReviewedGroundTruth Resolve(OcrGroundedSnapshot snapshot, OcrGroundedTarget target);

    /// <summary>
    /// Returns confidence captured with the exact grounded snapshot.
    /// </summary>
    float ResolveConfidence(OcrGroundedSnapshot snapshot, OcrGroundedTarget target);
}
