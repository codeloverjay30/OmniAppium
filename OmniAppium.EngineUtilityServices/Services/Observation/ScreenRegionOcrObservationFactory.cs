using Microsoft.Extensions.Logging;
using System.IO.Abstractions;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Stability;
using OmniAppium.EngineUtilityService.Services.Screenshots;

namespace OmniAppium.EngineUtilityServices.Services.Observation;

/// <summary>
/// Creates screen observation services with isolated region and OCR stability state.
/// </summary>
public sealed class ScreenRegionOcrObservationFactory
    : IScreenRegionOcrObservationFactory
{
    private readonly IScreenshotService _screenshots;
    private readonly IAndroidScreenOcrService _ocr;
    private readonly IAndroidScreenStabilityGuard _legacyGuard;
    private readonly IScreenFrameDifferenceAnalyzer _analyzer;
    private readonly IOcrTargetStabilityEvaluatorFactory _evaluatorFactory;
    private readonly IOcrObservationLineNormalizer _normalizer;

    private readonly ILogger<AndroidScreenObservationService> _logger;
    private readonly IFileSystem _diagnosticFileSystem;
    private readonly string _diagnosticRunDirectory;

    /// <summary>
    /// Initializes the factory with its required observation dependencies.
    /// </summary>
    public ScreenRegionOcrObservationFactory(
        IScreenshotService screenshots,
        IAndroidScreenOcrService ocr,
        IAndroidScreenStabilityGuard legacyGuard,
        IScreenFrameDifferenceAnalyzer analyzer,
        IOcrTargetStabilityEvaluatorFactory evaluatorFactory,
        IOcrObservationLineNormalizer normalizer,
        ILogger<AndroidScreenObservationService> logger,
        IFileSystem? diagnosticFileSystem = null)
    {
        _screenshots = screenshots
            ?? throw new ArgumentNullException(nameof(screenshots));
        _ocr = ocr
            ?? throw new ArgumentNullException(nameof(ocr));
        _legacyGuard = legacyGuard
            ?? throw new ArgumentNullException(nameof(legacyGuard));
        _analyzer = analyzer
            ?? throw new ArgumentNullException(nameof(analyzer));
        _evaluatorFactory = evaluatorFactory
            ?? throw new ArgumentNullException(nameof(evaluatorFactory));
        _normalizer = normalizer
            ?? throw new ArgumentNullException(nameof(normalizer));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        _diagnosticFileSystem = diagnosticFileSystem ?? new FileSystem();
        _diagnosticRunDirectory = _diagnosticFileSystem.Path.Combine(
            AppContext.BaseDirectory, "Screenshots", "roi-differences",
            $"{DateTimeOffset.Now:yyyyMMdd-HHmmss}-{Guid.NewGuid():N}");
    }

    /// <summary>
    /// Creates an observation service with validated options.
    /// </summary>
    public IAndroidScreenObservationService Create(
        ScreenObservationStabilityOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new AndroidScreenObservationService(
            _screenshots,
            _ocr,
            _legacyGuard,
            new ScreenStabilityOptions(),
            _analyzer,
            _evaluatorFactory,
            _normalizer,
            options with
            {
                RoiDifferenceMaskDirectory = options.RoiDifferenceMaskDirectory ??
                    _diagnosticRunDirectory
            },
            _logger,
            _diagnosticFileSystem);
    }
}