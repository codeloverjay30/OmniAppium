using OCRUtilityServices.Services;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Carries externally approved, digest-pinned evidence for a single grounded click.
/// </summary>
public sealed record TrustedGroundedClickRequest(
    OcrGroundedSnapshot Snapshot,
    string TargetId,
    TrustedReviewApprovalEvidence Approval,
    string ReviewPath,
    string AnnotationPath,
    string ScreenshotPath,
    string TrustedReviewSha256,
    string TrustedAnnotationSha256,
    string TrustedScreenshotSha256,
    float Confidence);
