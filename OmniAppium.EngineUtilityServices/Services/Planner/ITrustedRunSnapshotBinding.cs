using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Confirms that archived review evidence represents the current observed snapshot.
/// </summary>
public interface ITrustedRunSnapshotBinding
{
    /// <summary>
    /// Rejects a run that is not independently bound to the current snapshot and target.
    /// </summary>
    Task VerifyAsync(
        string runDirectory,
        OcrGroundedSnapshot snapshot,
        string targetId,
        CancellationToken cancellationToken);
}
