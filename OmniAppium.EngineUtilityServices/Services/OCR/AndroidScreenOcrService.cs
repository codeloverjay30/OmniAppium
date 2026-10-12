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
/// Coordinates Android screenshot capture, best-effort diagnostic image persistence,
/// and optical character recognition.
/// </summary>
public sealed class AndroidScreenOcrService(
    IScreenshotService screenshotService,
    IOCRUtilityService ocrUtilityService,
    ILoggerFactoryBaseUtilityService loggerFactoryBaseUtilityService,
    IOcrDiagnosticImageWriter? diagnosticImageWriter = null,
    IFileSystem? fileSystem = null)
    : IAndroidScreenOcrService
{
    private const string EmptyImageMessage =
        "The Android screenshot contains no image data.";

    private readonly IFileSystem _fileSystem = fileSystem ?? new FileSystem();

    private readonly IOcrDiagnosticImageWriter _diagnosticImageWriter =
        diagnosticImageWriter ?? new OcrDiagnosticImageWriter(fileSystem ?? new FileSystem());

    private readonly string _diagnosticSessionId =
        $"{DateTimeOffset.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture)}-{Guid.NewGuid():N}";

    private long _diagnosticSequence;

    private ILogger Logger => loggerFactoryBaseUtilityService.Logger;

    /// <summary>
    /// Captures the current Android screen and recognizes text from the captured image.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel recognition.</param>
    /// <returns>The OCR result produced from the captured Android screen.</returns>
    public async Task<OcrResult> RecognizeCurrentScreenAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        byte[] imageBuffer = screenshotService.CaptureScreenshotBytes();

        return await RecognizeAsync(imageBuffer, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Recognizes text from an existing screenshot and attempts to persist
    /// a diagnostic copy without making diagnostic IO a prerequisite for OCR.
    /// </summary>
    /// <param name="imageBuffer">The exact encoded screenshot submitted to OCR.</param>
    /// <param name="cancellationToken">A token used to cancel recognition.</param>
    /// <returns>The OCR result produced from the supplied image.</returns>
    public async Task<OcrResult> RecognizeAsync(
        ReadOnlyMemory<byte> imageBuffer,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (imageBuffer.IsEmpty)
        {
            throw new InvalidOperationException(EmptyImageMessage);
        }

        long diagnosticSequence = Interlocked.Increment(ref _diagnosticSequence);
        string diagnosticFileName = string.Create(
            CultureInfo.InvariantCulture,
            $"ocr-diagnostic-{diagnosticSequence:D6}.png");

        try
        {
            string diagnosticDirectory = _fileSystem.Path.Combine(
                AppContext.BaseDirectory,
                "Screenshots",
                _diagnosticSessionId);

            string diagnosticPath = _fileSystem.Path.Combine(
                diagnosticDirectory,
                diagnosticFileName);

            _fileSystem.Directory.CreateDirectory(diagnosticDirectory);

            await _diagnosticImageWriter
                .WriteAsync(imageBuffer, diagnosticPath, cancellationToken)
                .ConfigureAwait(false);

            Logger.LogInformation(
                "OCR diagnostic image #{DiagnosticSequence}: {DiagnosticPath}",
                diagnosticSequence,
                diagnosticPath);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex) when (
            ex is IOException
            or UnauthorizedAccessException
            or NotSupportedException
            or ArgumentException)
        {
            Logger.LogWarning(
                ex,
                "Failed to persist OCR diagnostic image #{DiagnosticSequence}. OCR recognition will continue.",
                diagnosticSequence);
        }

        cancellationToken.ThrowIfCancellationRequested();

        return await ocrUtilityService
            .RecognizeAsync(imageBuffer.ToArray(), "zh-TW", cancellationToken)
            .ConfigureAwait(false);
    }
}
