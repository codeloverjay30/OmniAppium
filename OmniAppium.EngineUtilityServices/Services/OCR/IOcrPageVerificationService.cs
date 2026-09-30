namespace OmniAppium.EngineUtilityServices.Services.OCR;

/// <summary>
/// Defines OCR-based verification of the current Android screen state.
/// </summary>
public interface IOcrPageVerificationService
{
    /// <summary>
    /// Waits until the expected text is uniquely recognized on the current
    /// Android screen.
    /// </summary>
    /// <param name="expectedText">
    /// The OCR text that identifies the expected screen state.
    /// </param>
    /// <param name="timeout">
    /// The maximum amount of time to wait for the expected text.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the verification operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous verification operation.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="expectedText"/> is null, empty, or consists
    /// only of white-space characters.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="timeout"/> is not greater than zero.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Thrown when the expected text cannot be found before the timeout expires.
    /// </exception>
    Task WaitForTextAsync(
        string expectedText,
        TimeSpan timeout,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Waits until all expected texts are uniquely recognized on the same
    /// current Android screen.
    /// </summary>
    /// <param name="expectedTexts">
    /// The OCR texts that collectively identify the expected screen state.
    /// </param>
    /// <param name="timeout">
    /// The maximum amount of time to wait for the expected screen state.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the verification operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous verification operation.
    /// </returns>
    Task WaitForAllTextAsync(
        IReadOnlyCollection<string> expectedTexts,
        TimeSpan timeout,
        CancellationToken cancellationToken = default);
}