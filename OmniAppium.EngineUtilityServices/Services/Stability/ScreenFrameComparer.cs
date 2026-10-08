using OpenCvSharp;

namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Compares encoded screenshots using OpenCV while preserving exact per-pixel tolerance semantics.
/// </summary>
public sealed class ScreenFrameComparer : IScreenFrameComparer
{
    private const int MaximumEncodedBytes = 32 * 1024 * 1024;
    private const long MaximumPixels = 16_777_216;

    /// <summary>
    /// Determines whether the ratio of different pixels is within the specified tolerance.
    /// </summary>
    public bool AreSimilar(ReadOnlyMemory<byte> first, ReadOnlyMemory<byte> second, double tolerance)
    {
        if (!double.IsFinite(tolerance) || tolerance is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(tolerance),
                "The tolerance must be between zero and one.");

        using Mat a = Decode(first, nameof(first));
        using Mat b = Decode(second, nameof(second));

        if (a.Width != b.Width || a.Height != b.Height)
            return false;

        using Mat normalizedA = NormalizeToBgra(a);
        using Mat normalizedB = NormalizeToBgra(b);
        using Mat difference = new();
        using Mat identicalPixelMask = new();

        Cv2.Absdiff(normalizedA, normalizedB, difference);

        // InRange produces one mask value per pixel: 255 only when all four
        // channel differences are zero. CountNonZero therefore counts pixels,
        // not individual channel differences.
        Cv2.InRange(difference, Scalar.All(0), Scalar.All(0), identicalPixelMask);

        long totalPixels = (long)a.Width * a.Height;
        long permittedDifferentPixels = (long)Math.Floor(totalPixels * tolerance);
        long differentPixels = totalPixels - Cv2.CountNonZero(identicalPixelMask);
        return differentPixels <= permittedDifferentPixels;
    }

    /// <summary>
    /// Decodes an encoded screenshot and validates resource limits.
    /// </summary>
    private static Mat Decode(ReadOnlyMemory<byte> bytes, string parameterName)
    {
        if (bytes.IsEmpty)
            throw new ArgumentException("The image cannot be empty.", parameterName);

        if (bytes.Length > MaximumEncodedBytes)
            throw new ArgumentException("The image exceeds the encoded size limit.", parameterName);

        Mat? decoded = null;
        try
        {
            decoded = Cv2.ImDecode(bytes.Span, ImreadModes.Unchanged);

            if (decoded.Empty())
                throw new ArgumentException("The image data is invalid or unsupported.", parameterName);

            long pixels = (long)decoded.Width * decoded.Height;
            if (pixels > MaximumPixels)
                throw new ArgumentException("The image exceeds the pixel limit.", parameterName);

            if (decoded.Depth() != MatType.CV_8U ||
                decoded.Channels() is not (1 or 3 or 4))
                throw new ArgumentException("The image pixel format is unsupported.", parameterName);

            Mat result = decoded;
            decoded = null;
            return result;
        }
        catch (OpenCvSharpException ex)
        {
            throw new ArgumentException("The image data is invalid or unsupported.", parameterName, ex);
        }
        finally
        {
            decoded?.Dispose();
        }
    }

    /// <summary>
    /// Converts an image into eight-bit BGRA to compare all visible color and alpha channels.
    /// </summary>
    private static Mat NormalizeToBgra(Mat source)
    {
        if (source.Channels() == 4)
            return source.Clone();

        var result = new Mat();
        try
        {
            ColorConversionCodes conversion = source.Channels() switch
            {
                1 => ColorConversionCodes.GRAY2BGRA,
                3 => ColorConversionCodes.BGR2BGRA,
                _ => throw new ArgumentException("The image pixel format is unsupported.", nameof(source))
            };
            Cv2.CvtColor(source, result, conversion);
            return result;
        }
        catch
        {
            result.Dispose();
            throw;
        }
    }
}
