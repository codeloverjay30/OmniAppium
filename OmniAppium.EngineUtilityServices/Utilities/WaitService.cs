using AiUtility.GeminiUtilityServices.DataAnnotations;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.BaseUtilityService;
using OpenQA.Selenium.Appium.Android;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public partial class WaitService(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess
    ):
        BaseUtility(
            loggerFactoryService ,
            toLogWhenSuccess
        ), IWaitService
    {
        private ILogger _logger => loggerFactoryService.Logger;

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Information , Message = "Successfully wait {milliseconds}ms")]
        static partial void LogSuccessForWaiting(ILogger logger , double milliseconds);

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Failed to wait {milliseconds}ms")]
        static partial void LogFailureForWaiting(ILogger logger , double milliseconds);

        public required AndroidDriver Driver { get; init; }

        [GeminiTool(Description = "等待")]
        public void Wait(TimeSpan timeout)
        {
            Wait(timeout.Milliseconds);
        }

        [GeminiTool(Description = "等待")]
        public void Wait(int milliseconds)
        {
            try
            {
                Thread.Sleep(milliseconds);
                LogSuccessForWaiting(_logger , milliseconds);
            }
            catch
            {
                LogFailureForWaiting(_logger , milliseconds);
            }
        }
    }
}
