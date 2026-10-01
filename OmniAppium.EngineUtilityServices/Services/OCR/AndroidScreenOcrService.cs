using System.IO.Abstractions;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Screenshots;

namespace OmniAppium.EngineUtilityServices.Services.OCR;

/// <summary>
/// Coordinates Android screenshot capture and optical character recognition.
/// </summary>
public sealed class AndroidScreenOcrService(
    IScreenshotService screenshotService,
    IOCRUtilityService ocrUtilityService,
    IOcrDiagnosticImageWriter? diagnosticImageWriter = null)
    : IAndroidScreenOcrService
{
    private readonly IOcrDiagnosticImageWriter _diagnosticImageWriter =
        diagnosticImageWriter
        ?? new OcrDiagnosticImageWriter(
            new FileSystem());

    /// <summary>
    /// Captures a fresh screenshot and recognizes text within it.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token used to cancel the recognition workflow.
    /// </param>
    /// <returns>
    /// Text and text-line bounds in the captured screenshot's pixel coordinates.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when screenshot capture returns no image data.
    /// </exception>
    public async Task<OcrResult> RecognizeCurrentScreenAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        screenshotService.TakeScreenshot();

        byte[] imageBuffer =
            screenshotService.GetBytesOfCachedScreenshotBytes();

        if (imageBuffer.Length == 0)
        {
            throw new InvalidOperationException(
                "The Android screenshot contains no image data.");
        }

        cancellationToken.ThrowIfCancellationRequested();

        // Preserve the exact image submitted to OCR for diagnostic inspection.
        string diagnosticPath = Path.Combine(
            AppContext.BaseDirectory,
            "ocr-diagnostic.png");
        
        await _diagnosticImageWriter.WriteAsync(
            imageBuffer,
            diagnosticPath,
            cancellationToken)
            .ConfigureAwait(false);
    
        return await ocrUtilityService
            .RecognizeAsync(imageBuffer,"zh-TW", cancellationToken)
            .ConfigureAwait(false);
    }
}