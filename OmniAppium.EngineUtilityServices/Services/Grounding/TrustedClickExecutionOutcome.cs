namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Carries a sanitized execution outcome without approval, paths, coordinates, or exception text.
/// </summary>
/// <param name="Status">The observable execution state.</param>
/// <param name="FailureCategory">The bounded diagnostic category.</param>
/// <param name="DispatchAttempted">Whether the device operation was entered.</param>
public sealed record TrustedClickExecutionOutcome(
    TrustedClickExecutionStatus Status,
    TrustedClickFailureCategory FailureCategory,
    bool DispatchAttempted);
