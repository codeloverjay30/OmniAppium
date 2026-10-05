using AiUtility.ToolKits.Execution;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Planner;

public sealed class OcrGroundedPlannerExecutionScopeTests
{
    [Fact]
    public void Constructor_WhenSnapshotIsNull_ShouldThrowArgumentNullException()
    {
        var leaseMock = new Mock<IDisposable>(MockBehavior.Strict);

        Action act =
            () => _ = new OcrGroundedPlannerExecutionScope(
                null!,
                leaseMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*snapshot*");

        leaseMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Constructor_WhenExecutionStateLeaseIsNull_ShouldThrowArgumentNullException()
    {
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        Action act =
            () => _ = new OcrGroundedPlannerExecutionScope(
                snapshot,
                null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*executionStateLease*");
    }

    [Fact]
    public void Snapshot_ShouldReturnSnapshotProvidedAtConstruction()
    {
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        var leaseMock =
            new Mock<IDisposable>(MockBehavior.Strict);

        var sut =
            new OcrGroundedPlannerExecutionScope(
                snapshot,
                leaseMock.Object);

        sut.Snapshot
            .Should()
            .BeSameAs(snapshot);

        leaseMock.VerifyNoOtherCalls();
    }


    [Fact]
    public void Dispose_WhenCalledTwice_ShouldDisposeExecutionStateLeaseExactlyOnce()
    {
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        var leaseMock = new Mock<IDisposable>(MockBehavior.Strict);

        leaseMock
            .Setup(lease => lease.Dispose());

        var sut =
            new OcrGroundedPlannerExecutionScope(
                snapshot,
                leaseMock.Object);

        sut.Dispose();
        sut.Dispose();

        leaseMock.Verify(
            lease => lease.Dispose(),
            Times.Once);

        leaseMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Dispose_WhenUsingRealAccessor_ShouldRestorePreviousExecutionState()
    {
        var accessor =
            new AiToolExecutionStateAccessor<
                OcrGroundedPlannerExecutionState>();

        OcrGroundedSnapshot parentSnapshot =
            CreateSnapshot("parent");

        OcrGroundedSnapshot childSnapshot =
            CreateSnapshot("child");

        using IDisposable parentLease =
            accessor.Push(
                new OcrGroundedPlannerExecutionState(
                    parentSnapshot));

        IDisposable childLease =
            accessor.Push(
                new OcrGroundedPlannerExecutionState(
                    childSnapshot));

        var sut =
            new OcrGroundedPlannerExecutionScope(
                childSnapshot,
                childLease);

        accessor.Current.Snapshot
            .Should()
            .BeSameAs(childSnapshot);

        sut.Dispose();

        accessor.Current.Snapshot
            .Should()
            .BeSameAs(parentSnapshot);
    }

    [Fact]
    public void Dispose_WhenNoParentStateExists_ShouldRemoveCurrentExecutionState()
    {
        var accessor =
            new AiToolExecutionStateAccessor<
                OcrGroundedPlannerExecutionState>();

        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        IDisposable lease =
            accessor.Push(
                new OcrGroundedPlannerExecutionState(
                    snapshot));

        var sut =
            new OcrGroundedPlannerExecutionScope(
                snapshot,
                lease);

        sut.Dispose();

        Action act =
            () => _ = accessor.Current;

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "No active AI tool execution state is available.");
    }

    private static OcrGroundedSnapshot CreateSnapshot(
        string snapshotId)
    {
        return new OcrGroundedSnapshot(
            snapshotId,
            Array.Empty<OcrGroundedTarget>());
    }
}
