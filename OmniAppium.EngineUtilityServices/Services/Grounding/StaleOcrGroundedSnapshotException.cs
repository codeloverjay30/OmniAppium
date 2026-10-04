namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Represents an attempt to execute an action using an OCR-grounded snapshot
/// that is no longer current.
/// </summary>
public sealed class StaleOcrGroundedSnapshotException
    : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="StaleOcrGroundedSnapshotException"/> class.
    /// </summary>
    /// <param name="snapshotId">
    /// The identifier of the stale snapshot.
    /// </param>
    /// <param name="currentSnapshotId">
    /// The identifier of the currently active snapshot.
    /// </param>
    public StaleOcrGroundedSnapshotException(
        string snapshotId,
        string currentSnapshotId)
        : base(
            $"OCR-grounded snapshot '{snapshotId}' is stale. " +
            $"The current snapshot is '{currentSnapshotId}'.")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(snapshotId);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentSnapshotId);

        SnapshotId = snapshotId;
        CurrentSnapshotId = currentSnapshotId;
    }

    /// <summary>
    /// Gets the identifier of the stale snapshot.
    /// </summary>
    public string SnapshotId { get; }

    /// <summary>
    /// Gets the identifier of the currently active snapshot.
    /// </summary>
    public string CurrentSnapshotId { get; }
}