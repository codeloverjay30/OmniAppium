using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Services.OCR;

/// <summary>
/// Recognizes text from screenshots captured by the active Android session.
/// </summary>
public interface IAndroidScreenOcrService
{
    /// <summary>
    /// Captures a fresh screenshot and recognizes text within it.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token used to cancel the recognition workflow.
    /// </param>
    /// <returns>
    /// Text and text-line bounds in the captured screenshot's pixel coordinates.
    /// </returns>
    Task<OcrResult> RecognizeCurrentScreenAsync(
        CancellationToken cancellationToken = default);
}