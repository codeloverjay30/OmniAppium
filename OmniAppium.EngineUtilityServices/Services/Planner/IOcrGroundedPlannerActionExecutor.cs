using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Defines execution operations for OCR-grounded actions selected by a planner.
/// </summary>
public interface IOcrGroundedPlannerActionExecutor
{
    /// <summary>
    /// Executes an OCR-grounded click selected by the planner.
    /// </summary>
    /// <param name="snapshot">
    /// The exact OCR-grounded snapshot used by the planner when selecting
    /// the target.
    /// </param>
    /// <param name="targetId">
    /// The exact OCR-grounded target identifier selected by the planner.
    /// </param>
    void Click(
        OcrGroundedSnapshot snapshot,
        string targetId);
}