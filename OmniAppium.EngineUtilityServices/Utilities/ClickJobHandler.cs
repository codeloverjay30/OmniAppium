using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class ClickJobHandler(IClickService clickService) : IJobHandler
    {
        public bool CanHandle(Job job) => job is ClickJob;
        public Task AutoExecuteAsync(Job job)
        {
            clickService.Click((ClickJob)job);
            return Task.CompletedTask;
        }
    }
}
