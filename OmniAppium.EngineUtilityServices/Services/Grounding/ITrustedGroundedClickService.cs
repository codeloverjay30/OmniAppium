namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Defines the signed-review-only device click boundary.
/// </summary>
public interface ITrustedGroundedClickService
{
    /// <summary>
    /// Authorizes evidence and executes at most one device click.
    /// </summary>
    Task ClickAsync(TrustedGroundedClickRequest request, CancellationToken cancellationToken = default);
}
