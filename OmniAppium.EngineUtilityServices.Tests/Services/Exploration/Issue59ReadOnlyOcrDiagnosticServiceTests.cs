using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppiumDemo.SmokeTests.Issue59;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59ReadOnlyOcrDiagnosticServiceTests
{
    private static readonly byte[] ValidPng = Convert.FromBase64String(
        "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAIAAAD8GO2jAAAAKUlEQVR4nO3NMQEAAAjDMMC/52ECvlRA00nqs3m9AwAAAAAAAAAAgMMWx/EDPS4YA2MAAAAASUVORK5CYII=");

    [Fact]
    public async Task CaptureAndLogAsync_ShouldUseExactlyOneScreenshotForOcr()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var logger = new Mock<ILogger<Issue59ReadOnlyOcrDiagnosticService>>();

        screenshots.Setup(s => s.CaptureScreenshotBytes(null)).Returns(ValidPng);
        ocr.Setup(s => s.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OcrResult("武將", Array.Empty<OcrTextLine>()));

        var sut = new Issue59ReadOnlyOcrDiagnosticService(
            screenshots.Object, ocr.Object, logger.Object);

        await sut.CaptureAndLogAsync("baseline");

        screenshots.Verify(s => s.CaptureScreenshotBytes(null), Times.Once);
        ocr.Verify(s => s.RecognizeAsync(
            It.Is<ReadOnlyMemory<byte>>(bytes => bytes.Length == ValidPng.Length),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CaptureAndLogAsync_ShouldRejectEmptyScreenshotBeforeOcr()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var logger = new Mock<ILogger<Issue59ReadOnlyOcrDiagnosticService>>();

        screenshots.Setup(s => s.CaptureScreenshotBytes(null)).Returns(Array.Empty<byte>());
        var sut = new Issue59ReadOnlyOcrDiagnosticService(
            screenshots.Object, ocr.Object, logger.Object);

        Func<Task> act = () => sut.CaptureAndLogAsync("baseline");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*screenshot size is invalid*");
        ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CaptureAndLogAsync_ShouldPreserveOcrFailure()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var ocr = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var logger = new Mock<ILogger<Issue59ReadOnlyOcrDiagnosticService>>();

        screenshots.Setup(s => s.CaptureScreenshotBytes(null)).Returns(ValidPng);
        ocr.Setup(s => s.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("OCR engine unavailable"));

        var sut = new Issue59ReadOnlyOcrDiagnosticService(
            screenshots.Object, ocr.Object, logger.Object);

        Func<Task> act = () => sut.CaptureAndLogAsync("follow-up-1");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*OCR engine unavailable*");
    }
}