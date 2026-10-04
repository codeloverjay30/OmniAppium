namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Represents an active authorization to execute an OCR-grounded action.
/// </summary>
internal sealed class OcrGroundedActionLease
    : IOcrGroundedActionLease
{
    private readonly Action _release;
    private int _isDisposed;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="OcrGroundedActionLease"/> class.
    /// </summary>
    /// <param name="release">
    /// The callback invoked when the authorization is released.
    /// </param>
    public OcrGroundedActionLease(Action release)
    {
        ArgumentNullException.ThrowIfNull(release);

        _release = release;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (Interlocked.Exchange(
                ref _isDisposed,
                1) != 0)
        {
            return;
        }

        _release();
    }
}