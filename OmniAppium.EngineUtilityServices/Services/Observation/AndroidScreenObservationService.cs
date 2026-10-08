using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Stability;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;

namespace OmniAppium.EngineUtilityServices.Services.Observation;

/// <summary>
/// Creates immutable Android screen observations from a single screen capture.
/// </summary>
public sealed class AndroidScreenObservationService
    : IAndroidScreenObservationService
{
    private const string EmptyScreenshotMessage =
        "The captured Android screen contains no image data.";

    private readonly IScreenshotService _screenshotService;
    private readonly IAndroidScreenStabilityGuard _stabilityGuard;
    private readonly ScreenStabilityOptions _stabilityOptions;
    private readonly IAndroidScreenOcrService _androidScreenOcrService;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AndroidScreenObservationService"/> class.
    /// </summary>
    /// <param name="screenshotService">
    /// The service used to capture the Android screen.
    /// </param>
    /// <param name="androidScreenOcrService">
    /// The service used to recognize text from the captured screen image.
    /// </param>
    public AndroidScreenObservationService(
        IScreenshotService screenshotService,
        IAndroidScreenOcrService androidScreenOcrService,
        IAndroidScreenStabilityGuard stabilityGuard,
        ScreenStabilityOptions stabilityOptions)
    {
        ArgumentNullException.ThrowIfNull(screenshotService);
        ArgumentNullException.ThrowIfNull(androidScreenOcrService);

        ArgumentNullException.ThrowIfNull(stabilityGuard);
        ArgumentNullException.ThrowIfNull(stabilityOptions);
        stabilityOptions.Validate();
        _stabilityGuard = stabilityGuard;
        _stabilityOptions = stabilityOptions;
        _screenshotService = screenshotService;
        _androidScreenOcrService = androidScreenOcrService;
    }

    /// <summary>
    /// Captures the current Android screen and creates an immutable observation
    /// whose image data and OCR result originate from the same captured image.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token used to cancel the observation before or during OCR recognition.
    /// </param>
    /// <returns>
    /// An immutable observation containing the captured image data and its OCR result.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the captured screen contains no image data.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> is canceled.
    /// </exception>
    public async Task<IAndroidScreenObservation> ObserveAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ScreenStabilityResult stability = await _stabilityGuard
            .WaitForStabilityAsync(_stabilityOptions, cancellationToken)
            .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();
        if (!stability.IsStable || stability.TimedOut)
        {
            throw new TimeoutException(
                $"Android screen stability timeout after {stability.SampleCount} samples.");
        }

        byte[] imageBytes =
            _screenshotService.CaptureScreenshotBytes();

        if (imageBytes.Length == 0)
        {
            throw new InvalidOperationException(
                EmptyScreenshotMessage);
        }

        cancellationToken.ThrowIfCancellationRequested();

        OcrResult ocrResult =
            await _androidScreenOcrService
                .RecognizeAsync(
                    imageBytes,
                    cancellationToken)
                .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        return new AndroidScreenObservation(
            imageBytes,
            ocrResult);
    }
}