using CoordinateUtilityServices;
using FluentAssertions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.OCR;

public sealed class Issue48OcrClickRetryTimeoutBoundaryTests
{
    private const string Target = "登入遊戲";

    [Fact]
    public async Task ClickTextAsync_WhenRetryFollowsAuthorizationFailure_ShouldNotReuseConsumedEvidence()
    {
        OcrResult observation = CreateObservation();
        OcrTextMatch match = CreateMatch();
        var screen = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var matcher = new Mock<IOcrTextMatcher>(MockBehavior.Strict);
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var evidence = new Mock<IAtomicGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var authorization = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);

        screen.Setup(x => x.RecognizeCurrentScreenAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(observation);
        matcher.Setup(x => x.FindUniqueMatch(
                observation, Target, OcrTextMatchMode.NormalizedContains))
            .Returns(match);

        int consumptionAttempts = 0;
        evidence.Setup(x => x.Consume(observation, Target, match))
            .Returns(() =>
            {
                if (Interlocked.Increment(ref consumptionAttempts) != 1)
                {
                    throw new InvalidOperationException(
                        "Reviewed click evidence has already been consumed.");
                }

                return new ConsumedGroundedClickEvidence(null!, 1.0f);
            });

        authorization.Setup(x => x.Authorize(
                match.Text, 1.0f, match.Bounds, It.IsAny<ReviewedGroundTruth>()))
            .Throws(new InvalidOperationException("Reviewed evidence rejected"));

        OcrClickService sut = CreateService(
            screen, matcher, click, authorization, evidence);

        Func<Task> firstAttempt = () => sut.ClickTextAsync(
            Target, TimeSpan.FromSeconds(1), TestContext.Current.CancellationToken);
        await firstAttempt.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Reviewed evidence rejected*");

        Func<Task> secondAttempt = () => sut.ClickTextAsync(
            Target, TimeSpan.FromSeconds(1), TestContext.Current.CancellationToken);
        await secondAttempt.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Reviewed click evidence has already been consumed*");

        consumptionAttempts.Should().Be(2);
        screen.Verify(x => x.RecognizeCurrentScreenAsync(
            It.IsAny<CancellationToken>()), Times.Exactly(2));
        authorization.Verify(x => x.Authorize(
            match.Text, 1.0f, match.Bounds, It.IsAny<ReviewedGroundTruth>()),
            Times.Once);
        click.Verify(x => x.ClickAbsolute(
            It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        click.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ClickTextAsync_WhenOcrExceedsTimeout_ShouldNeverConsumeEvidenceOrClick()
    {
        var screen = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var matcher = new Mock<IOcrTextMatcher>(MockBehavior.Strict);
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var evidence = new Mock<IAtomicGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var authorization = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);

        screen.Setup(x => x.RecognizeCurrentScreenAsync(It.IsAny<CancellationToken>()))
            .Returns((CancellationToken token) => WaitForCancellationAsync(token));

        OcrClickService sut = CreateService(
            screen, matcher, click, authorization, evidence);

        Func<Task> act = () => sut.ClickTextAsync(
            Target, TimeSpan.FromMilliseconds(100),
            TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<TimeoutException>()
            .WithMessage($"*OCR target '{Target}' could not be safely located*");

        screen.Verify(x => x.RecognizeCurrentScreenAsync(
            It.IsAny<CancellationToken>()), Times.Once);
        matcher.VerifyNoOtherCalls();
        evidence.VerifyNoOtherCalls();
        authorization.VerifyNoOtherCalls();
        click.VerifyNoOtherCalls();
    }

    private static async Task<OcrResult> WaitForCancellationAsync(
        CancellationToken cancellationToken)
    {
        await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
        throw new InvalidOperationException(
            "OCR recognition unexpectedly completed without cancellation.");
    }

    private static OcrResult CreateObservation() =>
        new(Target, [new OcrTextLine(
            Target, Rectangle.FromXYWH(100, 200, 80, 40))]);

    private static OcrTextMatch CreateMatch() =>
        new(Target, Rectangle.FromXYWH(100, 200, 80, 40));

    private static OcrClickService CreateService(
        Mock<IAndroidScreenOcrService> screen,
        Mock<IOcrTextMatcher> matcher,
        Mock<IClickService> click,
        Mock<IGroundedClickAuthorizationService> authorization,
        Mock<IAtomicGroundedClickEvidenceProvider> evidence)
    {
        var loggerFactory = new Mock<ILoggerFactoryBaseUtilityService>(
            MockBehavior.Strict);
        loggerFactory.SetupGet(x => x.Logger).Returns(NullLogger.Instance);

        return new OcrClickService(
            screen.Object,
            matcher.Object,
            click.Object,
            loggerFactory.Object,
            authorization.Object,
            evidence.Object);
    }
}
