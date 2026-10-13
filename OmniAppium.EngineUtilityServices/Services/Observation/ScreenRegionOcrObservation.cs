using Microsoft.Extensions.Logging;
using OmniAppium.EngineUtilityServices.Models.Observation;

namespace OmniAppium.EngineUtilityServices.Services.Observation;

/// <summary>
/// Coordinates a configured region-based OCR observation and records its outcome.
/// </summary>
public sealed class ScreenRegionOcrObservation : IAndroidScreenObservationService
{
    private readonly IAndroidScreenObservationService _observationService;
    private readonly ILogger<ScreenRegionOcrObservation> _logger;
    private readonly string _targetText;

    /// <summary>
    /// Initializes an observation wrapper using the configured region and OCR target.
    /// </summary>
    /// <param name="factory">The factory that creates region-based OCR observers.</param>
    /// <param name="options">The validated observation configuration.</param>
    /// <param name="logger">The logger for observation lifecycle events.</param>
    public ScreenRegionOcrObservation(
        IScreenRegionOcrObservationFactory factory,
        ScreenObservationStabilityOptions options,
        ILogger<ScreenRegionOcrObservation> logger)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        options.Validate();

        _observationService = factory.Create(options)
            ?? throw new InvalidOperationException(
                "The screen region OCR observation factory returned null.");

        _logger = logger;
        _targetText = options.TargetText;
    }

    /// <summary>
    /// Waits for a stable region and OCR target, returning the matching screen observation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token for the observation.</param>
    /// <returns>The stable Android screen observation.</returns>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels the observation.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// The region or OCR target does not stabilize before the configured timeout.
    /// </exception>
    public async Task<IAndroidScreenObservation> ObserveAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Screen region OCR observation started. Target={TargetText}",
            _targetText);

        try
        {
            IAndroidScreenObservation result =
                await _observationService.ObserveAsync(cancellationToken)
                    .ConfigureAwait(false);

            _logger.LogInformation(
                "Screen region OCR observation succeeded. Target={TargetText}",
                _targetText);

            return result;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                "Screen region OCR observation canceled. Target={TargetText}",
                _targetText);
            throw;
        }
        catch (TimeoutException exception)
        {
            _logger.LogWarning(
                exception,
                "Screen region OCR observation timed out. Target={TargetText}",
                _targetText);
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Screen region OCR observation failed. Target={TargetText}",
                _targetText);
            throw;
        }
    }
}
