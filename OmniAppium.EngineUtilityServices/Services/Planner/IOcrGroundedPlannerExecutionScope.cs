using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Represents an active OCR-grounded planner execution scope.
/// </summary>
public interface IOcrGroundedPlannerExecutionScope : IDisposable
{
    /// <summary>
    /// Gets the immutable OCR-grounded snapshot owned by this execution scope.
    /// </summary>
    OcrGroundedSnapshot Snapshot { get; }
}
