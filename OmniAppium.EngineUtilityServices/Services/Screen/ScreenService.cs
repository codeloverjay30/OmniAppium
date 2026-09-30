using OpenQA.Selenium.Appium.Android;
using System;
using System.Drawing;

namespace OmniAppium.EngineUtilityService.Services.Screen
{
    /// <summary>
    /// Provides cached and refreshed screen dimensions for an Android session.
    /// </summary>
    public abstract class ScreenService
    {
        private readonly Lazy<Size> _screenSize;
        public required AndroidDriver Driver { get; init; }

        /// <summary>
        /// Initializes lazy screen-size retrieval for the Android session.
        /// </summary>
        protected ScreenService()
        {
            _screenSize = new Lazy<Size>(() => Driver.Manage().Window.Size);
        }

        // 預載的值 (Lazy)
        public Size ScreenSize => _screenSize.Value;

        // 即時獲取的值 (不經過 Lazy 快取)
        /// <summary>
        /// Reads the current screen dimensions from the Android driver.
        /// </summary>
        /// <returns>The current screen dimensions.</returns>
        public Size GetFreshScreenSize() => Driver.Manage().Window.Size;
    }
}
