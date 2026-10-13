using OCRUtilityServices.Models;
using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Observation;

/// <summary>
/// Normalizes OCR text-line centers against the captured image dimensions.
/// </summary>
public sealed class OcrObservationLineNormalizer
    : IOcrObservationLineNormalizer
{
    private const int MaximumEncodedBytes = 32 * 1024 * 1024;
    private const long MaximumPixels = 16_777_216;

    /// <summary>
    /// Converts OCR line centers to normalized screen coordinates.
    /// </summary>
    public IReadOnlyList<NormalizedOcrTextLine> Normalize(
        OcrResult result,
        ReadOnlyMemory<byte> imageBytes)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (imageBytes.IsEmpty)
        {
            throw new ArgumentException(
                "The image cannot be empty.",
                nameof(imageBytes));
        }

        if (imageBytes.Length > MaximumEncodedBytes)
        {
            throw new ArgumentException(
                "The image exceeds the encoded size limit.",
                nameof(imageBytes));
        }

        using Mat image = Decode(imageBytes);

        int width = image.Width;
        int height = image.Height;

        if (width <= 0 ||
            height <= 0 ||
            (long)width * height > MaximumPixels)
        {
            throw new ArgumentException(
                "The image dimensions are invalid or exceed the pixel limit.",
                nameof(imageBytes));
        }

        var normalized = new List<NormalizedOcrTextLine>(
            result.Lines.Count);

        foreach (OcrTextLine line in result.Lines)
        {
            double centerX = line.Bounds.Center.X / (double)width;
            double centerY = line.Bounds.Center.Y / (double)height;

            if (!double.IsFinite(centerX) ||
                !double.IsFinite(centerY) ||
                centerX is < 0 or > 1 ||
                centerY is < 0 or > 1)
            {
                throw new InvalidOperationException(
                    "OCR line coordinates fall outside the captured image.");
            }

            normalized.Add(
                new NormalizedOcrTextLine(
                    line.Text,
                    centerX,
                    centerY));
        }

        return normalized;
    }

    /// <summary>
    /// Decodes an image and translates native decoding failures.
    /// </summary>
    private static Mat Decode(ReadOnlyMemory<byte> imageBytes)
    {
        try
        {
            Mat image = Cv2.ImDecode(
                imageBytes.Span,
                ImreadModes.Unchanged);

            if (image.Empty())
            {
                image.Dispose();

                throw new ArgumentException(
                    "The image data is invalid or unsupported.",
                    nameof(imageBytes));
            }

            return image;
        }
        catch (OpenCvSharpException ex)
        {
            throw new ArgumentException(
                "The image data is invalid or unsupported.",
                nameof(imageBytes),
                ex);
        }
    }
}