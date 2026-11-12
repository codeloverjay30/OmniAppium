namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Decodes an encoded screenshot into a validated RGB24 frame.
/// </summary>
public interface IRedDotFrameDecoder
{
    /// <summary>
    /// Decodes PNG or JPEG bytes into row-major RGB24 pixels.
    /// </summary>
    RedDotFrame Decode(ReadOnlyMemory<byte> encodedImage);
}
