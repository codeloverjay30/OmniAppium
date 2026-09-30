using AiUtility.GeminiKits.Attributes;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.BaseUtilityService;
using OpenQA.Selenium.Appium.Android;

namespace OmniAppium.EngineUtilityService.Services.Wait
{
    /// <summary>
    /// Provides blocking waits for automation jobs.
    /// </summary>
    /// <param name="loggerFactoryService">The logging service.</param>
    /// <param name="toLogWhenSuccess">Whether successful operations should be logged.</param>
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

        /// <summary>
        /// Logs a completed wait.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="milliseconds">The wait duration in milliseconds.</param>
        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Information , Message = "Successfully wait {milliseconds}ms")]
        static partial void LogSuccessForWaiting(ILogger logger , double milliseconds);

        /// <summary>
        /// Logs a failed wait.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="milliseconds">The requested duration in milliseconds.</param>
        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Failed to wait {milliseconds}ms")]
        static partial void LogFailureForWaiting(ILogger logger , double milliseconds);

        public required AndroidDriver Driver { get; init; }

        /// <summary>
        /// Waits for the millisecond component of the specified duration.
        /// </summary>
        /// <param name="timeout">The duration whose millisecond component is used.</param>
        [GeminiTool(Description = "等待")]
        public void Wait(TimeSpan timeout)
        {
            Wait(timeout.Milliseconds);
        }

        /// <summary>
        /// Blocks the current thread for the specified duration.
        /// </summary>
        /// <param name="milliseconds">The wait duration in milliseconds.</param>
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
