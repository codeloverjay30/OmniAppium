namespace OmniAppium.EngineUtilityServices.Services.Navigation;

/// <summary>
/// Represents trusted execution and screen-verification facts for one action.
/// </summary>
/// <param name="Kind">The observed action category.</param>
/// <param name="Executed">Whether the authorized executor actually invoked the action.</param>
/// <param name="Succeeded">Whether the executor confirmed successful execution.</param>
/// <param name="PostActionScreenVerified">Whether a fresh post-action screen was verified.</param>
/// <param name="ExpectedDestinationVerified">Whether the expected destination was verified.</param>
public sealed record NavigationActionEvidence(
    NavigationActionKind Kind,
    bool Executed,
    bool Succeeded,
    bool PostActionScreenVerified,
    bool ExpectedDestinationVerified);