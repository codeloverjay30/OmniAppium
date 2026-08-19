using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    /// <summary>
    /// 開發裝置(i.e. 測試用的裝置) (如:我的手機)的相關組態
    /// </summary>
    public class DevelopmentDeviceConfig
    {
        /// <summary>
        /// 開發裝置預期的螢幕尺寸大小
        /// </summary>
        public required Size ScreenSize { get; init; }
    }
}
