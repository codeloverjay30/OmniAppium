using AiUtility.ToolKits.Abstractions;
using AiUtility.ToolKits.Services;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Planner;

public sealed class OcrGroundedPlannerExecutionScopeFactoryTests
{
    private readonly Mock<IAndroidScreenOcrService>
        _androidScreenOcrServiceMock;

    private readonly Mock<IOcrGroundedSnapshotFactory>
        _snapshotFactoryMock;

    private readonly Mock<IOcrGroundedSnapshotFreshnessGuard>
        _freshnessGuardMock;

    private readonly Mock<
        IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>>
        _executionStateAccessorMock;

    public OcrGroundedPlannerExecutionScopeFactoryTests()
    {
        _androidScreenOcrServiceMock =
            new Mock<IAndroidScreenOcrService>(
                MockBehavior.Strict);

        _snapshotFactoryMock =
            new Mock<IOcrGroundedSnapshotFactory>(
                MockBehavior.Strict);

        _freshnessGuardMock =
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(
                MockBehavior.Strict);

        _executionStateAccessorMock =
            new Mock<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenAndroidScreenOcrServiceIsNull()
    {
        Action act =
            () => new OcrGroundedPlannerExecutionScopeFactory(
                null!,
                _snapshotFactoryMock.Object,
                _freshnessGuardMock.Object,
                _executionStateAccessorMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage(
                "*androidScreenOcrService*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenSnapshotFactoryIsNull()
    {
        Action act =
            () => new OcrGroundedPlannerExecutionScopeFactory(
                _androidScreenOcrServiceMock.Object,
                null!,
                _freshnessGuardMock.Object,
                _executionStateAccessorMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage(
                "*snapshotFactory*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenFreshnessGuardIsNull()
    {
        Action act =
            () => new OcrGroundedPlannerExecutionScopeFactory(
                _androidScreenOcrServiceMock.Object,
                _snapshotFactoryMock.Object,
                null!,
                _executionStateAccessorMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage(
                "*freshnessGuard*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenExecutionStateAccessorIsNull()
    {
        Action act =
            () => new OcrGroundedPlannerExecutionScopeFactory(
                _androidScreenOcrServiceMock.Object,
                _snapshotFactoryMock.Object,
                _freshnessGuardMock.Object,
                null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage(
                "*executionStateAccessor*");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentNullException_WhenObservationIsNull()
    {
        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        Func<Task> act =
            async () =>
                await sut.CreateAsync(
                    null!);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithMessage(
                "*observation*");

        VerifyNoOcrCapture();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowOperationCanceledException_WhenCancellationWasAlreadyRequested()
    {
        OcrResult ocrResult =
            CreateOcrResult(
                "Start Battle");

        IAndroidScreenObservation observation =
            CreateObservation(
                ocrResult);

        using CancellationTokenSource cancellationTokenSource =
            new();

        cancellationTokenSource.Cancel();

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        Func<Task> act =
            async () =>
                await sut.CreateAsync(
                    observation,
                    cancellationTokenSource.Token);

        await act.Should()
            .ThrowAsync<OperationCanceledException>()
            .WithMessage("*canceled*");

        _snapshotFactoryMock.Verify(
            factory =>
                factory.Create(
                    It.IsAny<string>(),
                    It.IsAny<OcrResult>()),
            Times.Never);

        _freshnessGuardMock.Verify(
            guard =>
                guard.MarkCurrent(
                    It.IsAny<string>()),
            Times.Never);

        _executionStateAccessorMock.Verify(
            accessor =>
                accessor.Push(
                    It.IsAny<
                        OcrGroundedPlannerExecutionState>()),
            Times.Never);

        VerifyNoOcrCapture();
    }

    [Fact]
    public async Task CreateAsync_ShouldUseOcrResultFromProvidedObservation()
    {
        OcrResult ocrResult =
            CreateOcrResult(
                "Start Battle");

        IAndroidScreenObservation observation =
            CreateObservation(
                ocrResult);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "ocr-snapshot-00000001");

        Mock<IDisposable> leaseMock =
            CreateLeaseMock();

        _snapshotFactoryMock
            .Setup(factory =>
                factory.Create(
                    "ocr-snapshot-00000001",
                    It.Is<OcrResult>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                ocrResult))))
            .Returns(snapshot);

        _freshnessGuardMock
            .Setup(guard =>
                guard.MarkCurrent(
                    snapshot.SnapshotId));

        _executionStateAccessorMock
            .Setup(accessor =>
                accessor.Push(
                    It.Is<
                        OcrGroundedPlannerExecutionState>(
                        state =>
                            ReferenceEquals(
                                state.Snapshot,
                                snapshot))))
            .Returns(
                leaseMock.Object);

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        IOcrGroundedPlannerExecutionScope scope =
            await sut.CreateAsync(
                observation);

        scope.Snapshot
            .Should()
            .BeSameAs(snapshot);

        _snapshotFactoryMock.Verify(
            factory =>
                factory.Create(
                    "ocr-snapshot-00000001",
                    It.Is<OcrResult>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                ocrResult))),
            Times.Once);

        VerifyNoOcrCapture();

        scope.Dispose();

        leaseMock.Verify(
            lease =>
                lease.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldMarkCreatedSnapshotAsCurrent()
    {
        OcrResult ocrResult =
            CreateOcrResult(
                "Start Battle");

        IAndroidScreenObservation observation =
            CreateObservation(
                ocrResult);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "ocr-snapshot-00000001");

        Mock<IDisposable> leaseMock =
            CreateLeaseMock();

        SetupSuccessfulScopeCreation(
            ocrResult,
            snapshot,
            leaseMock.Object);

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        IOcrGroundedPlannerExecutionScope scope =
            await sut.CreateAsync(
                observation);

        _freshnessGuardMock.Verify(
            guard =>
                guard.MarkCurrent(
                    snapshot.SnapshotId),
            Times.Once);

        scope.Dispose();

        leaseMock.Verify(
            lease =>
                lease.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldPushExecutionStateContainingCreatedSnapshot()
    {
        OcrResult ocrResult =
            CreateOcrResult(
                "Start Battle");

        IAndroidScreenObservation observation =
            CreateObservation(
                ocrResult);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "ocr-snapshot-00000001");

        Mock<IDisposable> leaseMock =
            CreateLeaseMock();

        _snapshotFactoryMock
            .Setup(factory =>
                factory.Create(
                    "ocr-snapshot-00000001",
                    It.Is<OcrResult>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                ocrResult))))
            .Returns(snapshot);

        _freshnessGuardMock
            .Setup(guard =>
                guard.MarkCurrent(
                    snapshot.SnapshotId));

        OcrGroundedPlannerExecutionState?
            capturedExecutionState = null;

        _executionStateAccessorMock
            .Setup(accessor =>
                accessor.Push(
                    It.IsAny<
                        OcrGroundedPlannerExecutionState>()))
            .Callback<
                OcrGroundedPlannerExecutionState>(
                state =>
                    capturedExecutionState = state)
            .Returns(
                leaseMock.Object);

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        IOcrGroundedPlannerExecutionScope scope =
            await sut.CreateAsync(
                observation);

        capturedExecutionState.Should()
            .NotBeNull();

        capturedExecutionState!
            .Snapshot
            .Should()
            .BeSameAs(snapshot);

        scope.Snapshot
            .Should()
            .BeSameAs(snapshot);

        scope.Dispose();

        leaseMock.Verify(
            lease =>
                lease.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnScopeOwningExecutionStateLease()
    {
        OcrResult ocrResult =
            CreateOcrResult(
                "Start Battle");

        IAndroidScreenObservation observation =
            CreateObservation(
                ocrResult);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "ocr-snapshot-00000001");

        Mock<IDisposable> leaseMock =
            CreateLeaseMock();

        SetupSuccessfulScopeCreation(
            ocrResult,
            snapshot,
            leaseMock.Object);

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        IOcrGroundedPlannerExecutionScope scope =
            await sut.CreateAsync(
                observation);

        leaseMock.Verify(
            lease =>
                lease.Dispose(),
            Times.Never);

        scope.Dispose();

        leaseMock.Verify(
            lease =>
                lease.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotDisposeLeaseMoreThanOnce_WhenScopeIsDisposedRepeatedly()
    {
        OcrResult ocrResult =
            CreateOcrResult(
                "Start Battle");

        IAndroidScreenObservation observation =
            CreateObservation(
                ocrResult);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "ocr-snapshot-00000001");

        Mock<IDisposable> leaseMock =
            CreateLeaseMock();

        SetupSuccessfulScopeCreation(
            ocrResult,
            snapshot,
            leaseMock.Object);

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        IOcrGroundedPlannerExecutionScope scope =
            await sut.CreateAsync(
                observation);

        scope.Dispose();
        scope.Dispose();

        leaseMock.Verify(
            lease =>
                lease.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldGenerateSequentialSnapshotIdentifiers()
    {
        OcrResult firstOcrResult =
            CreateOcrResult(
                "First screen");

        OcrResult secondOcrResult =
            CreateOcrResult(
                "Second screen");

        IAndroidScreenObservation firstObservation =
            CreateObservation(
                firstOcrResult);

        IAndroidScreenObservation secondObservation =
            CreateObservation(
                secondOcrResult);

        OcrGroundedSnapshot firstSnapshot =
            CreateSnapshot(
                "ocr-snapshot-00000001");

        OcrGroundedSnapshot secondSnapshot =
            CreateSnapshot(
                "ocr-snapshot-00000002");

        Mock<IDisposable> firstLeaseMock =
            CreateLeaseMock();

        Mock<IDisposable> secondLeaseMock =
            CreateLeaseMock();

        _snapshotFactoryMock
            .Setup(factory =>
                factory.Create(
                    "ocr-snapshot-00000001",
                    It.Is<OcrResult>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                firstOcrResult))))
            .Returns(firstSnapshot);

        _snapshotFactoryMock
            .Setup(factory =>
                factory.Create(
                    "ocr-snapshot-00000002",
                    It.Is<OcrResult>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                secondOcrResult))))
            .Returns(secondSnapshot);

        _freshnessGuardMock
            .Setup(guard =>
                guard.MarkCurrent(
                    firstSnapshot.SnapshotId));

        _freshnessGuardMock
            .Setup(guard =>
                guard.MarkCurrent(
                    secondSnapshot.SnapshotId));

        _executionStateAccessorMock
            .Setup(accessor =>
                accessor.Push(
                    It.Is<
                        OcrGroundedPlannerExecutionState>(
                        state =>
                            ReferenceEquals(
                                state.Snapshot,
                                firstSnapshot))))
            .Returns(
                firstLeaseMock.Object);

        _executionStateAccessorMock
            .Setup(accessor =>
                accessor.Push(
                    It.Is<
                        OcrGroundedPlannerExecutionState>(
                        state =>
                            ReferenceEquals(
                                state.Snapshot,
                                secondSnapshot))))
            .Returns(
                secondLeaseMock.Object);

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        IOcrGroundedPlannerExecutionScope firstScope =
            await sut.CreateAsync(
                firstObservation);

        IOcrGroundedPlannerExecutionScope secondScope =
            await sut.CreateAsync(
                secondObservation);

        firstScope.Snapshot.SnapshotId
            .Should()
            .Be("ocr-snapshot-00000001");

        secondScope.Snapshot.SnapshotId
            .Should()
            .Be("ocr-snapshot-00000002");

        VerifyNoOcrCapture();

        firstScope.Dispose();
        secondScope.Dispose();

        firstLeaseMock.Verify(
            lease =>
                lease.Dispose(),
            Times.Once);

        secondLeaseMock.Verify(
            lease =>
                lease.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldPropagateSnapshotFactoryException_WithoutPushingExecutionState()
    {
        OcrResult ocrResult =
            CreateOcrResult(
                "Start Battle");

        IAndroidScreenObservation observation =
            CreateObservation(
                ocrResult);

        _snapshotFactoryMock
            .Setup(factory =>
                factory.Create(
                    It.IsAny<string>(),
                    It.Is<OcrResult>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                ocrResult))))
            .Throws(
                new InvalidOperationException(
                    "Snapshot creation failed."));

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        Func<Task> act =
            async () =>
                await sut.CreateAsync(
                    observation);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Snapshot creation failed.");

        _freshnessGuardMock.Verify(
            guard =>
                guard.MarkCurrent(
                    It.IsAny<string>()),
            Times.Never);

        _executionStateAccessorMock.Verify(
            accessor =>
                accessor.Push(
                    It.IsAny<
                        OcrGroundedPlannerExecutionState>()),
            Times.Never);

        VerifyNoOcrCapture();
    }

    [Fact]
    public async Task CreateAsync_ShouldPropagateFreshnessGuardException_WithoutPushingExecutionState()
    {
        OcrResult ocrResult =
            CreateOcrResult(
                "Start Battle");

        IAndroidScreenObservation observation =
            CreateObservation(
                ocrResult);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "ocr-snapshot-00000001");

        _snapshotFactoryMock
            .Setup(factory =>
                factory.Create(
                    "ocr-snapshot-00000001",
                    It.Is<OcrResult>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                ocrResult))))
            .Returns(snapshot);

        _freshnessGuardMock
            .Setup(guard =>
                guard.MarkCurrent(
                    snapshot.SnapshotId))
            .Throws(
                new InvalidOperationException(
                    "Freshness update failed."));

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        Func<Task> act =
            async () =>
                await sut.CreateAsync(
                    observation);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Freshness update failed.");

        _executionStateAccessorMock.Verify(
            accessor =>
                accessor.Push(
                    It.IsAny<
                        OcrGroundedPlannerExecutionState>()),
            Times.Never);

        VerifyNoOcrCapture();
    }

    [Fact]
    public async Task CreateAsync_ShouldPropagateExecutionStatePushException()
    {
        OcrResult ocrResult =
            CreateOcrResult(
                "Start Battle");

        IAndroidScreenObservation observation =
            CreateObservation(
                ocrResult);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "ocr-snapshot-00000001");

        _snapshotFactoryMock
            .Setup(factory =>
                factory.Create(
                    "ocr-snapshot-00000001",
                    It.Is<OcrResult>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                ocrResult))))
            .Returns(snapshot);

        _freshnessGuardMock
            .Setup(guard =>
                guard.MarkCurrent(
                    snapshot.SnapshotId));

        _executionStateAccessorMock
            .Setup(accessor =>
                accessor.Push(
                    It.IsAny<
                        OcrGroundedPlannerExecutionState>()))
            .Throws(
                new InvalidOperationException(
                    "Execution-state push failed."));

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        Func<Task> act =
            async () =>
                await sut.CreateAsync(
                    observation);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Execution-state push failed.");

        VerifyNoOcrCapture();
    }

    [Fact]
    public async Task CreateAsync_ShouldNeverCaptureOrRecognizeAnotherScreen()
    {
        OcrResult ocrResult =
            CreateOcrResult(
                "Observation OCR");

        IAndroidScreenObservation observation =
            CreateObservation(
                ocrResult);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "ocr-snapshot-00000001");

        Mock<IDisposable> leaseMock =
            CreateLeaseMock();

        SetupSuccessfulScopeCreation(
            ocrResult,
            snapshot,
            leaseMock.Object);

        OcrGroundedPlannerExecutionScopeFactory sut =
            CreateSut();

        IOcrGroundedPlannerExecutionScope scope =
            await sut.CreateAsync(
                observation);

        VerifyNoOcrCapture();

        scope.Dispose();

        leaseMock.Verify(
            lease =>
                lease.Dispose(),
            Times.Once);
    }

    private OcrGroundedPlannerExecutionScopeFactory CreateSut()
    {
        return new OcrGroundedPlannerExecutionScopeFactory(
            _androidScreenOcrServiceMock.Object,
            _snapshotFactoryMock.Object,
            _freshnessGuardMock.Object,
            _executionStateAccessorMock.Object);
    }

    private static IAndroidScreenObservation CreateObservation(
        OcrResult ocrResult)
    {
        Mock<IAndroidScreenObservation> observationMock =
            new(MockBehavior.Strict);

        observationMock
            .SetupGet(observation =>
                observation.OcrResult)
            .Returns(ocrResult);

        return observationMock.Object;
    }

    private static OcrResult CreateOcrResult(
        string text)
    {
        return new OcrResult(
            text,
            Array.Empty<OcrTextLine>());
    }

    private static OcrGroundedSnapshot CreateSnapshot(
        string snapshotId)
    {
        return new OcrGroundedSnapshot(
            snapshotId,
            Array.Empty<OcrGroundedTarget>());
    }


    private static Mock<IDisposable> CreateLeaseMock()
    {
        Mock<IDisposable> leaseMock =
            new(MockBehavior.Strict);

        leaseMock
            .Setup(lease =>
                lease.Dispose());

        return leaseMock;
    }

    private void SetupSuccessfulScopeCreation(
        OcrResult ocrResult,
        OcrGroundedSnapshot snapshot,
        IDisposable lease)
    {
        _snapshotFactoryMock
            .Setup(factory =>
                factory.Create(
                    snapshot.SnapshotId,
                    It.Is<OcrResult>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                ocrResult))))
            .Returns(snapshot);

        _freshnessGuardMock
            .Setup(guard =>
                guard.MarkCurrent(
                    snapshot.SnapshotId));

        _executionStateAccessorMock
            .Setup(accessor =>
                accessor.Push(
                    It.Is<
                        OcrGroundedPlannerExecutionState>(
                        state =>
                            ReferenceEquals(
                                state.Snapshot,
                                snapshot))))
            .Returns(lease);
    }

    private void VerifyNoOcrCapture()
    {
        _androidScreenOcrServiceMock.Verify(
            service =>
                service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);

        _androidScreenOcrServiceMock.Verify(
            service =>
                service.RecognizeAsync(
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }
}