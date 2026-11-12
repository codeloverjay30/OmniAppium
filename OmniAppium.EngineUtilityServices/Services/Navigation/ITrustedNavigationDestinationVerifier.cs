using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Navigation;

/// <summary>
/// Verifies a destination using a fresh observation after an authorized navigation action.
/// </summary>
public interface ITrustedNavigationDestinationVerifier
{
    /// <summary>
    /// Observes the screen after a click and verifies the expected destination.
    /// </summary>
    /// <param name="snapshot">The trusted pre-action snapshot.</param>
    /// <param name="targetId">The authorized target identifier.</param>
    /// <param name="cancellationToken">Cancellation for verification.</param>
    /// <returns>Verification facts derived from a trusted screen observation.</returns>
    Task<NavigationDestinationVerification> VerifyAsync(
        OcrGroundedSnapshot snapshot,
        string targetId,
        CancellationToken cancellationToken = default);
}
