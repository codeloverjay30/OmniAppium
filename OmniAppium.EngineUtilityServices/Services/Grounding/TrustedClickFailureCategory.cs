namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Provides a bounded, non-sensitive failure classification.
/// </summary>
public enum TrustedClickFailureCategory
{
    None,
    Cancellation,
    Timeout,
    InvalidRequest,
    AuthorizationOrClaim,
    Unexpected
}
