namespace OmniAppium.EngineUtilityServices.Services.OCR;

/// <summary>
/// Defines an operation for locating visible text through OCR and clicking
/// the corresponding screen position.
/// </summary>
public interface IOcrClickService
{
    /// <summary>
    /// Waits for the specified text to be recognized and clicks the center
    /// of the matched OCR text region.
    /// </summary>
    /// <param name="targetText">The text to locate.</param>
    /// <param name="timeout">The maximum amount of time to wait for the text.</param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClickTextAsync(
        string targetText,
        TimeSpan timeout,
        CancellationToken cancellationToken = default);
}