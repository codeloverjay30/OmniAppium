using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.OCR;

namespace OmniAppium.EngineUtilityServices.Tests.Services.OCR;

public sealed class OcrPageVerificationServiceTests
{
    private readonly Mock<IAndroidScreenOcrService> _screenOcrService;
    private readonly Mock<IOcrTextMatcher> _ocrTextMatcher;

    public OcrPageVerificationServiceTests()
    {
        _screenOcrService = new Mock<IAndroidScreenOcrService>(
            MockBehavior.Strict);

        _ocrTextMatcher = new Mock<IOcrTextMatcher>(
            MockBehavior.Strict);
    }

    [Fact]
    public void WaitForTextAsync_WhenExpectedTextIsFoundImmediately_ShouldComplete()
    {
        // Arrange
        const string expectedText = "日常";

        OcrResult result = CreateOcrResult(expectedText);
        OcrTextLine matchedLine = CreateTextLine(expectedText);

        _screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        _ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    result,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains))
            .Returns(matchedLine);

        OcrPageVerificationService sut = CreateSut();

        // Act
        Action act = () =>
            sut.WaitForTextAsync(
                expectedText,
                TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

        // Assert
        act.Should().NotThrow();

        _screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);

        _ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUnique(
                    result,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains),
            Times.Once);
    }

    [Fact]
    public void WaitForTextAsync_WhenExpectedTextIsInitiallyMissing_ShouldRetryUntilFound()
    {
        // Arrange
        const string expectedText = "日常";

        Mock<IAndroidScreenOcrService> screenOcrService =
            new(MockBehavior.Strict);

        Mock<IOcrTextMatcher> ocrTextMatcher =
            new(MockBehavior.Strict);

        OcrResult firstResult =
            CreateOcrResult("其他文字");

        OcrResult secondResult =
            CreateOcrResult(expectedText);

        OcrTextLine matchedLine =
            CreateTextLine(expectedText);

        screenOcrService
            .SetupSequence(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(firstResult)
            .ReturnsAsync(secondResult);

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    firstResult,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains))
            .Throws(
                new InvalidOperationException(
                    $"OCR target '{expectedText}' was not found."));

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    secondResult,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains))
            .Returns(matchedLine);

        OcrPageVerificationService sut =
            new(
                screenOcrService.Object,
                ocrTextMatcher.Object,
                TimeProvider.System
            );

        // Act
        Action act = () =>
            sut.WaitForTextAsync(
                expectedText,
                TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

        // Assert
        act.Should().NotThrow();

        screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Exactly(2));

        ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUnique(
                    firstResult,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains),
            Times.Once);

        ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUnique(
                    secondResult,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains),
            Times.Once);

        screenOcrService.VerifyNoOtherCalls();
        ocrTextMatcher.VerifyNoOtherCalls();
    }

    [Fact]
    public void WaitForTextAsync_WhenExpectedTextRemainsMissing_ShouldThrowTimeoutException()
    {
        // Arrange
        const string expectedText = "日常";

        TimeSpan timeout =
            TimeSpan.FromMilliseconds(100);

        Mock<IAndroidScreenOcrService> screenOcrService =
            new(MockBehavior.Strict);

        Mock<IOcrTextMatcher> ocrTextMatcher =
            new(MockBehavior.Strict);

        OcrResult result =
            CreateOcrResult("其他文字");

        screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    result,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains))
            .Throws(
                new InvalidOperationException(
                    $"OCR target '{expectedText}' was not found."));

        OcrPageVerificationService sut =
            new(
                screenOcrService.Object,
                ocrTextMatcher.Object,
                TimeProvider.System
            );

        // Act
        Action act = () =>
            sut.WaitForTextAsync(
                expectedText,
                timeout).GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<TimeoutException>()
            .WithMessage(
                $"OCR target '{expectedText}' was not found within {timeout}.");

        screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);

        ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUnique(
                    result,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains),
            Times.Once);

        screenOcrService.VerifyNoOtherCalls();
        ocrTextMatcher.VerifyNoOtherCalls();
    }


    [Fact]
    public async Task WaitForTextAsync_WhenCallerCancels_ShouldPropagateOperationCanceledException()
    {
        // Arrange
        const string expectedText = "日常";

        using CancellationTokenSource cancellationSource = new();

        _screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .Returns<CancellationToken>(
                async cancellationToken =>
                {
                    cancellationSource.Cancel();

                    await Task.Delay(
                        Timeout.InfiniteTimeSpan,
                        cancellationToken);

                    return CreateOcrResult("unreachable");
                });

        OcrPageVerificationService sut = CreateSut();

        // Act
        Action act = () =>
            sut.WaitForTextAsync(
                expectedText,
                TimeSpan.FromSeconds(5),
                cancellationSource.Token).GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage("*canceled*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void WaitForTextAsync_WhenExpectedTextIsEmptyOrWhiteSpace_ShouldThrowArgumentException(
        string expectedText)
    {
        // Arrange
        OcrPageVerificationService sut = CreateSut();

        // Act
        Action act = () =>
            sut.WaitForTextAsync(
                expectedText,
                TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*expectedText*");

        _screenOcrService.VerifyNoOtherCalls();
        _ocrTextMatcher.VerifyNoOtherCalls();
    }

    [Fact]
    public void WaitForTextAsync_WhenExpectedTextIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        OcrPageVerificationService sut = CreateSut();

        // Act
        Action act = () =>
            sut.WaitForTextAsync(
                null!,
                TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*expectedText*");

        _screenOcrService.VerifyNoOtherCalls();
        _ocrTextMatcher.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void WaitForTextAsync_WhenTimeoutIsNotPositive_ShouldThrowArgumentOutOfRangeException(
        int timeoutMilliseconds)
    {
        // Arrange
        OcrPageVerificationService sut = CreateSut();

        TimeSpan timeout =
            TimeSpan.FromMilliseconds(timeoutMilliseconds);

        // Act
        Action act = () =>
            sut.WaitForTextAsync(
                "日常",
                timeout).GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*greater than zero*");

        _screenOcrService.VerifyNoOtherCalls();
        _ocrTextMatcher.VerifyNoOtherCalls();
    }

    [Fact]
    public void WaitForTextAsync_WhenMatcherReportsAmbiguousText_ShouldPropagateException()
    {
        // Arrange
        const string expectedText = "日常";

        OcrResult result = CreateOcrResult(
            expectedText,
            expectedText);

        _screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        _ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    result,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains))
            .Throws(
                new InvalidOperationException(
                    $"OCR target '{expectedText}' is ambiguous: " +
                    "multiple matching lines were found."));

        OcrPageVerificationService sut = CreateSut();

        // Act
        Action act = () =>
            sut.WaitForTextAsync(
                expectedText,
                TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                $"*'{expectedText}'*ambiguous*");

        _screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void WaitForTextAsync_WhenScreenOcrFails_ShouldPropagateInfrastructureException()
    {
        // Arrange
        const string expectedText = "日常";

        _screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new InvalidOperationException(
                    "The Android screenshot contains no image data."));

        OcrPageVerificationService sut = CreateSut();

        // Act
        Action act = () =>
            sut.WaitForTextAsync(
                expectedText,
                TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "The Android screenshot contains no image data.");

        _ocrTextMatcher.VerifyNoOtherCalls();
    }

    [Fact]
    public void WaitForTextAsync_WhenExpectedTextIsFound_ShouldNotRequireWordGeometry()
    {
        // Arrange
        const string expectedText = "日常";

        OcrTextLine lineWithoutWords =
            CreateTextLine(expectedText);

        OcrResult result =
            new(
                expectedText,
                [lineWithoutWords]);

        _screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        _ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    result,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains))
            .Returns(lineWithoutWords);

        OcrPageVerificationService sut = CreateSut();

        // Act
        Action act = () =>
            sut.WaitForTextAsync(
                expectedText,
                TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

        // Assert
        act.Should().NotThrow();

        _ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUnique(
                    result,
                    expectedText,
                    OcrTextMatchMode.NormalizedContains),
            Times.Once);

        _ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUniqueMatch(
                    It.IsAny<OcrResult>(),
                    It.IsAny<string>(),
                    It.IsAny<OcrTextMatchMode>()),
            Times.Never);
    }

    [Fact]
    public void WaitForAllTextAsync_WhenAllExpectedTextsExistOnSameScreen_ShouldComplete()
    {
        // Arrange
        string[] expectedTexts =
        [
            "日常",
        "週常",
        "成就"
        ];

        Mock<IAndroidScreenOcrService> screenOcrService =
            new(MockBehavior.Strict);

        Mock<IOcrTextMatcher> ocrTextMatcher =
            new(MockBehavior.Strict);

        OcrResult result =
            CreateOcrResult(
                "日常",
                "週常",
                "成就");

        screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        foreach (string expectedText in expectedTexts)
        {
            ocrTextMatcher
                .Setup(matcher =>
                    matcher.FindUnique(
                        result,
                        expectedText,
                        OcrTextMatchMode.NormalizedContains))
                .Returns(CreateTextLine(expectedText));
        }

        OcrPageVerificationService sut =
            new(
                screenOcrService.Object,
                ocrTextMatcher.Object,
                TimeProvider.System
            );

        // Act
        Action act = () =>
            sut.WaitForAllTextAsync(
                expectedTexts,
                TimeSpan.FromSeconds(5),
                TestContext.Current.CancellationToken
            ).GetAwaiter().GetResult();

        // Assert
        act.Should().NotThrow();

        screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);

        foreach (string expectedText in expectedTexts)
        {
            ocrTextMatcher.Verify(
                matcher =>
                    matcher.FindUnique(
                        result,
                        expectedText,
                        OcrTextMatchMode.NormalizedContains),
                Times.Once);
        }

        screenOcrService.VerifyNoOtherCalls();
        ocrTextMatcher.VerifyNoOtherCalls();
    }

    [Fact]
public void WaitForAllTextAsync_WhenExpectedTextsIsNull_ShouldThrowArgumentNullException()
{
    // Arrange
    OcrPageVerificationService sut = CreateSut();

    IReadOnlyCollection<string> expectedTexts = null!;

    // Act
    Action act = () =>
            sut.WaitForAllTextAsync(
            expectedTexts,
            TimeSpan.FromSeconds(5),
            TestContext.Current.CancellationToken).GetAwaiter().GetResult();

    // Assert
    act.Should()
        .Throw<ArgumentNullException>()
        .WithMessage("*expectedTexts*");
}

[Fact]
public void WaitForAllTextAsync_WhenExpectedTextsIsEmpty_ShouldThrowArgumentException()
{
    // Arrange
    OcrPageVerificationService sut = CreateSut();

    IReadOnlyCollection<string> expectedTexts =
        Array.Empty<string>();

    // Act
    Action act = () =>
            sut.WaitForAllTextAsync(
            expectedTexts,
            TimeSpan.FromSeconds(5),
            TestContext.Current.CancellationToken).GetAwaiter().GetResult();

    // Assert
    act.Should()
        .Throw<ArgumentException>()
        .WithMessage(
            "*At least one OCR page verification text is required.*");
}

[Theory]
[InlineData("")]
[InlineData(" ")]
[InlineData("   ")]
public void WaitForAllTextAsync_WhenExpectedTextsContainsInvalidText_ShouldThrowArgumentException(
    string invalidText)
{
    // Arrange
    OcrPageVerificationService sut = CreateSut();

    IReadOnlyCollection<string> expectedTexts =
    [
        "日常",
        invalidText,
        "成就"
    ];

    // Act
    Action act = () =>
            sut.WaitForAllTextAsync(
            expectedTexts,
            TimeSpan.FromSeconds(5),
            TestContext.Current.CancellationToken).GetAwaiter().GetResult();

    // Assert
    act.Should()
        .Throw<ArgumentException>()
        .WithMessage(
            "*The value cannot be an empty string or composed entirely of whitespace.*");
}

[Fact]
public void WaitForAllTextAsync_WhenTimeoutIsNotPositive_ShouldThrowArgumentOutOfRangeException()
{
    // Arrange
    OcrPageVerificationService sut = CreateSut();

    IReadOnlyCollection<string> expectedTexts =
    [
        "日常",
        "週常",
        "成就"
    ];

    // Act
    Action act = () =>
            sut.WaitForAllTextAsync(
            expectedTexts,
            TimeSpan.Zero,
            TestContext.Current.CancellationToken).GetAwaiter().GetResult();

    // Assert
    act.Should()
        .Throw<ArgumentOutOfRangeException>()
        .WithMessage(
            "*OCR page verification timeout must be greater than zero.*");
}

[Fact]
public void WaitForAllTextAsync_WhenMatcherReportsAmbiguousText_ShouldPropagateException()
{
    // Arrange
    const string ambiguousText = "週常";

    IReadOnlyCollection<string> expectedTexts =
    [
        "日常",
        ambiguousText,
        "成就"
    ];

    OcrResult result =
        CreateOcrResult(
            "日常",
            "週常",
            "成就");

    Mock<IAndroidScreenOcrService> screenOcrService =
        new(MockBehavior.Strict);

    Mock<IOcrTextMatcher> ocrTextMatcher =
        new(MockBehavior.Strict);

    screenOcrService
        .Setup(service =>
            service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()))
        .ReturnsAsync(result);

    ocrTextMatcher
        .Setup(matcher =>
            matcher.FindUnique(
                result,
                "日常",
                OcrTextMatchMode.NormalizedContains))
        .Returns(CreateTextLine("日常"));

    ocrTextMatcher
        .Setup(matcher =>
            matcher.FindUnique(
                result,
                ambiguousText,
                OcrTextMatchMode.NormalizedContains))
        .Throws(
            new InvalidOperationException(
                $"OCR target '{ambiguousText}' is ambiguous: multiple matching lines were found."));

    OcrPageVerificationService sut =
        new(
            screenOcrService.Object,
            ocrTextMatcher.Object,
            TimeProvider.System
        );

    // Act
    Action act = () =>
            sut.WaitForAllTextAsync(
            expectedTexts,
            TimeSpan.FromSeconds(5),
            TestContext.Current.CancellationToken).GetAwaiter().GetResult();

    // Assert
    act.Should()
        .Throw<InvalidOperationException>()
        .WithMessage(
            $"OCR target '{ambiguousText}' is ambiguous: multiple matching lines were found.");

    screenOcrService.Verify(
        service =>
            service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
        Times.Once);

    ocrTextMatcher.Verify(
        matcher =>
            matcher.FindUnique(
                result,
                "日常",
                OcrTextMatchMode.NormalizedContains),
        Times.Once);

    ocrTextMatcher.Verify(
        matcher =>
            matcher.FindUnique(
                result,
                ambiguousText,
                OcrTextMatchMode.NormalizedContains),
        Times.Once);

    ocrTextMatcher.VerifyNoOtherCalls();
}

[Fact]
public void WaitForAllTextAsync_WhenScreenOcrFails_ShouldPropagateInfrastructureException()
{
    // Arrange
    IReadOnlyCollection<string> expectedTexts =
    [
        "日常",
        "週常",
        "成就"
    ];

    const string exceptionMessage =
        "Android screenshot capture failed.";

    Mock<IAndroidScreenOcrService> screenOcrService =
        new(MockBehavior.Strict);

    Mock<IOcrTextMatcher> ocrTextMatcher =
        new(MockBehavior.Strict);

    screenOcrService
        .Setup(service =>
            service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()))
        .ThrowsAsync(
            new InvalidOperationException(
                exceptionMessage));

    OcrPageVerificationService sut =
        new(
            screenOcrService.Object,
            ocrTextMatcher.Object,
            TimeProvider.System
        );

    // Act
    Action act = () =>
            sut.WaitForAllTextAsync(
            expectedTexts,
            TimeSpan.FromSeconds(5),
            TestContext.Current.CancellationToken).GetAwaiter().GetResult();

    // Assert
    act.Should()
        .Throw<InvalidOperationException>()
        .WithMessage(exceptionMessage);

    screenOcrService.Verify(
        service =>
            service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
        Times.Once);

    ocrTextMatcher.VerifyNoOtherCalls();
}

    [Fact]
    public void WaitForAllTextAsync_WhenCallerCancels_ShouldPropagateOperationCanceledException()
    {
        // Arrange
        IReadOnlyCollection<string> expectedTexts =
        [
            "日常",
        "週常",
        "成就"
        ];

        Mock<IAndroidScreenOcrService> screenOcrService =
            new(MockBehavior.Strict);

        Mock<IOcrTextMatcher> ocrTextMatcher =
            new(MockBehavior.Strict);

        using var cancellationSource =
            new CancellationTokenSource();

        cancellationSource.Cancel();

        OcrPageVerificationService sut =
            new(
                screenOcrService.Object,
                ocrTextMatcher.Object,
                TimeProvider.System
            );

        // Act
        Action act = () =>
            sut.WaitForAllTextAsync(
                expectedTexts,
                TimeSpan.FromSeconds(5),
                cancellationSource.Token).GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage("*canceled*");

        screenOcrService.VerifyNoOtherCalls();
        ocrTextMatcher.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task WaitForAllTextAsync_WhenExpectedTextsAreInitiallyMissing_ShouldRetryUntilAllTextsAreFound()
    {
        // Arrange
        string[] expectedTexts =
        [
            "日常",
            "週常",
            "成就"
        ];

        TimeSpan timeout =
            TimeSpan.FromSeconds(10);

        var fakeTimeProvider =
            new FakeTimeProvider();

        Mock<IAndroidScreenOcrService> screenOcrService =
            new(MockBehavior.Strict);

        Mock<IOcrTextMatcher> ocrTextMatcher =
            new(MockBehavior.Strict);

        OcrResult firstResult =
            CreateOcrResult(
                "日常",
                "週常");

        OcrResult secondResult =
            CreateOcrResult(
                "日常",
                "週常",
                "成就");

        screenOcrService
            .SetupSequence(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(firstResult)
            .ReturnsAsync(secondResult);

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    firstResult,
                    "日常",
                    OcrTextMatchMode.NormalizedContains))
            .Returns(CreateTextLine("日常"));

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    firstResult,
                    "週常",
                    OcrTextMatchMode.NormalizedContains))
            .Returns(CreateTextLine("週常"));

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    firstResult,
                    "成就",
                    OcrTextMatchMode.NormalizedContains))
            .Throws(
                new InvalidOperationException(
                    "OCR target '成就' was not found."));

        foreach (string expectedText in expectedTexts)
        {
            ocrTextMatcher
                .Setup(matcher =>
                    matcher.FindUnique(
                        secondResult,
                        expectedText,
                        OcrTextMatchMode.NormalizedContains))
                .Returns(CreateTextLine(expectedText));
        }

        OcrPageVerificationService sut =
            new(
                screenOcrService.Object,
                ocrTextMatcher.Object,
                fakeTimeProvider);

        // Act
        Task verificationTask =
            sut.WaitForAllTextAsync(
                expectedTexts,
                timeout,
                TestContext.Current.CancellationToken);

        await Task.Yield();

        verificationTask.IsCompleted
            .Should()
            .BeFalse();

        fakeTimeProvider.Advance(
            TimeSpan.FromSeconds(2));

        Action act = () =>
            verificationTask.GetAwaiter().GetResult();

        // Assert
        act.Should().NotThrow();

        screenOcrService.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Exactly(2));

        foreach (string expectedText in expectedTexts)
        {
            ocrTextMatcher.Verify(
                matcher =>
                    matcher.FindUnique(
                        firstResult,
                        expectedText,
                        OcrTextMatchMode.NormalizedContains),
                Times.Once);

            ocrTextMatcher.Verify(
                matcher =>
                    matcher.FindUnique(
                        secondResult,
                        expectedText,
                        OcrTextMatchMode.NormalizedContains),
                Times.Once);
        }

        screenOcrService.VerifyNoOtherCalls();
        ocrTextMatcher.VerifyNoOtherCalls();
    }


    [Fact]
    public async Task WaitForAllTextAsync_WhenExpectedTextsRemainMissing_ShouldThrowTimeoutException()
    {
        // Arrange
        string[] expectedTexts =
        [
            "日常",
        "週常",
        "成就"
        ];

        TimeSpan timeout =
            TimeSpan.FromSeconds(5);

        var fakeTimeProvider =
            new FakeTimeProvider();

        Mock<IAndroidScreenOcrService> screenOcrService =
            new(MockBehavior.Strict);

        Mock<IOcrTextMatcher> ocrTextMatcher =
            new(MockBehavior.Strict);

        OcrResult result =
            CreateOcrResult(
                "日常",
                "週常");

        screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    result,
                    "日常",
                    OcrTextMatchMode.NormalizedContains))
            .Returns(CreateTextLine("日常"));

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    result,
                    "週常",
                    OcrTextMatchMode.NormalizedContains))
            .Returns(CreateTextLine("週常"));

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    result,
                    "成就",
                    OcrTextMatchMode.NormalizedContains))
            .Throws(
                new InvalidOperationException(
                    "OCR target '成就' was not found."));

        OcrPageVerificationService sut =
            new(
                screenOcrService.Object,
                ocrTextMatcher.Object,
                fakeTimeProvider);

        // Act
        Task verificationTask =
            sut.WaitForAllTextAsync(
                expectedTexts,
                timeout,
                TestContext.Current.CancellationToken);

        await Task.Yield();

        fakeTimeProvider.Advance(timeout);

        Action act = () =>
            verificationTask.GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<TimeoutException>()
            .WithMessage(
                $"OCR page markers [日常, 週常, 成就] were not all found within {timeout}.");
    }

    [Fact]
    public async Task WaitForAllTextAsync_WhenExpectedTextsExistAcrossDifferentSnapshots_ShouldNotCombineSnapshots()
    {
        // Arrange
        string[] expectedTexts =
        [
            "日常",
            "週常",
            "成就"
        ];

        TimeSpan timeout =
            TimeSpan.FromSeconds(5);

        var fakeTimeProvider =
            new FakeTimeProvider();

        Mock<IAndroidScreenOcrService> screenOcrService =
            new(MockBehavior.Strict);

        Mock<IOcrTextMatcher> ocrTextMatcher =
            new(MockBehavior.Strict);

        OcrResult firstResult =
            CreateOcrResult(
                "日常",
                "週常");

        OcrResult secondResult =
            CreateOcrResult(
                "成就");

        int recognitionCount = 0;

        screenOcrService
            .Setup(service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                int currentCall =
                    Interlocked.Increment(
                        ref recognitionCount);

                return currentCall % 2 == 1
                    ? firstResult
                    : secondResult;
            });

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    firstResult,
                    "日常",
                    OcrTextMatchMode.NormalizedContains))
            .Returns(CreateTextLine("日常"));

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    firstResult,
                    "週常",
                    OcrTextMatchMode.NormalizedContains))
            .Returns(CreateTextLine("週常"));

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    firstResult,
                    "成就",
                    OcrTextMatchMode.NormalizedContains))
            .Throws(
                new InvalidOperationException(
                    "OCR target '成就' was not found."));

        ocrTextMatcher
            .Setup(matcher =>
                matcher.FindUnique(
                    secondResult,
                    "日常",
                    OcrTextMatchMode.NormalizedContains))
            .Throws(
                new InvalidOperationException(
                    "OCR target '日常' was not found."));

        OcrPageVerificationService sut =
            new(
                screenOcrService.Object,
                ocrTextMatcher.Object,
                fakeTimeProvider);

        // Act
        Task verificationTask =
            sut.WaitForAllTextAsync(
                expectedTexts,
                timeout,
                TestContext.Current.CancellationToken);

        await Task.Yield();

        verificationTask.IsCompleted
            .Should()
            .BeFalse(
                "no single OCR snapshot contains all expected markers");

        fakeTimeProvider.Advance(timeout);

        Action act = () =>
            verificationTask.GetAwaiter().GetResult();

        // Assert
        act.Should()
            .Throw<TimeoutException>()
            .WithMessage(
                $"OCR page markers [日常, 週常, 成就] were not all found within {timeout}.");

        recognitionCount
            .Should()
            .BeGreaterThanOrEqualTo(
                1,
                "page verification must inspect at least one OCR snapshot");

        ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUnique(
                    firstResult,
                    "成就",
                    OcrTextMatchMode.NormalizedContains),
            Times.AtLeastOnce);

        ocrTextMatcher.Verify(
            matcher =>
                matcher.FindUnique(
                    secondResult,
                    "日常",
                    OcrTextMatchMode.NormalizedContains),
            Times.AtMostOnce);
    }



    private OcrPageVerificationService CreateSut()
    {
        return new OcrPageVerificationService(
            _screenOcrService.Object,
            _ocrTextMatcher.Object,
            TimeProvider.System
        );
    }

    private static OcrResult CreateOcrResult(
        params string[] texts)
    {
        OcrTextLine[] lines =
            texts
                .Select(CreateTextLine)
                .ToArray();

        return new OcrResult(
            string.Join(Environment.NewLine, texts),
            lines);
    }

    private static OcrTextLine CreateTextLine(
        string text)
    {
        return new OcrTextLine(
            text,
            CoordinateUtilityServices.Rectangle.FromXYWH(
                100,
                100,
                200,
                50));
    }
}
