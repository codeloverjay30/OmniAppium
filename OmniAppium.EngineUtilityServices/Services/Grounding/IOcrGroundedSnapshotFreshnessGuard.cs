namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Defines protection against actions executed from stale OCR-grounded snapshots.
/// </summary>
public interface IOcrGroundedSnapshotFreshnessGuard
{
    /// <summary>
    /// Marks the specified snapshot identifier as the current observable snapshot.
    /// </summary>
    /// <param name="snapshotId">
    /// The identifier of the current OCR-grounded snapshot.
    /// </param>
    void MarkCurrent(string snapshotId);

    /// <summary>
    /// Throws when the specified snapshot is no longer current.
    /// </summary>
    /// <param name="snapshot">
    /// The OCR-grounded snapshot used to authorize an action.
    /// </param>
    void ThrowIfStale(OcrGroundedSnapshot snapshot);
}