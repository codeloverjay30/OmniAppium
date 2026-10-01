using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Workflows;

namespace OmniAppium.EngineUtilityServices.Tests.Workflows;

public sealed class GameWorkflowStepExecutorTests
{
    private readonly Mock<IOcrClickService> _ocrClickServiceMock =
        new(MockBehavior.Strict);

    private readonly Mock<IOcrPageVerificationService>
        _ocrPageVerificationServiceMock =
            new(MockBehavior.Strict);

    [Fact]
    public async Task ExecuteAsync_TaskStep_ShouldClickTaskThenVerifyPageMarkers()
    {
        var step = new GameWorkflowStep(
            "任務",
            ["日常", "週常", "成就"],
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(10));

        using var cancellationTokenSource =
            new CancellationTokenSource();

        CancellationToken cancellationToken =
            cancellationTokenSource.Token;

        var sequence = new MockSequence();

        _ocrClickServiceMock
            .InSequence(sequence)
            .Setup(service => service.ClickTextAsync(
                "任務",
                TimeSpan.FromSeconds(10),
                cancellationToken))
            .Returns(Task.CompletedTask);

        _ocrPageVerificationServiceMock
            .InSequence(sequence)
            .Setup(service => service.WaitForAllTextAsync(
                It.Is<IReadOnlyCollection<string>>(
                    texts =>
                        texts.SequenceEqual(
                            new[] { "日常", "週常", "成就" })),
                TimeSpan.FromSeconds(10),
                cancellationToken))
            .Returns(Task.CompletedTask);

        var sut = new GameWorkflowStepExecutor(
            _ocrClickServiceMock.Object,
            _ocrPageVerificationServiceMock.Object);

        Func<Task> act = async () =>
            await sut.ExecuteAsync(
                step,
                cancellationToken);

        await act.Should().NotThrowAsync();

        _ocrClickServiceMock.VerifyAll();
        _ocrPageVerificationServiceMock.VerifyAll();
    }

    [Fact]
    public async Task ExecuteAsync_WhenClickFails_ShouldNotVerifyPage()
    {
        var step = new GameWorkflowStep(
            "任務",
            ["日常", "週常", "成就"],
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(10));

        var expectedException =
            new TimeoutException(
                "OCR target '任務' was not found within 00:00:10.");

        _ocrClickServiceMock
            .Setup(service => service.ClickTextAsync(
                "任務",
                TimeSpan.FromSeconds(10),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var sut = new GameWorkflowStepExecutor(
            _ocrClickServiceMock.Object,
            _ocrPageVerificationServiceMock.Object);

        Func<Task> act = async () =>
            await sut.ExecuteAsync(step);

        await act.Should()
            .ThrowAsync<TimeoutException>()
            .WithMessage(
                "OCR target '任務' was not found within 00:00:10.");

        _ocrPageVerificationServiceMock.Verify(
            service => service.WaitForAllTextAsync(
                It.IsAny<IReadOnlyCollection<string>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenVerificationFails_ShouldPropagateFailure()
    {
        var step = new GameWorkflowStep(
            "任務",
            ["日常", "週常", "成就"],
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(10));

        _ocrClickServiceMock
            .Setup(service => service.ClickTextAsync(
                "任務",
                TimeSpan.FromSeconds(10),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _ocrPageVerificationServiceMock
            .Setup(service => service.WaitForAllTextAsync(
                It.IsAny<IReadOnlyCollection<string>>(),
                TimeSpan.FromSeconds(10),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new TimeoutException(
                    "OCR page markers [日常, 週常, 成就] " +
                    "were not all found within 00:00:10."));

        var sut = new GameWorkflowStepExecutor(
            _ocrClickServiceMock.Object,
            _ocrPageVerificationServiceMock.Object);

        Func<Task> act = async () =>
            await sut.ExecuteAsync(step);

        await act.Should()
            .ThrowAsync<TimeoutException>()
            .WithMessage(
                "OCR page markers [日常, 週常, 成就] " +
                "were not all found within 00:00:10.");
    }

    [Fact]
public async Task ExecuteAsync_WhenStepIsNull_ShouldThrowArgumentNullException()
{
    var sut = new GameWorkflowStepExecutor(
        _ocrClickServiceMock.Object,
        _ocrPageVerificationServiceMock.Object);

    Func<Task> act = async () =>
        await sut.ExecuteAsync(null!);

    await act.Should()
        .ThrowAsync<ArgumentNullException>()
        .WithMessage("*step*");
}

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ExecuteAsync_WhenClickTextIsInvalid_ShouldThrowArgumentException(
        string clickText)
    {
        var step = new GameWorkflowStep(
            clickText,
            ["日常", "週常", "成就"],
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(10));

        var sut = new GameWorkflowStepExecutor(
            _ocrClickServiceMock.Object,
            _ocrPageVerificationServiceMock.Object);

        Func<Task> act = async () =>
            await sut.ExecuteAsync(step);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("*ClickText*");
    }
}