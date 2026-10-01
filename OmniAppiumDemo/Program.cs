#define DEVELOPING
#define IS_LOGGING
// #define AUTO_EXECUTE_TASKS

using System.Drawing;
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
using Microsoft.Extensions.Logging;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
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
using OmniAppium.LogServices;
using ReflectionUtilityServices;
using ThreadLevelLockingUtilityServices;
using ThreadLevelLockingUtilityServices.Models;
using TransversalUtilityServices;
using TypeUtilityServices;

#if DEVELOPING

var appDir = AppDomain.CurrentDomain.BaseDirectory;

var developmentDeviceConfigPath =
    Path.Combine(appDir, "development-device.config.json5");

var appConfigPath =
    Path.Combine(appDir, "app.config.json5");

var appiumConfigPath =
    Path.Combine(appDir, "appium.config.json5");

var connectionConfigPath =
    Path.Combine(appDir, "connection.config.json5");

var gameConfigPath =
    Path.Combine(appDir, "game.config.json5");

var jobsConfigPath =
    Path.Combine(appDir, "jobs.config.json5");

var geminiSecureConfigPath =
    Path.Combine(appDir, "secure.config.json5");

string geminiConfigPath = Path.Combine(
    appDir ,
    "gemini.config.json5");

var logDirectory =
    Path.Combine(appDir, "Logs");

var screenshotsDirectory =
    Path.Combine(appDir, "Screenshots");

Directory.CreateDirectory(logDirectory);
Directory.CreateDirectory(screenshotsDirectory);

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

IAiConfigService geminiConfigService =
    new AiConfigService
    {
        AiConfigPath = geminiConfigPath,
    };
    
GeminiApiOptions geminiApiOptions =
    geminiConfigService.ReadData<GeminiApiOptions>();

var logger =
    loggerFactoryService.Logger;

logger.LogInformation(
    "Starting OmniAppium automation engine.");

// Temporary diagnostic:
// Verify the OCRUtilityServices assembly loaded by this process.
Type ocrServiceType =
    typeof(OCRUtilityServices.Services.OCRUtilityService);

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

ArgumentNullException.ThrowIfNull(driver);

var driverControlService =
    new DriverControlService
    {
        Driver = driver
    };

try
{
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

    var currentScreenSize1 =
        screenService.GetFreshScreenSize();

#if IS_LOGGING

    logger.LogInformation(
        "Reference resolution: {Width}x{Height}",
        referenceScreenSize.Width,
        referenceScreenSize.Height);

    logger.LogInformation(
        "Current resolution: {Width}x{Height}",
        currentScreenSize1.Width,
        currentScreenSize1.Height);

#endif

    IResolutionScaler resolutionScaler =
        new ResolutionScaler(
            referenceScreenSize.Width,
            referenceScreenSize.Height,
            currentScreenSize1.Width,
            currentScreenSize1.Height);

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

IOCRUtilityService ocrUtilityService =
    new OCRUtilityService();

    IOcrTextMatcher ocrTextMatcher =
        new OcrTextMatcher();
    
IAndroidScreenOcrService screenOcrService =
    new AndroidScreenOcrService(
        screenshotService,
        ocrUtilityService);


    OcrResult ocrResult =
    await screenOcrService.RecognizeCurrentScreenAsync();

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

const string targetText = "任務";
// const string targetText = "VIP 9";

    try
    {
    
        IOcrClickService ocrClickService =
            new OcrClickService(
                screenOcrService,
                ocrTextMatcher,
                clickService);

        IOcrPageVerificationService pageVerificationService =
            new OcrPageVerificationService(
                screenOcrService,
                ocrTextMatcher,
                TimeProvider.System);
        
        const string clickTarget = "任務";

        TimeSpan clickTimeout = TimeSpan.FromSeconds(30);
        TimeSpan verificationTimeout = TimeSpan.FromSeconds(10);

        string[] expectedPageMarkers =
        [
            "日常",
            "週常",
            "成就"
        ];

        try
        {
            await ocrClickService.ClickTextAsync(
                clickTarget,
                clickTimeout);

            logger.LogInformation(
                "OCR target clicked successfully. ClickTarget: {ClickTarget}",
                clickTarget);

            logger.LogInformation(
                "Waiting for OCR page verification. ExpectedPageMarkers: {ExpectedPageMarkers}, Timeout: {Timeout}",
                string.Join(", ", expectedPageMarkers),
                verificationTimeout);

            await pageVerificationService.WaitForAllTextAsync(
                expectedPageMarkers,
                verificationTimeout);

            logger.LogInformation(
                "OCR page verification succeeded. ExpectedPageMarkers: {ExpectedPageMarkers}",
                string.Join(", ", expectedPageMarkers));
        }
        catch (TimeoutException ex)
        {
            logger.LogError(
                ex,
                "OCR page verification timed out. ClickTarget: {ClickTarget}, ExpectedPageMarkers: {ExpectedPageMarkers}",
                clickTarget,
                string.Join(", ", expectedPageMarkers));

            throw;
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(
                ex,
                "OCR navigation was canceled. ClickTarget: {ClickTarget}, ExpectedPageMarkers: {ExpectedPageMarkers}",
                clickTarget,
                string.Join(", ", expectedPageMarkers));

            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "OCR navigation failed. ClickTarget: {ClickTarget}, ExpectedPageMarkers: {ExpectedPageMarkers}",
                clickTarget,
                string.Join(", ", expectedPageMarkers));

            throw;
        }
    
    
}
catch (InvalidOperationException ex)
{
    logger.LogWarning(
        ex,
        "OCR target '{TargetText}' was not uniquely found.",
        targetText);
}

#if AUTO_EXECUTE_TASKS

    var aiExecutionSettings =
        new AiExecutionSettings
        {
            LastTokenCountNeededToBeKept = 5,
            MaxSteps = 20,

            Threshold =
                AiUtility.AiBaseUtilityServices
                    .Consts.Constants
                    .ExecutionSettings
                    .MAX_THRESHOLD,

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
            Timeout =
                TimeSpan.FromMinutes(2)
        };

    var circuitBreakerModel =
        new CircuitBreakerModel
        {
            ContinuousFailureCount = 0,
            MaxAllowedFailureCount = 3,

            CoolDown =
                TimeSpan.FromSeconds(30)
        };

    ISemaphoreSlimService semaphoreSlimService =
        new SemaphoreSlimService(
            loggerFactoryService:
                loggerFactoryService,

            globalSemaphoreSlimModel:
                globalSemaphoreSlimModel,

            maxRequestsPerWindow: 2,

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
    

    // Register the actual Appium services as Gemini tools.
    geminiToolRegistry.Register<ClickService>(
    () => clickService);

    geminiToolRegistry.Register<WaitService>(
        () => waitService);
    
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

    using var httpClient =
        new HttpClient();

    IGeminiApiClient geminiApiClient =
        new GeminiApiClient(
            loggerFactoryService,
            true)
        {
            HttpClient = httpClient,

            ApiKey =
                geminiApiKey,
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

    /*
     * Your current GeminiJobHandler constructor still requires:
     *
     * IToolRegistry<ToolMetadataBase, GeminiToolAttribute>
     * IToolDispatcher<ToolMetadataBase, GeminiToolAttribute>
     *
     * while the existing Gemini infrastructure exposes
     * IGeminiToolRegistry / IGeminiToolDispatcher.
     *
     * This type-contract mismatch should be corrected in GeminiJobHandler
     * before creating it here.
     */

#endif

    var handlers =
        new List<IJobHandler>
        {
            new OmniAppium.EngineUtilityService.Utilities.WaitJobHandler(
                waitService),

            new OmniAppium.EngineUtilityService.Utilities.ClickJobHandler(
                clickService),

            new OmniAppium.EngineUtilityService.Utilities.ScreenshotJobHandler(
                screenshotService)
        };

#if AUTO_EXECUTE_TASKS

    /*
     * Add GeminiJobHandler<WorkflowProgress> here after
     * its constructor types have been aligned with the existing
     * Gemini abstractions.
     *
     * Example target shape:
     *
     * IProgress<WorkflowProgress> progress =
     *     new Progress<WorkflowProgress>();
     *
     * var geminiJobHandler =
     *     new GeminiJobHandler<WorkflowProgress>(
     *         aiExecutionSettings,
     *         geminiToolRegistry,
     *         geminiToolDispatcher,
     *         geminiToolConverter,
     *         geminiSessionManager,
     *         screenshotService,
     *         progress);
     *
     * handlers.Add(geminiJobHandler);
     */

IProgress<WorkflowProgress> workflowProgress =
    new Progress<WorkflowProgress>(
        progress =>
        {
#if IS_LOGGING

            logger.LogInformation(
                "AI workflow progress: {Progress}",
                progress);

#endif
        });


IGeminiJobHandler geminiJobHandler =
    new GeminiJobHandler<WorkflowProgress>(
        aiExecutionSettings:
            aiExecutionSettings,

        registry:
            geminiToolRegistry,

        converter:
            geminiToolConverter,

        sessionManager:
            geminiSessionManager,

        screenshotService:
            screenshotService,

        progressBar:
            workflowProgress);

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

await executionService.ExecuteSequenceAsync(
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
