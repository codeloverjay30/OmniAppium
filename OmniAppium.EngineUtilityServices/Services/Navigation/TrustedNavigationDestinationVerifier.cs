using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;

namespace OmniAppium.EngineUtilityServices.Services.Navigation;

/// <summary>
/// Verifies a post-action screen using trusted destination criteria and the existing OCR page verifier.
/// </summary>
/// <remarks>
/// The trusted criteria resolver must be configured by the host, never from model-generated text.
/// The observation and OCR verifier perform separate screen captures; this implementation does not
/// claim that OCR was performed on the exact observed image.
/// </remarks>
public sealed class TrustedNavigationDestinationVerifier : ITrustedNavigationDestinationVerifier
{
    private readonly IAndroidScreenObservationService _observationService;
    private readonly IOcrPageVerificationService _pageVerificationService;
    private readonly Func<OcrGroundedSnapshot, string, IReadOnlyCollection<string>> _trustedCriteriaResolver;
    private readonly TimeSpan _timeout;

    /// <summary>
    /// Creates a verifier with trusted, host-supplied destination criteria.
    /// </summary>
    public TrustedNavigationDestinationVerifier(
        IAndroidScreenObservationService observationService,
        IOcrPageVerificationService pageVerificationService,
        Func<OcrGroundedSnapshot, string, IReadOnlyCollection<string>> trustedCriteriaResolver,
        TimeSpan timeout)
    {
        _observationService = observationService ?? throw new ArgumentNullException(nameof(observationService));
        _pageVerificationService = pageVerificationService ?? throw new ArgumentNullException(nameof(pageVerificationService));
        _trustedCriteriaResolver = trustedCriteriaResolver ?? throw new ArgumentNullException(nameof(trustedCriteriaResolver));
        if (timeout <= TimeSpan.Zero || timeout == Timeout.InfiniteTimeSpan)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), "Navigation verification timeout must be greater than zero.");
        }
        _timeout = timeout;
    }

    /// <inheritdoc/>
    public async Task<NavigationDestinationVerification> VerifyAsync(
        OcrGroundedSnapshot snapshot,
        string targetId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        cancellationToken.ThrowIfCancellationRequested();

        IReadOnlyCollection<string> expectedTexts = _trustedCriteriaResolver(snapshot, targetId)
            ?? throw new InvalidOperationException("Trusted navigation destination criteria are missing.");
        if (expectedTexts.Count == 0 || expectedTexts.Any(string.IsNullOrWhiteSpace))
        {
            throw new InvalidOperationException("Trusted navigation destination criteria are missing or invalid.");
        }

        // A post-action capture must succeed before OCR-based destination verification begins.
        var observation = await _observationService.ObserveAsync(cancellationToken).ConfigureAwait(false);
        if (observation is null || observation.ImageBytes.IsEmpty)
        {
            return new NavigationDestinationVerification(false, false);
        }

        try
        {
            await _pageVerificationService
                .WaitForAllTextAsync(expectedTexts.ToArray(), _timeout, cancellationToken)
                .ConfigureAwait(false);
            return new NavigationDestinationVerification(true, true);
        }
        catch (TimeoutException)
        {
            return new NavigationDestinationVerification(true, false);
        }
    }
}