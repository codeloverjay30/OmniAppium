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
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Workflows;
using OmniAppium.LogServices;
using OmniAppiumDemo.DependencyInjection;
using ReflectionUtilityServices;
using ThreadLevelLockingUtilityServices;
using ThreadLevelLockingUtilityServices.Models;
using TransversalUtilityServices;
using TypeUtilityServices;

#if DEVELOPING

var appDir =
    AppDomain.CurrentDomain.BaseDirectory;

var developmentDeviceConfigPath =
    Path.Combine(
        appDir,
        "development-device.config.json5");

var appConfigPath =
    Path.Combine(
        appDir,
        "app.config.json5");

var appiumConfigPath =
    Path.Combine(
        appDir,
        "appium.config.json5");

var connectionConfigPath =
    Path.Combine(
        appDir,
        "connection.config.json5");

var gameConfigPath =
    Path.Combine(
        appDir,
        "game.config.json5");

var jobsConfigPath =
    Path.Combine(
        appDir,
        "jobs.config.json5");

var geminiSecureConfigPath =
    Path.Combine(
        appDir,
        "secure.config.json5");

var geminiConfigPath =
    Path.Combine(
        appDir,
        "gemini.config.json5");

var logDirectory =
    Path.Combine(
        appDir,
        "Logs");

var screenshotsDirectory =
    Path.Combine(
        appDir,
        "Screenshots");

Directory.CreateDirectory(
    logDirectory);

Directory.CreateDirectory(
    screenshotsDirectory);

var loggingConfigurationService =
    new LoggingConfigurationService
    {
        LogDirectory = logDirectory
    };

loggingConfigurationService.Configure(
    args);

var globalLoggerFactory =
    loggingConfigurationService.LoggerFactory;

ILoggerFactoryBaseUtilityService loggerFactoryService =
    new LoggerFactoryBaseUtilityService(
        globalLoggerFactory);

ILogger logger =
    loggerFactoryService.Logger;

logger.LogInformation(
    "Starting OmniAppium automation engine.");

IAiConfigService geminiConfigService =
    new AiConfigService
    {
        AiConfigPath =
            geminiConfigPath
    };

GeminiApiOptions geminiApiOptions =
    geminiConfigService
        .ReadData<GeminiApiOptions>();

Type ocrServiceType =
    typeof(
        OCRUtilityServices.Services.OCRUtilityService);

logger.LogInformation(
    "OCRUtilityServices runtime assembly: {Assembly}; Location: {Location}",
    ocrServiceType.Assembly.FullName,
    ocrServiceType.Assembly.Location);

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
                Path =
                    developmentDeviceConfigPath
            },

        AppiumConfig =
            new ConfigBean<AppiumConfig>
            {
                Path =
                    appiumConfigPath
            },

        AppConfig =
            new ConfigBean<AppConfig>
            {
                Path =
                    appConfigPath
            },

        ConnectionConfig =
            new ConfigBean<ConnectionConfig>
            {
                Path =
                    connectionConfigPath
            },

        GameConfig =
            new ConfigBean<GameConfig>
            {
                Path =
                    gameConfigPath
            },

        TransversalService =
            transversalService
    };

driverFactory.Initialize();

var jobsConfig =
    new JobsConfig();

var jobsConfigService =
    new ConfigService<JobsConfig>(
        loggerFactoryService)
    {
        TransversalService =
            transversalService
    };

jobsConfigService.ValidateConfig(
    jobsConfigPath,
    ref jobsConfig);

ArgumentNullException.ThrowIfNull(
    jobsConfig);

ArgumentNullException.ThrowIfNull(
    jobsConfig.Jobs);

if (jobsConfig.Jobs.Count == 0)
{
    throw new InvalidOperationException(
        "No automation jobs were configured.");
}

#if IS_LOGGING

logger.LogInformation(
    "Loaded {JobCount} automation jobs.",
    jobsConfig.Jobs.Count);

foreach (var job in jobsConfig.Jobs)
{
    logger.LogInformation(
        "Configured job: {JobType}, Name: {JobName}",
        job.GetType().Name,
        job.JobName);
}

#endif

var driver =
    driverFactory.Create();

ArgumentNullException.ThrowIfNull(
    driver);

var driverControlService =
    new DriverControlService
    {
        Driver = driver
    };

try
{
    /*
     * Runtime-owned Appium services remain in the composition root.
     *
     * The DI container consumes these instances but does not create
     * the active Appium driver.
     */

    ScreenService screenService =
        new AndroidScreenService
        {
            Driver = driver
        };

    var developmentDeviceConfig =
        driverFactory
            .DevelopmentDeviceConfig
            .Data;

    ArgumentNullException.ThrowIfNull(
        developmentDeviceConfig);

    ArgumentNullException.ThrowIfNull(
        developmentDeviceConfig.ScreenSize);

    var referenceScreenSize =
        developmentDeviceConfig.ScreenSize;

    var currentScreenSize =
        screenService.GetFreshScreenSize();

#if IS_LOGGING

    logger.LogInformation(
        "Reference resolution: {Width}x{Height}",
        referenceScreenSize.Width,
        referenceScreenSize.Height);

    logger.LogInformation(
        "Current resolution: {Width}x{Height}",
        currentScreenSize.Width,
        currentScreenSize.Height);

#endif

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
            ScreenService =
                screenService,

            Scaler =
                resolutionScaler
        };

    WaitService waitService =
        new WaitService(
            loggerFactoryService,
            true)
        {
            Driver =
                driver
        };

    var screenshotService =
        new ScreenshotService(
            loggerFactoryService,
            true)
        {
            Driver =
                driver
        };

// Existing Android/Appium runtime initialization remains above this point.
// driver
// screenshotService
// clickService
// waitService
// loggerFactoryService
// etc.

var services =
    new ServiceCollection();

/*
 * Runtime-owned Appium services.
 */

services.AddSingleton<ILoggerFactoryBaseUtilityService>(
    loggerFactoryService);

services.AddSingleton<IScreenshotService>(
    screenshotService);

services.AddSingleton<IClickService>(clickService);

services.AddSingleton<IWaitService>(
    waitService);

/*
 * OCR and Android screen observation infrastructure.
 */

services.AddOcrUtilityServices();
services.AddAndroidOcrServices();
services.AddAndroidScreenObservation();

#if AUTO_EXECUTE_TASKS || GEMINI_READ_ONLY_SMOKE_TEST

/*
 * Gemini infrastructure.
 */

var aiExecutionSettings =
    new AiExecutionSettings
    {
        LastTokenCountNeededToBeKept =
            5,

        MaxSteps =
            20,

        Threshold =
            AiUtility.AiBaseUtilityServices
                .Consts.Constants
                .ExecutionSettings
                .MAX_THRESHOLD,

        ToolExecutionTimeout =
            TimeSpan.FromMinutes(2),

        ForceSequentialToolExecution =
            true
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
        Timeout =
            TimeSpan.FromMinutes(2)
    };

var circuitBreakerModel =
    new CircuitBreakerModel
    {
        ContinuousFailureCount =
            0,

        MaxAllowedFailureCount =
            3,

        CoolDown =
            TimeSpan.FromSeconds(30)
    };

ISemaphoreSlimService semaphoreSlimService =
    new SemaphoreSlimService(
        loggerFactoryService:
            loggerFactoryService,

        globalSemaphoreSlimModel:
            globalSemaphoreSlimModel,

        maxRequestsPerWindow:
            2,

        maxLimitRate:
            TimeSpan.FromSeconds(30),

        watchdogModel:
            watchdogModel,

        circuitBreakerModel:
            circuitBreakerModel,

        needToStartWatchDog:
            false);

ITypeUtilityService typeUtilityService =
    new TypeUtilityService();

IJsonUtilityService jsonUtilityService =
    new JsonUtilityService(
        typeUtilityService);

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
        AiConfigPath =
            geminiSecureConfigPath
    };

string geminiApiKey =
    aiConfigService.GetApiKey();

ArgumentException.ThrowIfNullOrWhiteSpace(
    geminiApiKey);

var httpClient =
    new HttpClient();

IGeminiApiClient geminiApiClient =
    new GeminiApiClient(
        loggerFactoryService,
        true)
    {
        HttpClient =
            httpClient,

        ApiKey =
            geminiApiKey,

        ApiOptions =
            geminiApiOptions
    };

IGeminiConversationManager geminiConversationManager =
    new GeminiConversationManager(
        loggerFactoryService,
        geminiApiClient);

IGeminiToolExecutor geminiToolExecutor =
    new GeminiToolExecutor(
        geminiToolRegistry,
        typeUtilityService);

/*
 * Register actual Appium services as Gemini tools.
 */

geminiToolRegistry.Register<ClickService>(
    () => clickService);

geminiToolRegistry.Register<WaitService>(
    () => waitService);

// ------------------------------------------------------------
// Keep the rest of your EXISTING Gemini infrastructure here.
//
// For example:
// - GeminiToolService
// - GeminiApiClient
// - GeminiConversationManager
// - GeminiToolExecutor
// - GeminiSessionManager
//
// Do not replace their existing constructors merely to match
// this example.
// ------------------------------------------------------------

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
#if IS_LOGGING

            logger.LogInformation(
                "Gemini workflow progress: {Progress}",
                progress);

#endif
        });

// Register the already-created Gemini runtime dependencies
// and GeminiJobHandler.
services.AddGeminiAutomation(
    aiExecutionSettings,
    geminiToolRegistry,
    geminiToolConverter,
    geminiSessionManager,
    workflowProgress);

#endif

    services.AddSingleton<
        IAndroidScreenOcrService,
        AndroidScreenOcrService>();

    services.AddSingleton<
        IGameWorkflowStepExecutor,
        GameWorkflowStepExecutor>();
    
    // IMPORTANT:
    // Nothing required by GeminiJobHandler may be registered after this.
    using ServiceProvider serviceProvider =
        services.BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });


#if AUTO_EXECUTE_TASKS

    /*
     * Gemini infrastructure.
     */

    var aiExecutionSettings =
        new AiExecutionSettings
        {
            LastTokenCountNeededToBeKept =
                5,

            MaxSteps =
                20,

            Threshold =
                AiUtility.AiBaseUtilityServices
                    .Consts.Constants
                    .ExecutionSettings
                    .MAX_THRESHOLD,

            ToolExecutionTimeout =
                TimeSpan.FromMinutes(2),

            ForceSequentialToolExecution =
                true
        };

    services.AddSingleton(
        aiExecutionSettings);

    var globalSemaphoreSlimModel =
        new SemaphoreSlimModel
        {
            InitialCount = 2,
            MaxCount = 2
        };

    var watchdogModel =
        new WatchdogModel
        {
            Timeout =
                TimeSpan.FromMinutes(2)
        };

    var circuitBreakerModel =
        new CircuitBreakerModel
        {
            ContinuousFailureCount =
                0,

            MaxAllowedFailureCount =
                3,

            CoolDown =
                TimeSpan.FromSeconds(30)
        };

    ISemaphoreSlimService semaphoreSlimService =
        new SemaphoreSlimService(
            loggerFactoryService:
                loggerFactoryService,

            globalSemaphoreSlimModel:
                globalSemaphoreSlimModel,

            maxRequestsPerWindow:
                2,

            maxLimitRate:
                TimeSpan.FromSeconds(30),

            watchdogModel:
                watchdogModel,

            circuitBreakerModel:
                circuitBreakerModel,

            needToStartWatchDog:
                false);

    services.AddSingleton(
        semaphoreSlimService);

    services.AddSingleton<
        ITypeUtilityService,
        TypeUtilityService>();

    services.AddSingleton<
        IJsonUtilityService>(
            serviceProvider =>
                new JsonUtilityService(
                    serviceProvider
                        .GetRequiredService<
                            ITypeUtilityService>()));

    services.AddSingleton<
        IEnumUtilityService,
        EnumUtilityService>();

    services.AddSingleton<
        IExpressionTreeUtilityService,
        ExpressionTreeUtilityService>();

    services.AddSingleton<
        IReflectionUtilityService>(
            serviceProvider =>
                new ReflectionUtilityService(
                    serviceProvider
                        .GetRequiredService<
                            IExpressionTreeUtilityService>()));

    services.AddSingleton<
        IGeminiToolRegistry>(
            serviceProvider =>
                new GeminiToolRegistry(
                    serviceProvider
                        .GetRequiredService<
                            IReflectionUtilityService>()));

    services.AddSingleton<
        IAiParameterSchemaGenerator>(
            serviceProvider =>
                new GeminiSchemaGenerator(
                    serviceProvider
                        .GetRequiredService<
                            IJsonUtilityService>(),

                    serviceProvider
                        .GetRequiredService<
                            ITypeUtilityService>()));

    services.AddSingleton<
        IGeminiParameterPropertyMapper,
        GeminiParameterPropertyMapper>();

    services.AddSingleton<
        GeminiToolConverter>(
            serviceProvider =>
                new GeminiToolConverter(
                    serviceProvider
                        .GetRequiredService<
                            IJsonUtilityService>(),

                    serviceProvider
                        .GetRequiredService<
                            IEnumUtilityService>(),

                    serviceProvider
                        .GetRequiredService<
                            IAiParameterSchemaGenerator>(),

                    serviceProvider
                        .GetRequiredService<
                            IGeminiParameterPropertyMapper>()));

    services.AddSingleton<
        IGeminiToolService>(
            serviceProvider =>
                new GeminiToolService(
                    serviceProvider
                        .GetRequiredService<
                            IGeminiToolRegistry>(),

                    serviceProvider
                        .GetRequiredService<
                            GeminiToolConverter>(),

                    loggerFactoryService,
                    true));

    IAiConfigService aiConfigService =
        new AiConfigService
        {
            AiConfigPath =
                geminiSecureConfigPath
        };

    string geminiApiKey =
        aiConfigService.GetApiKey();

    ArgumentException.ThrowIfNullOrWhiteSpace(
        geminiApiKey);

    var httpClient =
        new HttpClient();

    services.AddSingleton(
        httpClient);

    services.AddSingleton<
        IGeminiApiClient>(
            _ =>
                new GeminiApiClient(
                    loggerFactoryService,
                    true)
                {
                    HttpClient =
                        httpClient,

                    ApiKey =
                        geminiApiKey,

                    ApiOptions =
                        geminiApiOptions
                });

    services.AddSingleton<
        IGeminiConversationManager>(
            serviceProvider =>
                new GeminiConversationManager(
                    loggerFactoryService,

                    serviceProvider
                        .GetRequiredService<
                            IGeminiApiClient>()));

    services.AddSingleton<
        IGeminiToolExecutor>(
            serviceProvider =>
                new GeminiToolExecutor(
                    serviceProvider
                        .GetRequiredService<
                            IGeminiToolRegistry>(),

                    serviceProvider
                        .GetRequiredService<
                            ITypeUtilityService>()));

    services.AddSingleton<
        IGeminiSessionManager>(
            serviceProvider =>
                new GeminiSessionManager(
                    loggerFactoryService,

                    serviceProvider
                        .GetRequiredService<
                            IGeminiConversationManager>(),

                    serviceProvider
                        .GetRequiredService<
                            IGeminiToolService>(),

                    serviceProvider
                        .GetRequiredService<
                            IGeminiToolExecutor>(),

                    serviceProvider
                        .GetRequiredService<
                            ISemaphoreSlimService>()));

    services.AddSingleton<
        IProgress<WorkflowProgress>>(
            new Progress<WorkflowProgress>(
                progress =>
                {
#if IS_LOGGING

                    logger.LogInformation(
                        "AI workflow progress: {Progress}",
                        progress);

#endif
                }));

    /*
     * Slice 3:
     *
     * GeminiJobHandler consumes IAndroidScreenObservationService.
     * It no longer owns screenshot acquisition directly.
     */

    services.AddSingleton<
        IGeminiJobHandler,
        GeminiJobHandler<WorkflowProgress>>();

#endif

#if AUTO_EXECUTE_TASKS

    /*
     * Gemini tool registration requires the shared registry instance
     * created by the container.
     */

    IGeminiToolRegistry geminiToolRegistry =
        serviceProvider
            .GetRequiredService<
                IGeminiToolRegistry>();

    geminiToolRegistry.Register<ClickService>(
        () => clickService);

    geminiToolRegistry.Register<WaitService>(
        () => waitService);

#endif

    /*
     * Real-device checkpoint:
     *
     * Verify screenshot -> OCR using the exact service instance used
     * by the application graph.
     */

    IAndroidScreenOcrService androidScreenOcrService =
        serviceProvider
            .GetRequiredService<
                IAndroidScreenOcrService>();

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

    services.AddSingleton<
        IOcrClickService,
        OcrClickService>();

    services.AddSingleton<
        IOcrPageVerificationService,
        OcrPageVerificationService>();
    

    services.AddSingleton<
        IGameWorkflowStepExecutor,
        GameWorkflowStepExecutor>();
    
    /*
     * Real-device OCR workflow.
     */

    IGameWorkflowStepExecutor gameWorkflowStepExecutor =
        serviceProvider
            .GetRequiredService<
                IGameWorkflowStepExecutor>();

    GameWorkflowStep taskWorkflowStep =
        new(
            ClickText:
                "任務",

            VerificationTexts:
            [
                "日常",
                "週常",
                "成就"
            ],

            ClickTimeout:
                TimeSpan.FromSeconds(30),

            VerificationTimeout:
                TimeSpan.FromSeconds(10));

#if IS_LOGGING

    logger.LogInformation(
        "Executing OCR workflow: Click {ClickText} and verify [{VerificationTexts}]",
        taskWorkflowStep.ClickText,
        string.Join(
            ", ",
            taskWorkflowStep.VerificationTexts));

#endif

    await gameWorkflowStepExecutor
        .ExecuteAsync(
            taskWorkflowStep);

#if IS_LOGGING

    logger.LogInformation(
        "OCR workflow completed successfully.");

#endif

// ↓↓↓ 這整段都是要新增的 ↓↓↓

#if GEMINI_READ_ONLY_SMOKE_TEST

IGeminiJobHandler geminiJobHandler =
    serviceProvider.GetRequiredService<IGeminiJobHandler>();

var smokeTestJob =
    new GeminiJob
    {
        JobName = "GeminiReadOnlyScreenObservationSmokeTest",
        UserTask =
            """
            Observe and describe the current Android screen.
            Do not click, tap, swipe, type, navigate, or invoke any tool.
            """,
        Prompt =
            """
            This is a read-only integration smoke test.

            Inspect the supplied Android screen image and briefly describe
            what is currently visible.

            Do not perform any action on the device.
            Do not invoke tools.

            Return only a short description of the visible screen.
            """
    };

await geminiJobHandler.AutoExecuteAsync(
    smokeTestJob);

#endif

    // ↑↑↑ 新增到這裡 ↑↑↑

    /*
     * Job handlers that wrap runtime-owned Appium services.
     */

    var handlers =
        new List<IJobHandler>
        {
            new WaitJobHandler(
                waitService),

            new ClickJobHandler(
                clickService),

            new ScreenshotJobHandler(
                screenshotService)
        };

#if AUTO_EXECUTE_TASKS

    IGeminiJobHandler geminiJobHandler =
        serviceProvider
            .GetRequiredService<
                IGeminiJobHandler>();

    handlers.Add(
        geminiJobHandler);

    IAutoTaskExecutionUtilityService executionService =
        new AutoTaskExecutionUtilityService(
            handlers);

#if IS_LOGGING

    logger.LogInformation(
        "DIAGNOSTIC: Before ExecuteSequenceAsync. JobCount={JobCount}",
        jobsConfig.Jobs.Count);

    logger.LogInformation(
        "Starting configured automation sequence.");

#endif

    await executionService
        .ExecuteSequenceAsync(
            jobsConfig.Jobs);

#if IS_LOGGING

    logger.LogInformation(
        "DIAGNOSTIC: After ExecuteSequenceAsync.");

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

#if IS_LOGGING

        logger.LogInformation(
            "Android driver was disposed.");

#endif
    }
    catch (Exception disposeException)
    {
        logger.LogError(
            disposeException,
            "Failed to dispose the Android driver.");
    }
}

#endif