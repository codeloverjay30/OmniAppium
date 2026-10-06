using System.Globalization;
using System.IO.Abstractions;
using System.Threading;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Screenshots;

namespace OmniAppium.EngineUtilityServices.Services.OCR;

/// <summary>
/// Coordinates Android screenshot capture, diagnostic image persistence,
/// and optical character recognition.
/// </summary>
public sealed class AndroidScreenOcrService(
    IScreenshotService screenshotService,
    IOCRUtilityService ocrUtilityService,
    ILoggerFactoryBaseUtilityService loggerFactoryBaseUtilityService,
    IOcrDiagnosticImageWriter? diagnosticImageWriter = null)
    : IAndroidScreenOcrService
{
    private const string EmptyImageMessage =
        "The Android screenshot contains no image data.";

    private readonly IOcrDiagnosticImageWriter _diagnosticImageWriter =
        diagnosticImageWriter
        ?? new OcrDiagnosticImageWriter(new FileSystem());

    private long _diagnosticSequence;

    private ILogger Logger =>
        loggerFactoryBaseUtilityService.Logger;

    /// <summary>
    /// Captures the current Android screen and recognizes text from the
    /// captured image.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token used to cancel the recognition workflow.
    /// </param>
    /// <returns>
    /// The OCR result produced from the captured Android screen.
    /// </returns>
    public async Task<OcrResult> RecognizeCurrentScreenAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        byte[] imageBuffer =
            screenshotService.CaptureScreenshotBytes();

        return await RecognizeAsync(
                imageBuffer,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Recognizes text from an already captured Android screen image without
    /// capturing another screenshot.
    /// </summary>
    /// <param name="imageBuffer">
    /// The exact encoded image buffer to submit to OCR.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the recognition workflow.
    /// </param>
    /// <returns>
    /// The OCR result produced from the supplied image.
    /// </returns>
    public async Task<OcrResult> RecognizeAsync(
        ReadOnlyMemory<byte> imageBuffer,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (imageBuffer.IsEmpty)
        {
            throw new InvalidOperationException(
                EmptyImageMessage);
        }

        long diagnosticSequence =
            Interlocked.Increment(
                ref _diagnosticSequence);

        string diagnosticFileName =
            string.Create(
                CultureInfo.InvariantCulture,
                $"ocr-diagnostic-{diagnosticSequence:D6}.png");

        string diagnosticPath =
            Path.Combine(
                AppContext.BaseDirectory,
                diagnosticFileName);

        Logger.LogInformation(
            "OCR diagnostic image #{DiagnosticSequence}: {DiagnosticPath}",
            diagnosticSequence,
            diagnosticPath);

        await _diagnosticImageWriter
            .WriteAsync(
                imageBuffer,
                diagnosticPath,
                cancellationToken)
            .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        return await ocrUtilityService
            .RecognizeAsync(
                imageBuffer.ToArray(),
                "zh-TW",
                cancellationToken)
            .ConfigureAwait(false);
    }
}