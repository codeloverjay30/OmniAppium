namespace OmniAppium.EngineUtilityServices.Services.Navigation;

/// <summary>
/// Validates navigation completion using trusted execution evidence.
/// </summary>
public interface INavigationCompletionGuard
{
    /// <summary>
    /// Rejects incomplete or unverified navigation evidence.
    /// </summary>
    /// <param name="evidence">Evidence collected from trusted execution boundaries.</param>
    void Validate(NavigationCompletionEvidence evidence);
}
