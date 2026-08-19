using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class ScreenshotJobHandler(IScreenshotService screenshotService) : IJobHandler
    {
        public bool CanHandle(Job job) => job is ScreenshotJob;
        public Task AutoExecuteAsync(Job job)
        {
            screenshotService.TakeAndSaveScreenshot(((ScreenshotJob)job).FileName);
            return Task.CompletedTask;
        }
    }
}
