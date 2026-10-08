
#define DEVELOPING
#define IS_LOGGING
#define GEMINI_READ_ONLY_SMOKE_TEST
// #define AUTO_EXECUTE_TASKS

using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.AiBaseUtilityServices.Services;
using AiUtility.Configurations;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Executor;
using AiUtility.GeminiKits.Mappers;
using AiUtility.GeminiKits.Registry;
using AiUtility.GeminiKits.Services;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OCRUtilityServices.Models;
using OmniAppium.ConfigUtilityService.Controllers;
using OmniAppium.ConfigUtilityService.Factories;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.ConfigUtilityService.Services;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityService.Services.Screen;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityService.Services.Wait;
using OmniAppium.EngineUtilityService.Utilities;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppium.EngineUtilityServices.Workflows;
using OmniAppium.LogServices;
using OmniAppiumDemo.DependencyInjection;
using ReflectionUtilityServices;
using System.IO.Abstractions;
using ThreadLevelLockingUtilityServices;
using ThreadLevelLockingUtilityServices.Models;
using TransversalUtilityServices;
using TypeUtilityServices;

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

    services.AddSingleton<
        IOcrGroundedClickService,
        OcrGroundedClickService>();

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

    IGeminiToolExecutor geminiToolExecutor =
        new GeminiToolExecutor(
            geminiToolRegistry,
            typeUtilityService);

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

    // Register real Appium tools only for automation mode.
    // The read-only smoke test should not expose mutating tools.

#if AUTO_EXECUTE_TASKS

    geminiToolRegistry.Register<ClickService>(
        () => clickService);

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

    IGameWorkflowStepExecutor gameWorkflowStepExecutor =
        serviceProvider.GetRequiredService<
            IGameWorkflowStepExecutor>();

    logger.LogInformation(
        "Production DI graph initialized successfully.");

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
