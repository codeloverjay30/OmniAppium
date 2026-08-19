using ExceptionHandlingUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.ConfigUtilityService.Services;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TransversalUtilityServices;

namespace OmniAppium.ConfigUtilityService.Factories
{
    public partial class DriverFactory(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess
    ) : BaseUtility(loggerFactoryService , toLogWhenSuccess)
    {
        private ILogger _logger => loggerFactoryService.Logger;

        [LoggerMessage(Level = LogLevel.Information , Message = "Successfully to {OperationName}")]
        static partial void LogSuccessForConfiguringDriver(ILogger logger,string OperationName);

        [LoggerMessage(Level = LogLevel.Error , Message = "Failed to {OperationName}")]
        static partial void LogFailureForConfiguringDriver(ILogger logger,string OperationName);

        public required ConfigBean<DevelopmentDeviceConfig> DevelopmentDeviceConfig { get; init; }
        public required ConfigBean<AppConfig> AppConfig { get; init; }
        public required ConfigBean<ConnectionConfig> ConnectionConfig { get; init; }
        public required ConfigBean<GameConfig> GameConfig { get; init; }
        public required ConfigBean<AppiumConfig> AppiumConfig { get; init; }
        public required ITransversalService TransversalService { get; init; }
        public ILoggerFactoryBaseUtilityService LoggerFactoryService { get => loggerFactoryService; }
        public AppiumOptions Options { get; private set; }

        public void Initialize()
        {
            this.EnsureAndSetConfig();
            this.InitializeOptions();
        }


        public void EnsureAndSetConfig()
        {
            var success = this.SafeExecute<bool>(() =>
            {
                // 呼叫統一的泛型驗證邏輯
                ValidateAndUpdateConfig(DevelopmentDeviceConfig);
                ValidateAndUpdateConfig(AppiumConfig);
                ValidateAndUpdateConfig(AppConfig);
                ValidateAndUpdateConfig(ConnectionConfig);
                ValidateAndUpdateConfig(GameConfig);
                return true;
            } , operationName: "讀取和設定組態檔的組態",false);

            if(!success)
            {
                LogFailureForConfiguringDriver(_logger, "讀取和設定組態檔的組態");
                throw new Exception("讀取組態設定失敗");
            }

            LogSuccessForConfiguringDriver(_logger, "讀取和設定組態檔的組態");
        }

        public void InitializeOptions()
        {
            var success = this.SafeExecute<bool>(() =>
            {
                var appConfigData = this.AppConfig.Data;
                var gameConfigData = this.GameConfig.Data;
                var appiumConfigData = this.AppiumConfig.Data;

                var options = new AppiumOptions();
                options.PlatformName = appiumConfigData.PlatformName;
                options.AutomationName = "UiAutomator2"; // 新版建議明確指定
                options.DeviceName = appiumConfigData.DeviceName;
                options.AddAdditionalAppiumOption("appium:udid" , appiumConfigData.DeviceUdid);

                // --- 關鍵修改：使用 Dictionary 包裝 Settings ---
                var settings = new Dictionary<string , object>
                {
                    // 降低品質是加速截圖最有效的方法
                    { "screenshotQuality", 50 }, 
                    // 忽略無效視圖，減少 UI 樹掃描時間
                    { "ignoreUnimportantViews", true },
                    // 遊戲環境必備：不要等待畫面完全靜止才截圖
                    { "waitForIdleTimeout", 0 }
                };
                options.AddAdditionalAppiumOption("appium:settings" , settings);
                // --------------------------------------------

                options.AddAdditionalAppiumOption("appium:appPackage" , appConfigData.PackageName);
                options.AddAdditionalAppiumOption("appium:appActivity" , appConfigData.MainActivity);

                options.AddAdditionalAppiumOption("appium:noReset" , appiumConfigData.NoReset);
                options.AddAdditionalAppiumOption("appium:ignoreHiddenApiPolicyError" , appiumConfigData.IgnoreHiddenApiPolicyError);
                options.AddAdditionalAppiumOption("appium:ensureWebviewsHavePages" , appiumConfigData.EnsureWebviewsHavePages);
                options.AddAdditionalAppiumOption("appium:noAnalyzer" , appiumConfigData.NoAnalyzer);
                options.AddAdditionalAppiumOption("appium:skipDeviceInitialization" , appiumConfigData.SkipDeviceInitialization);
                options.AddAdditionalAppiumOption("appium:skipDeviceInitialization" , appiumConfigData.SkipDeviceInitialization);
                options.AddAdditionalAppiumOption("appium:skipDeviceInitialization" , appiumConfigData.SkipDeviceInitialization);
                options.AddAdditionalAppiumOption("appium:appWaitActivity" , appConfigData.MainActivity);
                options.AddAdditionalAppiumOption("appium:appWaitDuration" , 20000); // 等待 20 秒

                this.Options = options;
                return true;
            } , operationName: "設定AppiumOptions的組態" , false);

            if(!success)
            {
                LogFailureForConfiguringDriver(_logger, "設定AppiumOptions的組態");
                throw new Exception("設定AppiumOptions的組態失敗");
            }

            LogSuccessForConfiguringDriver(_logger, "設定AppiumOptions的組態");
        }

        public AndroidDriver Create()
        {
            var driver = this.SafeExecute<AndroidDriver?>(() =>
            {
                var appConfigData = this.AppConfig.Data;
                var gameConfigData = this.GameConfig.Data;
                var appiumConfigData = this.AppiumConfig.Data;

                var uri = new Uri(appiumConfigData.AppiumServerUri);
                var driver = new AndroidDriver(uri , Options , TimeSpan.FromMinutes(3));
                driver.ActivateApp(appConfigData.PackageName);
                return driver;
            } , operationName: "建立一個AndroidDriver實體" , null);

            if(driver == null)
            {
                LogFailureForConfiguringDriver(_logger , "建立一個AndroidDriver實體");
                throw new InvalidOperationException("建立一個AndroidDriver實體");
            }
            LogSuccessForConfiguringDriver(_logger, "建立一個AndroidDriver實體");
            return driver;
        }

        private void ValidateAndUpdateConfig<T>(ConfigBean<T> configBean) where T : class
        {
            var service = new ConfigService<T>(LoggerFactoryService)
            {
                TransversalService = TransversalService
            };

            var data = configBean.Data;
            service.ValidateConfig(configBean.Path , ref data);
            configBean.Data = data;
        }
    }
}
