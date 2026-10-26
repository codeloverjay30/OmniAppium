using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing; // 系統級
using OmniRectangle = OmniAppium.ConfigUtilityServices.Models.Rectangle; // 你的自定義類別
using CoordinateRectangle = CoordinateUtilityServices.Rectangle;

namespace OmniAppium.ConfigUtilityServices.Extensions
{
    public static class RectangleExtensions
    {
        /// <summary>
        /// Convert your own model <see cref="OmniAppium.ConfigUtilityServices.Models.Rectangle"/> type to <see cref="System.Drawing.Rectangle"/> structure.
        /// </summary>
        /// <param name="omniRect"></param>
        /// <returns></returns>
        public static Rectangle ToSystemDrawing(this OmniRectangle omniRect)
        {
            ArgumentNullException.ThrowIfNull(omniRect.LeftTop);
            return new Rectangle(
                Math.Max(0, omniRect.LeftTop.X),
                Math.Max(0, omniRect.LeftTop.Y),
                Math.Max(1, omniRect.Width),
                Math.Max(1, omniRect.Height)
            );
        }
        
        /// <summary>
        /// Converts a domain rectangle to an integer imaging rectangle without
        /// rounding, clamping, or silently changing its dimensions.
        /// </summary>
        /// <param name="area">The domain rectangle to convert.</param>
        /// <returns>An equivalent integer imaging rectangle.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when dimensions are not positive or coordinates exceed integer bounds.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when coordinates cannot be represented exactly as integers.
        /// </exception>
        public static Rectangle ToSystemDrawing(this CoordinateRectangle area)
        {
            if (area.Width <= 0 || area.Height <= 0)
                throw new ArgumentOutOfRangeException(nameof(area), "Screenshot rectangle dimensions must be positive.");

            double x = area.TopLeft.X;
            double y = area.TopLeft.Y;
            double width = area.Width;
            double height = area.Height;

            if (x < int.MinValue || x > int.MaxValue ||
                y < int.MinValue || y > int.MaxValue ||
                width > int.MaxValue || height > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(area), "Screenshot rectangle exceeds integer coordinate bounds.");

            if (Math.Truncate(x) != x || Math.Truncate(y) != y ||
                Math.Truncate(width) != width || Math.Truncate(height) != height)
                throw new ArgumentException("Screenshot rectangle coordinates and dimensions must be integral.", nameof(area));

            if (x + width > int.MaxValue || y + height > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(area), "Screenshot rectangle right or bottom edge exceeds integer bounds.");

            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }
    }
}
