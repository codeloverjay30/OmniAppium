using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.ConfigUtilityService.Services;
using OmniAppium.EngineUtilityService.Utilities;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace OmniAppium.EngineUtilityService.Services
{
    public partial class AppiumDriverService(
        ILoggerFactoryBaseUtilityService loggerFactoryService,
        IEnumerable<IJobHandler> handlers, // 所有註冊過的服務
        bool toLogWhenSuccess
    ): BaseUtility(loggerFactoryService,toLogWhenSuccess)
    {
        private readonly ILogger _logger = loggerFactoryService.Logger;
        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Information , Message = "Successfully do the job: {OperationName}")]
        static partial void LogSuccessToExecuteJob(ILogger logger , string OperationName);

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Failed to do the job: {OperationName}")]
        static partial void LogFailureToExecuteJob(ILogger logger , string OperationName);

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Receive an invalid Argument: {OperationName}")]
        static partial void LogFailureForInvalidArgument(ILogger logger , string OperationName);

        public required IClickService ClickService { get; init; }
        public required IWaitService WaitService { get; init; }
        public required IScreenshotService ScreenshotService { get; init; }

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
