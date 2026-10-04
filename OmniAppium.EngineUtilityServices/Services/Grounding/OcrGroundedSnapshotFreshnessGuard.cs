using System.Runtime.CompilerServices;
using OmniAppium.EngineUtilityServices.Services.Grounding;

[assembly: InternalsVisibleTo(
    "OmniAppium.EngineUtilityServices.Tests")]
/// <summary>
/// Coordinates OCR-grounded snapshot freshness with authorized actions.
/// </summary>
public sealed class OcrGroundedSnapshotFreshnessGuard
    : IOcrGroundedSnapshotFreshnessGuard
{
    private readonly object _syncRoot = new();
    private readonly IOcrGroundedFreshnessWaitStrategy _waitStrategy;

    private string? _currentSnapshotId;
    private int _activeActionCount;

     /// <summary>
    /// Initializes a new instance of the
    /// <see cref="OcrGroundedSnapshotFreshnessGuard"/> class
    /// using monitor-based synchronization.
    /// </summary>
    public OcrGroundedSnapshotFreshnessGuard()
        : this(new MonitorOcrGroundedFreshnessWaitStrategy())
    {
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="OcrGroundedSnapshotFreshnessGuard"/> class
    /// using the specified freshness wait strategy.
    /// </summary>
    /// <param name="waitStrategy">
    /// The strategy used to coordinate freshness state transitions.
    /// </param>
    internal OcrGroundedSnapshotFreshnessGuard(
        IOcrGroundedFreshnessWaitStrategy waitStrategy)
    {
        ArgumentNullException.ThrowIfNull(waitStrategy);

        _waitStrategy = waitStrategy;
    }

    /// <inheritdoc/>
    public void MarkCurrent(string snapshotId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(snapshotId);

        lock (_syncRoot)
        {
            while (_activeActionCount > 0)
            {
                _waitStrategy.Wait(_syncRoot);
            }

            _currentSnapshotId = snapshotId;
        }
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="snapshot"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no current snapshot has been established.
    /// </exception>
    /// <exception cref="StaleOcrGroundedSnapshotException">
    /// Thrown when <paramref name="snapshot"/> is no longer current.
    /// </exception>
    public IOcrGroundedActionLease AcquireCurrent(
        OcrGroundedSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        lock (_syncRoot)
        {
            ThrowIfStale(snapshot);

            checked
            {
                _activeActionCount++;
            }

            try
            {
                return new OcrGroundedActionLease(
                    ReleaseAction);
            }
            catch
            {
                _activeActionCount--;
                throw;
            }
        }
    }

    /// <summary>
    /// Releases a previously acquired action authorization.
    /// </summary>
    private void ReleaseAction()
    {
        lock (_syncRoot)
        {
            if (_activeActionCount <= 0)
            {
                throw new InvalidOperationException(
                    "OCR-grounded action authorization state is invalid.");
            }

            _activeActionCount--;

            if (_activeActionCount == 0)
            {
                _waitStrategy.PulseAll(_syncRoot);
            }
        }
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Thrown when no current snapshot has been established.</exception>
    /// <exception cref="StaleOcrGroundedSnapshotException">Thrown when the snapshot is no longer current.</exception>
    public void ThrowIfStale(
        OcrGroundedSnapshot snapshot)
    {
        if (_currentSnapshotId is null)
        {
            throw new InvalidOperationException(
                "No current OCR-grounded snapshot has been established.");
        }

        if (!StringComparer.Ordinal.Equals(
                _currentSnapshotId,
                snapshot.SnapshotId))
        {
            throw new StaleOcrGroundedSnapshotException(
                snapshot.SnapshotId,
                _currentSnapshotId);
        }
    }
}