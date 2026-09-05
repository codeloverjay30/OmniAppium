

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
        /// <summary>
        /// Captures a fresh screenshot from the current Appium driver session.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the driver fails to return a screenshot.
        /// </exception>
        public void TakeScreenshot()
        {
            ClearBitmaps();

            var screenshot =
                Driver.GetScreenshot();

            if (screenshot is null)
            {
                throw new InvalidOperationException(
                    "The Appium driver returned a null screenshot.");
            }

            if (screenshot.AsByteArray is not { Length: > 0 })
            {
                throw new InvalidOperationException(
                    "The Appium driver returned an empty screenshot.");
            }

            _rawScreenshot =
                screenshot;

            HasBeenCropped =
                false;
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

        /// <summary>
        /// Crops the currently captured screenshot to the specified area.
        /// </summary>
        /// <param name="area">
        /// The rectangular area to crop.
        /// </param>
        [SupportedOSPlatform("windows")]
        [RequiresRuntime(6, 1, "WINDOWS")]
        public void CropScreenshot(
            System.Drawing.Rectangle area)
        {
            ArgumentNullException.ThrowIfNull(
                _rawScreenshot,
                nameof(_rawScreenshot));

            if (_fullBitmap is null)
            {
                using var stream =
                    new MemoryStream(
                        _rawScreenshot.AsByteArray,
                        writable: false);

                using var sourceBitmap =
                    new Bitmap(stream);

                _fullBitmap =
                    new Bitmap(sourceBitmap);
            }

            _croppedBitmap?.Dispose();

            _croppedBitmap =
                _fullBitmap.Clone(
                    area,
                    _fullBitmap.PixelFormat);

            HasBeenCropped =
                true;
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

        /// <summary>
        /// Gets the cached screenshot as an image byte array.
        /// A new screenshot is captured when no cached raw screenshot exists.
        /// </summary>
        /// <param name="imageFormat">
        /// The image format to return. PNG is used when no format is specified.
        /// </param>
        /// <returns>
        /// The screenshot encoded as an image byte array.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a screenshot cannot be captured or contains no image data.
        /// </exception>
        [SupportedOSPlatform("windows")]
        [RequiresRuntime(6, 1, "WINDOWS")]
        public byte[] GetBytesOfCachedScreenshotBytes(
            ImageFormat? imageFormat = null)
        {
            var format =
                imageFormat ?? ImageFormat.Png;

            if (_rawScreenshot is null)
            {
                TakeScreenshot();
            }

            if (_rawScreenshot is null)
            {
                throw new InvalidOperationException(
                    "A screenshot could not be captured from the Appium driver.");
            }

            var rawBytes =
                _rawScreenshot.AsByteArray;

            if (rawBytes is null || rawBytes.Length == 0)
            {
                throw new InvalidOperationException(
                    "The captured screenshot contains no image data.");
            }

            if (format.Guid == ImageFormat.Png.Guid)
            {
                return rawBytes;
            }

            using var inputStream =
                new MemoryStream(
                    rawBytes,
                    writable: false);

            using var bitmap =
                new Bitmap(inputStream);

            using var outputStream =
                new MemoryStream();

            bitmap.Save(
                outputStream,
                format);

            return outputStream.ToArray();
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
