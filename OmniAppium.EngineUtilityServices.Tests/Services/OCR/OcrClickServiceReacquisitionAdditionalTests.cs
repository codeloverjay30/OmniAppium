using CoordinateUtilityServices;
using FluentAssertions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityServices.Services.OCR;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.OCR;

public sealed class OcrClickServiceReacquisitionAdditionalTests
{
    private const string Target = "登入遊戲";

    [Fact]
    public async Task ClickTextAsync_CallerAlreadyCanceled_ShouldPropagateCancellationWithoutOcrOrClick()
    {
        var screen = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var click = new Mock<IClickService>(MockBehavior.Strict);
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        OcrClickService sut = CreateService(screen, click);

        Func<Task> act = () => sut.ClickTextAsync(
            Target,
            TimeSpan.FromSeconds(5),
            cancellationSource.Token);

        await act.Should()
            .ThrowAsync<OperationCanceledException>();

        screen.Verify(
            service => service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
        click.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ClickTextAsync_ExactNormalizedLineWithoutWords_ShouldClickLineCenter()
    {
        var screen = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var click = new Mock<IClickService>(MockBehavior.Strict);

        OcrResult result = CreateResult(
            "登 入 遊 戲",
            Rectangle.FromXYWH(100, 200, 80, 40));

        screen.Setup(service => service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        click.Setup(service => service.ClickAbsolute(140, 220));

        OcrClickService sut = CreateService(screen, click);

        Func<Task> act = () => sut.ClickTextAsync(
            Target,
            TimeSpan.FromSeconds(5),
            TestContext.Current.CancellationToken);

        await act.Should().NotThrowAsync();

        screen.Verify(
            service => service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
        click.Verify(service => service.ClickAbsolute(140, 220), Times.Once);
        click.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ClickTextAsync_SubstringWithoutWordGeometry_ShouldTimeoutWithoutClick()
    {
        var screen = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var click = new Mock<IClickService>(MockBehavior.Strict);

        OcrResult result = CreateResult(
            "點擊登入遊戲開始",
            Rectangle.FromXYWH(100, 200, 160, 40));

        screen.Setup(service => service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        OcrClickService sut = CreateService(screen, click);

        Func<Task> act = () => sut.ClickTextAsync(
            Target,
            TimeSpan.FromMilliseconds(150),
            TestContext.Current.CancellationToken);

        await act.Should()
            .ThrowAsync<TimeoutException>()
            .WithMessage(
                $"*OCR target '{Target}' could not be safely located*");

        screen.Verify(
            service => service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
        click.VerifyNoOtherCalls();
    }

    private static OcrClickService CreateService(
        Mock<IAndroidScreenOcrService> screen,
        Mock<IClickService> click)
    {
        var loggerFactory =
            new Mock<ILoggerFactoryBaseUtilityService>(MockBehavior.Strict);

        loggerFactory.SetupGet(factory => factory.Logger)
            .Returns(NullLogger.Instance);

        return new OcrClickService(
            screen.Object,
            new OcrTextMatcher(),
            click.Object,
            loggerFactory.Object);
    }

    private static OcrResult CreateResult(
        string lineText,
        Rectangle bounds)
    {
        return new OcrResult(
            lineText,
            [new OcrTextLine(lineText, bounds)]);
    }
}
