using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Supplies screenshot bytes and target observations captured by a trusted device observation pipeline.
/// </summary>
public interface ITrustedSnapshotEvidenceSource
{
    /// <summary>
    /// Retrieves trusted evidence associated with the exact snapshot instance and requested target.
    /// </summary>
    Task<TrustedSnapshotEvidence> GetEvidenceAsync(
        OcrGroundedSnapshot snapshot,
        string targetId,
        CancellationToken cancellationToken);
}
