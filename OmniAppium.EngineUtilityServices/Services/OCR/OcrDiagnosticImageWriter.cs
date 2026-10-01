using System.IO.Abstractions;

namespace OmniAppium.EngineUtilityServices.Services.OCR;

public sealed class OcrDiagnosticImageWriter(
    IFileSystem fileSystem)
    : IOcrDiagnosticImageWriter
{
    private const string DiagnosticFileName = "ocr-diagnostic.png";

    /// <summary>
    /// Writes the latest OCR input image to the diagnostic output file.
    /// </summary>
    /// <param name="imageBuffer">
    /// The exact image buffer submitted for OCR recognition.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the write operation.
    /// </param>
    public async Task WriteAsync(
        ReadOnlyMemory<byte> imageBuffer,
        string? path = null,
        CancellationToken cancellationToken = default)
    {
        if (imageBuffer.IsEmpty)
        {
            throw new ArgumentException(
                "The OCR diagnostic image buffer cannot be empty.",
                nameof(imageBuffer));
        }

        cancellationToken.ThrowIfCancellationRequested();

        path ??= fileSystem.Path.Combine(
            AppContext.BaseDirectory,
            DiagnosticFileName);

        await fileSystem.File
            .WriteAllBytesAsync(
                path,
                imageBuffer.ToArray(),
                cancellationToken)
            .ConfigureAwait(false);
    }
}
