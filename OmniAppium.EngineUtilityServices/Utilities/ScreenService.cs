using OpenQA.Selenium.Appium.Android;
using System;
using System.Drawing;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public abstract class ScreenService
    {
        private readonly Lazy<Size> _screenSize;
        public required AndroidDriver Driver { get; init; }

        protected ScreenService()
        {
            _screenSize = new Lazy<Size>(() => Driver.Manage().Window.Size);
        }

        // 預載的值 (Lazy)
        public Size ScreenSize => _screenSize.Value;

        // 即時獲取的值 (不經過 Lazy 快取)
        public Size GetFreshScreenSize() => Driver.Manage().Window.Size;
    }
}
