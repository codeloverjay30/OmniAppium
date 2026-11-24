#define DEVELOPING
#define IS_LOGGING
// #define GEMINI_READ_ONLY_SMOKE_TEST
//#define ISSUE48_CLICK_SMOKE_TEST
#define AUTO_EXECUTE_TASKS

#if ISSUE48_CLICK_SMOKE_TEST && GEMINI_READ_ONLY_SMOKE_TEST
#error Issue48 and Gemini read-only smoke test modes are mutually exclusive.
#endif

#if ISSUE48_CLICK_SMOKE_TEST && AUTO_EXECUTE_TASKS
#error Issue48 smoke test and automatic task execution are mutually exclusive.
#endif


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OCRUtilityServices.Models;
using OmniAppium.ConfigUtilityServices.Models;
using OmniAppium.EngineUtilityService.Utilities;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppium.EngineUtilityServices.Services.Stability;
using OmniAppium.EngineUtilityServices.Workflows;
using OmniAppium.ConfigUtilityServices.Services;
using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.AiBaseUtilityServices.Services;
using AiUtility.Configurations;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Executor;
using AiUtility.GeminiKits.Mappers;
using AiUtility.GeminiKits.Registry;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiKits.Security;
using AiUtility.GeminiUtilityServices.Configs;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AiUtility.ToolKits.Abstractions;
using AiUtility.ToolKits.Execution;
using AiUtility.ToolKits.Services;
using CommonModels;
using CoordinateUtilityServices;
using EnumUtilityServices;
using ExpressionTreeUtilityServices;
using JsonUtilityServices;
using LoggerFactoryUtilityServices;

using OmniAppium.ConfigUtilityServices.Controllers;
using OmniAppium.ConfigUtilityServices.Factories;
using OmniAppium.EngineUtilityService.Services.Screen;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityService.Services.Wait;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.LogServices;
using OmniAppiumDemo.DependencyInjection;
using ReflectionUtilityServices;
using System.IO.Abstractions;
using ThreadLevelLockingUtilityServices;
using ThreadLevelLockingUtilityServices.Models;
using TransversalUtilityServices;
using TypeUtilityServices;
using OmniAppium.EngineUtilityServices.Interfaces;
using OmniAppiumDemo.Games.BeautyThreeKingdoms.Startup;
using OmniAppium.EngineUtilityServices.Startup;
using OCRUtilityServices.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using OmniAppiumDemo.SmokeTests;
using OmniAppiumDemo.SmokeTests.Issue55;
using OmniAppiumDemo.SmokeTests.Issue59;
using OmniAppium.EngineUtilityServices.Services.Exploration;

namespace OmniAppiumDemo;

/// <summary>
/// Provides the ROI and OCR observation entry point for an existing Appium host.
/// </summary>
public static class Program
{
    private sealed class Issue59DiagnosticRunner(OCRUtilityServices.Paddle.Python.IPythonOcrProcessRunner inner) : OCRUtilityServices.Paddle.Python.IPythonOcrProcessRunner
    {
        public string? ResponseJson { get; private set; }
        public async Task<string> RunAsync(string imagePath, System.Threading.CancellationToken cancellationToken)
        {
            string json = await inner.RunAsync(imagePath, cancellationToken);
            ResponseJson = json;
            return json;
        }
    }

    private static OCRUtilityServices.Paddle.Python.PythonOcrProcessOptions ReadIssue59PythonOcrOptions()
    {
        string executable = Environment.GetEnvironmentVariable("ISSUE59_PYTHON_EXECUTABLE")
            ?? throw new InvalidOperationException("Set ISSUE59_PYTHON_EXECUTABLE.");
        string script = Environment.GetEnvironmentVariable("ISSUE59_PYTHON_BRIDGE_SCRIPT")
            ?? throw new InvalidOperationException("Set ISSUE59_PYTHON_BRIDGE_SCRIPT.");
        return new(Path.GetFullPath(executable), Path.GetFullPath(script), TimeSpan.FromMinutes(3));
    }

    private sealed record ScreenRegion(double X, double Y, double Width, double Height)
    {
        public OpenCvSharp.Rect ToPixels(OpenCvSharp.Mat image)
        {
            if (!(X >= 0 && Y >= 0 && Width > 0 && Height > 0 &&
                  X + Width <= 1 && Y + Height <= 1))
                throw new InvalidDataException("Readiness regions must be normalized to [0, 1].");

            int left = (int)Math.Floor(X * image.Width);
            int top = (int)Math.Floor(Y * image.Height);
            int right = (int)Math.Ceiling((X + Width) * image.Width);
            int bottom = (int)Math.Ceiling((Y + Height) * image.Height);
            return new OpenCvSharp.Rect(left, top, right - left, bottom - top);
        }
    }

    private sealed record ScreenReadinessOptions(
        string ReferenceImage, ScreenRegion TemplateRegion, ScreenRegion SearchRegion,
        double MinimumScore, int TimeoutSeconds, int PollMilliseconds)
    {
        public string? Reject(byte[] screenshot, OpenCvSharp.Mat template)
        {
            using var frame = OpenCvSharp.Cv2.ImDecode(screenshot, OpenCvSharp.ImreadModes.Grayscale);
            using var search = new OpenCvSharp.Mat(frame, SearchRegion.ToPixels(frame));
            if (template.Width > search.Width || template.Height > search.Height)
                throw new InvalidDataException("Readiness template exceeds the search region.");
            using var scores = new OpenCvSharp.Mat();
            OpenCvSharp.Cv2.MatchTemplate(search, template, scores, OpenCvSharp.TemplateMatchModes.CCoeffNormed);
            OpenCvSharp.Cv2.MinMaxLoc(scores, out _, out double maximum, out _, out _);
            return double.IsFinite(maximum) && maximum >= MinimumScore
                ? null
                : $"target screen not ready (template score {maximum:F3}, minimum {MinimumScore:F3})";
        }
    }

    private static async Task<int> RunIssue59BottomCaptureAsync(
        Func<byte[]> captureFrame,
        Func<byte[], Task<int>> verifyFrame,
        Microsoft.Extensions.Logging.ILogger logger,
        string? screenshotsDirectory,
        TimeSpan retryDelay,
        Func<byte[], string?>? rejectScreen = null,
        TimeSpan? readinessTimeout = null)
    {
        var timer = System.Diagnostics.Stopwatch.StartNew();
        const int maximumAttempts = 4;
        string captureId = Guid.NewGuid().ToString("N");
        byte[] screenshot = Array.Empty<byte>();
        string? rejection = null;
        for (int attempt = 1; readinessTimeout.HasValue || attempt <= maximumAttempts; attempt++)
        {
            if (attempt > 1 && retryDelay > TimeSpan.Zero)
            {
                TimeSpan remaining = readinessTimeout is { } timeout
                    ? timeout - timer.Elapsed : retryDelay;
                if (remaining <= TimeSpan.Zero)
                    break;
                await Task.Delay(remaining < retryDelay ? remaining : retryDelay);
                if (readinessTimeout is { } deadline && timer.Elapsed >= deadline)
                    break;
            }

            screenshot = captureFrame();
            rejection = GetIssue59BottomFrameRejection(screenshot);
            if (rejection is null && rejectScreen is not null)
                rejection = rejectScreen(screenshot);
            if (readinessTimeout is { } limit && timer.Elapsed >= limit)
                rejection ??= "readiness deadline elapsed during capture";

            string? diagnosticPath = null;
            if (screenshotsDirectory is not null && screenshot.Length > 0)
            {
                diagnosticPath = Path.Combine(screenshotsDirectory, $"issue59-bottom-{captureId}-{attempt:D2}.png");
                await File.WriteAllBytesAsync(diagnosticPath, screenshot);
            }

            logger.LogInformation(
                "Issue 59 bottom frame {Attempt}/{MaximumAttempts}: {Decision}, path={Path}",
                attempt, readinessTimeout.HasValue ? "deadline" : maximumAttempts.ToString(), rejection ?? "accepted", diagnosticPath ?? "(none)");
            if (rejection is null)
            {
                logger.LogInformation("Issue 59 bottom navigation input: {Path}", diagnosticPath);
                break;
            }
        }

        if (rejection is not null)
        {
            if (readinessTimeout.HasValue)
            {
                logger.LogError("Screen readiness timed out after {ElapsedMs} ms: {Reason}",
                    timer.ElapsedMilliseconds, rejection);
                return 2;
            }

            logger.LogError(
                "Issue 59 bottom navigation: no usable frame after {Attempts} attempts, elapsedMs={ElapsedMs}, lastReason={Reason}",
                maximumAttempts, timer.ElapsedMilliseconds, rejection);
            return 2;
        }

        var captureMs = timer.ElapsedMilliseconds;
        int exitCode = await verifyFrame(screenshot);
        logger.LogInformation(
            "Issue 59 bottom navigation: captureMs={CaptureMs}, verificationMs={VerificationMs}, totalMs={TotalMs}, exitCode={ExitCode}",
            captureMs, timer.ElapsedMilliseconds - captureMs, timer.ElapsedMilliseconds, exitCode);
        return exitCode;
    }

    private static async Task<(ScreenReadinessOptions Options, OpenCvSharp.Mat Reference)> LoadScreenReadinessAsync(string readinessPath)
    {
        readinessPath = Path.GetFullPath(readinessPath);
        var readiness = System.Text.Json.JsonSerializer.Deserialize<ScreenReadinessOptions>(
            await File.ReadAllTextAsync(readinessPath));
        if (readiness is null || string.IsNullOrWhiteSpace(readiness.ReferenceImage) ||
            readiness.TemplateRegion is null || readiness.SearchRegion is null ||
            readiness.TimeoutSeconds <= 0 || readiness.PollMilliseconds <= 0 ||
            !(readiness.MinimumScore > 0 && readiness.MinimumScore <= 1))
            throw new InvalidDataException("Invalid screen readiness configuration.");

        string referencePath = Path.Combine(Path.GetDirectoryName(readinessPath)!, readiness.ReferenceImage);
        var reference = OpenCvSharp.Cv2.ImRead(referencePath, OpenCvSharp.ImreadModes.Grayscale);
        if (reference.Empty())
        {
            reference.Dispose();
            throw new InvalidDataException($"Could not load readiness reference: {referencePath}");
        }
        return (readiness, reference);

    }

    private static async Task<int> RunIssue59ReadinessTimeoutRegressionAsync(string[] args)
    {
        if (args.Length != 3)
        {
            Console.Error.WriteLine("Usage: --issue59-readiness-timeout-regression <readiness.json> <update.png>");
            return 2;
        }

        var (readiness, referenceImage) = await LoadScreenReadinessAsync(args[1]);
        using var reference = referenceImage;
        using var template = new OpenCvSharp.Mat(reference, readiness.TemplateRegion.ToPixels(reference));
        byte[] updating = await File.ReadAllBytesAsync(args[2]);
        string? frameRejection = GetIssue59BottomFrameRejection(updating);
        if (frameRejection is not null)
        {
            Console.Error.WriteLine($"Update fixture failed the basic frame check: {frameRejection}");
            return 2;
        }

        string? reason = readiness.Reject(updating, template);
        if (reason is null)
        {
            Console.Error.WriteLine("Update fixture unexpectedly matched the ready screen.");
            return 2;
        }

        var logger = new ReadinessRegressionLogger();
        int captures = 0;
        int verifyCalls = 0;
        int exitCode = await RunIssue59BottomCaptureAsync(
            () => { captures++; return updating; },
            _ => { verifyCalls++; return Task.FromResult(0); },
            logger, screenshotsDirectory: null, retryDelay: TimeSpan.FromMilliseconds(50),
            rejectScreen: screenshot => readiness.Reject(screenshot, template),
            readinessTimeout: TimeSpan.FromSeconds(1));

        bool passed = captures > 0 && verifyCalls == 0 && exitCode == 2 && logger.TimedOut;
        Console.WriteLine($"Issue #59 readiness timeout: captures={captures}, verifyCalls={verifyCalls}, exitCode={exitCode}, pass={passed}");
        return passed ? 0 : 2;
    }

    private sealed class ReadinessRegressionLogger : Microsoft.Extensions.Logging.ILogger
    {
        public bool TimedOut { get; private set; }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel level) =>
            level == Microsoft.Extensions.Logging.LogLevel.Error;

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel level,
            Microsoft.Extensions.Logging.EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(level))
                return;

            string message = formatter(state, exception);
            TimedOut |= message.StartsWith("Screen readiness timed out", StringComparison.Ordinal);
            Console.WriteLine(message);
        }
    }

    private static async Task<int> RunIssue59BottomRetakeRegressionAsync(string[] args)
    {
        if (args.Length != 3)
        {
            Console.Error.WriteLine("Usage: --issue59-bottom-retake-regression <dark.png> <normal.png>");
            return 2;
        }

        byte[] darkFrame = await File.ReadAllBytesAsync(args[1]);
        byte[] normalFrame = await File.ReadAllBytesAsync(args[2]);
        string? darkReason = GetIssue59BottomFrameRejection(darkFrame);
        string? normalReason = GetIssue59BottomFrameRejection(normalFrame);
        if (darkReason?.StartsWith("dark frame", StringComparison.Ordinal) != true || normalReason is not null)
        {
            Console.Error.WriteLine($"Unexpected fixtures. Dark: {darkReason ?? "accepted"}. Normal: {normalReason ?? "accepted"}.");
            return 2;
        }

        // The regression reports capture and verification counts directly.
        var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;

        async Task<bool> CheckAsync(string name, byte[][] frames, int expectedCaptures, int expectedVerifications, int expectedExit)
        {
            int captures = 0;
            int verifications = 0;
            bool verifiedNormalFrame = true;
            int exitCode = await RunIssue59BottomCaptureAsync(
                () => frames[captures++],
                frame =>
                {
                    verifications++;
                    verifiedNormalFrame &= ReferenceEquals(frame, normalFrame);
                    return Task.FromResult(0);
                },
                logger, screenshotsDirectory: null, retryDelay: TimeSpan.Zero);
            bool passed = captures == expectedCaptures && verifications == expectedVerifications
                && exitCode == expectedExit && verifiedNormalFrame;
            Console.WriteLine($"Issue #59 {name}: captures={captures}, verifyCalls={verifications}, exitCode={exitCode}, pass={passed}");
            return passed;
        }

        bool recovered = await CheckAsync("dark then normal", [darkFrame, normalFrame], 2, 1, 0);
        bool exhausted = await CheckAsync("four dark frames",
            [darkFrame, darkFrame, darkFrame, darkFrame, normalFrame], 4, 0, 2);
        return recovered && exhausted ? 0 : 2;
    }

    private static string? GetIssue59BottomFrameRejection(byte[] screenshot)
    {
        if (screenshot.Length == 0)
            return "empty screenshot";

        using var frame = OpenCvSharp.Cv2.ImDecode(screenshot, OpenCvSharp.ImreadModes.Grayscale);
        if (frame.Empty())
            return "image decode failed";

        if (frame.Width != 2340 || frame.Height != 1080)
            return $"unexpected resolution {frame.Width}x{frame.Height}";

        const byte minimumMedianLuminance = 57;
        var samples = new byte[77];
        int index = 0;
        for (int y = 1; y <= 7; y++)
        {
            for (int x = 1; x <= 11; x++)
                samples[index++] = frame.At<byte>(y * frame.Height / 8, x * frame.Width / 12);
        }

        Array.Sort(samples);
        byte median = samples[samples.Length / 2];
        return median < minimumMedianLuminance
            ? $"dark frame (median {median} < {minimumMedianLuminance})"
            : null;
    }

    private static async Task<int> VerifyIssue59BottomNavigationAsync(IServiceProvider provider, byte[] screenshot)
    {
        using var frame = OpenCvSharp.Cv2.ImDecode(screenshot, OpenCvSharp.ImreadModes.Color);
        if (frame.Empty() || frame.Width != 2340 || frame.Height != 1080) { Console.Error.WriteLine("Issue #59: expected a 2340x1080 screenshot."); return 2; }
        byte[] template = await File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory, "Templates", "Notifications", "red-exclamation.png"));
        var templateOptions = new NotificationTemplateMatchOptions(RequireExclamationGlyph: true);
        var cases = new[] { (Id: "general", Text: "武將", BadgeX: 1508), (Id: "equipment", Text: "裝備", BadgeX: 1678), (Id: "bag", Text: "背包", BadgeX: 1848) };
        var bottomRegion = CoordinateUtilityServices.Rectangle.FromXYWH(1415, 985, 475, 80);
        var bottomOcr = await provider.GetRequiredService<OCRUtilityServices.Services.IOCRUtilityService>().RecognizeRegionAsync(screenshot, bottomRegion, "zh-TW");
        if (!bottomOcr.Lines.Any(line => string.Concat(line.Text.Where(c => !char.IsWhiteSpace(c))) == "裝備"))
        {
            var equipmentRegion = CoordinateUtilityServices.Rectangle.FromXYWH(1560, 960, 170, 120);
            var retry = await provider.GetRequiredService<OCRUtilityServices.Services.IOCRUtilityService>().RecognizeRegionAsync(screenshot, equipmentRegion, "zh-TW");
            Console.WriteLine("Issue #59 equipment retry OCR: " + string.Join(" | ", retry.Lines.Select(line => line.Text)));
            if (NotificationRegionalOcrCandidateSelector.Select(retry, equipmentRegion, frame.Width, frame.Height, "裝備", "equipment") is not null)
            {
                var combined = bottomOcr.Lines.Concat(retry.Lines.Where(line => string.Concat(line.Text.Where(c => !char.IsWhiteSpace(c))) == "裝備")).ToArray();
                bottomOcr = new OCRUtilityServices.Models.OcrResult(string.Join(Environment.NewLine, combined.Select(line => line.Text)), combined);
            }
        }
        Console.WriteLine("Issue #59 bottom OCR: " + string.Join(" | ", bottomOcr.Lines.Select(line => line.Text)));
        var bottomTargets = new List<OcrGroundedTarget>();
        foreach (var item in cases)
        {
            var label = NotificationRegionalOcrCandidateSelector.Select(bottomOcr, bottomRegion, frame.Width, frame.Height, item.Text, item.Id);
            if (label is null) { Console.Error.WriteLine($"Issue #59 FAIL: missing unique OCR label {item.Text}."); return 2; }
            bottomTargets.Add(label);
        }
        var bottomIcons = new SameFrameNotificationIconDetector().Detect(screenshot, bottomTargets);
        Console.WriteLine("Issue #59 bottom icons: " + string.Join(",", bottomIcons.Select(icon => icon.TargetId)));
        var bottomObservation = new AndroidScreenObservation(screenshot, bottomOcr);
        var bottomAssociation = provider.GetRequiredService<IRedDotObservationIntegrationService>().Analyze(new RedDotObservationInput(Guid.NewGuid().ToString("N"), bottomObservation, bottomTargets) { IconTargets = bottomIcons, NotificationTemplate = template, TemplateOptions = templateOptions });
        bool passed = bottomIcons.Count == cases.Length;
        foreach (var item in cases)
        {
            var badges = bottomAssociation.Evidence.Where(badge => Math.Abs(badge.Bounds.Center.X - item.BadgeX) <= 30 && badge.Bounds.Center.Y >= 895 && badge.Bounds.Center.Y <= 950).ToArray();
            bool correct = badges.Length == 1 && !badges[0].IsAmbiguous && badges[0].CandidateNodeIds.Count == 1 && badges[0].CandidateNodeIds[0] == item.Id;
            Console.WriteLine($"Issue #59 {item.Text}: badges={badges.Length}; owners={string.Join(",", badges.SelectMany(badge => badge.CandidateNodeIds))}; pass={correct}");
            passed &= correct;
        }
        Console.WriteLine(passed ? "Issue #59 PASS: all three bottom badges have distinct expected owners." : "Issue #59 FAIL: bottom-navigation ownership was not verified.");
        return passed ? 0 : 2;
    }

    /// <summary>
    /// Initializes the Appium demo and runs the selected smoke test or automation mode.
    /// </summary>
    public static async Task<int> Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--issue59-readiness-timeout-regression")
            return await RunIssue59ReadinessTimeoutRegressionAsync(args);

        if (args.Length > 0 && args[0] == "--issue59-bottom-retake-regression")
            return await RunIssue59BottomRetakeRegressionAsync(args);

        if (args.Length > 0 && args[0] == "--issue59-badge-file")
        {
            if (args.Length != 2 && (args.Length != 3 || args[2] != "--bottom-navigation")) { Console.Error.WriteLine("Usage: --issue59-badge-file <2340x1080 screenshot> [--bottom-navigation]"); return 2; }
            byte[] screenshot = await File.ReadAllBytesAsync(args[1]);
            bool IsActivityBadge(double x, double y) => x >= 1630 && x < 1710 && y >= 65 && y < 135;
            var services = new ServiceCollection();
            services.AddLogging();
            services.UseIssue59PythonOcr(ReadIssue59PythonOcrOptions());
            services.AddIssue59RedDotExploration();
            using var provider = services.BuildServiceProvider();
            if (args.Length == 3)
                return await VerifyIssue59BottomNavigationAsync(provider, screenshot);

            using var frame = OpenCvSharp.Cv2.ImDecode(screenshot, OpenCvSharp.ImreadModes.Color);
            if (frame.Empty() || frame.Width != 2340 || frame.Height != 1080) { Console.Error.WriteLine("Issue #59: expected a 2340x1080 screenshot."); return 2; }
            byte[] template = await File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory, "Templates", "Notifications", "red-exclamation.png"));
            var templateOptions = new NotificationTemplateMatchOptions(RequireExclamationGlyph: true);
            int nearbyBadges = provider.GetRequiredService<INotificationTemplateMatcher>().Match(screenshot, template, templateOptions).Count(match => IsActivityBadge(match.X + match.Width / 2.0, match.Y + match.Height / 2.0));
            Console.WriteLine($"Issue #59 activity-slot badge candidates: {nearbyBadges}");
            if (nearbyBadges != 1) { Console.Error.WriteLine("Issue #59: expected exactly one badge near the activity icon."); return 2; }
            var region = CoordinateUtilityServices.Rectangle.FromXYWH(1568, 126, 142, 74);
            var ocr = provider.GetRequiredService<OCRUtilityServices.Services.IOCRUtilityService>();
            var result = await ocr.RecognizeRegionAsync(screenshot, region, "zh-TW");
            Console.WriteLine("Issue #59 regional OCR: " + string.Join(" | ", result.Lines.Select(line => line.Text)));
            var target = NotificationRegionalOcrCandidateSelector.Select(result, region, frame.Width, frame.Height, "超值活動", "issue59-activity");
            if (target is null) { Console.Error.WriteLine("Issue #59: no unique corroborated activity label."); return 2; }
            OcrGroundedTarget[] targets = [target];
            var icons = new SameFrameNotificationIconDetector().Detect(screenshot, targets);
            if (icons.Count != 1)
            {
                var activityBadge = provider.GetRequiredService<INotificationTemplateMatcher>()
                    .Match(screenshot, template, templateOptions).Single(match => IsActivityBadge(match.X + match.Width / 2.0, match.Y + match.Height / 2.0));
                var circles = new SameFrameNotificationIconDiagnostics().Diagnose(screenshot, targets, activityBadge.X + activityBadge.Width / 2.0, activityBadge.Y + activityBadge.Height / 2.0);
                Console.WriteLine($"Issue #59 circle diagnostics: {circles.Count}; label bounds={target.Bounds}");
                foreach (var circle in circles)
                    Console.WriteLine($"circle=({circle.CenterX:F1},{circle.CenterY:F1}) radius={circle.Radius:F1} reason={circle.Reason} nearest={circle.NearestText} labelReason={circle.NearestLabelReason} aligned={circle.AlignedLabelCount}");
                Console.Error.WriteLine($"Issue #59: expected one same-frame icon, found {icons.Count}.");
                return 2;
            }
            var observation = new AndroidScreenObservation(screenshot, result);
            var input = new RedDotObservationInput(Guid.NewGuid().ToString("N"), observation, targets)
            {
                IconTargets = icons,
                NotificationTemplate = template,
                TemplateOptions = templateOptions
            };
            var association = provider.GetRequiredService<IRedDotObservationIntegrationService>().Analyze(input);
            var activityBadges = association.Evidence.Where(evidence => IsActivityBadge(evidence.Bounds.Center.X, evidence.Bounds.Center.Y)).ToArray();
            foreach (var badge in activityBadges)
                Console.WriteLine($"Issue #59 badge confidence={badge.Confidence:F3}; ambiguous={badge.IsAmbiguous}; owners={string.Join(",", badge.CandidateNodeIds)}");
            if (activityBadges.Length != 1 || activityBadges[0].IsAmbiguous || activityBadges[0].CandidateNodeIds[0] != target.Id)
            {
                Console.Error.WriteLine("Issue #59: activity badge ownership was not verified.");
                return 2;
            }
            Console.WriteLine("Issue #59 PASS: badge uniquely associated with 超值活動.");
            return 0;
        }

        if (args.Length > 0 && args[0] == "--issue59-ocr-smoke")
        {
            string target = args.Length is 7 or 11 ? args[6] : "限時禮包";
            int left = 1818, top = 238, right = 1928, bottom = 276;
            if (string.IsNullOrWhiteSpace(target) ||
                (args.Length != 2 &&
                 (args.Length is not (6 or 7 or 11) ||
                  !int.TryParse(args[2], out left) || !int.TryParse(args[3], out top) ||
                  !int.TryParse(args[4], out right) || !int.TryParse(args[5], out bottom) ||
                  left < 0 || top < 0 || right <= left || bottom <= top)))
            {
                Console.Error.WriteLine("Usage: --issue59-ocr-smoke <image> [left top right bottom [target [cropLeft cropTop cropRight cropBottom]]]");
                return 2;
            }

            var smokeServices = new ServiceCollection();
            smokeServices.UseIssue59PythonOcr(ReadIssue59PythonOcrOptions());
            Issue59DiagnosticRunner? diagnosticRunner = null;
            smokeServices.AddSingleton<OCRUtilityServices.Paddle.Python.IPythonOcrProcessRunner>(provider =>
                diagnosticRunner = new Issue59DiagnosticRunner(new OCRUtilityServices.Paddle.Python.PythonOcrProcessRunner(
                    provider.GetRequiredService<OCRUtilityServices.Paddle.Python.PythonOcrProcessOptions>())));
            using var smokeProvider = smokeServices.BuildServiceProvider();
            var ocr = smokeProvider.GetRequiredService<OCRUtilityServices.Services.IOCRUtilityService>();
            CoordinateUtilityServices.Rectangle? region = null;
            if (args.Length == 11)
            {
                if (!int.TryParse(args[7], out int cropLeft) || !int.TryParse(args[8], out int cropTop) ||
                    !int.TryParse(args[9], out int cropRight) || !int.TryParse(args[10], out int cropBottom) ||
                    cropLeft < 0 || cropTop < 0 || cropRight <= cropLeft || cropBottom <= cropTop)
                {
                    Console.Error.WriteLine("Invalid OCR crop bounds.");
                    return 2;
                }

                region = CoordinateUtilityServices.Rectangle.FromXYWH(
                    cropLeft, cropTop, cropRight - cropLeft, cropBottom - cropTop);
            }

            byte[] image = await File.ReadAllBytesAsync(args[1]);
            var result = region is { } crop
                ? await ocr.RecognizeRegionAsync(image, crop, "zh-TW")
                : await ocr.RecognizeAsync(image);
            if (diagnosticRunner?.ResponseJson is { } json)
            {
                var envelope = global::Issue59.Contract.OcrContractReader.Read(json);
                double dx = region is { } roi ? roi.TopLeft.X : 0;
                double dy = region is { } roiY ? roiY.TopLeft.Y : 0;
                foreach (var item in envelope.ResolvedTextInstances.Concat(envelope.UnresolvedMergedBoxes))
                {
                    var box = item.Bounds;
                    Console.WriteLine($"[Bridge] {item.Text} ({box.Left + dx}, {box.Top + dy})-({box.Right + dx}, {box.Bottom + dy}) status={item.LocalizationStatus} corroboration={item.CorroborationCount}");
                }
            }

            var matches = result.Lines.Where(line => string.Equals(line.Text, target, StringComparison.Ordinal)).ToArray();
            if (matches.Length != 1 || result.Lines.Any(line =>
                    line.Text != target && line.Text.Contains(target, StringComparison.Ordinal)))
            {
                Console.Error.WriteLine($"Expected one independent match for '{target}'. Exact matches: {matches.Length}.");
                return 2;
            }

            var bounds = matches[0].Bounds;
            Console.WriteLine($"{target}: ({bounds.TopLeft.X}, {bounds.TopLeft.Y})–({bounds.BottomRight.X}, {bounds.BottomRight.Y})");
            bool withinExpectedRegion = bounds.TopLeft.X >= left && bounds.TopLeft.Y >= top &&
                bounds.BottomRight.X <= right && bounds.BottomRight.Y <= bottom;
            if (!withinExpectedRegion)
                Console.Error.WriteLine($"Expected ROI: ({left}, {top})–({right}, {bottom})");
            return withinExpectedRegion ? 0 : 2;
        }

#if DEVELOPING

        // ============================================================
        // 1. Application paths.
        // ============================================================

        string appDir =
            AppDomain.CurrentDomain.BaseDirectory;

        string developmentDeviceConfigPath =
            Path.Combine(appDir, "development-device.config.json5");

        string appConfigPath =
            Path.Combine(appDir, "app.config.json5");

        string appiumConfigPath =
            Path.Combine(appDir, "appium.config.json5");

        string connectionConfigPath =
            Path.Combine(appDir, "connection.config.json5");

        string gameConfigPath =
            Path.Combine(appDir, "game.config.json5");

        string jobsConfigPath =
            Path.Combine(appDir, "jobs.config.json5");

        string geminiSecureConfigPath =
            Path.Combine(appDir, "secure.config.json5");

        string geminiConfigPath =
            Path.Combine(appDir, "gemini.config.json5");

        string logDirectory =
            Path.Combine(appDir, "Logs");

        string screenshotsDirectory =
            Path.Combine(appDir, "Screenshots");

        Directory.CreateDirectory(logDirectory);
        Directory.CreateDirectory(screenshotsDirectory);

        // ============================================================
        // 2. Logging.
        // ============================================================

        var loggingConfigurationService =
            new LoggingConfigurationService
            {
                LogDirectory = logDirectory
            };

        loggingConfigurationService.Configure(args);

        var globalLoggerFactory =
            loggingConfigurationService.LoggerFactory;

        ILoggerFactoryBaseUtilityService loggerFactoryService =
            new LoggerFactoryBaseUtilityService(
                globalLoggerFactory);

        ILogger logger =
            loggerFactoryService.Logger;

        logger.LogInformation(
            "Starting OmniAppium automation engine.");

        // ============================================================
        // 3. Configuration.
        // ============================================================

        IAiConfigService geminiConfigService =
            new AiConfigService
            {
                AiConfigPath = geminiConfigPath
            };

#if AUTO_EXECUTE_TASKS || GEMINI_READ_ONLY_SMOKE_TEST

        GeminiApiOptions geminiApiOptions =
            geminiConfigService.ReadData<GeminiApiOptions>();

        ArgumentNullException.ThrowIfNull(geminiApiOptions);

#endif

        ITransversalService transversalService =
            new DFSTransversalService();

        if (args.Contains("--issue55-smoke", StringComparer.Ordinal))
        {
            bool confirmed =
                args.Contains(
                    "--issue55-development-confirmed",
                    StringComparer.Ordinal)
                && !Console.IsInputRedirected
                && ConfirmIssue55Operation();

            using var successObserver =
                new Issue55ClickSuccessLogObserver();

            using var issue55LoggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddProvider(successObserver);
            });


            ILoggerFactoryBaseUtilityService issue55LoggerFactoryService =
                new LoggerFactoryBaseUtilityService(issue55LoggerFactory);
    
            Issue55SmokeResult result =
                await Issue55SmokeBootstrapper.RunIssue55SmokeAsync(
                    upstreamEndpoint: new Uri("http://127.0.0.1:4723/"),

                    createDriver: proxyEndpoint =>
                    {
                        var factory = new DriverFactory(
                            loggerFactoryService,
                            true)
                        {
                            DevelopmentDeviceConfig =
                                new ConfigBean<DevelopmentDeviceConfig>
                                {
                                    Path = developmentDeviceConfigPath
                                },
                            AppiumConfig =
                                new ConfigBean<AppiumConfig>
                                {
                                    Path = appiumConfigPath
                                },
                            AppConfig =
                                new ConfigBean<AppConfig>
                                {
                                    Path = appConfigPath
                                },
                            ConnectionConfig =
                                new ConfigBean<ConnectionConfig>
                                {
                                    Path = connectionConfigPath
                                },
                            GameConfig =
                                new ConfigBean<GameConfig>
                                {
                                    Path = gameConfigPath
                                },
                            TransversalService = transversalService
                        };

                        factory.Initialize();

                        factory.AppiumConfig.Data.AppiumServerUri =
                            proxyEndpoint.AbsoluteUri;

                        return factory.Create();
                    },

                    createTapAction: driver =>
                    {
                        var screenService = new AndroidScreenService
                        {
                            Driver = driver
                        };

                        var clickService = new ClickService(
                            issue55LoggerFactoryService,
                            toLogWhenSuccess: true)
                        {
                            ScreenService = screenService,

                            // Issue #55 tests absolute coordinates only.
                            // The scaler is required by the class but is not used by ClickAbsolute.
                            Scaler = new ResolutionScaler(1, 1, 1, 1)
                        };

                        return () => clickService.ClickAbsolute(1, 1);
                    },

                    successLogObserved: () => successObserver.SuccessLogObserved,

                    confirmed: confirmed,
                    proxyLogger: loggerFactoryService.LoggerFactory.CreateLogger<Issue55AppiumFaultProxy>()
                );

            logger.LogInformation(
                "Issue55 smoke: {Status}; {Reason}; " +
                "Actions={Actions}; Injected={Injected}; Forwarded={Forwarded}",
                result.Status,
                result.Reason,
                result.ActionRequests,
                result.InjectedFailures,
                result.ForwardedActionRequests);

            return result.Status switch
            {
                Issue55SmokeStatus.Passed => 0,
                Issue55SmokeStatus.Failed => 1,
                _ => 2
            };
        }
        
        var driverFactory =
            new DriverFactory(
                loggerFactoryService,
                true)
            {
                DevelopmentDeviceConfig =
                    new ConfigBean<DevelopmentDeviceConfig>
                    {
                        Path = developmentDeviceConfigPath
                    },

                AppiumConfig =
                    new ConfigBean<AppiumConfig>
                    {
                        Path = appiumConfigPath
                    },

                AppConfig =
                    new ConfigBean<AppConfig>
                    {
                        Path = appConfigPath
                    },

                ConnectionConfig =
                    new ConfigBean<ConnectionConfig>
                    {
                        Path = connectionConfigPath
                    },

                GameConfig =
                    new ConfigBean<GameConfig>
                    {
                        Path = gameConfigPath
                    },

                TransversalService = transversalService
            };

        driverFactory.Initialize();

        // ============================================================
        // 4. Load automation jobs only when required.
        // ============================================================

#if AUTO_EXECUTE_TASKS

var jobsConfig =
    new JobsConfig();

var jobsConfigService =
    new ConfigService<JobsConfig>(
        loggerFactoryService)
    {
        TransversalService = transversalService
    };

jobsConfigService.ValidateConfig(
    jobsConfigPath,
    ref jobsConfig);

ArgumentNullException.ThrowIfNull(jobsConfig);
ArgumentNullException.ThrowIfNull(jobsConfig.Jobs);

if (jobsConfig.Jobs.Count == 0)
{
    throw new InvalidOperationException(
        "No automation jobs were configured.");
}

logger.LogInformation(
    "Loaded {JobCount} automation jobs.",
    jobsConfig.Jobs.Count);

#endif

        // ============================================================
        // 5. Create the Appium driver.
        // ============================================================

        var driver =
            driverFactory.Create();

        ArgumentNullException.ThrowIfNull(driver);

        var driverControlService =
            new DriverControlService
            {
                Driver = driver
            };

        try
        {
            // ========================================================
            // 6. Runtime-owned Appium services.
            // ========================================================

            ScreenService screenService =
                new AndroidScreenService
                {
                    Driver = driver
                };

            var developmentDeviceConfig =
                driverFactory.DevelopmentDeviceConfig.Data;

            ArgumentNullException.ThrowIfNull(
                developmentDeviceConfig);

            ArgumentNullException.ThrowIfNull(
                developmentDeviceConfig.ScreenSize);

            var referenceScreenSize =
                developmentDeviceConfig.ScreenSize;

            var currentScreenSize =
                screenService.GetFreshScreenSize();

            logger.LogInformation(
                "Reference resolution: {Width}x{Height}",
                referenceScreenSize.Width,
                referenceScreenSize.Height);

            logger.LogInformation(
                "Current resolution: {Width}x{Height}",
                currentScreenSize.Width,
                currentScreenSize.Height);

            IResolutionScaler resolutionScaler =
                new ResolutionScaler(
                    referenceScreenSize.Width,
                    referenceScreenSize.Height,
                    currentScreenSize.Width,
                    currentScreenSize.Height);

            ClickService clickService =
                new ClickService(
                    loggerFactoryService,
                    true)
                {
                    ScreenService = screenService,
                    Scaler = resolutionScaler
                };

            WaitService waitService =
                new WaitService(
                    loggerFactoryService,
                    true)
                {
                    Driver = driver
                };

            using var screenshotService =
                new ScreenshotService(
                    loggerFactoryService,
                    true)
                {
                    Driver = driver
                };

            // ========================================================
            // 7. Dependency injection composition root.
            // ========================================================

            var services =
                new ServiceCollection();

            // --------------------------------------------------------
            // 7.1. External runtime dependencies.
            // --------------------------------------------------------

            // Reuse the application's existing logger factory in the same DI container.
            services.AddSingleton<ILoggerFactory>(globalLoggerFactory);
            services.AddLogging();

            services.AddSingleton<
                ILoggerFactoryBaseUtilityService>(
                loggerFactoryService);

            services.AddSingleton<IScreenshotService>(
                screenshotService);

            services.AddSingleton<IClickService>(
                clickService);

            services.AddSingleton<IWaitService>(
                waitService);

            // --------------------------------------------------------
            // 7.2. Production OCR infrastructure.
            // --------------------------------------------------------

            services.AddOcrUtilityServices();
            if (Environment.GetEnvironmentVariable("ISSUE59_PYTHON_OCR_ENABLED") == "1")
                services.UseIssue59PythonOcr(ReadIssue59PythonOcrOptions());

            services.AddAndroidOcrServices();

            // AddAndroidOcrServices already registers:
            // IFileSystem
            // IOcrDiagnosticImageWriter
            // IAndroidScreenOcrService
            // IOcrClickService
            // IOcrPageVerificationService
            // TimeProvider
            // IGameWorkflowStepExecutor

            // --------------------------------------------------------
            // 7.3. Production screen observation.
            // --------------------------------------------------------

            services.AddAndroidScreenObservation();

            // --------------------------------------------------------
            // 7.4. Production grounding.
            // --------------------------------------------------------

            services.AddSingleton<
                IOcrGroundedSnapshotFactory,
                OcrGroundedSnapshotFactory>();

            services.AddSingleton<
                IOcrGroundedSnapshotFreshnessGuard,
                OcrGroundedSnapshotFreshnessGuard>();

            // services.AddSingleton<
            //     IOcrGroundedClickService,
            //     OcrGroundedClickService>();

            services.AddSingleton<
                IOcrGroundedClickService,
                RejectingProductionGroundedClickService>();

            // --------------------------------------------------------
            // 7.5. Production planner.
            // --------------------------------------------------------

            services.AddSingleton<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>,
                AiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>();

            services.AddSingleton<
                IOcrGroundedPlannerActionExecutor,
                OcrGroundedPlannerActionExecutor>();


            services.AddSingleton<
                IOcrGroundedPlannerExecutionScopeFactory,
                OcrGroundedPlannerExecutionScopeFactory>();

            // Register the screen stability dependencies.
            services.AddSingleton<IScreenFrameComparer, ScreenFrameComparer>();

            services.AddSingleton<IAndroidScreenStabilityGuard,
                AndroidScreenStabilityGuard>();

            services.AddSingleton<
                IScreenFrameDifferenceAnalyzer,
                ScreenFrameDifferenceAnalyzer>();

            services.AddSingleton<
                IOcrTargetStabilityEvaluatorFactory,
                OcrTargetStabilityEvaluatorFactory>();


            services.AddSingleton<
                IOcrObservationLineNormalizer,
                OcrObservationLineNormalizer>();

            // Register the validated stability policy.
            services.AddSingleton(_ =>
            {
                var options = new ScreenStabilityOptions
                {
                    RequiredConsecutiveMatches = 2,
                    SampleInterval = TimeSpan.FromMilliseconds(250),
                    Timeout = TimeSpan.FromSeconds(30),
                    MaximumDifferenceRatio = 0.01
                };
                options.Validate();
                return options;
            });

            // Register a candidate ROI around the lower-left "任務" label.
            // The normalized bounds are based on the supplied game screenshot.
            // Validate actual OCR recognition and pixel differences before production use.

            var taskRegion = new NormalizedScreenRegion(
                X: 0.2222,
                Y: 0.7731,
                Width: 0.0556,
                Height: 0.1157);

            services.AddSingleton(_ =>
            {
                var options = new ScreenObservationStabilityOptions
                {
                    Region = taskRegion,
                    TargetText = "任務",
                    MaximumDifferenceRatio = 0.03,
                    MaximumCenterMovement = 0.02,
                    RequiredConsecutiveMatches = 2,
                    SampleInterval = TimeSpan.FromMilliseconds(250),
                    Timeout = TimeSpan.FromSeconds(args.Contains("--issue59-smoke", StringComparer.Ordinal) ? 180 : 45)
                };

                options.Validate();
                return options;
            });

            // Explicitly select the ROI + OCR constructor.
            // This registration overrides the earlier observation service registration.
            services.AddSingleton<IAndroidScreenObservationService>(provider =>
                new AndroidScreenObservationService(
                    provider.GetRequiredService<IScreenshotService>(),
                    provider.GetRequiredService<IAndroidScreenOcrService>(),
                    provider.GetRequiredService<IAndroidScreenStabilityGuard>(),
                    provider.GetRequiredService<ScreenStabilityOptions>(),
                    provider.GetRequiredService<IScreenFrameDifferenceAnalyzer>(),
                    provider.GetRequiredService<IOcrTargetStabilityEvaluatorFactory>(),
                    provider.GetRequiredService<IOcrObservationLineNormalizer>(),
                    provider.GetRequiredService<ScreenObservationStabilityOptions>())
                {
                    UseRegionalOcrForStability = args.Contains("--issue59-smoke", StringComparer.Ordinal)
                });

            // ========================================================
            // 8. Gemini infrastructure.
            // ========================================================

#if AUTO_EXECUTE_TASKS || GEMINI_READ_ONLY_SMOKE_TEST

            var aiExecutionSettings =
                new AiExecutionSettings
                {
                    LastTokenCountNeededToBeKept = 5,

                    MaxSteps = 20,

                    Threshold =
                        AiUtility.AiBaseUtilityServices
                            .Consts.Constants
                            .ExecutionSettings.MAX_THRESHOLD,

                    ToolExecutionTimeout =
                        TimeSpan.FromMinutes(2),

                    ForceSequentialToolExecution = true
                };

            var globalSemaphoreSlimModel =
                new SemaphoreSlimModel
                {
                    InitialCount = 2,
                    MaxCount = 2
                };

            var watchdogModel =
                new WatchdogModel
                {
                    Timeout = TimeSpan.FromMinutes(2)
                };

            var circuitBreakerModel =
                new CircuitBreakerModel
                {
                    ContinuousFailureCount = 0,
                    MaxAllowedFailureCount = 3,
                    CoolDown = TimeSpan.FromSeconds(30)
                };

            ISemaphoreSlimService semaphoreSlimService =
                new SemaphoreSlimService(
                    loggerFactoryService: loggerFactoryService,
                    globalSemaphoreSlimModel: globalSemaphoreSlimModel,
                    maxRequestsPerWindow: 2,
                    maxLimitRate: TimeSpan.FromSeconds(30),
                    watchdogModel: watchdogModel,
                    circuitBreakerModel: circuitBreakerModel,
                    needToStartWatchDog: false);

            ITypeUtilityService typeUtilityService =
                new TypeUtilityService();

            IJsonUtilityService jsonUtilityService =
                new JsonUtilityService(typeUtilityService);

            IEnumUtilityService enumUtilityService =
                new EnumUtilityService();

            IExpressionTreeUtilityService expressionTreeUtilityService =
                new ExpressionTreeUtilityService();

            IReflectionUtilityService reflectionUtilityService =
                new ReflectionUtilityService(
                    expressionTreeUtilityService);

            IGeminiToolRegistry geminiToolRegistry =
                new GeminiToolRegistry(
                    reflectionUtilityService);

            IAiParameterSchemaGenerator parameterSchemaGenerator =
                new GeminiSchemaGenerator(
                    jsonUtilityService,
                    typeUtilityService);

            IGeminiParameterPropertyMapper parameterPropertyMapper =
                new GeminiParameterPropertyMapper();

            var geminiToolConverter =
                new GeminiToolConverter(
                    jsonUtilityService,
                    enumUtilityService,
                    parameterSchemaGenerator,
                    parameterPropertyMapper);

            IGeminiToolService geminiToolService =
                new GeminiToolService(
                    geminiToolRegistry,
                    geminiToolConverter,
                    loggerFactoryService,
                    true);

            IAiConfigService aiConfigService =
                new AiConfigService
                {
                    AiConfigPath = geminiSecureConfigPath
                };

            string geminiApiKey =
                aiConfigService.GetApiKey();

            ArgumentException.ThrowIfNullOrWhiteSpace(
                geminiApiKey);

            using var httpClient =
                new HttpClient();

            IGeminiApiClient geminiApiClient =
                new GeminiApiClient(
                    loggerFactoryService,
                    true)
                {
                    HttpClient = httpClient,
                    ApiKey = geminiApiKey,
                    ApiOptions = geminiApiOptions
                };

            IGeminiConversationManager geminiConversationManager =
                new GeminiConversationManager(
                    loggerFactoryService,
                    geminiApiClient);

            // Deny unapproved methods even if they are registered later.
            const AiClickExecutionMode aiClickMode = AiClickExecutionMode.Protected;
            const bool isProductionAiRuntime = true;
            GeminiToolExecutionPolicy aiExecutionPolicy =
                GeminiClickExecutionPolicyFactory.Create(aiClickMode, isProductionAiRuntime);

            IGeminiToolExecutor geminiToolExecutor =
                new GeminiToolExecutor(
                    geminiToolRegistry,
                    typeUtilityService,
                    aiExecutionPolicy);

            IGeminiSessionManager geminiSessionManager =
                new GeminiSessionManager(
                    loggerFactoryService,
                    geminiConversationManager,
                    geminiToolService,
                    geminiToolExecutor,
                    semaphoreSlimService);

            IProgress<WorkflowProgress> workflowProgress =
                new Progress<WorkflowProgress>(
                    progress =>
                    {
                        logger.LogInformation(
                            "Gemini workflow progress: {Progress}",
                            progress);
                    });

            GeminiWorkflowClickScope geminiWorkflowClickScope = new();

            // Register real Appium tools only for automation mode.
            // The read-only smoke test should not expose mutating tools.

#if AUTO_EXECUTE_TASKS

// Read-only smoke tests must not register mutating AI tools.
bool isReadOnlySmokeTest =
    args.Contains("--issue59-smoke", StringComparer.Ordinal) || args.Contains("--issue61-export-review", StringComparer.Ordinal);

if (!isReadOnlySmokeTest)
{
    // Production is always Protected. Direct AI clicking is permitted only
    // when explicitly selected for a non-production development run.
    geminiToolRegistry.RegisterAiClickTools(
        aiClickMode,
        isProductionAiRuntime,
        () => clickService,
        geminiWorkflowClickScope.ResolveClickTool,
        isTrustedPlannerConfigured:
            !string.IsNullOrWhiteSpace(
                Environment.GetEnvironmentVariable("PLANNER_TRUSTED_REQUEST_JSON"))
            && !args.Contains("--issue50-smoke", StringComparer.Ordinal));

    geminiToolRegistry.Register<WaitService>(
        () => waitService);
}

#endif

            services.AddGeminiAutomation(
                aiExecutionSettings,
                geminiToolRegistry,
                geminiToolConverter,
                geminiSessionManager,
                workflowProgress);

#endif

            AddRegionOcrObservation(services);

            // ========================================================
            // Game-specific startup profile and coordination.
            // ========================================================

            services.AddSingleton<
                IGameStartupProfile,
                BeautyThreeKingdomsStartupProfile>();

            services.AddTransient<
                IGameStartupCoordinator,
                GameStartupCoordinator>();

            // Issue 54 smoke provisioning remains isolated from the planner.
            services.AddIssue54SmokeProvisioning(args);

            // Planner approvals must originate from an independent reviewer.
            // Never activate planner provisioning during Issue 50/54 smoke runs.
            string? plannerRequestPath =
                Environment.GetEnvironmentVariable("PLANNER_TRUSTED_REQUEST_JSON");

            if (!args.Contains("--issue50-smoke", StringComparer.Ordinal) &&
                !string.IsNullOrWhiteSpace(plannerRequestPath))
            {
                string claimDirectory = RequirePlannerEnvironment("PLANNER_CLAIM_DIRECTORY");
                string publicKeyPath = RequirePlannerEnvironment("PLANNER_TRUSTED_PUBLIC_KEY_PEM");
                string reviewerId = RequirePlannerEnvironment("PLANNER_REVIEWER_ID");
                string keyId = RequirePlannerEnvironment("PLANNER_KEY_ID");
                string fingerprint = RequirePlannerEnvironment("PLANNER_PUBLIC_KEY_SPKI_SHA256");

                IFileSystem plannerFileSystem = new FileSystem();
                string absoluteClaimDirectory =
                    plannerFileSystem.Path.GetFullPath(claimDirectory);
                string absoluteRequestPath =
                    plannerFileSystem.Path.GetFullPath(plannerRequestPath);

                if (!plannerFileSystem.Directory.Exists(absoluteClaimDirectory))
                    throw new InvalidOperationException(
                        "Planner approval claim directory does not exist.");

                if (!plannerFileSystem.File.Exists(absoluteRequestPath))
                    throw new InvalidOperationException(
                        "Planner trusted request JSON does not exist.");

                TrustedReviewerKey trustedKey = LoadPlannerTrustedReviewerKey(
                    plannerFileSystem, publicKeyPath, reviewerId, keyId, fingerprint);

                // Issue #61 destination verification for the trusted planner.
                string navigationTargetText = RequirePlannerEnvironment("PLANNER_NAVIGATION_TARGET_TEXT").Trim();
                string destinationText = RequirePlannerEnvironment("PLANNER_NAVIGATION_DESTINATION_TEXT").Trim();
                if (string.Equals(navigationTargetText, destinationText, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException("Destination text must differ from the navigation target label.");
                }

                services.AddScoped<global::OmniAppium.EngineUtilityServices.Services.Navigation.ITrustedNavigationDestinationVerifier>(sp =>
                    new global::OmniAppium.EngineUtilityServices.Services.Navigation.TrustedNavigationDestinationVerifier(
                        sp.GetRequiredService<IAndroidScreenObservationService>(),
                        sp.GetRequiredService<global::OmniAppium.EngineUtilityServices.Services.OCR.IOcrPageVerificationService>(),
                        (snapshot, targetId) => snapshot.Targets.Any(target =>
                            string.Equals(target.Id, targetId, StringComparison.Ordinal) &&
                            string.Equals(target.Text.Trim(), navigationTargetText, StringComparison.Ordinal))
                            ? new[] { destinationText }
                            : Array.Empty<string>(),
                        TimeSpan.FromSeconds(15)));

                services.AddTrustedPlannerClick(
                    [trustedKey],
                    absoluteClaimDirectory,
                    sp => new JsonPlannerTrustedClickRequestProvider(
                        sp.GetRequiredService<IFileSystem>(),
                        absoluteRequestPath));
            }

            // Legacy click routes remain blocked in production.
            services.AddProductionClickSecurity();

            // Issue #59: red-dot observation integration.
            services.AddIssue59RedDotExploration();

            // ========================================================
            // 9. Build the service provider exactly once.
            // ========================================================

            using ServiceProvider serviceProvider =
                services.BuildServiceProvider(
                    new ServiceProviderOptions
                    {
                        ValidateOnBuild = true,
                        ValidateScopes = true
                    });

            if (args.Contains("--issue50-smoke", StringComparer.Ordinal))
            {
                if (clickService is not ClickService realClickService)
                {
                    throw new InvalidOperationException(
                        "Issue 50 smoke test requires the concrete ClickService.");
                }

                var result = await Issue50RealDeviceSmokeTestRunner.RunAsync(
                    args,
                    realClickService,
                    serviceProvider,
                    logger,
                    CancellationToken.None);

                return result;
            }

            if (args.Contains("--issue61-export-review", StringComparer.Ordinal))
            {
                byte[] reviewImage = serviceProvider.GetRequiredService<IScreenshotService>().CaptureScreenshotBytes();
                if (reviewImage.Length == 0) throw new InvalidOperationException("Review screenshot is empty.");
                using var issue61Frame = OpenCvSharp.Cv2.ImDecode(reviewImage, OpenCvSharp.ImreadModes.Color);
                if (issue61Frame.Empty()) throw new InvalidOperationException("Cannot decode the review screenshot.");
                int issue61ReviewLeft = (int)Math.Floor(issue61Frame.Width * (1568.0 / 2340));
                int issue61ReviewTop = (int)Math.Floor(issue61Frame.Height * (126.0 / 1080));
                int issue61ReviewRight = (int)Math.Ceiling(issue61Frame.Width * (1710.0 / 2340));
                int issue61ReviewBottom = (int)Math.Ceiling(issue61Frame.Height * (200.0 / 1080));
                var issue61ReviewRegion = CoordinateUtilityServices.Rectangle.FromXYWH(issue61ReviewLeft, issue61ReviewTop, issue61ReviewRight - issue61ReviewLeft, issue61ReviewBottom - issue61ReviewTop);
                string issue61DiagnosticDirectory = Path.Combine(screenshotsDirectory, "Issue61Diagnostics", Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(issue61DiagnosticDirectory);
                await File.WriteAllBytesAsync(Path.Combine(issue61DiagnosticDirectory, "original.png"), reviewImage);
                var issue61CropBounds = new OpenCvSharp.Rect(issue61ReviewLeft, issue61ReviewTop, issue61ReviewRight - issue61ReviewLeft, issue61ReviewBottom - issue61ReviewTop);
                using var issue61Crop = new OpenCvSharp.Mat(issue61Frame, issue61CropBounds);
                if (!OpenCvSharp.Cv2.ImWrite(Path.Combine(issue61DiagnosticDirectory, "roi.png"), issue61Crop)) throw new IOException("Could not save Issue #61 ROI.");
                Console.WriteLine($"Issue #61 diagnostics: {issue61DiagnosticDirectory} | Image={issue61Frame.Width}x{issue61Frame.Height} | ROI={issue61CropBounds}");
                var reviewOcr = await serviceProvider.GetRequiredService<IAndroidScreenOcrService>().RecognizeRegionAsync(reviewImage, issue61ReviewRegion, CancellationToken.None);
                Console.WriteLine("Issue #61 regional OCR: " + string.Join(" | ", reviewOcr.Lines.Select(line => line.Text)));
                var snapshot = serviceProvider.GetRequiredService<IOcrGroundedSnapshotFactory>()
                    .Create(DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N"), reviewOcr);
                var targets = snapshot.Targets.Where(target =>
                    string.Equals(target.Text?.Trim(), "超值活動", StringComparison.Ordinal)).ToArray();
                if (targets.Length != 1)
                {
                    throw new InvalidOperationException($"Review export requires a screen capture and exactly one 超值活動 target. Matches: {targets.Length}.");
                }

                string directory = Path.Combine(screenshotsDirectory, "Issue61Review", snapshot.SnapshotId);
                Directory.CreateDirectory(directory);
                await File.WriteAllBytesAsync(Path.Combine(directory, "original.png"), reviewImage);
                await File.WriteAllTextAsync(Path.Combine(directory, "review-request.json"),
                    System.Text.Json.JsonSerializer.Serialize(new { Snapshot = snapshot, TargetId = targets[0].Id },
                        new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
                Console.WriteLine($"Issue #61 review evidence exported to {directory}");
                if (args.Contains("--issue61-verify-destination", StringComparer.Ordinal))
                {
                    Console.WriteLine("請手動點擊「超值活動」一次，等頁面顯示後按 Enter。");
                    if (Console.ReadLine() is null) return 2;
                    byte[] destinationImage = serviceProvider.GetRequiredService<IScreenshotService>().CaptureScreenshotBytes();
                    await File.WriteAllBytesAsync(Path.Combine(directory, "destination.png"), destinationImage);
                    using var destinationFrame = OpenCvSharp.Cv2.ImDecode(destinationImage, OpenCvSharp.ImreadModes.Color);
                    if (destinationFrame.Empty()) throw new InvalidOperationException("Destination screenshot could not be decoded.");
                    int left = (int)Math.Floor(destinationFrame.Width * (64.0 / 2048));
                    int top = (int)Math.Floor(destinationFrame.Height * (88.0 / 945));
                    int right = (int)Math.Ceiling(destinationFrame.Width * (290.0 / 2048));
                    int bottom = (int)Math.Ceiling(destinationFrame.Height * (193.0 / 945));
                    var destinationRegion = CoordinateUtilityServices.Rectangle.FromXYWH(left, top, right - left, bottom - top);
                    var destinationOcr = await serviceProvider.GetRequiredService<IAndroidScreenOcrService>().RecognizeRegionAsync(destinationImage, destinationRegion, CancellationToken.None);
                    Console.WriteLine("Issue #61 destination OCR: " + string.Join(" | ", destinationOcr.Lines.Select(line => line.Text)));
                    if (!destinationOcr.Lines.Any(line => string.Equals(line.Text?.Trim(), "每日首儲", StringComparison.Ordinal)))
                    {
                        Console.Error.WriteLine("Issue #61 FAIL: 每日首儲 was not verified. See destination.png in the export directory.");
                        return 2;
                    }
                    Console.WriteLine("Issue #61 PASS: 每日首儲 verified after manual navigation.");
                }
                return 0;
            }

            if (args.Contains("--issue59-smoke", StringComparer.Ordinal))
            {
                logger.LogInformation(
                    "Starting Issue 59 red-dot real-device smoke test.");

                if (args.Contains("--bottom-navigation", StringComparer.Ordinal))
                {
                    if (Environment.GetEnvironmentVariable("ISSUE59_PYTHON_OCR_ENABLED") != "1")
                    {
                        logger.LogError("Set ISSUE59_PYTHON_OCR_ENABLED=1 for Issue 59 bottom navigation.");
                        return 2;
                    }

                    string? readinessPath = Environment.GetEnvironmentVariable("ISSUE59_SCREEN_READINESS_JSON");
                    if (string.IsNullOrWhiteSpace(readinessPath))
                    {
                        logger.LogError("Set ISSUE59_SCREEN_READINESS_JSON to the readiness configuration file.");
                        return 2;
                    }

                    var (readiness, referenceImage) = await LoadScreenReadinessAsync(readinessPath);
                    using var reference = referenceImage;
                    using var template = new OpenCvSharp.Mat(reference, readiness.TemplateRegion.ToPixels(reference));

                    var screenshots = serviceProvider.GetRequiredService<IScreenshotService>();
                    return await RunIssue59BottomCaptureAsync(
                        () => screenshots.CaptureScreenshotBytes(),
                        screenshot => VerifyIssue59BottomNavigationAsync(serviceProvider, screenshot),
                        logger, screenshotsDirectory, TimeSpan.FromMilliseconds(readiness.PollMilliseconds),
                        screenshot => readiness.Reject(screenshot, template),
                        TimeSpan.FromSeconds(readiness.TimeoutSeconds));
                }

                Issue59RedDotSmokeTestRunner runner =
                    serviceProvider.GetRequiredService<Issue59RedDotSmokeTestRunner>();

                RedDotAssociationResult result =
                    await runner.RunAsync(Issue59OcrTargetFactory.Create);

                logger.LogInformation(
                    "Issue 59 observation: {ObservationId}",
                    result.ObservationId);

                logger.LogInformation(
                    "Issue 59 detected red dots: {Count}",
                    result.Evidence.Count);

                logger.LogInformation(
                    "Issue 59 associated nodes: {Count}",
                    result.Nodes.Count);

                foreach (RedDotEvidence evidence in result.Evidence)
                {
                    logger.LogInformation(
                        "Red-dot confidence={Confidence:F3}, " +
                        "ambiguous={IsAmbiguous}, " +
                        "candidateNodeIds={CandidateNodeIds}",
                        evidence.Confidence,
                        evidence.IsAmbiguous,
                        string.Join(",", evidence.CandidateNodeIds));
                }

                logger.LogInformation(
                    "Issue 59 red-dot smoke test completed.");

                return 0;
            }

            // ========================================================
            // 10. Resolve production services.
            // ========================================================

            IAndroidScreenObservationService observationService =
                serviceProvider.GetRequiredService<
                    IAndroidScreenObservationService>();

            IAndroidScreenOcrService androidScreenOcrService =
                serviceProvider.GetRequiredService<
                    IAndroidScreenOcrService>();

            IOcrGroundedPlannerExecutionScopeFactory plannerScopeFactory =
                serviceProvider.GetRequiredService<
                    IOcrGroundedPlannerExecutionScopeFactory>();

            // Do not eagerly resolve the legacy workflow executor here.
            // Its constructor requires IOcrClickService, which is
            // intentionally blocked by AddProductionClickSecurity().
            // Resolve IGameWorkflowStepExecutor only inside a workflow
            // entry point that explicitly supports its click policy.

            // Legacy startup requires IOcrClickService. Keep it out of
            // protected automatic execution and read-only smoke tests.
            #if !AUTO_EXECUTE_TASKS && !GEMINI_READ_ONLY_SMOKE_TEST && !ISSUE48_CLICK_SMOKE_TEST
            IGameStartupCoordinator startupCoordinator =
                serviceProvider.GetRequiredService<IGameStartupCoordinator>();
            #endif

            logger.LogInformation(
                "Production DI graph initialized successfully.");

#if ISSUE48_CLICK_SMOKE_TEST
            logger.LogInformation(
                "Issue48 Phase 1: startup coordination, Gemini, and automatic tasks are disabled.");

            var issue48Runner = new OmniAppiumDemo.SmokeTests.Issue48ClickSmokeTestRunner(
                serviceProvider.GetRequiredService<IAndroidScreenOcrService>(),
                serviceProvider.GetRequiredService<IOcrGroundedSnapshotFactory>(),
                serviceProvider.GetRequiredService<IGroundedClickAuthorizationService>());

            OcrGroundedSnapshot issue48Snapshot =
                await issue48Runner.VerifyReadOnlyAsync().ConfigureAwait(false);

            logger.LogInformation(
                "Issue48 Phase 1 PASS: SnapshotId={SnapshotId}; TargetCount={TargetCount}; " +
                "missing reviewed evidence rejected; intercepted ClickAttempts=0.",
                issue48Snapshot.SnapshotId,
                issue48Snapshot.Targets.Count);

            // Phase 2 always enters target review; no automatic click is permitted.
            logger.LogInformation(
                "Issue48 Phase 2: acquiring a fresh OCR snapshot for the task target.");

            OcrResult phase2Observation = await serviceProvider
                .GetRequiredService<IAndroidScreenOcrService>()
                .RecognizeCurrentScreenAsync()
                .ConfigureAwait(false);

            OcrGroundedSnapshot phase2Snapshot = serviceProvider
                .GetRequiredService<IOcrGroundedSnapshotFactory>()
                .Create(Guid.NewGuid().ToString("N"), phase2Observation);

            // Record the actual OCR output before selecting a target.
            foreach (OcrGroundedTarget candidate in phase2Snapshot.Targets)
            {
                logger.LogInformation(
                    "Issue48 Phase 2 OCR candidate: TargetId={TargetId}; Text={Text}; Bounds={Bounds}.",
                    candidate.Id,
                    candidate.Text,
                    candidate.Bounds);
            }

            // Match the exact target label after removing OCR whitespace.
            OcrGroundedTarget[] taskTargets = phase2Snapshot.Targets
                .Where(target =>
                    target.Text is not null &&
                    string.Equals(
                        string.Concat(
                            target.Text.Where(c => !char.IsWhiteSpace(c))),
                        "任務",
                        StringComparison.Ordinal) &&
                    target.Bounds.Width > 0 &&
                    target.Bounds.Height > 0)
                .ToArray();


            if (taskTargets.Length != 1)
            {
                logger.LogWarning(
                    "Issue48 Phase 2 INCONCLUSIVE: expected exactly one '任務' " +
                    "target, found {Count}. No click was issued.",
                    taskTargets.Length);
            }
            else
            {
                OcrGroundedTarget taskTarget = taskTargets[0];

                logger.LogInformation(
                    "Issue48 Phase 2 REVIEW: SnapshotId={SnapshotId}; " +
                    "TargetId={TargetId}; Text={Text}; Bounds={Bounds}.",
                    phase2Snapshot.SnapshotId,
                    taskTarget.Id,
                    taskTarget.Text,
                    taskTarget.Bounds);

                if (Console.IsInputRedirected)
                {
                    logger.LogWarning(
                        "Issue48 Phase 2 BLOCKED: interactive operator review is unavailable. " +
                        "No click was issued.");
                }
                else
                {
                    logger.LogInformation("Issue48 Phase 2 - Task Target Review");
                    logger.LogInformation("SnapshotId: {SnapshotId}", phase2Snapshot.SnapshotId);
                    logger.LogInformation("TargetId: {TargetId}", taskTarget.Id);
                    logger.LogInformation("Text: {Text}", taskTarget.Text);
                    logger.LogInformation("Bounds: {Bounds}", taskTarget.Bounds);
                    logger.LogInformation(
                        "Check the current device screen. Type REVIEW to confirm this target.");
                    Console.WriteLine("Confirmation: ");

                    string? confirmation = Console.ReadLine();
                    if (!string.Equals(confirmation, "REVIEW", StringComparison.Ordinal))
                    {
                        logger.LogWarning(
                            "Issue48 Phase 2 CANCELLED: operator did not confirm the target. " +
                            "No click was issued.");
                    }
                    else
                    {
                        // The trusted reviewer must be independently registered in DI.
                        // It must validate the exact screenshot, target identity, and bounds.
                        // Operator confirmation alone must never manufacture evidence.
                        var reviewedEvidenceSource = serviceProvider.GetService<
                            Func<OcrGroundedSnapshot, OcrGroundedTarget,
                                (ReviewedGroundTruth Evidence, float Confidence)?>>();

                        if (reviewedEvidenceSource is null)
                        {
                            logger.LogWarning(
                                "Issue48 Phase 2 BLOCKED: no independent ReviewedGroundTruth " +
                                "source is registered. No click was issued.");
                        }
                        else
                        {
                            var reviewed = reviewedEvidenceSource(
                                phase2Snapshot, taskTarget);

                            if (reviewed is null)
                            {
                                logger.LogWarning(
                                    "Issue48 Phase 2 BLOCKED: independent review rejected " +
                                    "the target or did not provide evidence. No click was issued.");
                            }
                            else
                            {
                                // The runner registers one-use evidence, forwards exactly
                                // one authorized real click, then rejects evidence replay.
                                issue48Runner.VerifyApprovedSingleClick(
                                    phase2Snapshot,
                                    taskTarget.Id,
                                    reviewed.Value.Evidence,
                                    reviewed.Value.Confidence,
                                    clickService,
                                    operatorConfirmedDeviceClick: true);

                                logger.LogInformation(
                                    "Issue48 Phase 2 PASS: one real click was attempted " +
                                    "for TargetId={TargetId}; evidence replay was rejected.",
                                    taskTarget.Id);
                            }
                        }
                    }
                }
            }

#elif !AUTO_EXECUTE_TASKS && !GEMINI_READ_ONLY_SMOKE_TEST

            logger.LogInformation(
                "Starting game startup coordination.");

            await startupCoordinator
                .EnsureMainScreenReadyAsync();

            logger.LogInformation(
                "Game startup coordination completed; main screen is ready.");
#else
    logger.LogInformation(
        "Skipping legacy game startup coordination for Gemini automation.");
#endif

            logger.LogInformation(
                "Starting real-device ROI + OCR stability verification.");

            IAndroidScreenObservation stableObservation =
                await RunRegionOcrObservationAsync(serviceProvider);
    
            logger.LogInformation(
                "Real-device ROI + OCR stability verification completed.");
    
            // ========================================================
            // 11. Real-device OCR checkpoint.
            // ========================================================

            OcrResult ocrResult =
                await androidScreenOcrService
                    .RecognizeCurrentScreenAsync();

#if IS_LOGGING

            logger.LogInformation(
                "OCR recognized text: {Text}",
                ocrResult.Text);

            foreach (OcrTextLine line in ocrResult.Lines)
            {
                logger.LogInformation(
                    "OCR line: {Text}; " +
                    "TopLeft=({Left}, {Top}); " +
                    "BottomRight=({Right}, {Bottom}); " +
                    "Center=({CenterX}, {CenterY})",
                    line.Text,
                    line.Bounds.TopLeft.X,
                    line.Bounds.TopLeft.Y,
                    line.Bounds.BottomRight.X,
                    line.Bounds.BottomRight.Y,
                    line.Bounds.Center.X,
                    line.Bounds.Center.Y);
            }

#endif

            // ========================================================
            // 12. Real-device OCR workflow.
            // ========================================================

#if !AUTO_EXECUTE_TASKS && !GEMINI_READ_ONLY_SMOKE_TEST

    GameWorkflowStep taskWorkflowStep =
        new(
            ClickText: "任務",

            VerificationTexts:
            [
                "日常",
                "週常",
                "成就"
            ],

            ClickTimeout: TimeSpan.FromSeconds(30),

            VerificationTimeout: TimeSpan.FromSeconds(10));

    logger.LogInformation(
        "Executing OCR workflow.");

    // await gameWorkflowStepExecutor.ExecuteAsync(
    //     taskWorkflowStep);

    logger.LogInformation(
        "OCR workflow completed.");

#endif

            // ========================================================
            // 13. Gemini read-only smoke test.
            // ========================================================

#if GEMINI_READ_ONLY_SMOKE_TEST

            IGeminiJobHandler smokeTestHandler =
                serviceProvider.GetRequiredService<
                    IGeminiJobHandler>();

            var smokeTestJob =
                new GeminiJob
                {
                    JobName =
                        "GeminiReadOnlyScreenObservationSmokeTest",

                    UserTask =
                        """
                Observe and describe the current Android screen.
                Do not click, tap, swipe, type, navigate,
                or invoke any tool.
                """,

                    Prompt =
                        """
                This is a read-only integration smoke test.

                Inspect the supplied Android screen image
                and briefly describe what is currently visible.

                Do not perform any action on the device.
                Do not invoke tools.

                Return only a short description of the
                visible screen.
                """
                };

            logger.LogInformation(
                "Starting Gemini read-only smoke test.");

            await smokeTestHandler.AutoExecuteAsync(
                smokeTestJob);

            logger.LogInformation(
                "Gemini read-only smoke test completed.");

#endif

            // ========================================================
            // 14. Configured automation jobs.
            // ========================================================

#if AUTO_EXECUTE_TASKS

    var handlers =
        new List<IJobHandler>
        {
            new WaitJobHandler(waitService),
            new ClickJobHandler(clickService),
            new ScreenshotJobHandler(screenshotService)
        };

    // The handler and registered click factory must share this workflow scope.
    using IServiceScope automationWorkflowScope = serviceProvider.CreateScope();
    using IDisposable automationClickScopeLease =
        geminiWorkflowClickScope.Enter(automationWorkflowScope);

    IGeminiJobHandler automationGeminiHandler =
        automationWorkflowScope.ServiceProvider.GetRequiredService<
            IGeminiJobHandler>();

    handlers.Add(automationGeminiHandler);

    IAutoTaskExecutionUtilityService executionService =
        new AutoTaskExecutionUtilityService(
            handlers);

    logger.LogInformation(
        "Executing {JobCount} configured jobs.",
        jobsConfig.Jobs.Count);

    await executionService.ExecuteSequenceAsync(
        jobsConfig.Jobs);

    logger.LogInformation(
        "Configured automation sequence completed.");

#endif
        }
        catch (Exception ex)
        {
            logger.LogCritical(
                ex,
                "OmniAppium automation sequence terminated unexpectedly.");

            throw;
        }
        finally
        {
            try
            {
                driverControlService.Dispose();

                logger.LogInformation(
                    "Android driver was disposed.");
            }
            catch (Exception disposeException)
            {
                logger.LogError(
                    disposeException,
                    "Failed to dispose the Android driver.");
            }
        }

#endif

        return 0;
    }
    /// <summary>
    /// Runs a region-based OCR observation using the already initialized service provider.
    /// </summary>
    /// <param name="services">The existing application service provider with an active Appium driver.</param>
    /// <param name="cancellationToken">The token used to cancel the observation.</param>
    /// <returns>The stable observation containing the matching screenshot and OCR result.</returns>
    public static async Task<IAndroidScreenObservation> RunRegionOcrObservationAsync(
        IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);
        cancellationToken.ThrowIfCancellationRequested();

        ScreenObservationStabilityOptions options =
            services.GetRequiredService<ScreenObservationStabilityOptions>();

        options.Validate();

        IScreenRegionOcrObservationFactory factory =
            services.GetRequiredService<IScreenRegionOcrObservationFactory>();

        ILogger<ScreenRegionOcrObservation> logger =
            services.GetRequiredService<ILogger<ScreenRegionOcrObservation>>();

        IAndroidScreenObservationService observation =
            new ScreenRegionOcrObservation(factory, options, logger);

        // The wrapper preserves cancellation and propagates timeouts and failures.
        return await observation.ObserveAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Registers region-based OCR observation dependencies in the existing service collection.
    /// </summary>
    /// <param name="services">The service collection configured by the existing application host.</param>
    /// <returns>The same service collection for further registration.</returns>
    public static IServiceCollection AddRegionOcrObservation(
        IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddTransient<
            IScreenFrameDifferenceAnalyzer,
            ScreenFrameDifferenceAnalyzer>();

        services.AddTransient<
            IOcrTargetStabilityEvaluatorFactory,
            OcrTargetStabilityEvaluatorFactory>();

        services.AddTransient<
            IOcrObservationLineNormalizer,
            OcrObservationLineNormalizer>();

        services.AddTransient<
            IScreenRegionOcrObservationFactory,
            ScreenRegionOcrObservationFactory>();

        // IScreenshotService, IAndroidScreenOcrService and
        // IAndroidScreenStabilityGuard must already be registered by the host.
        return services;
    }

    /// <summary>
    /// Reads a required planner security setting.
    /// </summary>
    private static string RequirePlannerEnvironment(string name)
    {
        string? value = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{name} is required.");
        return value;
    }

    /// <summary>
    /// Validates an externally pinned ECDSA P-256 reviewer public key.
    /// </summary>
    private static TrustedReviewerKey LoadPlannerTrustedReviewerKey(
        IFileSystem fileSystem,
        string publicKeyPath,
        string reviewerId,
        string keyId,
        string fingerprint)
    {
        ArgumentNullException.ThrowIfNull(fileSystem);
        string absolutePath = fileSystem.Path.GetFullPath(publicKeyPath);
        if (!fileSystem.File.Exists(absolutePath))
            throw new InvalidOperationException("Planner trusted public key file does not exist.");

        string pem = fileSystem.File.ReadAllText(absolutePath);
        using System.Security.Cryptography.ECDsa key =
            System.Security.Cryptography.ECDsa.Create();
        key.ImportFromPem(pem);
        if (key.KeySize != 256 ||
            key.ExportParameters(false).Curve.Oid.Value != "1.2.840.10045.3.1.7")
            throw new InvalidOperationException("Planner trusted key must be ECDSA P-256.");

        byte[] spki = key.ExportSubjectPublicKeyInfo();
        try
        {
            byte[] actual = System.Security.Cryptography.SHA256.HashData(spki);
            byte[] expected;
            try
            {
                expected = Convert.FromHexString(fingerprint);
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException(
                    "Planner public key fingerprint is invalid.", ex);
            }

            try
            {
                if (expected.Length != 32 ||
                    !System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(
                        actual, expected))
                    throw new InvalidOperationException(
                        "Planner public key fingerprint mismatch.");
            }
            finally
            {
                System.Security.Cryptography.CryptographicOperations.ZeroMemory(expected);
                System.Security.Cryptography.CryptographicOperations.ZeroMemory(actual);
            }
        }
        finally
        {
            System.Security.Cryptography.CryptographicOperations.ZeroMemory(spki);
        }

        return new TrustedReviewerKey(reviewerId, keyId, pem);
    }

    /// <summary>
    /// Requests explicit operator confirmation for the Issue 55 diagnostic.
    /// </summary>
    /// <returns>True when the exact confirmation phrase is entered.</returns>
    private static bool ConfirmIssue55Operation()
    {
        Console.Write(
            "Type ISSUE55-DISPATCH-FAILURE to continue: ");

        string? confirmation = Console.ReadLine();

        return string.Equals(
            confirmation,
            "ISSUE55-DISPATCH-FAILURE",
            StringComparison.Ordinal);
    }
}
