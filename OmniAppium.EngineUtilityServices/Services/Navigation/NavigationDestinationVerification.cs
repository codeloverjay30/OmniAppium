namespace OmniAppium.EngineUtilityServices.Services.Navigation;

/// <summary>
/// Describes the result of trusted post-action screen and destination verification.
/// </summary>
/// <param name="PostActionScreenVerified">Whether a fresh post-action screen was verified.</param>
/// <param name="ExpectedDestinationVerified">Whether the expected destination was confirmed.</param>
public sealed record NavigationDestinationVerification(
    bool PostActionScreenVerified,
    bool ExpectedDestinationVerified);