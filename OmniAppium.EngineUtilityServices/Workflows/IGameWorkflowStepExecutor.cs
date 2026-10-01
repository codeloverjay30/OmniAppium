namespace OmniAppium.EngineUtilityServices.Workflows;

/// <summary>
/// Executes a single declarative game workflow step.
/// </summary>
public interface IGameWorkflowStepExecutor
{
    /// <summary>
    /// Executes the specified game workflow step.
    /// </summary>
    /// <param name="step">The declarative step to execute.</param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync(
        GameWorkflowStep step,
        CancellationToken cancellationToken = default);
}