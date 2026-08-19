using System;
using System.Threading.Tasks;
#if WINDOWS
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using InMemoryRandomAccessStream = Windows.Storage.Streams.InMemoryRandomAccessStream;
#endif

namespace OCRUtilityService.Services
{
    public class OCRUtilityService : IOCRUtilityService
    {
        /// <summary>
        /// 快速 OCR 方法，適用於小圖或對速度要求較高的場景。
        /// Windows OCR 引擎會自動利用 CPU 多核心優化，因此不需要額外處理多線程。
        /// </summary>
        public async Task<string> QuickOcrAsync(byte [ ] imageBuffer)
        {
            if(imageBuffer == null || imageBuffer.Length == 0)
                return string.Empty;

#if WINDOWS
    // 只有在編譯目標為 Windows 時，以下代碼才會被包含進去
    using (var stream = new InMemoryRandomAccessStream())
    {
        await stream.WriteAsync(imageBuffer.AsBuffer());
        stream.Seek(0);
        var decoder = await BitmapDecoder.CreateAsync(stream);

        using (var softwareBitmap = await decoder.GetSoftwareBitmapAsync())
        {
            var ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
            if (ocrEngine == null) return "OCR Engine initialization failed.";
            var result = await ocrEngine.RecognizeAsync(softwareBitmap);
            return result.Text;
        }
    }
#else
            // 如果是 net10.0 (非 Windows 平台)，回傳尚未實作或使用其他 OCR 方案
            return await Task.FromResult("OCR is only supported on Windows platform in this implementation.");
#endif
        }
    }
}
