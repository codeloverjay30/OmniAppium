using AiUtility.ToolKits.Abstractions;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Planner;

public sealed class OcrGroundedPlannerClickToolTests
{
    private readonly Mock<
        IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>>
        _executionStateAccessorMock;

    private readonly Mock<IOcrGroundedPlannerActionExecutor>
        _actionExecutorMock;

    public OcrGroundedPlannerClickToolTests()
    {
        _executionStateAccessorMock =
            new Mock<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict);

        _actionExecutorMock =
            new Mock<IOcrGroundedPlannerActionExecutor>(
                MockBehavior.Strict);
    }

    [Fact]
    public void Click_WhenExecutionStateIsActive_ShouldDelegateExactSnapshotAndTargetId()
    {
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-A");

        OcrGroundedPlannerExecutionState executionState =
            new(snapshot);

        const string targetId = "ocr-0";

        _executionStateAccessorMock
            .SetupGet(accessor => accessor.Current)
            .Returns(executionState);

        _actionExecutorMock
            .Setup(
                executor =>
                    executor.Click(
                        It.Is<OcrGroundedSnapshot>(
                            actual =>
                                ReferenceEquals(
                                    actual,
                                    snapshot)),
                        targetId));

        OcrGroundedPlannerClickTool sut =
            CreateSut();

        Action act =
            () => sut.Click(targetId);

        act.Should()
            .NotThrow();

        _executionStateAccessorMock.VerifyGet(
            accessor => accessor.Current,
            Times.Once);

        _actionExecutorMock.Verify(
            executor =>
                executor.Click(
                    It.Is<OcrGroundedSnapshot>(
                        actual =>
                            ReferenceEquals(
                                actual,
                                snapshot)),
                    targetId),
            Times.Once);

        _executionStateAccessorMock.VerifyNoOtherCalls();
        _actionExecutorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetIdContainsWhitespace_ShouldPreserveTargetIdExactly()
    {
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-A");

        OcrGroundedPlannerExecutionState executionState =
            new(snapshot);

        const string targetId = "  ocr-0  ";

        _executionStateAccessorMock
            .SetupGet(accessor => accessor.Current)
            .Returns(executionState);

        _actionExecutorMock
            .Setup(
                executor =>
                    executor.Click(
                        snapshot,
                        targetId));

        OcrGroundedPlannerClickTool sut =
            CreateSut();

        Action act =
            () => sut.Click(targetId);

        act.Should()
            .NotThrow();

        _actionExecutorMock.Verify(
            executor =>
                executor.Click(
                    snapshot,
                    targetId),
            Times.Once);

        _executionStateAccessorMock.VerifyGet(
            accessor => accessor.Current,
            Times.Once);

        _executionStateAccessorMock.VerifyNoOtherCalls();
        _actionExecutorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenExecutionStateIsMissing_ShouldPropagateStateAccessorFailure()
    {
        const string targetId = "ocr-0";

        _executionStateAccessorMock
            .SetupGet(accessor => accessor.Current)
            .Throws(
                new InvalidOperationException(
                    "No active AI tool execution state is available."));

        OcrGroundedPlannerClickTool sut =
            CreateSut();

        Action act =
            () => sut.Click(targetId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*No active AI tool execution state is available*");

        _executionStateAccessorMock.VerifyGet(
            accessor => accessor.Current,
            Times.Once);

        _actionExecutorMock.Verify(
            executor =>
                executor.Click(
                    It.IsAny<OcrGroundedSnapshot>(),
                    It.IsAny<string>()),
            Times.Never);

        _executionStateAccessorMock.VerifyNoOtherCalls();
        _actionExecutorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenActionExecutorFails_ShouldPropagateOriginalFailure()
    {
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-A");

        OcrGroundedPlannerExecutionState executionState =
            new(snapshot);

        const string targetId = "ocr-0";

        _executionStateAccessorMock
            .SetupGet(accessor => accessor.Current)
            .Returns(executionState);

        _actionExecutorMock
            .Setup(
                executor =>
                    executor.Click(
                        snapshot,
                        targetId))
            .Throws(
                new InvalidOperationException(
                    "Planner grounded click failed."));

        OcrGroundedPlannerClickTool sut =
            CreateSut();

        Action act =
            () => sut.Click(targetId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*Planner grounded click failed*");

        _executionStateAccessorMock.VerifyGet(
            accessor => accessor.Current,
            Times.Once);
    
        _actionExecutorMock.Verify(
            executor =>
                executor.Click(
                    snapshot,
                    targetId),
            Times.Once);

        _executionStateAccessorMock.VerifyNoOtherCalls();
        _actionExecutorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenInvokedTwice_ShouldResolveExecutionStateForEachInvocation()
    {
        OcrGroundedSnapshot firstSnapshot =
            CreateSnapshot("snapshot-A");

        OcrGroundedSnapshot secondSnapshot =
            CreateSnapshot("snapshot-B");

        OcrGroundedPlannerExecutionState firstState =
            new(firstSnapshot);

        OcrGroundedPlannerExecutionState secondState =
            new(secondSnapshot);

        const string firstTargetId = "ocr-0";
        const string secondTargetId = "ocr-1";

        _executionStateAccessorMock
            .SetupSequence(
                accessor => accessor.Current)
            .Returns(firstState)
            .Returns(secondState);

        _actionExecutorMock
            .Setup(
                executor =>
                    executor.Click(
                        firstSnapshot,
                        firstTargetId));

        _actionExecutorMock
            .Setup(
                executor =>
                    executor.Click(
                        secondSnapshot,
                        secondTargetId));

        OcrGroundedPlannerClickTool sut =
            CreateSut();

        sut.Click(firstTargetId);
        sut.Click(secondTargetId);

        _executionStateAccessorMock.VerifyGet(
            accessor => accessor.Current,
            Times.Exactly(2));

        _actionExecutorMock.Verify(
            executor =>
                executor.Click(
                    firstSnapshot,
                    firstTargetId),
            Times.Once);

        _actionExecutorMock.Verify(
            executor =>
                executor.Click(
                    secondSnapshot,
                    secondTargetId),
            Times.Once);

        _executionStateAccessorMock.VerifyNoOtherCalls();
        _actionExecutorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenExecutionStateChanges_ShouldNotReusePreviousSnapshot()
    {
        OcrGroundedSnapshot firstSnapshot =
            CreateSnapshot("snapshot-A");

        OcrGroundedSnapshot secondSnapshot =
            CreateSnapshot("snapshot-B");

        OcrGroundedPlannerExecutionState firstState =
            new(firstSnapshot);

        OcrGroundedPlannerExecutionState secondState =
            new(secondSnapshot);

        const string targetId = "ocr-0";

        _executionStateAccessorMock
            .SetupSequence(
                accessor => accessor.Current)
            .Returns(firstState)
            .Returns(secondState);

        _actionExecutorMock
            .Setup(
                executor =>
                    executor.Click(
                        firstSnapshot,
                        targetId));

        _actionExecutorMock
            .Setup(
                executor =>
                    executor.Click(
                        secondSnapshot,
                        targetId));

        OcrGroundedPlannerClickTool sut =
            CreateSut();

        sut.Click(targetId);
        sut.Click(targetId);

        _actionExecutorMock.Verify(
            executor =>
                executor.Click(
                    firstSnapshot,
                    targetId),
            Times.Once);

        _actionExecutorMock.Verify(
            executor =>
                executor.Click(
                    secondSnapshot,
                    targetId),
            Times.Once);

        _actionExecutorMock.Verify(
            executor =>
                executor.Click(
                    It.Is<OcrGroundedSnapshot>(
                        actual =>
                            !ReferenceEquals(
                                actual,
                                firstSnapshot) &&
                            !ReferenceEquals(
                                actual,
                                secondSnapshot)),
                    It.IsAny<string>()),
            Times.Never);

        _executionStateAccessorMock.VerifyGet(
            accessor => accessor.Current,
            Times.Exactly(2));
    
        _executionStateAccessorMock.VerifyNoOtherCalls();
        _actionExecutorMock.VerifyNoOtherCalls();
    }

    private OcrGroundedPlannerClickTool CreateSut()
    {
        return new OcrGroundedPlannerClickTool(
            _executionStateAccessorMock.Object,
            _actionExecutorMock.Object);
    }

    private static OcrGroundedSnapshot CreateSnapshot(
        string snapshotId)
    {
        return new OcrGroundedSnapshot(
            snapshotId,
            Array.Empty<OcrGroundedTarget>());
    }
}