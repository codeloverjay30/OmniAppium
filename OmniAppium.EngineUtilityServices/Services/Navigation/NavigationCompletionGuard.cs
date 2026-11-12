namespace OmniAppium.EngineUtilityServices.Services.Navigation;

/// <summary>
/// Enforces fail-closed navigation completion without executing device actions.
/// </summary>
public sealed class NavigationCompletionGuard : INavigationCompletionGuard
{
    /// <summary>
    /// Validates session completion and each trusted navigation action.
    /// </summary>
    /// <param name="evidence">Evidence from trusted execution and verification components.</param>
    /// <exception cref="ArgumentNullException">Evidence or its action collection is null.</exception>
    /// <exception cref="InvalidOperationException">Required navigation evidence is missing or invalid.</exception>
    public void Validate(NavigationCompletionEvidence evidence)
    {
        ArgumentNullException.ThrowIfNull(evidence);
        ArgumentNullException.ThrowIfNull(evidence.Actions);

        if (!evidence.SessionSucceeded)
        {
            throw new InvalidOperationException(
                "The workflow session did not succeed.");
        }

        if (!evidence.RequiresNavigationVerification)
        {
            return;
        }

        bool hasTrustedNavigation = false;

        foreach (NavigationActionEvidence action in evidence.Actions)
        {
            if (action is null)
            {
                throw new InvalidOperationException(
                    "The navigation evidence contains a null action.");
            }

            if (action.Kind != NavigationActionKind.TrustedGroundedClick)
            {
                continue;
            }

            if (!action.Executed || !action.Succeeded)
            {
                throw new InvalidOperationException(
                    "The navigation action was not successfully executed.");
            }

            if (!action.PostActionScreenVerified || !action.ExpectedDestinationVerified)
            {
                throw new InvalidOperationException(
                    "The navigation action is missing post-action screen or destination verification.");
            }

            hasTrustedNavigation = true;
        }

        if (!hasTrustedNavigation)
        {
            throw new InvalidOperationException(
                "The navigation workflow contains no trusted executed navigation action.");
        }
    }
}