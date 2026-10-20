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

namespace OmniAppium.EngineUtilityServices.Tests.Services.OCR;

public sealed class Issue48OcrClickAuthorizationRegressionTests
{
    private const string Target = "登入遊戲";

    [Fact]
    public void ClickTextAsync_WhenAtomicEvidenceIsUnavailable_ShouldNeverClick()
    {
        var fixture = CreateFixture();
        var evidence = new Mock<IGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var authorization = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);
        var sut = CreateService(fixture, authorization.Object, evidence.Object);

        Action act = () => sut.ClickTextAsync(Target, TimeSpan.FromSeconds(1))
            .GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Atomic reviewed click evidence provider is required*");
        fixture.Click.Verify(x => x.ClickAbsolute(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        authorization.VerifyNoOtherCalls();
    }

    [Fact]
    public void ClickTextAsync_WhenAuthorizationRejects_ShouldNeverClick()
    {
        var fixture = CreateFixture();
        var evidence = new Mock<IAtomicGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var authorization = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);
        evidence.Setup(x => x.Consume(fixture.Result, Target, fixture.Match))
            .Returns(new ConsumedGroundedClickEvidence(null!, 0.1f));
        authorization.Setup(x => x.Authorize(
                It.IsAny<string>(), It.IsAny<float>(),
                It.IsAny<Rectangle>(), It.IsAny<ReviewedGroundTruth>()))
            .Throws(new InvalidOperationException("Reviewed evidence rejected"));
        var sut = CreateService(fixture, authorization.Object, evidence.Object);

        Action act = () => sut.ClickTextAsync(Target, TimeSpan.FromSeconds(1))
            .GetAwaiter().GetResult();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Reviewed evidence rejected*");
        fixture.Click.Verify(x => x.ClickAbsolute(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public void ClickTextAsync_WhenCancelledAfterRecognition_ShouldNotConsumeEvidenceOrClick()
    {
        using var cancellation = new CancellationTokenSource();
        var fixture = CreateFixture(cancellation);
        var evidence = new Mock<IAtomicGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var authorization = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);
        var sut = CreateService(fixture, authorization.Object, evidence.Object);

        Action act = () => sut.ClickTextAsync(Target, TimeSpan.FromSeconds(1), cancellation.Token)
            .GetAwaiter().GetResult();

        act.Should().Throw<OperationCanceledException>();
        evidence.VerifyNoOtherCalls();
        authorization.VerifyNoOtherCalls();
        fixture.Click.Verify(x => x.ClickAbsolute(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
    }

    private static Fixture CreateFixture(CancellationTokenSource? cancellation = null)
    {
        var result = new OcrResult(Target, [
            new OcrTextLine(Target, Rectangle.FromXYWH(100, 200, 80, 40))
        ]);
        var match = new OcrTextMatch(Target, Rectangle.FromXYWH(100, 200, 80, 40));
        var screen = new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);
        var matcher = new Mock<IOcrTextMatcher>(MockBehavior.Strict);
        var click = new Mock<IClickService>(MockBehavior.Strict);
        screen.Setup(x => x.RecognizeCurrentScreenAsync(It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                cancellation?.Cancel();
                return Task.FromResult(result);
            });
        matcher.Setup(x => x.FindUniqueMatch(result, Target, OcrTextMatchMode.NormalizedContains))
            .Returns(match);
        return new Fixture(result, match, screen, matcher, click);
    }

    private static OcrClickService CreateService(
        Fixture fixture,
        IGroundedClickAuthorizationService authorization,
        IGroundedClickEvidenceProvider evidence)
    {
        var logger = new Mock<ILoggerFactoryBaseUtilityService>(MockBehavior.Strict);
        logger.SetupGet(x => x.Logger).Returns(NullLogger.Instance);
        return new OcrClickService(
            fixture.Screen.Object, fixture.Matcher.Object, fixture.Click.Object,
            logger.Object, authorization, evidence);
    }

    private sealed record Fixture(
        OcrResult Result,
        OcrTextMatch Match,
        Mock<IAndroidScreenOcrService> Screen,
        Mock<IOcrTextMatcher> Matcher,
        Mock<IClickService> Click);
}
