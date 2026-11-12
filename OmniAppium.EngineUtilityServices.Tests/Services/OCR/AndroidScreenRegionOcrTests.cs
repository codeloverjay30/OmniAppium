using CoordinateUtilityServices;
using FluentAssertions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Services.OCR;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.OCR;

public sealed class AndroidScreenRegionOcrTests
{
    [Fact]
    public async Task RecognizeRegionAsync_ShouldDelegateWithOriginalRegionAndLanguage()
    {
        byte[] screenshot = [1, 2, 3];
        Rectangle region = Rectangle.FromXYWH(2030, 272, 250, 85);
        var ocr = new Mock<IOCRUtilityService>(MockBehavior.Strict);
        OcrResult expected = new("限時禮包", Array.Empty<OcrTextLine>());
        ocr.Setup(x => x.RecognizeRegionAsync(
                It.Is<byte[]>(b => b.SequenceEqual(screenshot)),
                region, "zh-TW", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        AndroidScreenOcrService sut = CreateService(ocr.Object);

        OcrResult actual = await sut.RecognizeRegionAsync(screenshot, region);

        actual.Should().BeSameAs(expected);
        ocr.VerifyAll();
    }

    [Fact]
    public async Task RecognizeRegionAsync_EmptyBuffer_ShouldRejectBeforeCallingOcr()
    {
        var ocr = new Mock<IOCRUtilityService>(MockBehavior.Strict);
        AndroidScreenOcrService sut = CreateService(ocr.Object);
        Rectangle region = Rectangle.FromXYWH(0, 0, 10, 10);

        Func<Task> act = () => sut.RecognizeRegionAsync(ReadOnlyMemory<byte>.Empty, region);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*contains no image data*");
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RecognizeRegionAsync_InvalidRegion_ShouldRejectBeforeCallingOcr()
    {
        var ocr = new Mock<IOCRUtilityService>(MockBehavior.Strict);
        AndroidScreenOcrService sut = CreateService(ocr.Object);
        Rectangle region = Rectangle.FromXYWH(-1, 0, 10, 10);

        Func<Task> act = () => sut.RecognizeRegionAsync(new byte[] { 1 }, region);

        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*OCR region must have finite*");
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RecognizeRegionAsync_Cancelled_ShouldNotCallOcr()
    {
        var ocr = new Mock<IOCRUtilityService>(MockBehavior.Strict);
        AndroidScreenOcrService sut = CreateService(ocr.Object);
        using var source = new CancellationTokenSource();
        source.Cancel();

        Func<Task> act = () => sut.RecognizeRegionAsync(
            new byte[] { 1 }, Rectangle.FromXYWH(0, 0, 10, 10), source.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
        ocr.VerifyNoOtherCalls();
    }

    private static AndroidScreenOcrService CreateService(IOCRUtilityService ocr)
    {
        var screenshot = new Mock<IScreenshotService>(MockBehavior.Strict);
        var loggerFactory = new Mock<ILoggerFactoryBaseUtilityService>(MockBehavior.Strict);
        loggerFactory.SetupGet(x => x.Logger)
            .Returns(Mock.Of<ILogger>());
        return new AndroidScreenOcrService(screenshot.Object, ocr, loggerFactory.Object);
    }
}
