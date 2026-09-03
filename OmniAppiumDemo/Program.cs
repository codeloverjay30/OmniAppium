#define DEVELOPING
#undef SIMPLE_TESTING

#define IS_LOGGING
#define AUTO_EXECUTE_TASKS

// using AiUtility.GeminiUtilityServices.Services;
using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.AiBaseUtilityServices.Services;
using AiUtility.Configurations;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Executor;
using AiUtility.GeminiKits.Registry;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Extensions;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using CommonModels;
using CoordinateUtilityServices;
using EnumUtilityServices;
using ExpressionTreeUtilityServices;
using JsonUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Controllers;
using OmniAppium.ConfigUtilityService.Factories;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityService.Services;
using OmniAppium.EngineUtilityService.Utilities;
using OmniAppium.LogServices;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Interactions;
using ReflectionUtilityServices;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
using System.Reflection;
using System.Text.Json;
using ThreadLevelLockingUtilityServices;
using ThreadLevelLockingUtilityServices.Models;
using TransversalUtilityServices;
using TypeUtilityServices;
using static OmniAppium.Options.JsonOptions;


JsonSerializerOptions _options = new JsonSerializerOptions
{
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping ,
    WriteIndented = true
};

#if DEVELOPING
string appDir = AppDomain.CurrentDomain.BaseDirectory;
var logDirectory = Path.Combine(appDir , "Logs");

var loggingConfigurationService = new LoggingConfigurationService { LogDirectory = logDirectory };
loggingConfigurationService.Configure(args);
var globalLoggerFactory = loggingConfigurationService.LoggerFactory;

var developmentDeviceConfigPath = Path.Combine(appDir , "development-device.config.json5");
var appConfigPath = Path.Combine(appDir , "app.config.json5");
var appiumConfigPath = Path.Combine(appDir , "appium.config.json5");
var connectionConfigPath = Path.Combine(appDir , "connection.config.json5");
var gameConfigPath = Path.Combine(appDir , "game.config.json5");
var tasksConfigPath = Path.Combine(appDir , "tasks.config.json5");
var geminiSecureConfigPath = Path.Combine(appDir , "secure.config.json5");

var screenshotsDirectory = Path.Combine(appDir , "Screenshots");
if(!Directory.Exists(screenshotsDirectory))
{
    Directory.CreateDirectory(screenshotsDirectory);
}
else
{
    Directory.Delete(screenshotsDirectory, true);
    Directory.CreateDirectory(screenshotsDirectory);
}

var screenshot1_Path = Path.Combine(screenshotsDirectory , "screenshot1.png");

ILoggerFactoryBaseUtilityService loggerFactoryService = new LoggerFactoryBaseUtilityService(globalLoggerFactory);

loggerFactoryService.Logger.LogInformation("Hello World");

ITransversalService transversalService = new DFSTransversalService();

var driverFactory = new DriverFactory(loggerFactoryService , true)
{
    DevelopmentDeviceConfig = new ConfigBean<DevelopmentDeviceConfig>() { Path =  developmentDeviceConfigPath },
    AppiumConfig = new ConfigBean<AppiumConfig>() { Path = appiumConfigPath } ,
    AppConfig = new ConfigBean<AppConfig>() { Path = appConfigPath } ,
    ConnectionConfig = new ConfigBean<ConnectionConfig>() { Path = connectionConfigPath } ,
    GameConfig = new ConfigBean<GameConfig>() { Path = gameConfigPath } ,
    TransversalService = transversalService ,
};

driverFactory.Initialize();

#if IS_LOGGING

loggerFactoryService.Logger.LogInformation(JsonSerializer.Serialize(driverFactory.DevelopmentDeviceConfig.Data));
loggerFactoryService.Logger.LogInformation(new String('-' , 50));
loggerFactoryService.Logger.LogInformation(JsonSerializer.Serialize(driverFactory.AppiumConfig.Data));
loggerFactoryService.Logger.LogInformation(new String('-' , 50));
loggerFactoryService.Logger.LogInformation(JsonSerializer.Serialize(driverFactory.AppConfig.Data));
loggerFactoryService.Logger.LogInformation(new String('-' , 50));
loggerFactoryService.Logger.LogInformation(JsonSerializer.Serialize(driverFactory.ConnectionConfig.Data));
loggerFactoryService.Logger.LogInformation(new String('-' , 50));
loggerFactoryService.Logger.LogInformation(JsonSerializer.Serialize(driverFactory.GameConfig.Data));
loggerFactoryService.Logger.LogInformation(new String('-' , 50));
#endif

var driver = driverFactory.Create();

#if IS_LOGGING

ArgumentNullException.ThrowIfNull(driver , nameof(driver));

var driverControlService = new DriverControlService { Driver = driver };

ScreenService screenService = new AndroidScreenService { Driver = driver };

var developmentDeviceConfig = driverFactory.DevelopmentDeviceConfig.Data;
var developmentDeviceScreenSize = developmentDeviceConfig.ScreenSize;

#if AUTO_EXECUTE_TASKS

var aiConfigPath = @"D:\workspace\utility_packages\experiments\Scrawler\Phone\AndroidApp\GamesScrawler\OmniAppium.Engine\OmniAppiumDemo\secure.config.json5";

var targettDeviceScreenSize = screenService.GetFreshScreenSize();
IResolutionScaler resolutionScaler = new ResolutionScaler(
    developmentDeviceScreenSize.Width ,
    developmentDeviceScreenSize.Height ,
    targettDeviceScreenSize.Width ,
    targettDeviceScreenSize.Height
);

#region ai execution settings
AiExecutionSettings aiExecutionSettings = new AiExecutionSettings
{
    LastTokenCountNeededToBeKept = 5,
    MaxSteps = 10,
    Threshold = AiUtility.AiBaseUtilityServices.Consts.Constants.ExecutionSettings.MAX_THRESHOLD,
    ToolExecutionTimeout = TimeSpan.FromSeconds(30),
    ForceSequentialToolExecution = false,
};
#endregion

#region `String` to `ReadOnlyMemory<char>`
IStringFormmattingUtilityService stringFormmattingUtilityService = new StringFormmattingUtilityService();
#endregion

#region Schema of Request for Gemini AI Studio
GeminiGenerateRequest request = new GeminiGenerateRequest(stringFormmattingUtilityService);
request.AddUserMessage(
    text: "Task 1".AsMemory()
);

#endregion

#region POCO about status
StatusJsonModels statusJsonModels = new StatusJsonModels();
#endregion

#region Service about SemaphoreSlim 
int maxRequestsPerWindow = 2;
TimeSpan maxLimitRate = TimeSpan.FromSeconds(30);

SemaphoreSlimModel globalSemaphoreSlimModel = new SemaphoreSlimModel
{
    InitialCount = 5,
    MaxCount = 5,
};

WatchdogModel watchdogModel = new WatchdogModel
{
    Timeout = TimeSpan.FromSeconds(30),
};

CircuitBreakerModel circuitBreakerModel = new CircuitBreakerModel
{
    ContinuousFailureCount = 0,
    MaxAllowedFailureCount = 3, 
    CoolDown = TimeSpan.FromSeconds(30),
};

ISemaphoreSlimService semaphoreSlimService = new SemaphoreSlimService(
    loggerFactoryService: loggerFactoryService,
    globalSemaphoreSlimModel: globalSemaphoreSlimModel,
    maxRequestsPerWindow : maxRequestsPerWindow,
    maxLimitRate: maxLimitRate,
    watchdogModel: watchdogModel,
    circuitBreakerModel: circuitBreakerModel,
    needToStartWatchDog: false
);

# endregion

ITypeUtilityService typeUtilityService = new TypeUtilityService();
IJsonUtilityService jsonUtilityService = new JsonUtilityService(typeUtilityService);
IEnumUtilityService enumUtilityService = new EnumUtilityService();

IExpressionTreeUtilityService expressionTreeUtilityService = new ExpressionTreeUtilityService();
IReflectionUtilityService reflectionUtilityService = new ReflectionUtilityService(expressionTreeUtilityService);

IGeminiToolRegistry geminiToolRegistry = new GeminiToolRegistry(reflectionUtilityService);

GeminiToolConverter geminiToolConverter = new GeminiToolConverter(
    jsonUtilityService,
    enumUtilityService
);
 
IGeminiToolService geminiToolService = new GeminiToolService(
    geminiToolRegistry, 
    geminiToolConverter,
    loggerFactoryService , 
    true
)
{
    ConfigPath = aiConfigPath
};

AiBaseUtilityService aiBaseUtilityService = new AiBaseUtilityService(
    loggerFactoryService, 
    true
)
{
    ConfigPath = aiConfigPath    
};

ApiKeyConfig apiKeyConfig = aiBaseUtilityService.GetApiKeyConfig();

// 確保有正確地讀取到組態
ArgumentNullException.ThrowIfNull(apiKeyConfig);
ArgumentNullException.ThrowIfNullOrWhiteSpace(apiKeyConfig.API_KEY);

HttpClient httpClient = new HttpClient();
IGeminiApiClient geminiApiClient = new GeminiApiClient(
    loggerFactoryService,
    true
)
{
    HttpClient = httpClient,
    ApiKey = apiKeyConfig.API_KEY,
    ConfigPath = aiConfigPath,
};

IGeminiConversationManager geminiConversationManager = new GeminiConversationManager(
    loggerFactoryService,
    geminiApiClient
);


IGeminiToolExecutor geminiToolExecutor = new GeminiToolExecutor(
    geminiToolRegistry,
    typeUtilityService
);

IGeminiSessionManager geminiSessionManager = new GeminiSessionManager(
    loggerFactoryService,
    geminiConversationManager,
    geminiToolService,
    geminiToolExecutor,
    semaphoreSlimService
);

#if EXECUTES_TASK
statusJsonModels = await geminiSessionManager.ExecuteWithToolSupportAsync<WorkflowProgress>(
    request: request,
    userTask:"自動登入",
    settings:aiExecutionSettings
);
#endif

#if IS_LOGGING
loggerFactoryService.Logger.LogInformation($"statusJsonModels.IsAllSuccess:{statusJsonModels.IsAllSuccess}");
loggerFactoryService.Logger.LogInformation($"statusJsonModels.IsAllFailure:{statusJsonModels.IsAllFailure}");
loggerFactoryService.Logger.LogInformation($"statusJsonModels.HasNoneStatus:{statusJsonModels.HasNoneStatus}");
loggerFactoryService.Logger.LogInformation($"statusJsonModels:{JsonSerializer.Serialize<StatusJsonModels>(statusJsonModels,DefaultOptions)}");
#endif

IClickService clickService = new ClickService(loggerFactoryService , true) { ScreenService = screenService , Scaler = resolutionScaler };
IWaitService waitService = new WaitService(loggerFactoryService , true) { Driver = driver };
IScreenshotService screenshotService = new ScreenshotService(loggerFactoryService , true) { Driver = driver };

#if IS_LOGGING
loggerFactoryService.Logger.LogInformation("Wait 10s to load the app");
#endif

waitService.Wait(10000);

// clickService.TapAt(1957 , 172);
clickService.TapAt(1167 , 740);

screenshotService.TakeAndSaveScreenshot(screenshot1_Path);
#endif

#if IS_LOGGING
loggerFactoryService.Logger.LogInformation("The driver has clicked by finger");
#endif

driverControlService.Dispose();

#if IS_LOGGING
loggerFactoryService.Logger.LogInformation("The driver has been disposed");
#endif

#endif

#endif

Console.ReadKey();




