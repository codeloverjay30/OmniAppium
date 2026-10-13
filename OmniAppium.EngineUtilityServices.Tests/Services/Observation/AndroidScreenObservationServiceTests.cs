
using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Observation;

public sealed class AndroidScreenObservationServiceTests
{
    private readonly Mock<IScreenshotService> _screenshots =
        new(MockBehavior.Strict);

    private readonly Mock<IAndroidScreenOcrService> _ocr =
        new(MockBehavior.Strict);

    private readonly Mock<IAndroidScreenStabilityGuard> _guard =
        new(MockBehavior.Strict);

    private readonly ScreenStabilityOptions _options = new();

    private AndroidScreenObservationService CreateSut()
    {
        return new AndroidScreenObservationService(
            _screenshots.Object,
            _ocr.Object,
            _guard.Object,
            _options);
    }

    private void SetupGuard(ScreenStabilityResult result)
    {
        _guard.Setup(x => x.WaitForStabilityAsync(
                It.IsAny<ScreenStabilityOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
    }

    private static OcrResult CreateOcrResult()
    {
        return new OcrResult(
            Text: "Test OCR text",
            Lines: Array.Empty<OcrTextLine>());
    }

    [Fact]
    public async Task ObserveAsync_WhenStable_ShouldReturnObservation()
    {
        byte[] image = [1, 2, 3, 4];
        OcrResult ocrResult = CreateOcrResult();

        SetupGuard(new ScreenStabilityResult(true, false, 3));

        _screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(image);

        _ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ocrResult);

        var sut = CreateSut();

        var observation = await sut.ObserveAsync();

        observation.ImageBytes.ToArray().Should().Equal(image);
        observation.OcrResult.Should().BeSameAs(ocrResult);
        observation.OcrResult.Text.Should().Be("Test OCR text");

        _guard.Verify(x => x.WaitForStabilityAsync(
            _options,
            It.IsAny<CancellationToken>()), Times.Once);

        _screenshots.Verify(
            x => x.CaptureScreenshotBytes(null),
            Times.Once);

        _ocr.Verify(x => x.RecognizeAsync(
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(false, false, 2)]
    [InlineData(false, true, 2)]
    [InlineData(true, true, 4)]
    public void ObserveAsync_WhenGuardRejects_ShouldFailClosed(
        bool stable,
        bool timedOut,
        int samples)
    {
        SetupGuard(
            new ScreenStabilityResult(stable, timedOut, samples));

        var sut = CreateSut();

        Action act = () =>
            sut.ObserveAsync().GetAwaiter().GetResult();

        act.Should()
            .Throw<TimeoutException>()
            .WithMessage(
                $"Android screen stability timeout after {samples} samples.");

        _screenshots.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenAlreadyCancelled_ShouldNotCallDependencies()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var sut = CreateSut();

        Action act = () =>
            sut.ObserveAsync(cts.Token).GetAwaiter().GetResult();

        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage("*canceled*");

        _guard.VerifyNoOtherCalls();
        _screenshots.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenGuardThrows_ShouldPropagateException()
    {
        _guard.Setup(x => x.WaitForStabilityAsync(
                It.IsAny<ScreenStabilityOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new InvalidOperationException("Guard capture failed."));

        var sut = CreateSut();

        Action act = () =>
            sut.ObserveAsync().GetAwaiter().GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Guard capture failed.");

        _screenshots.VerifyNoOtherCalls();
        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenScreenshotIsEmpty_ShouldNotRunOcr()
    {
        SetupGuard(new ScreenStabilityResult(true, false, 3));

        _screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(Array.Empty<byte>());

        var sut = CreateSut();

        Action act = () =>
            sut.ObserveAsync().GetAwaiter().GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "The captured Android screen contains no image data.");

        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenScreenshotThrows_ShouldPropagateException()
    {
        SetupGuard(new ScreenStabilityResult(true, false, 3));

        _screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Throws(
                new InvalidOperationException(
                    "Appium screenshot failed."));

        var sut = CreateSut();

        Action act = () =>
            sut.ObserveAsync().GetAwaiter().GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Appium screenshot failed.");

        _ocr.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObserveAsync_WhenOcrThrows_ShouldPropagateException()
    {
        SetupGuard(new ScreenStabilityResult(true, false, 3));

        _screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns([1, 2, 3]);

        _ocr.Setup(x => x.RecognizeAsync(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new InvalidOperationException("OCR engine failed."));

        var sut = CreateSut();

        Action act = () =>
            sut.ObserveAsync().GetAwaiter().GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("OCR engine failed.");
    }

    [Fact]
    public void Constructor_WhenScreenshotServiceIsNull_ShouldThrow()
    {
        Action act = () => new AndroidScreenObservationService(
            null!,
            _ocr.Object,
            _guard.Object,
            _options);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*screenshotService*");
    }

    [Fact]
    public void Constructor_WhenOcrServiceIsNull_ShouldThrow()
    {
        Action act = () => new AndroidScreenObservationService(
            _screenshots.Object,
            null!,
            _guard.Object,
            _options);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*androidScreenOcrService*");
    }

    [Fact]
    public void Constructor_WhenGuardIsNull_ShouldThrow()
    {
        Action act = () => new AndroidScreenObservationService(
            _screenshots.Object,
            _ocr.Object,
            null!,
            _options);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*stabilityGuard*");
    }

    [Fact]
    public void Constructor_WhenOptionsAreNull_ShouldThrow()
    {
        Action act = () => new AndroidScreenObservationService(
            _screenshots.Object,
            _ocr.Object,
            _guard.Object,
            null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*stabilityOptions*");
    }

    [Fact]
    public void Constructor_WhenOptionsAreInvalid_ShouldThrow()
    {
        var invalid = new ScreenStabilityOptions
        {
            RequiredConsecutiveMatches = 0
        };

        Action act = () => new AndroidScreenObservationService(
            _screenshots.Object,
            _ocr.Object,
            _guard.Object,
            invalid);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*RequiredConsecutiveMatches must be positive*");
    }
}