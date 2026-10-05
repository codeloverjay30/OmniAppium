namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Creates OCR-grounded planner execution scopes from the current Android screen.
/// </summary>
public interface IOcrGroundedPlannerExecutionScopeFactory
{
    /// <summary>
    /// Creates an execution scope from a fresh OCR observation of the current screen.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token used to cancel OCR recognition before the scope is established.
    /// </param>
    /// <returns>
    /// The established OCR-grounded planner execution scope.
    /// </returns>
    Task<IOcrGroundedPlannerExecutionScope> CreateAsync(
        CancellationToken cancellationToken = default);
}
