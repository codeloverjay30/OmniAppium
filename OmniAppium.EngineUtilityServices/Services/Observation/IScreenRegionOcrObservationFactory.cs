using OmniAppium.EngineUtilityServices.Models.Observation;

namespace OmniAppium.EngineUtilityServices.Services.Observation;

/// <summary>
/// Defines a factory for creating region-based OCR screen observation services.
/// </summary>
public interface IScreenRegionOcrObservationFactory
{
    /// <summary>
    /// Creates an independent screen observation service for the specified options.
    /// </summary>
    /// <param name="options">The region and OCR stability configuration.</param>
    /// <returns>A configured screen observation service.</returns>
    IAndroidScreenObservationService Create(
        ScreenObservationStabilityOptions options);
}