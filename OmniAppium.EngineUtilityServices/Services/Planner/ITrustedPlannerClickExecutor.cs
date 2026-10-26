using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Defines the trusted execution boundary for planner click requests.
/// </summary>
public interface ITrustedPlannerClickExecutor
{
    /// <summary>
    /// Executes a planner click only after trusted authorization succeeds.
    /// </summary>
    /// <param name="snapshot">The current OCR-grounded snapshot.</param>
    /// <param name="targetId">The requested target identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the click operation.</returns>
    Task ClickAsync(
        OcrGroundedSnapshot snapshot,
        string targetId,
        CancellationToken cancellationToken = default);
}