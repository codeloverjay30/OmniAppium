using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Contains evidence and confidence captured in a single atomic operation.
/// </summary>
public sealed record ConsumedGroundedClickEvidence(
    ReviewedGroundTruth Evidence,
    float Confidence);
