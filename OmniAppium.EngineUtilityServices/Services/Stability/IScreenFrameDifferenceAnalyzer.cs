using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Provides diagnostic pixel-difference measurements, optionally within a region.
/// </summary>
public interface IScreenFrameDifferenceAnalyzer
{
    /// <summary>
    /// Measures the fraction of pixels whose decoded channel values differ.
    /// </summary>
    ScreenFrameDifference Analyze(
        ReadOnlyMemory<byte> first,
        ReadOnlyMemory<byte> second,
        NormalizedScreenRegion? region = null,
        byte channelTolerance = 0);
}