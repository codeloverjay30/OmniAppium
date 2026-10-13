using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Produces diagnostic ROI difference masks using the same comparison rules as the analyzer.
/// </summary>
public interface IScreenFrameDifferenceMaskProvider
{
    /// <summary>
    /// Returns a PNG mask with white pixels where channel differences exceed tolerance.
    /// </summary>
    byte[] CreateDifferenceMask(
        ReadOnlyMemory<byte> first,
        ReadOnlyMemory<byte> second,
        NormalizedScreenRegion? region = null,
        byte channelTolerance = 0);
}
