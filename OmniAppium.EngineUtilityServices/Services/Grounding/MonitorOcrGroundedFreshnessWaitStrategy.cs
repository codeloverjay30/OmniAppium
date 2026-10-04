namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Provides monitor-based synchronization for OCR-grounded
/// freshness transitions.
/// </summary>
internal sealed class MonitorOcrGroundedFreshnessWaitStrategy
    : IOcrGroundedFreshnessWaitStrategy
{
    /// <inheritdoc/>
    public void Wait(object syncRoot)
    {
        ArgumentNullException.ThrowIfNull(syncRoot);

        Monitor.Wait(syncRoot);
    }

    /// <inheritdoc/>
    public void PulseAll(object syncRoot)
    {
        ArgumentNullException.ThrowIfNull(syncRoot);

        Monitor.PulseAll(syncRoot);
    }
}