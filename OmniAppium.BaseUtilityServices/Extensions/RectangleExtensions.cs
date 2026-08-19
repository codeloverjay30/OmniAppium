using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing; // 系統級
using OmniRectangle = OmniAppium.ConfigUtilityService.Models.Rectangle; // 你的自定義類別

namespace OmniAppium.ConfigUtilityService.Extensions
{
    public static class RectangleExtensions
    {
        /// <summary>
        /// Convert your own model <see cref="OmniAppium.ConfigUtilityService.Models.Rectangle"/> type to <see cref="System.Drawing.Rectangle"/> structure.
        /// </summary>
        /// <param name="omniRect"></param>
        /// <returns></returns>
        public static Rectangle ToSystemDrawing(this OmniRectangle omniRect)
        {
            ArgumentNullException.ThrowIfNull(omniRect.LeftTop);
            return new Rectangle(
                Math.Max(0 , omniRect.LeftTop.X) ,
                Math.Max(0 , omniRect.LeftTop.Y) ,
                Math.Max(1 , omniRect.Width) ,
                Math.Max(1 , omniRect.Height)
            );
        }
    }
}
