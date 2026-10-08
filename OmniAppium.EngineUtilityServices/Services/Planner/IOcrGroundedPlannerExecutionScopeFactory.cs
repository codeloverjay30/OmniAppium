using global::OmniAppium.EngineUtilityServices.Models.Observation;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Creates OCR-grounded planner execution scopes from Android screen observations.
/// </summary>
public interface IOcrGroundedPlannerExecutionScopeFactory
{
    /// <summary>
    /// Creates an OCR-grounded planner execution scope from an already-acquired
    /// Android screen observation.
    /// </summary>
    /// <param name="observation">
    /// The screen observation used to establish the planner state.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel scope creation.
    /// </param>
    /// <returns>
    /// The established planner execution scope.
    /// </returns>
    Task<IOcrGroundedPlannerExecutionScope> CreateAsync(
        IAndroidScreenObservation observation,
        CancellationToken cancellationToken = default);
}
