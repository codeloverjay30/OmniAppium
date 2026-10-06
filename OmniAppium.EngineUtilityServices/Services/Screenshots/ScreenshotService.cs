

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

namespace OmniAppium.EngineUtilityService.Services.Screenshots
{
    /// <summary>
    /// Captures, crops, and saves screenshots from an Android driver session.
    /// </summary>
    /// <param name="loggerFactoryService">The logging service.</param>
    /// <param name="toLogWhenSuccess">Whether successful image saves should be logged.</param>
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
        private readonly object _screenshotStateSync = new();

        /// <summary>
        /// Logs a successful screenshot save.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="filename">The destination file name.</param>
        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Information , Message = "Successfully take Screenshot, and save it into {filename}")]
        static partial void LogSuccessForTakingScreenshot(ILogger logger , string filename);

        /// <summary>
        /// Logs a failed screenshot save.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="filename">The destination file name.</param>
        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Failed to take Screenshot, and it will NOT save it into {filename}")]
        static partial void LogFailureForTakingScreenshot(ILogger logger , string filename);

        public required AndroidDriver Driver { get; init; }

        private OpenQA.Selenium.Screenshot? _rawScreenshot;
        private Bitmap? _fullBitmap;
        private Bitmap? _croppedBitmap;
        private bool _hasBeenCropped;


        /// <summary>
        /// Gets the currently cached raw screenshot.
        /// </summary>
        public OpenQA.Selenium.Screenshot? Image
        {
            get
            {
                lock (_screenshotStateSync)
                {
                    return _rawScreenshot;
                }
            }
        }

        /// <summary>
        /// Gets the currently cached cropped image.
        /// </summary>
        public Bitmap? CroppedImage
        {
            get
            {
                lock (_screenshotStateSync)
                {
                    return _croppedBitmap;
                }
            }
        }

        /// <summary>
        /// Gets whether the cached screenshot has been cropped.
        /// </summary>
        public bool HasBeenCropped
        {
            get
            {
                lock (_screenshotStateSync)
                {
                    return _hasBeenCropped;
                }
            }
        }


        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the driver fails to return a screenshot.
        /// </exception>
        public void TakeScreenshot()
        {
            lock (_screenshotStateSync)
            {
                TakeScreenshotCore();
            }
        }

        /// <summary>
        /// Captures a fresh screenshot while the caller owns the screenshot-state lock.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the driver fails to return a screenshot.
        /// </exception>
        private void TakeScreenshotCore()
        {
            ClearBitmapsCore();

            OpenQA.Selenium.Screenshot? screenshot =
                Driver.GetScreenshot();

            if (screenshot is null)
            {
                throw new InvalidOperationException(
                    "The Appium driver returned a null screenshot.");
            }

            byte[] rawBytes = screenshot.AsByteArray;

            if (rawBytes.Length == 0)
            {
                throw new InvalidOperationException(
                    "The captured screenshot contains no image data.");
            }

            _rawScreenshot = screenshot;
            _hasBeenCropped = false;
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the screenshot cannot be captured or contains no image data.
        /// </exception>
        [SupportedOSPlatform("windows")]
        [RequiresRuntime(6, 1, "WINDOWS")]
        public byte[] CaptureScreenshotBytes(
            ImageFormat? imageFormat = null)
        {
            lock (_screenshotStateSync)
            {
                TakeScreenshotCore();

                return GetBytesOfCachedScreenshotBytesCore(
                    imageFormat);
            }
        }

        /// <inheritdoc/>
        [SupportedOSPlatform("windows")]
        [RequiresRuntime(6, 1, "WINDOWS")]
        public void SaveCroppedImage(
            string filename,
            ImageFormat? imageFormat = null)
        {
            lock (_screenshotStateSync)
            {
                SaveCroppedImageCore(
                    filename,
                    imageFormat);
            }
        }


        /// <summary>
        /// Saves the cached cropped image while the caller owns the screenshot-state lock.
        /// </summary>
        /// <param name="filename">
        /// The destination file name.
        /// </param>
        /// <param name="imageFormat">
        /// The image format, or null to use PNG.
        /// </param>
        private void SaveCroppedImageCore(
            string filename,
            ImageFormat? imageFormat)
        {
            ArgumentNullException.ThrowIfNull(
                _croppedBitmap);

            ImageFormat format =
                imageFormat ?? ImageFormat.Png;

            ExecuteWithLogging(
                filename,
                () => _croppedBitmap.Save(
                    filename,
                    format));
        }


        /// <inheritdoc/>
        public void SaveImage(string filename)
        {
            ArgumentNullException.ThrowIfNull(_rawScreenshot);
            ExecuteWithLogging(filename , () => _rawScreenshot.SaveAsFile(filename));
        }

        /// <inheritdoc/>
        [SupportedOSPlatform("windows")]
        [RequiresRuntime(6, 1, "WINDOWS")]
        public void CropScreenshot(
            System.Drawing.Rectangle area)
        {
            lock (_screenshotStateSync)
            {
                CropScreenshotCore(area);
            }
        }


        /// <summary>
        /// Crops the cached screenshot while the caller owns the screenshot-state lock.
        /// </summary>
        /// <param name="area">
        /// The rectangular area to retain.
        /// </param>
        private void CropScreenshotCore(
            System.Drawing.Rectangle area)
        {
            ArgumentNullException.ThrowIfNull(
                _rawScreenshot,
                nameof(_rawScreenshot));

            if (_fullBitmap is null)
            {
                byte[] rawBytes =
                    _rawScreenshot.AsByteArray;

                using var stream =
                    new MemoryStream(
                        rawBytes,
                        writable: false);

                _fullBitmap =
                    new Bitmap(stream);
            }

            _croppedBitmap?.Dispose();

            _croppedBitmap =
                _fullBitmap.Clone(
                    area,
                    _fullBitmap.PixelFormat);

            _hasBeenCropped = true;
        }


        /// <inheritdoc/>
        public void CropScreenshot(OmniRectangle area)
        {
            ArgumentNullException.ThrowIfNull(area);

            System.Drawing.Rectangle rectangle =
                area.ToSystemDrawing();
            CropScreenshot(rectangle);
    
        }

        /// <inheritdoc/>
        public void TakeAndSaveScreenshot(
            System.Drawing.Rectangle area,
            string filename,
            ImageFormat? imageFormat = null)
        {
            lock (_screenshotStateSync)
            {
                TakeScreenshotCore();
                CropScreenshotCore(area);
                SaveCroppedImageCore(
                    filename,
                    imageFormat);
            }
        }

        /// <inheritdoc/>
        public void TakeAndSaveScreenshot(
            OmniRectangle area,
            string filename,
            ImageFormat? imageFormat = null)
        {
            ArgumentNullException.ThrowIfNull(area);

            System.Drawing.Rectangle rectangle =
                area.ToSystemDrawing();

            TakeAndSaveScreenshot(
                rectangle,
                filename,
                imageFormat);
        }


        /// <inheritdoc/>
        public void TakeAndSaveScreenshot(
            string filename)
        {
            lock (_screenshotStateSync)
            {
                TakeScreenshotCore();
                SaveImage(filename);
            }
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a screenshot cannot be captured or contains no image data.
        /// </exception>
        [SupportedOSPlatform("windows")]
        [RequiresRuntime(6, 1, "WINDOWS")]
        public byte[] GetBytesOfCachedScreenshotBytes(
            ImageFormat? imageFormat = null)
        {
            lock (_screenshotStateSync)
            {
                if (_rawScreenshot is null)
                {
                    TakeScreenshotCore();
                }

                return GetBytesOfCachedScreenshotBytesCore(
                    imageFormat);
            }
        }


        /// <summary>
        /// Returns encoded bytes for the cached screenshot while the caller owns
        /// the screenshot-state lock.
        /// </summary>
        /// <param name="imageFormat">
        /// The image format to return. PNG is used when no format is specified.
        /// </param>
        /// <returns>
        /// The encoded screenshot bytes.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the cached screenshot is unavailable or contains no image data.
        /// </exception>
        private byte[] GetBytesOfCachedScreenshotBytesCore(
            ImageFormat? imageFormat)
        {
            if (_rawScreenshot is null)
            {
                throw new InvalidOperationException(
                    "A screenshot could not be captured from the Appium driver.");
            }

            byte[] rawBytes =
                _rawScreenshot.AsByteArray;

            if (rawBytes.Length == 0)
            {
                throw new InvalidOperationException(
                    "The captured screenshot contains no image data.");
            }

            ImageFormat format =
                imageFormat ?? ImageFormat.Png;

            if (format.Guid == ImageFormat.Png.Guid)
            {
                return rawBytes.ToArray();
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



        /// <summary>
        /// Executes an image-save operation and logs its outcome.
        /// </summary>
        /// <param name="filename">The destination file name used in log messages.</param>
        /// <param name="action">The image-save operation.</param>
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

        /// <summary>
        /// Disposes and clears the cached full and cropped bitmaps.
        /// </summary>
        private void ClearBitmapsCore()
        {
            _fullBitmap?.Dispose();
            _fullBitmap = null;
            _croppedBitmap?.Dispose();
            _croppedBitmap = null;
        }

        /// <summary>
        /// Releases the cached screenshot resources.
        /// </summary>
        public void Dispose()
        {
            lock (_screenshotStateSync)
            {
                ClearBitmapsCore();

                _rawScreenshot = null;
                _hasBeenCropped = false;
            }

            GC.SuppressFinalize(this);
        }
    }
}
