using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Measures exact decoded pixel differences in full frames or normalized regions.
/// </summary>
public sealed class ScreenFrameDifferenceAnalyzer : IScreenFrameDifferenceAnalyzer, IScreenFrameDifferenceMaskProvider
{
    private const int MaximumEncodedBytes = 32 * 1024 * 1024;
    private const long MaximumPixels = 16_777_216;

    /// <summary>
    /// Analyzes two screenshots and returns an exact per-pixel difference ratio.
    /// </summary>
    public ScreenFrameDifference Analyze(
        ReadOnlyMemory<byte> first,
        ReadOnlyMemory<byte> second,
        NormalizedScreenRegion? region = null,
        byte channelTolerance = 0)
    {
        region?.Validate();
        using Mat a = Decode(first, nameof(first));
        using Mat b = Decode(second, nameof(second));

        if (a.Width != b.Width || a.Height != b.Height)
            return new ScreenFrameDifference(1, 0, 0, false);

        Rect roi = GetRegion(a.Width, a.Height, region);
        using Mat aRoi = new(a, roi);
        using Mat bRoi = new(b, roi);
        using Mat aBgra = Normalize(aRoi);
        using Mat bBgra = Normalize(bRoi);
        using Mat difference = new();
        using Mat identical = new();

        Cv2.Absdiff(aBgra, bBgra, difference);
        Cv2.InRange(difference, Scalar.All(0), Scalar.All(channelTolerance), identical);

        long total = (long)roi.Width * roi.Height;
        long different = total - Cv2.CountNonZero(identical);
        return new ScreenFrameDifference((double)different / total, different, total, true);
    }

    /// <summary>
    /// Creates a grayscale PNG mask of pixels exceeding the configured channel tolerance.
    /// White pixels are different; black pixels are within tolerance.
    /// </summary>
    public byte[] CreateDifferenceMask(
        ReadOnlyMemory<byte> first,
        ReadOnlyMemory<byte> second,
        NormalizedScreenRegion? region = null,
        byte channelTolerance = 0)
    {
        region?.Validate();
        using Mat a = Decode(first, nameof(first));
        using Mat b = Decode(second, nameof(second));
        if (a.Width != b.Width || a.Height != b.Height)
            throw new ArgumentException("The screenshot dimensions must match.");

        Rect roi = GetRegion(a.Width, a.Height, region);
        using Mat aRoi = new(a, roi);
        using Mat bRoi = new(b, roi);
        using Mat aBgra = Normalize(aRoi);
        using Mat bBgra = Normalize(bRoi);
        using Mat difference = new();
        using Mat withinTolerance = new();
        using Mat mask = new();

        Cv2.Absdiff(aBgra, bBgra, difference);
        Cv2.InRange(difference, Scalar.All(0), Scalar.All(channelTolerance), withinTolerance);
        Cv2.BitwiseNot(withinTolerance, mask);
        if (!Cv2.ImEncode(".png", mask, out byte[] encoded) || encoded.Length == 0)
        {
            throw new InvalidOperationException(
                "Failed to encode the ROI difference mask as PNG.");
        }
        return encoded;
    }

    /// <summary>
    /// Decodes and validates an encoded image with strict resource limits.
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
            if ((long)decoded.Width * decoded.Height > MaximumPixels)
                throw new ArgumentException("The image exceeds the pixel limit.", parameterName);
            if (decoded.Depth() != MatType.CV_8U || decoded.Channels() is not (1 or 3 or 4))
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
    /// Converts a supported image to four-channel BGRA for exact comparison.
    /// </summary>
    private static Mat Normalize(Mat source)
    {
        Mat result = new();
        try
        {
            switch (source.Channels())
            {
                case 1:
                    Cv2.CvtColor(source, result, ColorConversionCodes.GRAY2BGRA);
                    break;
                case 3:
                    Cv2.CvtColor(source, result, ColorConversionCodes.BGR2BGRA);
                    break;
                case 4:
                    source.CopyTo(result);
                    break;
                default:
                    throw new ArgumentException("The image pixel format is unsupported.");
            }
            return result;
        }
        catch
        {
            result.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Converts a normalized region into a nonempty integer pixel rectangle.
    /// </summary>
    private static Rect GetRegion(int width, int height, NormalizedScreenRegion? region)
    {
        if (region is null)
            return new Rect(0, 0, width, height);

        int left = Math.Clamp((int)Math.Floor(region.X * width), 0, width - 1);
        int top = Math.Clamp((int)Math.Floor(region.Y * height), 0, height - 1);
        int right = Math.Clamp((int)Math.Ceiling((region.X + region.Width) * width), left + 1, width);
        int bottom = Math.Clamp((int)Math.Ceiling((region.Y + region.Height) * height), top + 1, height);
        return new Rect(left, top, right - left, bottom - top);
    }
}