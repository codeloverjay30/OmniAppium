using OmniAppium.EngineUtilityServices.Models.Observation;

namespace OmniAppium.EngineUtilityServices.Services.Observation;

/// <summary>
/// Defines a service that creates immutable observations of the current Android screen.
/// </summary>
public interface IAndroidScreenObservationService
{
    /// <summary>
    /// Captures the current Android screen and creates an immutable observation.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// An immutable observation of the captured Android screen.
    /// </returns>
    Task<IAndroidScreenObservation> ObserveAsync(
        CancellationToken cancellationToken = default);
}
