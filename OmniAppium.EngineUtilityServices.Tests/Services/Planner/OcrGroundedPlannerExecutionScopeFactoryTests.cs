using AiUtility.ToolKits.Abstractions;
using AiUtility.ToolKits.Execution;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OCRUtilityServices.Models;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Planner;

public sealed class OcrGroundedPlannerExecutionScopeFactoryTests
{
    [Fact]
    public void Constructor_WhenAndroidScreenOcrServiceIsNull_ShouldThrowArgumentNullException()
    {
        var snapshotFactoryMock =
            new Mock<IOcrGroundedSnapshotFactory>(MockBehavior.Strict);

        var freshnessGuardMock =
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);

        var executionStateAccessorMock =
            new Mock<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict);

        Action act =
            () => _ = new OcrGroundedPlannerExecutionScopeFactory(
                null!,
                snapshotFactoryMock.Object,
                freshnessGuardMock.Object,
                executionStateAccessorMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*androidScreenOcrService*");

        snapshotFactoryMock.VerifyNoOtherCalls();
        freshnessGuardMock.VerifyNoOtherCalls();
        executionStateAccessorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateAsync_WhenSuccessful_ShouldCreateAndExposeGroundedExecutionState()
    {
        var ocrServiceMock =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        var snapshotFactoryMock =
            new Mock<IOcrGroundedSnapshotFactory>(MockBehavior.Strict);

        var freshnessGuardMock =
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);

        var executionStateAccessorMock =
            new Mock<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict);

        var leaseMock =
            new Mock<IDisposable>(MockBehavior.Strict);

        OcrResult ocrResult =
            CreateOcrResult();

        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "snapshot-created-by-factory");

        string? generatedSnapshotId = null;

        ocrServiceMock
            .Setup(
                service => service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(ocrResult);

        snapshotFactoryMock
            .Setup(
                factory => factory.Create(
                    It.IsAny<string>(),
                    ocrResult))
            .Callback<string, OcrResult>(
                (snapshotId, _) =>
                    generatedSnapshotId = snapshotId)
            .Returns(snapshot);

        freshnessGuardMock
            .Setup(
                guard => guard.MarkCurrent(
                    snapshot.SnapshotId));

        executionStateAccessorMock
            .Setup(
                accessor => accessor.Push(
                    It.Is<OcrGroundedPlannerExecutionState>(
                        state =>
                            ReferenceEquals(
                                state.Snapshot,
                                snapshot))))
            .Returns(leaseMock.Object);

        leaseMock
            .Setup(lease => lease.Dispose());

        var sut =
            new OcrGroundedPlannerExecutionScopeFactory(
                ocrServiceMock.Object,
                snapshotFactoryMock.Object,
                freshnessGuardMock.Object,
                executionStateAccessorMock.Object);

        IOcrGroundedPlannerExecutionScope scope =
            await sut.CreateAsync();

        scope.Snapshot
            .Should()
            .BeSameAs(snapshot);

        generatedSnapshotId
            .Should()
            .Be("ocr-snapshot-00000001");

        executionStateAccessorMock.Verify(
            accessor => accessor.Push(
                It.Is<OcrGroundedPlannerExecutionState>(
                    state =>
                        ReferenceEquals(
                            state.Snapshot,
                            snapshot))),
            Times.Once);

        leaseMock.Verify(
            lease => lease.Dispose(),
            Times.Never);

        scope.Dispose();

        leaseMock.Verify(
            lease => lease.Dispose(),
            Times.Once);

        ocrServiceMock.Verify(
            service => service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);

        snapshotFactoryMock.Verify(
            factory => factory.Create(
                "ocr-snapshot-00000001",
                ocrResult),
            Times.Once);

        freshnessGuardMock.Verify(
            guard => guard.MarkCurrent(
                snapshot.SnapshotId),
            Times.Once);

        ocrServiceMock.VerifyNoOtherCalls();
        snapshotFactoryMock.VerifyNoOtherCalls();
        freshnessGuardMock.VerifyNoOtherCalls();
        executionStateAccessorMock.VerifyNoOtherCalls();
        leaseMock.VerifyNoOtherCalls();
    }


    [Fact]
    public async Task CreateAsync_WhenCancellationTokenIsProvided_ShouldForwardSameTokenToOcrService()
    {
        var ocrServiceMock =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        var snapshotFactoryMock =
            new Mock<IOcrGroundedSnapshotFactory>(MockBehavior.Strict);

        var freshnessGuardMock =
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);

        var executionStateAccessorMock =
            new Mock<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict);

        OcrResult ocrResult =
            CreateOcrResult();

        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        using var cancellationTokenSource =
            new CancellationTokenSource();

        CancellationToken expectedToken =
            cancellationTokenSource.Token;

        var leaseMock =
            new Mock<IDisposable>(MockBehavior.Strict);

        leaseMock
            .Setup(lease => lease.Dispose());

        ocrServiceMock
            .Setup(
                service => service.RecognizeCurrentScreenAsync(
                    expectedToken))
            .ReturnsAsync(ocrResult);

        snapshotFactoryMock
            .Setup(
                factory => factory.Create(
                    It.IsAny<string>(),
                    ocrResult))
            .Returns(snapshot);

        freshnessGuardMock
            .Setup(
                guard => guard.MarkCurrent(
                    snapshot.SnapshotId));

        executionStateAccessorMock
            .Setup(
                accessor => accessor.Push(
                    It.Is<OcrGroundedPlannerExecutionState>(
                        state =>
                            ReferenceEquals(
                                state.Snapshot,
                                snapshot))))
            .Returns(leaseMock.Object);

        var sut =
            new OcrGroundedPlannerExecutionScopeFactory(
                ocrServiceMock.Object,
                snapshotFactoryMock.Object,
                freshnessGuardMock.Object,
                executionStateAccessorMock.Object);

        using IOcrGroundedPlannerExecutionScope scope =
            await sut.CreateAsync(expectedToken);

        scope.Snapshot
            .Should()
            .BeSameAs(snapshot);

        ocrServiceMock.Verify(
            service => service.RecognizeCurrentScreenAsync(
                expectedToken),
            Times.Once);

        snapshotFactoryMock.Verify(
            factory => factory.Create(
                It.IsAny<string>(),
                ocrResult),
            Times.Once);

        freshnessGuardMock.Verify(
            guard => guard.MarkCurrent(
                snapshot.SnapshotId),
            Times.Once);

        executionStateAccessorMock.Verify(
            accessor => accessor.Push(
                It.IsAny<OcrGroundedPlannerExecutionState>()),
            Times.Once);

        leaseMock.Verify(
            lease => lease.Dispose(),
            Times.Never);

        scope.Dispose();

        leaseMock.Verify(
            lease => lease.Dispose(),
            Times.Once);
    

        ocrServiceMock.VerifyNoOtherCalls();
        snapshotFactoryMock.VerifyNoOtherCalls();
        freshnessGuardMock.VerifyNoOtherCalls();
        executionStateAccessorMock.VerifyNoOtherCalls();
        leaseMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateAsync_WhenAlreadyCancelled_ShouldNotInvokeDependencies()
    {
        var ocrServiceMock =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        var snapshotFactoryMock =
            new Mock<IOcrGroundedSnapshotFactory>(MockBehavior.Strict);

        var freshnessGuardMock =
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);

        var executionStateAccessorMock =
            new Mock<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        cancellationTokenSource.Cancel();

        var sut =
            new OcrGroundedPlannerExecutionScopeFactory(
                ocrServiceMock.Object,
                snapshotFactoryMock.Object,
                freshnessGuardMock.Object,
                executionStateAccessorMock.Object);

        Func<Task> act =
            async () =>
                _ = await sut.CreateAsync(
                    cancellationTokenSource.Token);

        await act.Should()
            .ThrowAsync<OperationCanceledException>()
            .WithMessage(
                "*The operation was canceled*");

        ocrServiceMock.VerifyNoOtherCalls();
        snapshotFactoryMock.VerifyNoOtherCalls();
        freshnessGuardMock.VerifyNoOtherCalls();
        executionStateAccessorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateAsync_WhenOcrFails_ShouldPropagateOriginalExceptionAndNotEstablishState()
    {
        var ocrServiceMock =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        var snapshotFactoryMock =
            new Mock<IOcrGroundedSnapshotFactory>(MockBehavior.Strict);

        var freshnessGuardMock =
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);

        var executionStateAccessorMock =
            new Mock<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict);

        ocrServiceMock
            .Setup(
                service => service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new InvalidOperationException(
                    "OCR capture failed."));

        var sut =
            new OcrGroundedPlannerExecutionScopeFactory(
                ocrServiceMock.Object,
                snapshotFactoryMock.Object,
                freshnessGuardMock.Object,
                executionStateAccessorMock.Object);

        Func<Task> act =
            async () => _ = await sut.CreateAsync();

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "OCR capture failed.");

        ocrServiceMock.Verify(
            service => service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);

        ocrServiceMock.VerifyNoOtherCalls();
        snapshotFactoryMock.VerifyNoOtherCalls();
        freshnessGuardMock.VerifyNoOtherCalls();
        executionStateAccessorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateAsync_WhenSnapshotCreationFails_ShouldNotMarkFreshnessOrPushState()
    {
        var ocrServiceMock =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        var snapshotFactoryMock =
            new Mock<IOcrGroundedSnapshotFactory>(MockBehavior.Strict);

        var freshnessGuardMock =
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);

        var executionStateAccessorMock =
            new Mock<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict);

        OcrResult ocrResult =
            CreateOcrResult();

        ocrServiceMock
            .Setup(
                service => service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(ocrResult);

        snapshotFactoryMock
            .Setup(
                factory => factory.Create(
                    It.IsAny<string>(),
                    ocrResult))
            .Throws(
                new InvalidOperationException(
                    "Snapshot creation failed."));

        var sut =
            new OcrGroundedPlannerExecutionScopeFactory(
                ocrServiceMock.Object,
                snapshotFactoryMock.Object,
                freshnessGuardMock.Object,
                executionStateAccessorMock.Object);

        Func<Task> act =
            async () => _ = await sut.CreateAsync();

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Snapshot creation failed.");

        ocrServiceMock.Verify(
            service => service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);

        snapshotFactoryMock.Verify(
            factory => factory.Create(
                It.IsAny<string>(),
                ocrResult),
            Times.Once);

        ocrServiceMock.VerifyNoOtherCalls();
        snapshotFactoryMock.VerifyNoOtherCalls();
        freshnessGuardMock.VerifyNoOtherCalls();
        executionStateAccessorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateAsync_WhenPushFails_ShouldPropagateOriginalException()
    {
        var ocrServiceMock =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        var snapshotFactoryMock =
            new Mock<IOcrGroundedSnapshotFactory>(MockBehavior.Strict);

        var freshnessGuardMock =
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);

        var executionStateAccessorMock =
            new Mock<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict);

        OcrResult ocrResult =
            CreateOcrResult();

        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        ocrServiceMock
            .Setup(
                service => service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(ocrResult);

        snapshotFactoryMock
            .Setup(
                factory => factory.Create(
                    It.IsAny<string>(),
                    ocrResult))
            .Returns(snapshot);

        freshnessGuardMock
            .Setup(
                guard => guard.MarkCurrent(
                    snapshot.SnapshotId));

        executionStateAccessorMock
            .Setup(
                accessor => accessor.Push(
                    It.IsAny<OcrGroundedPlannerExecutionState>()))
            .Throws(
                new InvalidOperationException(
                    "Execution state push failed."));

        var sut =
            new OcrGroundedPlannerExecutionScopeFactory(
                ocrServiceMock.Object,
                snapshotFactoryMock.Object,
                freshnessGuardMock.Object,
                executionStateAccessorMock.Object);

        Func<Task> act =
            async () => _ = await sut.CreateAsync();

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Execution state push failed.");

        ocrServiceMock.Verify(
            service => service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);

        snapshotFactoryMock.Verify(
            factory => factory.Create(
                It.IsAny<string>(),
                ocrResult),
            Times.Once);

        freshnessGuardMock.Verify(
            guard => guard.MarkCurrent(
                snapshot.SnapshotId),
            Times.Once);

        executionStateAccessorMock.Verify(
            accessor => accessor.Push(
                It.Is<OcrGroundedPlannerExecutionState>(
                    state =>
                        ReferenceEquals(
                            state.Snapshot,
                            snapshot))),
            Times.Once);

        ocrServiceMock.VerifyNoOtherCalls();
        snapshotFactoryMock.VerifyNoOtherCalls();
        freshnessGuardMock.VerifyNoOtherCalls();
        executionStateAccessorMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateAsync_WhenCalledTwice_ShouldGenerateDifferentSnapshotIds()
    {
        var ocrServiceMock =
            new Mock<IAndroidScreenOcrService>(MockBehavior.Strict);

        var snapshotFactoryMock =
            new Mock<IOcrGroundedSnapshotFactory>(MockBehavior.Strict);

        var freshnessGuardMock =
            new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);

        var executionStateAccessorMock =
            new Mock<
                IAiToolExecutionStateAccessor<
                    OcrGroundedPlannerExecutionState>>(
                MockBehavior.Strict);

        OcrResult ocrResult =
            CreateOcrResult();

        var generatedIds =
            new List<string>();

        ocrServiceMock
            .Setup(
                service => service.RecognizeCurrentScreenAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(ocrResult);

        snapshotFactoryMock
            .Setup(
                factory => factory.Create(
                    It.IsAny<string>(),
                    ocrResult))
            .Returns(
                (string snapshotId, OcrResult _) =>
                {
                    generatedIds.Add(snapshotId);

                    return CreateSnapshot(snapshotId);
                });

        freshnessGuardMock
            .Setup(
                guard => guard.MarkCurrent(
                    It.IsAny<string>()));

        executionStateAccessorMock
            .Setup(
                accessor => accessor.Push(
                    It.IsAny<OcrGroundedPlannerExecutionState>()))
            .Returns(
                () =>
                {
                    var leaseMock =
                        new Mock<IDisposable>(
                            MockBehavior.Strict);

                    leaseMock
                        .Setup(lease => lease.Dispose());

                    return leaseMock.Object;
                });

        var sut =
            new OcrGroundedPlannerExecutionScopeFactory(
                ocrServiceMock.Object,
                snapshotFactoryMock.Object,
                freshnessGuardMock.Object,
                executionStateAccessorMock.Object);

        using IOcrGroundedPlannerExecutionScope first =
            await sut.CreateAsync();

        using IOcrGroundedPlannerExecutionScope second =
            await sut.CreateAsync();

        generatedIds
            .Should()
            .HaveCount(2)
            .And.OnlyHaveUniqueItems();

        generatedIds[0]
            .Should()
            .Be("ocr-snapshot-00000001");

        generatedIds[1]
            .Should()
            .Be("ocr-snapshot-00000002");

        ocrServiceMock.Verify(
            service => service.RecognizeCurrentScreenAsync(
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));

        snapshotFactoryMock.Verify(
            factory => factory.Create(
                It.IsAny<string>(),
                ocrResult),
            Times.Exactly(2));

        freshnessGuardMock.Verify(
            guard => guard.MarkCurrent(
                It.IsAny<string>()),
            Times.Exactly(2));

        executionStateAccessorMock.Verify(
            accessor => accessor.Push(
                It.IsAny<OcrGroundedPlannerExecutionState>()),
            Times.Exactly(2));

        ocrServiceMock.VerifyNoOtherCalls();
        snapshotFactoryMock.VerifyNoOtherCalls();
        freshnessGuardMock.VerifyNoOtherCalls();
        executionStateAccessorMock.VerifyNoOtherCalls();
    }

    private static OcrResult CreateOcrResult()
    {
        return new OcrResult(
            string.Empty,
            Array.Empty<OcrTextLine>());
    }

    private static OcrGroundedSnapshot CreateSnapshot(
        string snapshotId)
    {
        return new OcrGroundedSnapshot(
            snapshotId,
            Array.Empty<OcrGroundedTarget>());
    }
}
