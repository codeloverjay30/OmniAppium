using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace OmniAppium.BaseUtilityService
{
    public class BaseUtility(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess = true
    ): BaseAbstractUtility(loggerFactoryService, toLogWhenSuccess)
    {
        // 將 SerializerOptions 設為靜態，避免重複分配記憶體
        protected static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping ,
            WriteIndented = true
        };
    }
}
