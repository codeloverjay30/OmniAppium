using AiUtility.GeminiKits.Attributes;
using AiUtility.ToolKits.Abstractions;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Exposes OCR-grounded click execution to an AI planner.
/// </summary>
public sealed class OcrGroundedPlannerClickTool
{
    private readonly IAiToolExecutionStateAccessor<
        OcrGroundedPlannerExecutionState> _executionStateAccessor;

    private readonly IOcrGroundedPlannerActionExecutor _actionExecutor;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="OcrGroundedPlannerClickTool"/> class.
    /// </summary>
    /// <param name="executionStateAccessor">
    /// Provides the state owned by the current logical AI tool execution.
    /// </param>
    /// <param name="actionExecutor">
    /// Executes OCR-grounded planner actions.
    /// </param>
    public OcrGroundedPlannerClickTool(
        IAiToolExecutionStateAccessor<
            OcrGroundedPlannerExecutionState> executionStateAccessor,
        IOcrGroundedPlannerActionExecutor actionExecutor)
    {
        ArgumentNullException.ThrowIfNull(
            executionStateAccessor);

        ArgumentNullException.ThrowIfNull(
            actionExecutor);

        _executionStateAccessor =
            executionStateAccessor;

        _actionExecutor =
            actionExecutor;
    }

    /// <summary>
    /// Clicks the OCR-grounded target selected by the planner against the
    /// exact snapshot owned by the current logical execution.
    /// </summary>
    /// <param name="targetId">
    /// The exact OCR-grounded target identifier selected by the planner.
    /// </param>
    [GeminiTool(
        Description =
            "Clicks an OCR-grounded target from the current planner snapshot.")]
    public void Click(
        string targetId)
    {
        OcrGroundedPlannerExecutionState executionState =
            _executionStateAccessor.Current;

        _actionExecutor.Click(
            executionState.Snapshot,
            targetId);
    }
}