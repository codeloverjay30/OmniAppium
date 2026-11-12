namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Coordinates red-dot decoding, detection, and association for a single observation.
/// </summary>
public interface IRedDotObservationIntegrationService
{
    /// <summary>
    /// Produces read-only exploration evidence without invoking device actions.
    /// </summary>
    RedDotAssociationResult Analyze(RedDotObservationInput input);
}
