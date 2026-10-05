using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Models.Observation;

/// <summary>
/// Represents an immutable observation of a single Android screen capture.
/// </summary>
public interface IAndroidScreenObservation
{
    /// <summary>
    /// Gets the immutable image data captured for this observation.
    /// </summary>
    ReadOnlyMemory<byte> ImageBytes { get; }

    /// <summary>
    /// Gets the OCR result produced from the exact image represented by this observation.
    /// </summary>
    OcrResult OcrResult { get; }
}