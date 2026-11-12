using CoordinateUtilityServices;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OpenCvSharp;

namespace OmniAppiumDemo.SmokeTests.Issue59;

/// <summary>
/// Creates OCR-grounded targets from an existing Android screen observation
/// for read-only Issue 59 red-dot analysis.
/// </summary>
public static class Issue59OcrTargetFactory
{
    private const int MaximumEncodedBytes = 32 * 1024 * 1024;
    private const long MaximumPixels = 16_777_216;

    /// <summary>
    /// Converts OCR text lines from the captured observation into validated
    /// OCR-grounded targets without acquiring another screen observation.
    /// </summary>
    /// <param name="observation">
    /// The observation containing the captured image and its OCR result.
    /// </param>
    /// <returns>
    /// A read-only collection of validated OCR-grounded targets.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the observation is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the captured image is empty, oversized, or invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the OCR result or its coordinates are invalid.
    /// </exception>
    public static IReadOnlyList<OcrGroundedTarget> Create(
        IAndroidScreenObservation observation)
    {
        ArgumentNullException.ThrowIfNull(observation);

        var ocrResult = observation.OcrResult
            ?? throw new InvalidOperationException(
                "Issue 59 observation OCR result cannot be null.");

        ReadOnlyMemory<byte> imageBytes = observation.ImageBytes;

        if (imageBytes.IsEmpty)
        {
            throw new ArgumentException(
                "Issue 59 observation image cannot be empty.",
                nameof(observation));
        }

        if (imageBytes.Length > MaximumEncodedBytes)
        {
            throw new ArgumentException(
                "Issue 59 observation image exceeds the encoded size limit.",
                nameof(observation));
        }

        using Mat image = DecodeImage(imageBytes);

        int imageWidth = image.Width;
        int imageHeight = image.Height;

        if (imageWidth <= 0 ||
            imageHeight <= 0 ||
            (long)imageWidth * imageHeight > MaximumPixels)
        {
            throw new ArgumentException(
                "Issue 59 observation image dimensions are invalid.",
                nameof(observation));
        }

        var targets = new List<OcrGroundedTarget>(
            ocrResult.Lines.Count);

        for (int index = 0; index < ocrResult.Lines.Count; index++)
        {
            var line = ocrResult.Lines[index];

            if (line is null)
            {
                throw new InvalidOperationException(
                    $"Issue 59 OCR line {index} cannot be null.");
            }

            string text = line.Text?.Trim() ?? string.Empty;

            if (text.Length == 0)
            {
                continue;
            }

            Rectangle bounds = line.Bounds;

            ValidateBounds(
                bounds,
                imageWidth,
                imageHeight,
                index);

            string targetId = $"issue59-ocr-{index:D6}";

            targets.Add(
                new OcrGroundedTarget(
                    targetId,
                    text,
                    bounds));
        }

        return targets.AsReadOnly();
    }

    /// <summary>
    /// Decodes an encoded screenshot and converts native decoding failures
    /// into managed argument exceptions.
    /// </summary>
    /// <param name="imageBytes">The encoded screenshot.</param>
    /// <returns>The decoded image.</returns>
    private static Mat DecodeImage(
        ReadOnlyMemory<byte> imageBytes)
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
                    "Issue 59 observation image data is invalid.",
                    nameof(imageBytes));
            }

            return image;
        }
        catch (OpenCvSharpException ex)
        {
            throw new ArgumentException(
                "Issue 59 observation image decoding failed.",
                nameof(imageBytes),
                ex);
        }
    }

    /// <summary>
    /// Ensures that an OCR rectangle has finite, positive dimensions and
    /// remains entirely within the captured image.
    /// </summary>
    /// <param name="bounds">The OCR bounding rectangle.</param>
    /// <param name="imageWidth">The image width in pixels.</param>
    /// <param name="imageHeight">The image height in pixels.</param>
    /// <param name="lineIndex">The original OCR line index.</param>
    private static void ValidateBounds(
        Rectangle bounds,
        int imageWidth,
        int imageHeight,
        int lineIndex)
    {
        double left = bounds.TopLeft.X;
        double top = bounds.TopLeft.Y;
        double right = bounds.BottomRight.X;
        double bottom = bounds.BottomRight.Y;

        if (!double.IsFinite(left) ||
            !double.IsFinite(top) ||
            !double.IsFinite(right) ||
            !double.IsFinite(bottom) ||
            left < 0 ||
            top < 0 ||
            right <= left ||
            bottom <= top ||
            right > imageWidth ||
            bottom > imageHeight)
        {
            throw new InvalidOperationException(
                $"Issue 59 OCR line {lineIndex} " +
                "has invalid bounds or coordinates outside the captured image.");
        }
    }
}