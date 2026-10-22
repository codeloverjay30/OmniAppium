#define DEVELOPING
#define IS_LOGGING
// #define GEMINI_READ_ONLY_SMOKE_TEST
#define ISSUE48_CLICK_SMOKE_TEST
// #define AUTO_EXECUTE_TASKS

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


namespace OmniAppiumDemo;

/// <summary>
/// Provides the ROI and OCR observation entry point for an existing Appium host.
/// </summary>
public static class Program
{
    /// <summary>
    /// Initializes the Appium demo and runs the selected smoke test or automation mode.
    /// </summary>
    public static async Task<int> Main(string[] args)
    {

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

            IClickService clickService =
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

            services.AddSingleton<OcrGroundedPlannerClickTool>();

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
                    Timeout = TimeSpan.FromSeconds(45)
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
                    provider.GetRequiredService<ScreenObservationStabilityOptions>()));

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

            OcrGroundedPlannerClickTool? aiPlannerClickTool = null;

            // Register real Appium tools only for automation mode.
            // The read-only smoke test should not expose mutating tools.

#if AUTO_EXECUTE_TASKS

    // Production is always Protected. Direct AI clicking is permitted only
    // when explicitly selected for a non-production development run.
    geminiToolRegistry.RegisterAiClickTools(
        aiClickMode,
        isProductionAiRuntime,
        () => clickService,
        () => aiPlannerClickTool ?? throw new InvalidOperationException("Planner tool is not initialized."));

    geminiToolRegistry.Register<WaitService>(
        () => waitService);

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

            // Apply the production click security policy last.
            services.AddProductionClickSecurity();

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

            // ========================================================
#if AUTO_EXECUTE_TASKS
            aiPlannerClickTool = serviceProvider.GetRequiredService<OcrGroundedPlannerClickTool>();
#endif

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

            IGameWorkflowStepExecutor gameWorkflowStepExecutor =
                serviceProvider.GetRequiredService<
                    IGameWorkflowStepExecutor>();

            IGameStartupCoordinator startupCoordinator =
                serviceProvider.GetRequiredService<IGameStartupCoordinator>();

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

#else
            logger.LogInformation(
                "Starting game startup coordination.");

            await startupCoordinator
                .EnsureMainScreenReadyAsync();

            logger.LogInformation(
                "Game startup coordination completed; main screen is ready.");
    
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

#if !GEMINI_READ_ONLY_SMOKE_TEST

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

    await gameWorkflowStepExecutor.ExecuteAsync(
        taskWorkflowStep);

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

    IGeminiJobHandler automationGeminiHandler =
        serviceProvider.GetRequiredService<
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
}