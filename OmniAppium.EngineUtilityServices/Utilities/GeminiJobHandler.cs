using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Models;
using AiUtility.GeminiUtilityServices.Configs;
using AiUtility.GeminiUtilityServices.DataAnnotations;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AiUtility.ToolKits.Abstractions;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class GeminiJobHandler<TProgress>(
        AiExecutionSettings aiExecutionSettings,
        IToolRegistry<ToolMetadataBase , GeminiToolAttribute> registry ,
        IToolDispatcher<ToolMetadataBase , GeminiToolAttribute> dispatcher ,
        IAiToolConverter<GeminiToolDeclaration> converter ,
        IGeminiSessionManager sessionManager,
        IScreenshotService screenshotService,
        IProgress<TProgress> progressBar
    ) : IGeminiJobHandler
        where TProgress : WorkflowProgress,new()
    {
        public static readonly GeminiGenerateRequest DefaultRequest = new GeminiConfig().DefaultRequestConfig;

        public AiExecutionSettings _aiExecutionSettings { get; private set; } = aiExecutionSettings;

        public void SetExecutionSettings(AiExecutionSettings aiExecutionSettings)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(_aiExecutionSettings.ToolExecutionTimeout.TotalMilliseconds , nameof(_aiExecutionSettings.ToolExecutionTimeout));
            _aiExecutionSettings = aiExecutionSettings;
        }
        public bool CanHandle(Job job) => job is GeminiJob;

        public async Task AutoExecuteAsync(Job job)
        {
            switch(job)
            {
                case GeminiJob gJob:
                    await AutoExecuteAsync(gJob);
                    break;
                /* TODO: other cases */
                default:
                    throw new ArgumentException("job is not a gemini job");
            }
        }
        public async Task AutoExecuteAsync(GeminiJob gJob)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(_aiExecutionSettings.ToolExecutionTimeout.TotalMilliseconds,nameof(_aiExecutionSettings.ToolExecutionTimeout));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(gJob.UserTask,nameof(GeminiJob.UserTask));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(gJob.Prompt,nameof(GeminiJob.Prompt));
            // 1. 將所有註冊的 Service 轉換為 Gemini 的工具聲明
            var tools = registry.GetAllTools()
                                .Select(m => converter.ToToolDeclaration(m))
                                .ToList();

            // 取得目前畫面截圖 (Png 格式)
            var imageBytes = screenshotService.GetBytesOfCachedScreenshotBytes(imageFormat:ImageFormat.Png);

            // 3. 呼叫 Gemini API (包含 Tools 資訊)
            var request = DefaultRequest.Clone();
            request.SetPrompt(gJob.Prompt);

            // 將指令和截圖加入part
            request.AddUserMessage(request.Prompt , imageBytes);

            using CancellationTokenSource cts = new CancellationTokenSource(_aiExecutionSettings.ToolExecutionTimeout);
            var ct = cts.Token;
            await sessionManager.ExecuteWithToolSupportAsync<TProgress>(
                    request: request ,
                    userTask: gJob.UserTask ,
                    settings: _aiExecutionSettings ,
                    ct:ct,
                    progress: progressBar
            );
        }
    }
}
