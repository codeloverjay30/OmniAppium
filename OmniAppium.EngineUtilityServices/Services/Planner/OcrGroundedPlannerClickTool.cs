using AiUtility.GeminiKits.Attributes;
using AiUtility.ToolKits.Abstractions;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Exposes only trusted OCR-grounded clicks to the Gemini planner.
/// </summary>
public sealed class OcrGroundedPlannerClickTool
{
    private readonly IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState> _state;
    private readonly TrustedPlannerClickExecutor _executor;

    /// <summary>
    /// Initializes the planner tool with its scoped state and trusted executor.
    /// </summary>
    public OcrGroundedPlannerClickTool(
        IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState> state,
        TrustedPlannerClickExecutor executor)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
        _executor = executor ?? throw new ArgumentNullException(nameof(executor));
    }

    /// <summary>
    /// Executes the current planner target using independently signed approval.
    /// </summary>
    [GeminiTool(Description = "Clicks an OCR-grounded target from the current planner snapshot.")]
    public Task Click(string targetId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        cancellationToken.ThrowIfCancellationRequested();
        OcrGroundedPlannerExecutionState state = _state.Current;
        return _executor.ClickAsync(state.Snapshot, targetId, cancellationToken);
    }
}