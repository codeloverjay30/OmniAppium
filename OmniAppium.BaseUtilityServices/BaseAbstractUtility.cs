using ExceptionHandlingUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;

namespace OmniAppium.BaseUtilityServices
{
    public abstract class BaseAbstractUtility(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess = true
    ): ExceptionHandler(loggerFactoryService,toLogWhenSuccess)
    {
        public ILoggerFactory ILoggerFactory => loggerFactoryService.LoggerFactory;
        public ILogger Logger => loggerFactoryService.Logger;
    }
}
