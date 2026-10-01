using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityServices.Services.OCR;

namespace OmniAppium.EngineUtilityServices.Tests.Services.OCR;

public sealed class OcrClickServiceTests
{
    private readonly Mock<IAndroidScreenOcrService> _screenOcrServiceMock;
    private readonly Mock<IOcrTextMatcher> _ocrTextMatcherMock;
    private readonly Mock<IClickService> _clickServiceMock;
    private readonly OcrClickService _sut;

    public OcrClickServiceTests()
    {
        _screenOcrServiceMock =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        _ocrTextMatcherMock =
            new Mock<IOcrTextMatcher>(MockBehavior.Strict);

        _clickServiceMock =
            new Mock<IClickService>(MockBehavior.Strict);

        _sut = new OcrClickService(
            _screenOcrServiceMock.Object,
            _ocrTextMatcherMock.Object,
            _clickServiceMock.Object);
    }

    [Fact]
    public void ClickTextAsync_WhenTargetIsFound_ShouldClickCenterOfMatchedBounds()
    {
        // Arrange
        const string targetText = "VIP 9";

        var targetLine = new OcrTextLine(
            targetText,
            Rectangle.FromXYWH(
                300,
                160,
                32,
                38));

        var result = new OcrResult(
            targetText,
            [targetLine]);

        var targetMatch = new OcrTextMatch(
            targetText,
            targetLine.Bounds);

        var screenOcrService =
            new Mock<IAndroidScreenOcrService>(
                MockBehavior.Strict);

        var ocrTextMatcher =
            new Mock<IOcrTextMatcher>(
                MockBehavior.Strict);

        var clickService =
            new Mock<IClickService>(
                MockBehavior.Strict);

        screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUniqueMatch(
                    result,
                    targetText,
                    OcrTextMatchMode.NormalizedContains))
            .Returns(targetMatch);

        double expectedX =
            targetMatch.Bounds.Center.X;

        double expectedY =
            targetMatch.Bounds.Center.Y;

        clickService
            .Setup(service =>
                service.ClickAbsolute(
                    expectedX,
                    expectedY));

        var sut = new OcrClickService(
            screenOcrService.Object,
            ocrTextMatcher.Object,
            clickService.Object);

        // Act
        Action act = () =>
            sut.ClickTextAsync(
                targetText,
                TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();

        // Assert
        act.Should().NotThrow();

        ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUniqueMatch(
                    result,
                    targetText,
                    OcrTextMatchMode.NormalizedContains),
            Times.Once);

        clickService.Verify(
            service =>
                service.ClickAbsolute(
                    expectedX,
                    expectedY),
            Times.Once);

        screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);

        screenOcrService.VerifyNoOtherCalls();
        ocrTextMatcher.VerifyNoOtherCalls();
        clickService.VerifyNoOtherCalls();
    }


    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void ClickTextAsync_WhenTargetTextIsEmptyOrWhiteSpace_ShouldThrowArgumentException(
        string targetText)
    {
        Action act = () =>
            _sut.ClickTextAsync(
                targetText,
                TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*targetText*");

        _screenOcrServiceMock.VerifyNoOtherCalls();
        _ocrTextMatcherMock.VerifyNoOtherCalls();
        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ClickTextAsync_WhenTargetTextIsNull_ShouldThrowArgumentNullException()
    {
        Action act = () =>
            _sut.ClickTextAsync(
                null!,
                TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*targetText*");

        _screenOcrServiceMock.VerifyNoOtherCalls();
        _ocrTextMatcherMock.VerifyNoOtherCalls();
        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ClickTextAsync_WhenTimeoutIsNotPositive_ShouldThrowArgumentOutOfRangeException(
        double timeoutSeconds)
    {
        Action act = () =>
            _sut.ClickTextAsync(
                "VIP 9",
                TimeSpan.FromSeconds(timeoutSeconds)).GetAwaiter().GetResult();

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*OCR click timeout must be greater than zero.*");

        _screenOcrServiceMock.VerifyNoOtherCalls();
        _ocrTextMatcherMock.VerifyNoOtherCalls();
        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ClickTextAsync_WhenRecognitionFails_ShouldPropagateExceptionWithoutClicking()
    {
        var expectedException =
            new InvalidOperationException(
                "Windows OCR engine initialization failed.");

        _screenOcrServiceMock
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        Action act = () =>
            _sut.ClickTextAsync(
                "VIP 9",
                TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*Windows OCR engine initialization failed.*");

        _screenOcrServiceMock.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);

        _ocrTextMatcherMock.VerifyNoOtherCalls();
        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ClickTextAsync_WhenMatcherThrowsUnexpectedInvalidOperationException_ShouldPropagateExceptionWithoutClicking()
    {
        // Arrange
        const string targetText = "VIP 9";
        const string expectedMessage =
            "OCR target 'VIP 9' is ambiguous: multiple matching lines were found.";

        var targetLine = new OcrTextLine(
            targetText,
            Rectangle.FromXYWH(
                300,
                160,
                32,
                38));

        var result = new OcrResult(
            targetText,
            [targetLine]);

        var screenOcrService =
            new Mock<IAndroidScreenOcrService>(
                MockBehavior.Strict);

        var ocrTextMatcher =
            new Mock<IOcrTextMatcher>(
                MockBehavior.Strict);

        var clickService =
            new Mock<IClickService>(
                MockBehavior.Strict);

        screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUniqueMatch(
                    result,
                    targetText,
                    OcrTextMatchMode.NormalizedContains))
            .Throws(
                new InvalidOperationException(
                    expectedMessage));

        var sut = new OcrClickService(
            screenOcrService.Object,
            ocrTextMatcher.Object,
            clickService.Object);

        // Act
        Action act = () =>
            sut.ClickTextAsync(
                targetText,
                TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedMessage);

        ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUniqueMatch(
                    result,
                    targetText,
                    OcrTextMatchMode.NormalizedContains),
            Times.Once);

        clickService.Verify(
            service =>
                service.ClickAbsolute(
                    It.IsAny<double>(),
                    It.IsAny<double>()),
            Times.Never);

        screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);

        screenOcrService.VerifyNoOtherCalls();
        ocrTextMatcher.VerifyNoOtherCalls();
        clickService.VerifyNoOtherCalls();
    }


    [Fact]
    public void ClickTextAsync_WhenCancellationIsRequested_ShouldNotClick()
    {
        using var cancellationTokenSource =
            new CancellationTokenSource();

        cancellationTokenSource.Cancel();

        Action act = () =>
            _sut.ClickTextAsync(
                "VIP 9",
                TimeSpan.FromSeconds(5),
                cancellationTokenSource.Token).GetAwaiter().GetResult();

        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage("*canceled*");

        _screenOcrServiceMock.VerifyNoOtherCalls();
        _ocrTextMatcherMock.VerifyNoOtherCalls();
        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ClickTextAsync_WhenMatchedCoordinateIsNegative_ShouldThrowWithoutClicking()
    {
        // Arrange
        const string targetText = "VIP 9";

        var targetLine = new OcrTextLine(
            targetText,
            Rectangle.FromXYWH(
                300,
                160,
                32,
                38));

        var result = new OcrResult(
            targetText,
            [targetLine]);

        var targetMatch = new OcrTextMatch(
            targetText,
            Rectangle.FromXYWH(
                -10,
                20,
                5,
                5));

        var screenOcrService =
            new Mock<IAndroidScreenOcrService>(
                MockBehavior.Strict);

        var ocrTextMatcher =
            new Mock<IOcrTextMatcher>(
                MockBehavior.Strict);

        var clickService =
            new Mock<IClickService>(
                MockBehavior.Strict);

        screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUniqueMatch(
                    result,
                    targetText,
                    OcrTextMatchMode.NormalizedContains))
            .Returns(targetMatch);

        var sut = new OcrClickService(
            screenOcrService.Object,
            ocrTextMatcher.Object,
            clickService.Object);

        // Act
        Action act = () =>
            sut.ClickTextAsync(
                targetText,
                TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                $"OCR target '{targetText}' produced an invalid click coordinate (-7.5, 22.5).");


        ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUniqueMatch(
                    result,
                    targetText,
                    OcrTextMatchMode.NormalizedContains),
            Times.Once);

        clickService.Verify(
            service =>
                service.ClickAbsolute(
                    It.IsAny<double>(),
                    It.IsAny<double>()),
            Times.Never);

        screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);

        screenOcrService.VerifyNoOtherCalls();
        ocrTextMatcher.VerifyNoOtherCalls();
        clickService.VerifyNoOtherCalls();
    }

    [Fact]
    public void ClickTextAsync_TargetContainedInRecognizedLine_ShouldUseNormalizedContains()
    {
        // Arrange
        const string targetText = "任務";
        const string recognizedText =
            "一 一 ∕ 丶 任 務";

        var targetLine = new OcrTextLine(
            recognizedText,
            Rectangle.FromXYWH(
                400,
                900,
                250,
                54));

        var result = new OcrResult(
            recognizedText,
            [targetLine]);

        var targetMatch = new OcrTextMatch(
            targetText,
            Rectangle.FromXYWH(
                530,
                910,
                90,
                32));

        var screenOcrService =
            new Mock<IAndroidScreenOcrService>(
                MockBehavior.Strict);

        var ocrTextMatcher =
            new Mock<IOcrTextMatcher>(
                MockBehavior.Strict);

        var clickService =
            new Mock<IClickService>(
                MockBehavior.Strict);

        screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUniqueMatch(
                    result,
                    targetText,
                    OcrTextMatchMode.NormalizedContains))
            .Returns(targetMatch);

        double expectedX =
            targetMatch.Bounds.Center.X;

        double expectedY =
            targetMatch.Bounds.Center.Y;

        clickService
            .Setup(service =>
                service.ClickAbsolute(
                    expectedX,
                    expectedY));

        var sut = new OcrClickService(
            screenOcrService.Object,
            ocrTextMatcher.Object,
            clickService.Object);

        // Act
        Action act = () =>
            sut.ClickTextAsync(
                targetText,
                TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();

        // Assert
        act.Should().NotThrow();

        ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUniqueMatch(
                    result,
                    targetText,
                    OcrTextMatchMode.NormalizedContains),
            Times.Once);

        clickService.Verify(
            service =>
                service.ClickAbsolute(
                    expectedX,
                    expectedY),
            Times.Once);

        screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);

        screenOcrService.VerifyNoOtherCalls();
        ocrTextMatcher.VerifyNoOtherCalls();
        clickService.VerifyNoOtherCalls();
    }


    [Fact]
    public void ClickTextAsync_WhenTargetIsPartOfLongOcrLine_ShouldNotClickLineCenter()
    {
        // Arrange
        const string targetText = "任務";

        var targetLine = new OcrTextLine(
            "掛 收 益 一 ∕ 丶 任 務 一 三 國 巔 峰 戰",
            Rectangle.FromXYWH(
                100,
                900,
                565,
                54),
            [
                new OcrTextWord(
                "掛",
                Rectangle.FromXYWH(100, 910, 40, 32)),
            new OcrTextWord(
                "收",
                Rectangle.FromXYWH(150, 910, 40, 32)),
            new OcrTextWord(
                "益",
                Rectangle.FromXYWH(200, 910, 40, 32)),
            new OcrTextWord(
                "任",
                Rectangle.FromXYWH(530, 910, 40, 32)),
            new OcrTextWord(
                "務",
                Rectangle.FromXYWH(580, 910, 40, 32)),
            new OcrTextWord(
                "三",
                Rectangle.FromXYWH(625, 910, 40, 32))
            ]);

        var result = new OcrResult(
            targetLine.Text,
            [targetLine]);

        var targetMatch = new OcrTextMatch(
            targetText,
            Rectangle.FromXYWH(
                530,
                910,
                90,
                32));

        var screenOcrService =
            new Mock<IAndroidScreenOcrService>(
                MockBehavior.Strict);

        var ocrTextMatcher =
            new Mock<IOcrTextMatcher>(
                MockBehavior.Strict);

        var clickService =
            new Mock<IClickService>(
                MockBehavior.Strict);

        screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUniqueMatch(
                    result,
                    targetText,
                    OcrTextMatchMode.NormalizedContains))
            .Returns(targetMatch);

        const double expectedX = 575;
        const double expectedY = 926;

        clickService
            .Setup(service =>
                service.ClickAbsolute(
                    expectedX,
                    expectedY));

        var sut = new OcrClickService(
            screenOcrService.Object,
            ocrTextMatcher.Object,
            clickService.Object);

        // Act
        Action act = () =>
            sut.ClickTextAsync(
                targetText,
                TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();

        // Assert
        act.Should().NotThrow();

        ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUniqueMatch(
                    result,
                    targetText,
                    OcrTextMatchMode.NormalizedContains),
            Times.Once);

        clickService.Verify(
            service =>
                service.ClickAbsolute(
                    expectedX,
                    expectedY),
            Times.Once);

        clickService.Verify(
            service =>
                service.ClickAbsolute(
                    targetLine.Bounds.Center.X,
                    targetLine.Bounds.Center.Y),
            Times.Never);

        screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);

        ocrTextMatcher.VerifyNoOtherCalls();
        clickService.VerifyNoOtherCalls();
        screenOcrService.VerifyNoOtherCalls();
    }


    private static OcrResult CreateOcrResult(
        string text)
    {
        return new OcrResult(
            text,
            Array.Empty<OcrTextLine>());
    }
}
