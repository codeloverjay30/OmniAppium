namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Defines synchronization operations used while coordinating
/// OCR-grounded snapshot freshness transitions.
/// </summary>
internal interface IOcrGroundedFreshnessWaitStrategy
{
    /// <summary>
    /// Waits until the freshness state may have changed.
    /// </summary>
    /// <param name="syncRoot">
    /// The monitor object protecting the freshness state.
    /// </param>
    void Wait(object syncRoot);

    /// <summary>
    /// Signals waiting freshness transitions that the protected state changed.
    /// </summary>
    /// <param name="syncRoot">
    /// The monitor object protecting the freshness state.
    /// </param>
    void PulseAll(object syncRoot);
}