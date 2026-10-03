namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Represents the OCR-grounded state of a captured screen.
/// </summary>
public sealed record OcrGroundedSnapshot(
    string SnapshotId,
    IReadOnlyList<OcrGroundedTarget> Targets);
