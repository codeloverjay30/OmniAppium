using CoordinateUtilityServices;
using Microsoft.Extensions.Logging;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;

namespace OmniAppiumDemo.SmokeTests.Issue59;

/// <summary>
/// Captures independent, untrusted OCR evidence after a smoke-test observation failure.
/// This service never creates trusted snapshots or authorizes actions.
/// </summary>
public sealed class Issue59ReadOnlyOcrDiagnosticService
{
    private const int MaximumEncodedBytes = 32 * 1024 * 1024;
    private readonly IScreenshotService _screenshots;
    private readonly IAndroidScreenOcrService _ocr;
    private readonly ILogger<Issue59ReadOnlyOcrDiagnosticService> _logger;

    /// <summary>
    /// Initializes a read-only diagnostic capture boundary.
    /// </summary>
    public Issue59ReadOnlyOcrDiagnosticService(
        IScreenshotService screenshots,
        IAndroidScreenOcrService ocr,
        ILogger<Issue59ReadOnlyOcrDiagnosticService> logger)
    {
        _screenshots = screenshots ?? throw new ArgumentNullException(nameof(screenshots));
        _ocr = ocr ?? throw new ArgumentNullException(nameof(ocr));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Captures one fresh screenshot and records OCR provenance without returning
    /// any observation or grounded target to the caller.
    /// </summary>
    public async Task CaptureAndLogAsync(
        string phase,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(phase);
        cancellationToken.ThrowIfCancellationRequested();

        string diagnosticId = Guid.NewGuid().ToString("N");
        byte[] imageBytes = _screenshots.CaptureScreenshotBytes();
        if (imageBytes.Length == 0 || imageBytes.Length > MaximumEncodedBytes)
            throw new InvalidOperationException(
                "Issue 59 diagnostic screenshot size is invalid.");

        OcrResult ocrResult = await _ocr.RecognizeAsync(
            imageBytes, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException(
                "Issue 59 diagnostic OCR returned null.");

        // This observation is local to diagnostics and is never returned to the
        // production association or click-authorization boundaries.
        IAndroidScreenObservation diagnosticObservation =
            new AndroidScreenObservation(imageBytes, ocrResult);

        IReadOnlyList<OcrGroundedTarget> targets;
        try
        {
            targets = Issue59OcrTargetFactory.Create(diagnosticObservation);
        }
        catch (Exception ex) when (ex is ArgumentException
            or InvalidOperationException
            or OpenCvSharp.OpenCvSharpException)
        {
            _logger.LogWarning(ex,
                "Issue 59 diagnostic target factory rejected OCR data; id={DiagnosticId}, phase={Phase}",
                diagnosticId, phase);
            targets = Array.Empty<OcrGroundedTarget>();
        }

        var rawLines = ocrResult.Lines.Select(line =>
            (Text: line?.Text, Bounds: line is null
                ? Rectangle.FromXYWH(0, 0, 0, 0)
                : line.Bounds)).ToArray();

        var region = Rectangle.FromXYWH(1350, 850, 250, 230);
        IReadOnlyList<Issue59OcrProvenanceDiagnostics.Entry> entries =
            Issue59OcrProvenanceDiagnostics.Inspect(rawLines, targets, region);

        _logger.LogWarning(
            "Issue 59 UNTRUSTED OCR diagnostic: id={DiagnosticId}, phase={Phase}, rawLines={RawLines}, targets={Targets}, warriorRegionLines={WarriorRegionLines}, capturedAt={CapturedAt}",
            diagnosticId, phase, rawLines.Length, targets.Count, entries.Count,
            DateTimeOffset.UtcNow);

        foreach (var entry in entries)
        {
            _logger.LogWarning(
                "Issue 59 UNTRUSTED warrior OCR: id={DiagnosticId}, rawIndex={RawIndex}, text={Text}, x={X}, y={Y}, width={Width}, height={Height}, retained={Retained}, reason={Reason}",
                diagnosticId, entry.RawIndex, entry.Text,
                entry.Bounds.TopLeft.X, entry.Bounds.TopLeft.Y,
                entry.Bounds.Width, entry.Bounds.Height,
                entry.Retained, entry.Reason);
        }
    }
}