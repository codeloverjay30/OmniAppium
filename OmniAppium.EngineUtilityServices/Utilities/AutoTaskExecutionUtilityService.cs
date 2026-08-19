using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AssemblyUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class AutoTaskExecutionUtilityService(
        IGeminiAgentService aiAgentService,
        IEnumerable<IJobHandler> handlers
    ) : IAutoTaskExecutionUtilityService
    {
        private readonly IGeminiAgentService _aiAgentService = aiAgentService;
        private ILogger _logger => _aiAgentService.LoggerFactoryService.Logger;
        private IAssembliesUtilityService _assembliesUilityService => _aiAgentService.AssembliesUtilityService;
        private GeminiGenerateRequest _request => _aiAgentService.Request;
        private IGeminiToolDispatcher _dispatcher => _aiAgentService.Dispatcher;
        private IGeminiToolService _toolService => _aiAgentService.ToolService;
        private IGeminiConversationManager _conversationManager => _aiAgentService.ConversationManager;
        private IGeminiToolRegistry _toolRegistry => _aiAgentService.ToolRegistry;
        private GeminiTool _tool => _aiAgentService.Tool;

        public async Task ExecuteAsync(Job job)
        {
            // 尋找第一個可以處理該 Job 的 Handler
            var handler = handlers.FirstOrDefault(h => h.CanHandle(job));

            if(handler != null)
            {
                await handler.AutoExecuteAsync(job);
            }
            else
            {
                // 可以根據需求決定是否拋出異常或記錄 Log
                throw new NotSupportedException($"沒有找到能處理 {job.GetType().Name} 的 Handler。");
            }
        }

        // 批次執行多個任務 (例如從 JSON 讀取的任務清單)
        public async Task ExecuteSequenceAsync(IEnumerable<Job> jobs)
        {
            foreach(var job in jobs)
            {
                await ExecuteAsync(job);
            }
        }
    }
}
