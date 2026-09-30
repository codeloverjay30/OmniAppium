using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    /// <summary>
    /// Handles jobs that capture and save screenshots.
    /// </summary>
    /// <param name="screenshotService">The screenshot service.</param>
    public class ScreenshotJobHandler(IScreenshotService screenshotService) : IJobHandler
    {
        /// <summary>
        /// Determines whether the supplied job captures a screenshot.
        /// </summary>
        /// <param name="job">The job to inspect.</param>
        /// <returns>True when the job is a screenshot job; otherwise, false.</returns>
        public bool CanHandle(Job job) => job is ScreenshotJob;
        /// <summary>
        /// Captures and saves the screenshot requested by the job.
        /// </summary>
        /// <param name="job">The screenshot job to execute.</param>
        /// <returns>A completed task after the screenshot operation finishes.</returns>
        public Task AutoExecuteAsync(Job job)
        {
            screenshotService.TakeAndSaveScreenshot(((ScreenshotJob)job).FileName);
            return Task.CompletedTask;
        }
    }
}
