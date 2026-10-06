using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing; // 系統級
using System.Text;
using OmniRectangle = OmniAppium.ConfigUtilityService.Models.Rectangle; // 你的自定義類別

namespace OmniAppium.EngineUtilityService.Services.Screenshots
{
    /// <summary>
    /// Defines screenshot capture, cropping, and persistence operations.
    /// </summary>
    public interface IScreenshotService
    {
        /// <summary>
        /// Captures a fresh screenshot.
        /// </summary>
        void TakeScreenshot();
        /// <summary>
        /// Saves the cached cropped image.
        /// </summary>
        /// <param name="filename">The destination file name.</param>
        /// <param name="imageFormat">The image format, or null to use PNG.</param>
        void SaveCroppedImage(string filename , System.Drawing.Imaging.ImageFormat? imageFormat = null);
        /// <summary>
        /// Saves the cached raw screenshot.
        /// </summary>
        /// <param name="filename">The destination file name.</param>
        void SaveImage(string filename);
        /// <summary>
        /// Crops the cached screenshot to the specified area.
        /// </summary>
        /// <param name="area">The rectangular area to retain.</param>
        void CropScreenshot(System.Drawing.Rectangle area);
        /// <summary>
        /// Crops the cached screenshot to the specified area.
        /// </summary>
        /// <param name="area">The rectangular area to retain.</param>
        void CropScreenshot(OmniRectangle area);

        /// <summary>
        /// Captures and saves a fresh screenshot.
        /// </summary>
        /// <param name="filename">The destination file name.</param>
        void TakeAndSaveScreenshot(string filename);
        /// <summary>
        /// Captures, crops, and saves a fresh screenshot.
        /// </summary>
        /// <param name="area">The rectangular area to retain.</param>
        /// <param name="filename">The destination file name.</param>
        /// <param name="imageFormat">The image format, or null to use PNG.</param>
        void TakeAndSaveScreenshot(System.Drawing.Rectangle area,string filename, System.Drawing.Imaging.ImageFormat? imageFormat = null);
        /// <summary>
        /// Captures, crops, and saves a fresh screenshot.
        /// </summary>
        /// <param name="area">The rectangular area to retain.</param>
        /// <param name="filename">The destination file name.</param>
        /// <param name="imageFormat">The image format, or null to use PNG.</param>
        void TakeAndSaveScreenshot(OmniRectangle area,string filename, System.Drawing.Imaging.ImageFormat? imageFormat = null);

        /// <summary>
        /// Captures a fresh screenshot and returns encoded bytes belonging to that exact capture.
        /// </summary>
        /// <param name="imageFormat">
        /// The image format to return. PNG is used when no format is specified.
        /// </param>
        /// <returns>
        /// The encoded bytes of the newly captured screenshot.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the screenshot cannot be captured or contains no image data.
        /// </exception>
        byte[] CaptureScreenshotBytes(
            System.Drawing.Imaging.ImageFormat? imageFormat = null);
    
        /// <summary>
        /// Returns the cached screenshot as encoded image bytes, capturing it when necessary.
        /// </summary>
        /// <param name="imageFormat">
        /// The image format to return. PNG is used when no format is specified.
        /// </param>
        /// <returns>
        /// The screenshot encoded as an image byte array.
        /// </returns>
        byte[] GetBytesOfCachedScreenshotBytes(
            System.Drawing.Imaging.ImageFormat? imageFormat = null
        );
    }
}
