using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class WaitJobHandler(IWaitService waitService) : IJobHandler
    {
        public bool CanHandle(Job job) => job is WaitJob;
        public Task AutoExecuteAsync(Job job)
        {
            waitService.Wait(((WaitJob)job).WaitMilliSecond);
            return Task.CompletedTask;
        }
    }
}
