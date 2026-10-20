using OmniAppium.EngineUtilityServices.Tests;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using CoordinateUtilityServices;
using FluentAssertions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.OCR;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.OCR;

public sealed class OcrClickServiceReacquisitionTests
{
    private const string Target = "登入遊戲";

    [Fact]
    public async Task ClickTextAsync_NotFoundThenFound_ShouldClickFreshMatchOnly()
    {
        var screen = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var matcher = new Mock<IOcrTextMatcher>(MockBehavior.Strict);
        var click = new Mock<IClickService>(MockBehavior.Strict);

        OcrResult first = CreateResult("其他文字");
        OcrResult second = CreateResult(Target);

        screen.SetupSequence(
                s => s.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(first)
            .ReturnsAsync(second);

        matcher.Setup(m => m.FindUniqueMatch(
                first, Target, OcrTextMatchMode.NormalizedContains))
            .Throws(new InvalidOperationException(
                $"OCR target '{Target}' was not found."));

        matcher.Setup(m => m.FindUniqueMatch(
                second, Target, OcrTextMatchMode.NormalizedContains))
            .Returns(new OcrTextMatch(
                Target,
                Rectangle.FromXYWH(100, 200, 80, 40)));

        click.Setup(c => c.ClickAbsolute(140, 220));

        var sut = CreateService(screen, matcher, click);

        Func<Task> act = () => sut.ClickTextAsync(
            Target, TimeSpan.FromSeconds(5));

        await act.Should().NotThrowAsync();

        screen.Verify(
            s => s.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));

        click.Verify(c => c.ClickAbsolute(140, 220), Times.Once);
        click.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ClickTextAsync_PersistentNotFound_ShouldTimeoutWithoutClick()
    {
        var screen = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var matcher = new Mock<IOcrTextMatcher>(MockBehavior.Strict);
        var click = new Mock<IClickService>(MockBehavior.Strict);

        OcrResult result = CreateResult("其他文字");

        screen.Setup(s => s.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        matcher.Setup(m => m.FindUniqueMatch(
                result, Target, OcrTextMatchMode.NormalizedContains))
            .Throws(new InvalidOperationException(
                $"OCR target '{Target}' was not found."));

        var sut = CreateService(screen, matcher, click);

        Func<Task> act = () => sut.ClickTextAsync(
            Target, TimeSpan.FromMilliseconds(150));

        await act.Should()
            .ThrowAsync<TimeoutException>()
            .WithMessage($"*OCR target '{Target}' could not be safely located*");

        click.Verify(
            c => c.ClickAbsolute(
                It.IsAny<double>(), It.IsAny<double>()),
            Times.Never);
    }

    [Fact]
    public async Task ClickTextAsync_AmbiguousTarget_ShouldFailClosed()
    {
        var screen = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var matcher = new Mock<IOcrTextMatcher>(MockBehavior.Strict);
        var click = new Mock<IClickService>(MockBehavior.Strict);

        OcrResult result = CreateResult(Target);

        screen.Setup(s => s.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        matcher.Setup(m => m.FindUniqueMatch(
                result, Target, OcrTextMatchMode.NormalizedContains))
            .Throws(new InvalidOperationException(
                $"OCR target '{Target}' is ambiguous: multiple matching lines were found."));

        var sut = CreateService(screen, matcher, click);

        Func<Task> act = () => sut.ClickTextAsync(
            Target, TimeSpan.FromSeconds(5));

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"*OCR target '{Target}' is ambiguous*");

        screen.Verify(
            s => s.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);

        click.Verify(
            c => c.ClickAbsolute(
                It.IsAny<double>(), It.IsAny<double>()),
            Times.Never);
    }

    private static OcrClickService CreateService(
        Mock<IAndroidScreenOcrService> screen,
        Mock<IOcrTextMatcher> matcher,
        Mock<IClickService> click)
    {
        var logger =
            new Mock<ILoggerFactoryBaseUtilityService>(MockBehavior.Strict);

        logger.SetupGet(x => x.Logger)
            .Returns(NullLogger.Instance);

        return new OcrClickService(
            screen.Object,
            matcher.Object,
            click.Object,
            logger.Object,
                GroundedClickTestDoubles.Authorization(),
                GroundedClickTestDoubles.Evidence());
    }

    private static OcrResult CreateResult(string text)
    {
        return new OcrResult(
            text,
            [
                new OcrTextLine(
                    text,
                    Rectangle.FromXYWH(0, 0, 100, 40))
            ]);
    }
}