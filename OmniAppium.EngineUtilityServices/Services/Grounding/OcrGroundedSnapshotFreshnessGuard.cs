namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Protects OCR-grounded actions from using snapshots that are no longer current.
/// </summary>
public sealed class OcrGroundedSnapshotFreshnessGuard
    : IOcrGroundedSnapshotFreshnessGuard
{
    private readonly object _syncRoot = new();

    private string? _currentSnapshotId;

    /// <inheritdoc/>
    public void MarkCurrent(string snapshotId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(snapshotId);

        lock (_syncRoot)
        {
            _currentSnapshotId = snapshotId;
        }
    }

    /// <inheritdoc/>
    public void ThrowIfStale(OcrGroundedSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        if (string.IsNullOrWhiteSpace(snapshot.SnapshotId))
        {
            throw new ArgumentException(
                "The OCR-grounded snapshot must contain a non-empty snapshot identifier.",
                nameof(snapshot));
        }

        string? currentSnapshotId;

        lock (_syncRoot)
        {
            currentSnapshotId = _currentSnapshotId;
        }

        if (currentSnapshotId is null)
        {
            throw new InvalidOperationException(
                "No current OCR-grounded snapshot has been registered.");
        }

        if (!string.Equals(
                snapshot.SnapshotId,
                currentSnapshotId,
                StringComparison.Ordinal))
        {
            throw new StaleOcrGroundedSnapshotException(
                snapshot.SnapshotId,
                currentSnapshotId);
        }
    }
}