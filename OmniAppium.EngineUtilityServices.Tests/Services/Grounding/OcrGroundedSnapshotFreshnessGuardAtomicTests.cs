using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class OcrGroundedSnapshotFreshnessGuardAtomicTests
{
    [Fact]
    public void AcquireCurrent_WhenSnapshotIsCurrent_ReturnsLease()
    {
        OcrGroundedSnapshotFreshnessGuard sut = new();
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-A");

        sut.MarkCurrent(snapshot.SnapshotId);

        using IOcrGroundedActionLease lease =
            sut.AcquireCurrent(snapshot);

        lease.Should().NotBeNull();
    }

    [Fact]
    public void AcquireCurrent_WhenNoCurrentSnapshotExists_ThrowsInvalidOperationException()
    {
        OcrGroundedSnapshotFreshnessGuard sut = new();
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-A");

        Action act =
            () => sut.AcquireCurrent(snapshot);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*No current OCR-grounded snapshot has been established*");
    }

    [Fact]
    public void AcquireCurrent_WhenSnapshotIsStale_ThrowsStaleOcrGroundedSnapshotException()
    {
        OcrGroundedSnapshotFreshnessGuard sut = new();
        OcrGroundedSnapshot snapshotA =
            CreateSnapshot("snapshot-A");

        sut.MarkCurrent("snapshot-B");

        Action act =
            () => sut.AcquireCurrent(snapshotA);

        StaleOcrGroundedSnapshotException exception =
            act.Should()
                .Throw<StaleOcrGroundedSnapshotException>()
                .WithMessage(
                    "*snapshot-A*stale*current snapshot is 'snapshot-B'*")
                .Which;

        exception.SnapshotId
            .Should()
            .Be("snapshot-A");

        exception.CurrentSnapshotId
            .Should()
            .Be("snapshot-B");
    }

    [Fact]
    public async Task MarkCurrent_WhenActionLeaseIsActive_WaitsUntilLeaseIsDisposed()
    {
        CancellationToken cancellationToken =
            TestContext.Current.CancellationToken;

        using ControlledMonitorWaitStrategy waitStrategy = new();

        OcrGroundedSnapshotFreshnessGuard sut =
            new(waitStrategy);

        OcrGroundedSnapshot snapshotA =
            CreateSnapshot("snapshot-A");

        sut.MarkCurrent(snapshotA.SnapshotId);

        IOcrGroundedActionLease lease =
            sut.AcquireCurrent(snapshotA);

        Task markCurrentTask =
            Task.Run(
                () => sut.MarkCurrent("snapshot-B"),
                cancellationToken);

        try
        {
            await waitStrategy.WaitUntilEnteredAsync(
                1,
                cancellationToken);

            waitStrategy.WaitCount
                .Should()
                .Be(
                    1,
                    "MarkCurrent must enter the freshness wait exactly once before the active lease is released");

            markCurrentTask.IsCompleted
                .Should()
                .BeFalse(
                    "MarkCurrent must remain blocked at the controlled wait boundary while the action lease is active");

            waitStrategy.AllowMonitorWait(1);

            lease.Dispose();

            await markCurrentTask.WaitAsync(
                cancellationToken);

            Action act =
                () => sut.AcquireCurrent(snapshotA);

            act.Should()
                .Throw<StaleOcrGroundedSnapshotException>()
                .WithMessage(
                    "*snapshot-A*stale*current snapshot is 'snapshot-B'*");
        }
        finally
        {
            waitStrategy.AllowMonitorWait(1);

            lease.Dispose();

            await markCurrentTask.WaitAsync(
                cancellationToken);
        }
    }

    [Fact]
    public void AcquireCurrent_AfterNewSnapshotBecomesCurrent_RejectsPreviousSnapshot()
    {
        OcrGroundedSnapshotFreshnessGuard sut = new();

        OcrGroundedSnapshot snapshotA =
            CreateSnapshot("snapshot-A");

        OcrGroundedSnapshot snapshotB =
            CreateSnapshot("snapshot-B");

        sut.MarkCurrent(snapshotA.SnapshotId);

        using (IOcrGroundedActionLease lease =
               sut.AcquireCurrent(snapshotA))
        {
        }

        sut.MarkCurrent(snapshotB.SnapshotId);

        Action act =
            () => sut.AcquireCurrent(snapshotA);

        StaleOcrGroundedSnapshotException exception =
            act.Should()
                .Throw<StaleOcrGroundedSnapshotException>()
                .WithMessage(
                    "*snapshot-A*stale*current snapshot is 'snapshot-B'*")
                .Which;

        exception.SnapshotId
            .Should()
            .Be("snapshot-A");

        exception.CurrentSnapshotId
            .Should()
            .Be("snapshot-B");
    }

    [Fact]
    public async Task Dispose_WhenCalledMultipleTimes_ReleasesAuthorizationOnlyOnce()
    {
        CancellationToken cancellationToken =
            TestContext.Current.CancellationToken;

        using ControlledMonitorWaitStrategy waitStrategy = new();

        OcrGroundedSnapshotFreshnessGuard sut =
            new(waitStrategy);

        OcrGroundedSnapshot snapshotA =
            CreateSnapshot("snapshot-A");

        sut.MarkCurrent(snapshotA.SnapshotId);

        IOcrGroundedActionLease firstLease =
            sut.AcquireCurrent(snapshotA);

        IOcrGroundedActionLease secondLease =
            sut.AcquireCurrent(snapshotA);

        Task markCurrentTask =
            Task.Run(
                () => sut.MarkCurrent("snapshot-B"),
                cancellationToken);

        try
        {
            await waitStrategy.WaitUntilEnteredAsync(
                1,
                cancellationToken);

            waitStrategy.WaitCount
                .Should()
                .Be(1);

            waitStrategy.AllowMonitorWait(1);

            firstLease.Dispose();

            Action secondDisposeOfFirstLease =
                () => firstLease.Dispose();

            secondDisposeOfFirstLease
                .Should()
                .NotThrow(
                    "disposing the same authorization lease repeatedly must be idempotent");

            Action disposeSecondLease =
                () => secondLease.Dispose();

            disposeSecondLease
                .Should()
                .NotThrow(
                    "double-disposing the first lease must not release the second authorization");

            await markCurrentTask.WaitAsync(
                cancellationToken);

            Action acquireOldSnapshot =
                () => sut.AcquireCurrent(snapshotA);

            acquireOldSnapshot
                .Should()
                .Throw<StaleOcrGroundedSnapshotException>()
                .WithMessage(
                    "*snapshot-A*stale*current snapshot is 'snapshot-B'*");
        }
        finally
        {
            waitStrategy.AllowMonitorWait(1);

            firstLease.Dispose();
            secondLease.Dispose();

            await markCurrentTask.WaitAsync(
                cancellationToken);
        }
    }

    private static OcrGroundedSnapshot CreateSnapshot(
        string snapshotId)
    {
        return new OcrGroundedSnapshot(
            snapshotId,
            []);
    }

    private sealed class ControlledMonitorWaitStrategy
        : IOcrGroundedFreshnessWaitStrategy,
          IDisposable
    {
        private readonly object _syncRoot = new();
        private readonly List<WaitControl> _waitControls = [];

        private int _waitCount;
        private bool _isDisposed;

        public int WaitCount
        {
            get
            {
                lock (_syncRoot)
                {
                    return _waitCount;
                }
            }
        }

        public Task WaitUntilEnteredAsync(
            int waitNumber,
            CancellationToken cancellationToken)
        {
            if (waitNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(waitNumber),
                    waitNumber,
                    "Wait number must be greater than zero.");
            }

            WaitControl waitControl;

            lock (_syncRoot)
            {
                ObjectDisposedException.ThrowIf(
                    _isDisposed,
                    this);

                waitControl =
                    GetOrCreateWaitControl(waitNumber);
            }

            return waitControl.Entered.Task.WaitAsync(
                cancellationToken);
        }

        public void AllowMonitorWait(
            int waitNumber)
        {
            if (waitNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(waitNumber),
                    waitNumber,
                    "Wait number must be greater than zero.");
            }

            WaitControl waitControl;

            lock (_syncRoot)
            {
                ObjectDisposedException.ThrowIf(
                    _isDisposed,
                    this);

                waitControl =
                    GetOrCreateWaitControl(waitNumber);
            }

            waitControl.AllowMonitorWait.Set();
        }

        public void Wait(
            object syncRoot)
        {
            ArgumentNullException.ThrowIfNull(syncRoot);

            WaitControl waitControl;

            lock (_syncRoot)
            {
                ObjectDisposedException.ThrowIf(
                    _isDisposed,
                    this);

                int waitNumber =
                    checked(++_waitCount);

                waitControl =
                    GetOrCreateWaitControl(waitNumber);
            }

            waitControl.Entered.TrySetResult();

            waitControl.AllowMonitorWait.Wait(
                TestContext.Current.CancellationToken);

            Monitor.Wait(syncRoot);
        }

        public void PulseAll(
            object syncRoot)
        {
            ArgumentNullException.ThrowIfNull(syncRoot);

            Monitor.PulseAll(syncRoot);
        }

        public void Dispose()
        {
            WaitControl[] waitControls;

            lock (_syncRoot)
            {
                if (_isDisposed)
                {
                    return;
                }

                _isDisposed = true;

                waitControls =
                    [.. _waitControls];
            }

            foreach (WaitControl waitControl in waitControls)
            {
                waitControl.AllowMonitorWait.Set();
            }

            foreach (WaitControl waitControl in waitControls)
            {
                waitControl.AllowMonitorWait.Dispose();
            }
        }

        private WaitControl GetOrCreateWaitControl(
            int waitNumber)
        {
            while (_waitControls.Count < waitNumber)
            {
                _waitControls.Add(
                    new WaitControl());
            }

            return _waitControls[waitNumber - 1];
        }

        private sealed class WaitControl
        {
            public TaskCompletionSource Entered { get; } =
                new(
                    TaskCreationOptions.RunContinuationsAsynchronously);

            public ManualResetEventSlim AllowMonitorWait { get; } =
                new(false);
        }
    }
}