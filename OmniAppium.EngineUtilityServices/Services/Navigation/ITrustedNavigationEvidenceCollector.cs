using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Navigation;

/// <summary>
/// Collects navigation evidence from trusted click execution and destination verification.
/// </summary>
public interface ITrustedNavigationEvidenceCollector
{
    /// <summary>
    /// Executes an authorized click and attempts post-action destination verification.
    /// </summary>
    /// <param name="snapshot">The current trusted OCR snapshot.</param>
    /// <param name="targetId">The authorized target identifier.</param>
    /// <param name="cancellationToken">Cancellation for the operation.</param>
    Task ExecuteAsync(OcrGroundedSnapshot snapshot, string targetId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a snapshot of the evidence collected during this collector's lifetime.
    /// </summary>
    IReadOnlyList<NavigationActionEvidence> GetEvidence();
}