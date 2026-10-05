using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Represents OCR-grounded state owned by a single logical planner execution.
/// </summary>
public sealed class OcrGroundedPlannerExecutionState
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="OcrGroundedPlannerExecutionState"/> class.
    /// </summary>
    /// <param name="snapshot">
    /// The exact OCR-grounded snapshot used by the planner for this execution.
    /// </param>
    public OcrGroundedPlannerExecutionState(
        OcrGroundedSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        Snapshot = snapshot;
    }

    /// <summary>
    /// Gets the exact OCR-grounded snapshot used by the planner.
    /// </summary>
    public OcrGroundedSnapshot Snapshot { get; }
}