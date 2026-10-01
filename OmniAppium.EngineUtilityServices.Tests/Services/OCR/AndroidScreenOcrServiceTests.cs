using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Services.OCR;
using System.Drawing.Imaging;

namespace OmniAppium.EngineUtilityServices.Tests.Services.OCR;

public sealed class AndroidScreenOcrServiceTests
{
    private readonly Mock<IScreenshotService> _screenshots = new(MockBehavior.Strict);
    private readonly Mock<IOCRUtilityService> _ocr = new(MockBehavior.Strict);

    [Fact]
    public async Task RecognizeCurrentScreenAsync_CapturesFreshBytesAndForwardsLanguageAndCancellation()
    {
        byte[] bytes = [1, 2, 3];
        using var cancellation = new CancellationTokenSource();
        var expected = new OcrResult("任務", []);
        var sequence = new MockSequence();
        _screenshots.InSequence(sequence).Setup(service => service.TakeScreenshot());
        _screenshots.InSequence(sequence)
            .Setup(service => service.GetBytesOfCachedScreenshotBytes(null)).Returns(bytes);
        _ocr.InSequence(sequence)
            .Setup(service => service.RecognizeAsync(bytes, "zh-TW", cancellation.Token))
            .ReturnsAsync(expected);
        var sut = new AndroidScreenOcrService(_screenshots.Object, _ocr.Object);

        var actual = await sut.RecognizeCurrentScreenAsync(cancellation.Token);

        actual.Should().BeSameAs(expected);
        _screenshots.VerifyAll();
        _ocr.VerifyAll();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_RejectsEmptyScreenshotBeforeRecognition()
    {
        _screenshots.Setup(service => service.TakeScreenshot());
        _screenshots.Setup(service => service.GetBytesOfCachedScreenshotBytes(null)).Returns([]);
        var sut = new AndroidScreenOcrService(_screenshots.Object, _ocr.Object);

        Action act = () => sut.RecognizeCurrentScreenAsync().GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The Android screenshot contains no image data.");
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_HonorsCancellationBeforeCapture()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var sut = new AndroidScreenOcrService(_screenshots.Object, _ocr.Object);

        Action act = () => sut.RecognizeCurrentScreenAsync(cancellation.Token).GetAwaiter().GetResult();

        act.Should().Throw<OperationCanceledException>()
            .WithMessage("The operation was canceled.");
        _screenshots.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_HonorsCancellationAfterCapture()
    {
        using var cancellation = new CancellationTokenSource();
        _screenshots.Setup(service => service.TakeScreenshot()).Callback(cancellation.Cancel);
        _screenshots.Setup(service => service.GetBytesOfCachedScreenshotBytes(null)).Returns([1]);
        var sut = new AndroidScreenOcrService(_screenshots.Object, _ocr.Object);

        Action act = () => sut.RecognizeCurrentScreenAsync(cancellation.Token).GetAwaiter().GetResult();

        act.Should().Throw<OperationCanceledException>()
            .WithMessage("The operation was canceled.");
        _screenshots.VerifyAll();
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_PropagatesScreenshotFailure()
    {
        _screenshots.Setup(service => service.TakeScreenshot())
            .Throws(new InvalidOperationException("Screenshot capture failed."));
        var sut = new AndroidScreenOcrService(_screenshots.Object, _ocr.Object);

        Action act = () => sut.RecognizeCurrentScreenAsync().GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>().WithMessage("Screenshot capture failed.");
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void RecognizeCurrentScreenAsync_PropagatesRecognitionFailure()
    {
        byte[] bytes = [1];
        _screenshots.Setup(service => service.TakeScreenshot());
        _screenshots.Setup(service => service.GetBytesOfCachedScreenshotBytes(null)).Returns(bytes);
        _ocr.Setup(service => service.RecognizeAsync(bytes, "zh-TW", CancellationToken.None))
            .ThrowsAsync(new InvalidOperationException("OCR recognition failed."));
        var sut = new AndroidScreenOcrService(_screenshots.Object, _ocr.Object);

        Action act = () => sut.RecognizeCurrentScreenAsync().GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>().WithMessage("OCR recognition failed.");
    }
}
