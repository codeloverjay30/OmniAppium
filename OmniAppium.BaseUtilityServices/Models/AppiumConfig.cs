using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class AppiumConfig
    {
        /// <summary>
        /// Appium 服務器的地址。
        /// </summary>
        /// <remarks>
        /// 必須配對於筆電連上手機熱點的address
        /// </remarks>
        public string AppiumServerUri { get; set; } = "http://127.0.0.1:4723/";

        /// <summary>
        /// 要連線的Android設備名稱。
        /// </summary>
        /// <remarks>
        /// 對應到appium:deviceName選項
        /// </remarks>
        public string DeviceName { get; set; } = "Android Emulator";

        /// <summary>
        /// 要連線至裝置的UDID（Unique Device Identifier）。這是用於識別和連接到特定Android設備的唯一標識符。
        /// 確保UDID正確無誤，否則自動化引擎將無法連接到目標設備。
        /// </summary>
        /// <remarks>
        /// 對應到appium:udid選項
        /// </remarks>

        public string? DeviceUdid { get; set; }

        /// <summary>
        /// 平台名稱，指定要自動化的目標平台，例如 "Android" 或 "iOS"。
        /// 這個參數告訴自動化引擎應該使用哪種驅動程序和策略來與設備進行交互。
        /// 對於Android遊戲自動化，通常設置為 "Android"。
        /// </summary>
        /// <remarks>
        /// 對應到`new AppiumOptions().PlatformName property`的值
        /// </remarks>
        public string PlatformName { get; set; } = "Android";

        /// <summary>
        /// 是否要重新設置遊戲狀態。
        /// 設置為 true 表示在每次啟動遊戲前都會清除遊戲數據，恢復到初始狀態；
        /// 設置為 false 則保持遊戲的當前狀態，適合需要保留進度的自動化任務。
        /// </summary>
        /// <remarks>
        /// 對應到appium:noReset選項
        /// </remarks>
        public bool NoReset { get; set; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:ignoreHiddenApiPolicyError選項
        /// </remarks>
        public bool IgnoreHiddenApiPolicyError { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:ensureWebviewsHavePages選項
        /// </remarks>
        public bool EnsureWebviewsHavePages { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:noAnalyzer選項
        /// </remarks>
        public bool NoAnalyzer { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:skipDeviceInitialization選項
        /// </remarks>
        public bool SkipDeviceInitialization { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:dontStopAppOnReset選項
        /// </remarks>
        public bool DontStopAppOnReset { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:forceAppLaunch選項
        /// </remarks>
        public bool ForceAppLaunch { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:shouldTerminateApp選項
        /// </remarks>
        public bool ShouldTerminateApp { get; init; } = false;
        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:settings[ignoreUnimportantViews]選項
        /// </remarks>
        public bool IgnoreUnimportantViews { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:settings[screenshotQuality]選項
        /// </remarks>
        [Range(0,100,ErrorMessage = "Must between 0 and 100")]
        public int ScreenshotQuality { get; init; } = 60;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:settings[waitForIdleTimeout]選項
        /// </remarks>
        public uint WaitForIdleTimeout { get; init; } = 0;

    }
}
