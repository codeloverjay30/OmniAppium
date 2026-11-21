using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Identifies why a regional OCR label was not admitted as a candidate.
/// </summary>
public enum NotificationRegionalOcrRejectionReason
{
    None,
    InvalidRegion,
    InvalidExpectedLabel,
    NoTextDetected,
    LabelMismatch,
    DuplicateLabel,
    InvalidBounds,
    OutOfRegion
}

/// <summary>
/// Carries a regional OCR candidate and its read-only selection diagnostic.
/// </summary>
public sealed record NotificationRegionalOcrSelectionDiagnostic(
    OcrGroundedTarget? Candidate,
    NotificationRegionalOcrRejectionReason Reason,
    string? ObservedText);
