using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class AppConfig
    {
    
        /// <summary>
        /// 該遊戲的啟動 Activity 名稱，這是 Android 應用的入口點，用於在設備上啟動遊戲。
        /// Activity 名稱通常需要查詢遊戲的 APK 文件或使用 Android Debug Bridge (ADB) 工具來獲取。
        /// </summary>
        /// <remarks>
        /// 對應到appium:appActivity選項
        /// </remarks>

        public string MainActivity { get; set; }


        /// <summary>
        /// 遊戲名稱，用於識別和管理不同的遊戲配置。這個名稱應該具有描述性，以便在多個遊戲配置中能夠清晰區分。例如，可以使用 "Candy Crush Saga" 或 "Clash of Clans" 作為遊戲名稱。
        /// </summary>
        public string? GameName { get; set; }
        /// <summary>
        /// 遊戲包名，這是 Android 應用的唯一標識符，用於在設備上定位和啟動遊戲。包名通常採用反向域名格式，例如 "com.king.candycrushsaga"。確保包名正確無誤，否則自動化引擎將無法找到並啟動遊戲。
        /// </summary>
        /// 對應到appium:appPackage選項
        /// </remarks>
        public string? PackageName { get; set; }

        public string PlatformBy { get; init; }
        public string WorkspacePath { get; init; }
    }
}
