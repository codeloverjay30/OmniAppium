using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing; // 系統級
using System.Text;
using OmniRectangle = OmniAppium.ConfigUtilityService.Models.Rectangle; // 你的自定義類別

namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IScreenshotService
    {
        void TakeScreenshot();
        void SaveCroppedImage(string filename , System.Drawing.Imaging.ImageFormat? imageFormat = null);
        void SaveImage(string filename);
        void CropScreenshot(System.Drawing.Rectangle area);
        void CropScreenshot(OmniRectangle area);

        void TakeAndSaveScreenshot(string filename);
        void TakeAndSaveScreenshot(System.Drawing.Rectangle area,string filename, System.Drawing.Imaging.ImageFormat? imageFormat = null);
        void TakeAndSaveScreenshot(OmniRectangle area,string filename, System.Drawing.Imaging.ImageFormat? imageFormat = null);

        public byte [ ] GetBytesOfCachedScreenshotBytes(
            System.Drawing.Imaging.ImageFormat? imageFormat = null
        );
    }
}
