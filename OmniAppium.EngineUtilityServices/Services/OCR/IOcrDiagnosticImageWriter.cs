namespace OmniAppium.EngineUtilityServices.Services.OCR;

/// <summary>
/// Defines a diagnostic writer for persisting the image submitted to OCR.
/// </summary>
public interface IOcrDiagnosticImageWriter
{
    /// <summary>
    /// Writes the latest OCR input image for diagnostic inspection.
    /// </summary>
    /// <param name="imageBuffer">
    /// The exact image buffer submitted to the OCR engine.
    /// </param>
    /// <param name="path">
    /// The path where the diagnostic image should be saved.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the write operation.
    /// </param>
    Task WriteAsync(
        ReadOnlyMemory<byte> imageBuffer,
        string? path = null,
        CancellationToken cancellationToken = default);
}