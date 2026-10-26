using CoordinateUtilityServices;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Represents trusted evidence bound to a specific OCR snapshot and target.
/// </summary>
/// <param name="Snapshot">The snapshot associated with the evidence.</param>
/// <param name="TargetId">The identifier of the observed target.</param>
/// <param name="TargetText">The text of the observed target.</param>
/// <param name="TargetBounds">The bounds of the observed target.</param>
/// <param name="ScreenshotBytes">The screenshot captured for verification.</param>
/// <param name="CapturedAtUtc">The UTC timestamp when the evidence was captured.</param>
public sealed record TrustedSnapshotEvidence(
    OcrGroundedSnapshot Snapshot,
    string TargetId,
    string TargetText,
    Rectangle TargetBounds,
    byte[] ScreenshotBytes,
    DateTimeOffset CapturedAtUtc);