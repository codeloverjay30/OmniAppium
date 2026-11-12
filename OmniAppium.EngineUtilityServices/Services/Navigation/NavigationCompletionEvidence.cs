namespace OmniAppium.EngineUtilityServices.Services.Navigation;

/// <summary>
/// Collects externally verified facts for one navigation-completion decision.
/// </summary>
/// <param name="RequiresNavigationVerification">Whether this workflow requires navigation evidence.</param>
/// <param name="SessionSucceeded">Whether the Gemini session completed successfully.</param>
/// <param name="Actions">Evidence produced by trusted executors and verifiers.</param>
/// <param name="ModelText">Untrusted model text, retained for diagnostics only.</param>
public sealed record NavigationCompletionEvidence(
    bool RequiresNavigationVerification,
    bool SessionSucceeded,
    IReadOnlyList<NavigationActionEvidence> Actions,
    string? ModelText = null);