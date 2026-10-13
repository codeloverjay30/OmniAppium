using System.Diagnostics;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Stability;
using OmniAppium.EngineUtilityService.Services.Screenshots;

namespace OmniAppium.EngineUtilityServices.Services.Observation;

/// <summary>
/// Produces immutable Android screen observations after verifying
/// region-based pixel stability and OCR target stability.
/// </summary>
public sealed class AndroidScreenObservationService
    : IAndroidScreenObservationService
{
    private const string EmptyScreenshotMessage =
        "The captured Android screen contains no image data.";

    private readonly ILogger<AndroidScreenObservationService>? _logger;
    private readonly IFileSystem? _diagnosticFileSystem;
    private readonly IScreenshotService _screenshots;
    private readonly IAndroidScreenOcrService _ocr;
    private readonly IAndroidScreenStabilityGuard _legacyGuard;
    private readonly ScreenStabilityOptions _legacyOptions;

    private readonly IScreenFrameDifferenceAnalyzer? _analyzer;
    private readonly IOcrTargetStabilityEvaluatorFactory? _evaluatorFactory;
    private readonly IOcrObservationLineNormalizer? _normalizer;
    private readonly ScreenObservationStabilityOptions? _options;

    /// <summary>
    /// Initializes the legacy full-screen observation strategy.
    /// </summary>
    public AndroidScreenObservationService(
        IScreenshotService screenshotService,
        IAndroidScreenOcrService androidScreenOcrService,
        IAndroidScreenStabilityGuard stabilityGuard,
        ScreenStabilityOptions stabilityOptions,
        ILogger<AndroidScreenObservationService>? logger = null)
    {
        _logger = logger;
        _screenshots = screenshotService ??
            throw new ArgumentNullException(nameof(screenshotService));

        _ocr = androidScreenOcrService ??
            throw new ArgumentNullException(nameof(androidScreenOcrService));

        _legacyGuard = stabilityGuard ??
            throw new ArgumentNullException(nameof(stabilityGuard));

        _legacyOptions = stabilityOptions ??
            throw new ArgumentNullException(nameof(stabilityOptions));

        _legacyOptions.Validate();
    }

    /// <summary>
    /// Initializes the region and OCR target observation strategy.
    /// </summary>
    public AndroidScreenObservationService(
        IScreenshotService screenshotService,
        IAndroidScreenOcrService androidScreenOcrService,
        IAndroidScreenStabilityGuard stabilityGuard,
        ScreenStabilityOptions stabilityOptions,
        IScreenFrameDifferenceAnalyzer analyzer,
        IOcrTargetStabilityEvaluatorFactory evaluatorFactory,
        IOcrObservationLineNormalizer normalizer,
        ScreenObservationStabilityOptions observationOptions,
        ILogger<AndroidScreenObservationService>? logger = null,
        IFileSystem? diagnosticFileSystem = null)
        : this(
            screenshotService,
            androidScreenOcrService,
            stabilityGuard,
            stabilityOptions,
            logger)
    {
        _analyzer = analyzer ??
            throw new ArgumentNullException(nameof(analyzer));

        _evaluatorFactory = evaluatorFactory ??
            throw new ArgumentNullException(nameof(evaluatorFactory));

        _normalizer = normalizer ??
            throw new ArgumentNullException(nameof(normalizer));

        _options = observationOptions ??
            throw new ArgumentNullException(nameof(observationOptions));

        _options.Validate();
        _diagnosticFileSystem = diagnosticFileSystem;
    }

    /// <summary>
    /// Captures and recognizes a stable screen observation.
    /// </summary>
    public async Task<IAndroidScreenObservation> ObserveAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (_options is null)
        {
            return await ObserveLegacyAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        return await ObserveRegionAndOcrAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the existing full-screen stability strategy.
    /// </summary>
    private async Task<IAndroidScreenObservation> ObserveLegacyAsync(
        CancellationToken cancellationToken)
    {
        ScreenStabilityResult stability =
            await _legacyGuard.WaitForStabilityAsync(
                _legacyOptions,
                cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        if (!stability.IsStable || stability.TimedOut)
        {
            throw new TimeoutException(
                $"Android screen stability timeout after {stability.SampleCount} samples.");
        }

        byte[] image = CaptureChecked();
        cancellationToken.ThrowIfCancellationRequested();

        OcrResult result = await _ocr.RecognizeAsync(
            image,
            cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        return new AndroidScreenObservation(image, result);
    }

    /// <summary>
    /// Evaluates pixel stability and OCR target stability
    /// against consecutive captures of the same screen.
    /// </summary>
    private async Task<IAndroidScreenObservation> ObserveRegionAndOcrAsync(
        CancellationToken cancellationToken)
    {
        ScreenObservationStabilityOptions options = _options!;
        IScreenFrameDifferenceAnalyzer analyzer = _analyzer!;
        IOcrObservationLineNormalizer normalizer = _normalizer!;

        IOcrTargetStabilityEvaluator evaluator =
            _evaluatorFactory!.Create(
                options.TargetText,
                options.MaximumCenterMovement);

        byte[]? previousImage = null;
        int consecutiveRegionMatches = 0;
        int sampleCount = 0;
        double? lastDifferenceRatio = null;
        bool lastOcrTargetFound = false;
        bool lastOcrStable = false;
        bool lastRegionStable = false;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            while (stopwatch.Elapsed < options.Timeout)
            {
                cancellationToken.ThrowIfCancellationRequested();

                byte[] image = CaptureChecked();
                sampleCount++;

                cancellationToken.ThrowIfCancellationRequested();

                bool regionStable = false;

                if (previousImage is not null)
                {
                    ScreenFrameDifference difference =
                        analyzer.Analyze(
                            previousImage,
                            image,
                            options.Region,
                            options.ChannelTolerance);

                    lastDifferenceRatio = difference.DifferenceRatio;

                    if (options.SaveRoiDifferenceMasks &&
                        _diagnosticFileSystem is not null &&
                        analyzer is IScreenFrameDifferenceMaskProvider maskProvider &&
                        difference.DimensionsMatch)
                    {
                        await TryWriteRoiDifferenceMaskAsync(
                            maskProvider, previousImage, image, options,
                            sampleCount, cancellationToken).ConfigureAwait(false);
                    }

                    regionStable =
                        difference.DimensionsMatch &&
                        difference.TotalPixels > 0 &&
                        difference.DifferenceRatio <=
                            options.MaximumDifferenceRatio;

                    consecutiveRegionMatches = regionStable
                        ? Math.Min(
                            consecutiveRegionMatches + 1,
                            options.RequiredConsecutiveMatches)
                        : 0;
                }

                // Keep the captured image and its OCR result paired.
                OcrResult ocrResult =
                    await _ocr.RecognizeAsync(
                        image,
                        cancellationToken).ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();

                IReadOnlyList<NormalizedOcrTextLine> lines =
                    normalizer.Normalize(ocrResult, image);

                // Do not accumulate OCR stability across a pixel-unstable gap.
                if (previousImage is not null && !regionStable)
                {
                    evaluator.Reset();
                }

                bool ocrStable = evaluator.Observe(
                    lines,
                    options.RequiredConsecutiveMatches);

                bool targetFound = lines.Any(
                    line => string.Equals(
                        string.Concat(line.Text.Where(
                            character => !char.IsWhiteSpace(character))),
                        string.Concat(options.TargetText.Where(
                            character => !char.IsWhiteSpace(character))),
                        StringComparison.Ordinal));

                lastOcrTargetFound = targetFound;
                lastOcrStable = ocrStable;
                lastRegionStable = regionStable;

                _logger?.LogInformation(
                    "ROI sample {SampleIndex}: RegionStable={RegionStable}, " +
                    "DifferenceRatio={DifferenceRatio}, MaximumDifferenceRatio={MaximumDifferenceRatio}, ChannelTolerance={ChannelTolerance}, " +
                    "RegionMatches={RegionMatches}, OcrTargetFound={OcrTargetFound}, " +
                    "OcrStable={OcrStable}, OcrLineCount={OcrLineCount}, " +
                    "Region=({RegionX},{RegionY},{RegionWidth},{RegionHeight})",
                    sampleCount, regionStable, lastDifferenceRatio,
                    options.MaximumDifferenceRatio, options.ChannelTolerance, consecutiveRegionMatches,
                    targetFound, ocrStable, lines.Count,
                    options.Region.X, options.Region.Y,
                    options.Region.Width, options.Region.Height);

                if (consecutiveRegionMatches >=
                        options.RequiredConsecutiveMatches &&
                    ocrStable)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    return new AndroidScreenObservation(
                        image,
                        ocrResult);
                }

                previousImage = image;

                TimeSpan remaining =
                    options.Timeout - stopwatch.Elapsed;

                if (remaining <= TimeSpan.Zero)
                {
                    break;
                }

                TimeSpan delay =
                    options.SampleInterval < remaining
                        ? options.SampleInterval
                        : remaining;

                await Task.Delay(delay, cancellationToken)
                    .ConfigureAwait(false);
            }

            throw new TimeoutException(
                "Android screen ROI and OCR stability timeout " +
                $"after {sampleCount} samples. " +
                $"Target='{options.TargetText}', " +
                $"RegionMatches={consecutiveRegionMatches}, " +
                $"LastDifferenceRatio={lastDifferenceRatio?.ToString("F6") ?? "N/A"}, " +
                $"LastRegionStable={lastRegionStable}, " +
                $"LastOcrTargetFound={lastOcrTargetFound}, " +
                $"LastOcrStable={lastOcrStable}.");
        }
        finally
        {
            evaluator.Reset();
        }
    }

    /// <summary>
    /// Saves a diagnostic ROI difference mask without interrupting observation on filesystem failures.
    /// </summary>
    private async Task TryWriteRoiDifferenceMaskAsync(
        IScreenFrameDifferenceMaskProvider provider,
        byte[] previousImage,
        byte[] currentImage,
        ScreenObservationStabilityOptions options,
        int sampleCount,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            byte[] mask = provider.CreateDifferenceMask(
                previousImage, currentImage, options.Region, options.ChannelTolerance);
            string directory = options.RoiDifferenceMaskDirectory ??
                _diagnosticFileSystem!.Path.Combine(
                    AppContext.BaseDirectory, "Screenshots", "roi-differences");
            _diagnosticFileSystem!.Directory.CreateDirectory(directory);
            string path = _diagnosticFileSystem.Path.Combine(
                directory, $"roi-difference-{sampleCount:D3}.png");
            await _diagnosticFileSystem.File.WriteAllBytesAsync(
                path, mask, cancellationToken).ConfigureAwait(false);
            _logger?.LogInformation(
                "ROI difference mask saved. Sample={SampleIndex}, Path={Path}",
                sampleCount, path);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (
            exception is IOException or UnauthorizedAccessException or
            ArgumentException or OpenCvSharp.OpenCvSharpException)
        {
            _logger?.LogWarning(exception,
                "Failed to save ROI difference mask for sample {SampleIndex}.", sampleCount);
        }
    }

    /// <summary>
    /// Captures a screenshot and rejects empty image data.
    /// </summary>
    private byte[] CaptureChecked()
    {
        byte[] image = _screenshots.CaptureScreenshotBytes();

        if (image is null || image.Length == 0)
        {
            throw new InvalidOperationException(
                EmptyScreenshotMessage);
        }

        return image;
    }
}