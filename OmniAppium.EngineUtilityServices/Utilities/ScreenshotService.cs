

using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using NetRuntimeUtilityServices;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using OmniRectangle = OmniAppium.ConfigUtilityService.Models.Rectangle; // 你的自定義類別

namespace OmniAppium.EngineUtilityService.Utilities
{
    [SupportedOSPlatform("windows")]
    [RequiresRuntime(6 , 1 , "WINDOWS")]
    public partial class ScreenshotService(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess
    ) :
        BaseUtility(loggerFactoryService , toLogWhenSuccess),
        IScreenshotService,
        IDisposable
    {
        private readonly ILogger _logger = loggerFactoryService.Logger;
        private readonly bool _toLogWhenSuccess = toLogWhenSuccess;

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Information , Message = "Successfully take Screenshot, and save it into {filename}")]
        static partial void LogSuccessForTakingScreenshot(ILogger logger , string filename);

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Failed to take Screenshot, and it will NOT save it into {filename}")]
        static partial void LogFailureForTakingScreenshot(ILogger logger , string filename);

        public required AndroidDriver Driver { get; init; }

        private Screenshot? _rawScreenshot;
        public Screenshot? Image => _rawScreenshot;

        /// <summary>
        /// Cached Bitmap of Raw screenshot <see cref="_rawScreenshot"/>
        /// </summary>
        private Bitmap? _fullBitmap;

        /// <summary>
        /// Cached Bitmap of cropped screenshot
        /// </summary>
        private Bitmap? _croppedBitmap;
        public Bitmap? CroppedImage => _croppedBitmap;
        public bool HasBeenCropped { get; private set; } = false;
        public void TakeScreenshot()
        {
            ClearBitmaps();
            _rawScreenshot = Driver.GetScreenshot();
            HasBeenCropped = false;
        }

        [SupportedOSPlatform("windows")]
        [RequiresRuntime(6,1, "WINDOWS")]
        public void SaveCroppedImage(string filename , ImageFormat? imageFormat = null)
        {
            ArgumentNullException.ThrowIfNull(_croppedBitmap);
            var format = imageFormat ?? ImageFormat.Png;
            ExecuteWithLogging(filename , () => _croppedBitmap.Save(filename , format));
        }

        public void SaveImage(string filename)
        {
            ArgumentNullException.ThrowIfNull(_rawScreenshot);
            ExecuteWithLogging(filename , () => _rawScreenshot.SaveAsFile(filename));
        }
        [SupportedOSPlatform("windows")]
        [RequiresRuntime(6 , 1 , "WINDOWS")]
        public void CropScreenshot(System.Drawing.Rectangle area)
        {
            ArgumentNullException.ThrowIfNull(_rawScreenshot , nameof(_rawScreenshot));

            // 如果尚未建立 fullBitmap，則建立它（Lazy Loading）
            if(_fullBitmap == null)
            {
                using var ms = new MemoryStream(_rawScreenshot.AsByteArray);
                _fullBitmap = new Bitmap(ms);
            }

            // 清理上一次的裁切結果
            _croppedBitmap?.Dispose();

            // 執行裁切
            _croppedBitmap = _fullBitmap.Clone(area , _fullBitmap.PixelFormat);
            HasBeenCropped = true;
        }
        public void CropScreenshot(OmniRectangle area)
        {
            Rectangle rectangle = area.ToSystemDrawing();
            CropScreenshot(rectangle);
        }

        public void TakeAndSaveScreenshot(System.Drawing.Rectangle area , string filename , ImageFormat? imageFormat = null)
        {
            TakeScreenshot();
            CropScreenshot(area);
            SaveCroppedImage(filename , imageFormat);
        }

        public void TakeAndSaveScreenshot(OmniRectangle area , string filename , ImageFormat? imageFormat = null)
        {
            TakeScreenshot();
            CropScreenshot(area);
            SaveCroppedImage(filename , imageFormat);
        }

        public void TakeAndSaveScreenshot(string filename)
        {
            TakeScreenshot();
            SaveImage(filename);
        }

        public byte [ ] GetBytesOfCachedScreenshotBytes(
            ImageFormat? imageFormat = null
        )
        {
            imageFormat = imageFormat ?? ImageFormat.Png; // 預設使用png格式來儲存截圖

            // 如果目前沒有截圖，先執行一次抓取
            if(_fullBitmap == null)
            {
                TakeScreenshot();
            }

            using(var ms = new MemoryStream())
            {
                // 將 Bitmap 轉為 特定格式的 byte array
                _fullBitmap!.Save(ms , imageFormat);
                return ms.ToArray();
            }
        }

        private void ExecuteWithLogging(string filename , Action action)
        {
            try
            {
                action();
                if(_toLogWhenSuccess)
                {
                    LogSuccessForTakingScreenshot(_logger , filename);
                }
            }
            catch(Exception ex)
            {
                LogFailureForTakingScreenshot(_logger , filename);
            }
        }

        private void ClearBitmaps()
        {
            _fullBitmap?.Dispose();
            _fullBitmap = null;
            _croppedBitmap?.Dispose();
            _croppedBitmap = null;
        }

        public void Dispose()
        {
            ClearBitmaps();
            GC.SuppressFinalize(this);
        }
    }
}
