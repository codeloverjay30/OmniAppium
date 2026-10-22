namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Represents the externally observable stage of a protected click attempt.
/// </summary>
public enum TrustedClickExecutionStatus
{
    RejectedBeforeConsumption,
    RejectedAlreadyConsumed,
    RejectedOrConsumedUndetermined,
    DispatchFailedOrAmbiguous,
    DispatchCompleted
}
