using OpenQA.Selenium.Appium.Android;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Controllers
{
    public class DriverControlService
    {
        public required AndroidDriver Driver { get; init; }

        public void Dispose()
        {
            // 這樣會關閉 Session，但因為有設定 dontStopAppOnReset，手機上的 App 不會被關掉
            this.Driver?.Quit();
        }
    }
}
