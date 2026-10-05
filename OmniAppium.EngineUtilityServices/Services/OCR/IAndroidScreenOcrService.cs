using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Services.OCR;

/// <summary>
/// Recognizes text from Android screen images.
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

    /// <summary>
    /// Recognizes text from an already captured Android screen image without
    /// capturing another screenshot.
    /// </summary>
    /// <param name="imageBuffer">
    /// The exact encoded image buffer to submit to OCR.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the recognition workflow.
    /// </param>
    /// <returns>
    /// Text and text-line bounds in the supplied image's pixel coordinates.
    /// </returns>
    Task<OcrResult> RecognizeAsync(
        ReadOnlyMemory<byte> imageBuffer,
        CancellationToken cancellationToken = default);
}