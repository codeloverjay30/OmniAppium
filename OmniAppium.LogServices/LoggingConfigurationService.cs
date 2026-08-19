using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogHelperServices;
using System.IO;

namespace OmniAppium.LogServices
{
    public class LoggingConfigurationService : IDisposable
    {
        private IHost _host;

        public ILoggerFactory LoggerFactory { get; private set; }

        public required string LogDirectory { get; init; }
        public void Configure(string [ ] args)
        {

            // 1. 建立 Serilog 配置
            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.Console();
            loggerConfiguration.SetSerilogConfig();
            loggerConfiguration.SetLogFileName(LogDirectory);

            var logger = loggerConfiguration.CreateLogger();

            // 2. 整合進 Generic Host
            _host = Host.CreateDefaultBuilder(args)
                .UseSerilog(logger) // 自動管理 Serilog 生命週期
                .Build();

            // 3. 從 DI 容器獲取 ILoggerFactory
            LoggerFactory = _host.Services.GetRequiredService<ILoggerFactory>();
        }

        public void Dispose()
        {
            _host?.Dispose();
            Log.CloseAndFlush(); // 確保日誌緩衝區已清空並關閉
        }
    }
}
