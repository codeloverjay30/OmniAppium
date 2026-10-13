using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Observation;

/// <summary>
/// Converts OCR line coordinates into normalized screen coordinates.
/// </summary>
public interface IOcrObservationLineNormalizer
{
    /// <summary>
    /// Normalizes OCR lines using the dimensions of the captured image.
    /// </summary>
    IReadOnlyList<NormalizedOcrTextLine> Normalize(
        OcrResult result,
        ReadOnlyMemory<byte> imageBytes);
}