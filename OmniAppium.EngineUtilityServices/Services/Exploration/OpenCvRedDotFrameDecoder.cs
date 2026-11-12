using System.Runtime.InteropServices;
using OpenCvSharp;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Decodes screenshots using the OpenCV dependency already referenced by the engine.
/// </summary>
public sealed class OpenCvRedDotFrameDecoder : IRedDotFrameDecoder
{
    private const int MaxPixels = 16_000_000;
    private const int MaxEncodedBytes = 64 * 1024 * 1024;

    /// <summary>
    /// Decodes a PNG or JPEG image and validates the resulting RGB24 buffer.
    /// </summary>
    public RedDotFrame Decode(ReadOnlyMemory<byte> encodedImage)
    {
        if (encodedImage.IsEmpty || encodedImage.Length > MaxEncodedBytes)
            throw new ArgumentException("Encoded screenshot size is invalid.", nameof(encodedImage));

        ReadOnlySpan<byte> bytes = encodedImage.Span;
        bool png = bytes.Length >= 8 && bytes[..8].SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });
        bool jpeg = bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF;
        if (!png && !jpeg)
            throw new ArgumentException("Screenshot must be PNG or JPEG.", nameof(encodedImage));

        try
        {
            using Mat bgr = Cv2.ImDecode(encodedImage.ToArray(), ImreadModes.Color);
            if (bgr.Empty() || bgr.Width <= 0 || bgr.Height <= 0 ||
                (long)bgr.Width * bgr.Height > MaxPixels)
                throw new ArgumentException("Decoded screenshot dimensions are invalid.", nameof(encodedImage));

            using var rgb = new Mat();
            Cv2.CvtColor(bgr, rgb, ColorConversionCodes.BGR2RGB);
            if (rgb.Type() != MatType.CV_8UC3 || !rgb.IsContinuous())
                throw new InvalidOperationException("Decoded screenshot must be contiguous RGB24.");

            int expectedLength = checked(rgb.Width * rgb.Height * 3);
            var pixels = new byte[expectedLength];
            Marshal.Copy(rgb.Data, pixels, 0, expectedLength);
            return new RedDotFrame(rgb.Width, rgb.Height, pixels);
        }
        catch (OpenCVException ex)
        {
            throw new ArgumentException("Screenshot decoding failed.", nameof(encodedImage), ex);
        }
    }
}