using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Models.Observation;

/// <summary>
/// Represents an immutable observation of a single Android screen capture
/// together with the OCR result produced from that exact capture.
/// </summary>
public sealed class AndroidScreenObservation : IAndroidScreenObservation
{
    private readonly byte[] _imageBytes;

    /// <summary>
    /// Initializes a new immutable Android screen observation.
    /// </summary>
    /// <param name="imageBytes">
    /// The encoded image bytes belonging to the captured screen.
    /// </param>
    /// <param name="ocrResult">
    /// The OCR result produced from the exact captured image.
    /// </param>
    public AndroidScreenObservation(
        byte[] imageBytes,
        OcrResult ocrResult)
    {
        ArgumentNullException.ThrowIfNull(imageBytes);
        ArgumentNullException.ThrowIfNull(ocrResult);

        if (imageBytes.Length == 0)
        {
            throw new ArgumentException(
                "Screen observation image data cannot be empty.",
                nameof(imageBytes));
        }

        _imageBytes = imageBytes.ToArray();
        OcrResult = ocrResult;
    }

    /// <summary>
    /// Gets the immutable image data captured for this observation.
    /// </summary>
    public ReadOnlyMemory<byte> ImageBytes => _imageBytes;

    /// <summary>
    /// Gets the OCR result produced from the exact image represented by this observation.
    /// </summary>
    public OcrResult OcrResult { get; }
}