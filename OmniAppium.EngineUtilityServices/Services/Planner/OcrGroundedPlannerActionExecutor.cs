using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Executes OCR-grounded actions selected by a planner by delegating them
/// to the corresponding grounded execution service.
/// </summary>
public sealed class OcrGroundedPlannerActionExecutor
    : IOcrGroundedPlannerActionExecutor
{
    private readonly IOcrGroundedClickService _clickService;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="OcrGroundedPlannerActionExecutor"/> class.
    /// </summary>
    /// <param name="clickService">
    /// The OCR-grounded click service responsible for validated and
    /// freshness-protected click execution.
    /// </param>
    public OcrGroundedPlannerActionExecutor(
        IOcrGroundedClickService clickService)
    {
        ArgumentNullException.ThrowIfNull(clickService);

        _clickService = clickService;
    }

    /// <inheritdoc/>
    public void Click(
        OcrGroundedSnapshot snapshot,
        string targetId)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);

        _clickService.Click(
            snapshot,
            targetId);
    }
}