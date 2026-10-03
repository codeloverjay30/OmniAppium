namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Defines operations for executing clicks against OCR-grounded targets.
/// </summary>
public interface IOcrGroundedClickService
{
    /// <summary>
    /// Clicks the specified target from an OCR-grounded snapshot.
    /// </summary>
    /// <param name="snapshot">
    /// The OCR-grounded snapshot containing the target.
    /// </param>
    /// <param name="targetId">
    /// The identifier of the target to click.
    /// </param>
    void Click(
        OcrGroundedSnapshot snapshot,
        string targetId);
}