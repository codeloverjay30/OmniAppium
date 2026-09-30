using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.ConfigUtilityService.Services;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityService.Services.Wait;
using OmniAppium.EngineUtilityService.Utilities;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace OmniAppium.EngineUtilityService.Services
{
    /// <summary>
    /// Dispatches automation jobs to registered handlers.
    /// </summary>
    /// <param name="loggerFactoryService">The logging service.</param>
    /// <param name="handlers">The registered job handlers.</param>
    /// <param name="toLogWhenSuccess">Whether successful operations should be logged.</param>
    public partial class AppiumDriverService(
        ILoggerFactoryBaseUtilityService loggerFactoryService,
        IEnumerable<IJobHandler> handlers, // 所有註冊過的服務
        bool toLogWhenSuccess
    ): BaseUtility(loggerFactoryService,toLogWhenSuccess)
    {
        private readonly ILogger _logger = loggerFactoryService.Logger;
        /// <summary>
        /// Logs a completed job.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="OperationName">The job name.</param>
        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Information , Message = "Successfully do the job: {OperationName}")]
        static partial void LogSuccessToExecuteJob(ILogger logger , string OperationName);

        /// <summary>
        /// Logs a failed job.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="OperationName">The job name.</param>
        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Failed to do the job: {OperationName}")]
        static partial void LogFailureToExecuteJob(ILogger logger , string OperationName);

        /// <summary>
        /// Logs an invalid job argument.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="OperationName">The job name.</param>
        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Receive an invalid Argument: {OperationName}")]
        static partial void LogFailureForInvalidArgument(ILogger logger , string OperationName);

        public required IClickService ClickService { get; init; }
        public required IWaitService WaitService { get; init; }
        public required IScreenshotService ScreenshotService { get; init; }

        /// <summary>
        /// Executes the supplied jobs in sequence.
        /// </summary>
        /// <param name="steps">The jobs to execute.</param>
        /// <returns>A task representing the sequence execution.</returns>
        public async Task ExecuteJobsAsync(List<Job> steps)
        {
            try
            {
                foreach(var step in steps)
                {
                    await ExecuteStepAsync(step);
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        /// <summary>
        /// Executes a job through its matching handler and logs failures.
        /// </summary>
        /// <param name="step">The job to execute.</param>
        /// <returns>A task representing the job execution.</returns>
        public async Task ExecuteStepAsync(Job step)
        {
            try
            {
                var handler = handlers.FirstOrDefault(h => h.CanHandle(step));

                if(handler != null)
                {
                    await handler.AutoExecuteAsync(step);
                }
                else
                {
                    throw new ArgumentException($"No handler found for job type: {step.GetType().Name}");
                }
            }
            catch(ArgumentException ex)
            {
                LogFailureForInvalidArgument(_logger , step.JobName);
            }
            catch(Exception ex)
            {
                LogFailureToExecuteJob(_logger , step.JobName);
            }
        }
    }
}
