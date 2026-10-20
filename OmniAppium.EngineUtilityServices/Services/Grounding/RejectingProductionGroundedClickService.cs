namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Rejects legacy grounded clicks that do not carry signed approval evidence.
/// </summary>
public sealed class RejectingProductionGroundedClickService : IOcrGroundedClickService
{
    /// <summary>
    /// Rejects a click request that cannot prove trusted review authorization.
    /// </summary>
    public void Click(OcrGroundedSnapshot snapshot, string targetId)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        throw new InvalidOperationException(
            "Trusted signed review approval is required for production grounded clicks.");
    }
}
