using AiUtility.AiBaseUtilityServices.Models;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IGeminiJobHandler: IJobHandler
    {
        void SetExecutionSettings(AiExecutionSettings aiExecutionSettings);
        Task AutoExecuteAsync(Job job);
        Task AutoExecuteAsync(GeminiJob gJob);

    }
}
