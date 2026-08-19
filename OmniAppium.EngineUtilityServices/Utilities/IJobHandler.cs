using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IJobHandler
    {
        // 判斷這個 Handler 是否能處理該 Job
        bool CanHandle(Job job);
        // 執行邏輯
        Task AutoExecuteAsync(Job job);
    }
}
