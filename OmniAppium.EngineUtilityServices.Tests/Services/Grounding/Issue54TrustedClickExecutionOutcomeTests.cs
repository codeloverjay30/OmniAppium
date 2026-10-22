
using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue54TrustedClickExecutionOutcomeTests
{
    [Fact]
    public async Task ClickAsync_ValidApproval_ShouldCompleteExactlyOneDispatch()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();

        int dispatchAttempts = 0;

        fixture.ConfigureExecutingGate(
            async (operation, cancellationToken) =>
            {
                Interlocked.Increment(ref dispatchAttempts);
                await operation(cancellationToken);
            });

        fixture.Click
            .Setup(click => click.ClickAbsolute(15d, 25d));

        await fixture.Sut.ClickAsync(fixture.Request);

        dispatchAttempts.Should().Be(1);

        fixture.VerifyGateCalledOnce();
        fixture.Click.Verify(
            click => click.ClickAbsolute(15d, 25d),
            Times.Once);

        fixture.Lease.Verify(
            lease => lease.Dispose(),
            Times.Once);

        fixture.VerifyNoUnexpectedCalls(2);
    }

    [Fact]
    public async Task ClickAsync_ReplayedApproval_ShouldRejectWithoutDispatch()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();

        fixture.ConfigureGateFailure(
            new InvalidOperationException(
                "Approval has already been claimed."));

        Func<Task> act = () =>
            fixture.Sut.ClickAsync(fixture.Request);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*Approval has already been claimed*");

        fixture.VerifyGateCalledOnce();
        fixture.Click.VerifyNoOtherCalls();

        fixture.Lease.Verify(
            lease => lease.Dispose(),
            Times.Once);

        fixture.VerifyNoUnexpectedCalls(1);
    }

    [Fact]
    public async Task ClickAsync_ClaimStoreFailure_ShouldRejectWithoutDispatch()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();

        fixture.ConfigureGateFailure(
            new IOException("Approval claim store unavailable."));

        Func<Task> act = () =>
            fixture.Sut.ClickAsync(fixture.Request);

        await act.Should()
            .ThrowAsync<IOException>()
            .WithMessage("*Approval claim store unavailable*");

        fixture.VerifyGateCalledOnce();
        fixture.Click.VerifyNoOtherCalls();

        fixture.Lease.Verify(
            lease => lease.Dispose(),
            Times.Once);

        fixture.VerifyNoUnexpectedCalls(1);
    }

    [Fact]
    public async Task ClickAsync_DeviceTimeout_ShouldPropagateAmbiguousFailureWithoutRetry()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();

        int dispatchAttempts = 0;

        fixture.ConfigureExecutingGate(
            async (operation, cancellationToken) =>
            {
                Interlocked.Increment(ref dispatchAttempts);
                await operation(cancellationToken);
            });

        fixture.Click
            .Setup(click => click.ClickAbsolute(15d, 25d))
            .Throws(new TimeoutException(
                "Device click outcome is unknown."));

        Func<Task> act = () =>
            fixture.Sut.ClickAsync(fixture.Request);

        await act.Should()
            .ThrowAsync<TimeoutException>()
            .WithMessage("*Device click outcome is unknown*");

        dispatchAttempts.Should().Be(1);

        fixture.VerifyGateCalledOnce();

        fixture.Click.Verify(
            click => click.ClickAbsolute(15d, 25d),
            Times.Once);

        fixture.Lease.Verify(
            lease => lease.Dispose(),
            Times.Once);

        fixture.VerifyNoUnexpectedCalls(2);
    }

    [Fact]
    public async Task ClickAsync_PreCancelledRequest_ShouldNotAuthorizeClaimOrDispatch()
    {
        var fixture = new Fixture();

        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();

        Func<Task> act = () =>
            fixture.Sut.ClickAsync(
                fixture.Request,
                cancellation.Token);

        await act.Should()
            .ThrowAsync<OperationCanceledException>();

        fixture.Guard.VerifyNoOtherCalls();
        fixture.Authorization.VerifyNoOtherCalls();
        fixture.Gate.VerifyNoOtherCalls();
        fixture.Click.VerifyNoOtherCalls();
        fixture.Lease.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ClickAsync_CancellationAfterAuthorization_ShouldNotEnterExecutionGate()
    {
        var fixture = new Fixture();

        using var cancellation = new CancellationTokenSource();

        fixture.ConfigureAuthorization(
            onAuthorized: () => cancellation.Cancel());

        Func<Task> act = () =>
            fixture.Sut.ClickAsync(
                fixture.Request,
                cancellation.Token);

        await act.Should()
            .ThrowAsync<OperationCanceledException>();

        fixture.Authorization.Verify(
            authorization => authorization.AuthorizeAsync(
                fixture.Request.Approval,
                "Target",
                fixture.Request.Confidence,
                It.IsAny<Rectangle>(),
                fixture.Request.ReviewPath,
                fixture.Request.AnnotationPath,
                fixture.Request.ScreenshotPath,
                fixture.Request.TrustedReviewSha256,
                fixture.Request.TrustedAnnotationSha256,
                fixture.Request.TrustedScreenshotSha256,
                cancellation.Token),
            Times.Once);

        fixture.Gate.VerifyNoOtherCalls();
        fixture.Click.VerifyNoOtherCalls();

        fixture.Lease.Verify(
            lease => lease.Dispose(),
            Times.Once);

        fixture.VerifyNoUnexpectedCalls(0);
    }

    [Fact]
    public async Task ClickAsync_CancellationAfterDispatchStarted_ShouldNotRetry()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();

        using var cancellation = new CancellationTokenSource();

        int dispatchAttempts = 0;

        fixture.ConfigureExecutingGate(
            async (operation, cancellationToken) =>
            {
                Interlocked.Increment(ref dispatchAttempts);
                await operation(cancellationToken);
            });

        fixture.Click
            .Setup(click => click.ClickAbsolute(15d, 25d))
            .Callback(() => cancellation.Cancel());

        Func<Task> act = () =>
            fixture.Sut.ClickAsync(
                fixture.Request,
                cancellation.Token);

        // The synchronous device operation has already been invoked.
        // The current production callback does not recheck cancellation
        // after ClickAbsolute returns, so completion is expected here.
        await act.Should().NotThrowAsync();

        cancellation.IsCancellationRequested.Should().BeTrue();
        dispatchAttempts.Should().Be(1);

        fixture.VerifyGateCalledOnce();

        fixture.Click.Verify(
            click => click.ClickAbsolute(15d, 25d),
            Times.Once);

        fixture.Lease.Verify(
            lease => lease.Dispose(),
            Times.Once);

        fixture.VerifyNoUnexpectedCalls(2);
    }

    private sealed class Fixture
    {
        internal readonly Mock<IClickService> Click =
            new(MockBehavior.Strict);

        internal readonly Mock<IOcrGroundedSnapshotFreshnessGuard> Guard =
            new(MockBehavior.Strict);

        internal readonly Mock<IOcrGroundedActionLease> Lease =
            new(MockBehavior.Strict);

        internal readonly Mock<ITrustedReviewIntegrityAuthorizationService> Authorization =
            new(MockBehavior.Strict);

        internal readonly Mock<ITrustedApprovalExecutionGate> Gate =
            new(MockBehavior.Strict);

        internal readonly TrustedGroundedClickRequest Request;

        internal readonly TrustedGroundedClickService Sut;

        internal Fixture()
        {
            const string digest =
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

            var approval = new TrustedReviewApprovalEvidence(
                "reviewer",
                "key",
                digest,
                digest,
                digest,
                DateTimeOffset.UtcNow.AddMinutes(-1),
                DateTimeOffset.UtcNow.AddMinutes(5),
                "signature");

            Request = new TrustedGroundedClickRequest(
                new OcrGroundedSnapshot(
                    "snapshot",
                    [
                        new OcrGroundedTarget(
                            "target",
                            "Target",
                            Rectangle.FromXYWH(10, 20, 10, 10))
                    ]),
                "target",
                approval,
                "review.json",
                "annotation.json",
                "screen.png",
                digest,
                digest,
                digest,
                0.99f);

            Sut = new TrustedGroundedClickService(
                Click.Object,
                Guard.Object,
                Authorization.Object,
                Gate.Object);
        }

        internal void ConfigureAuthorization(Action? onAuthorized = null)
        {
            Guard
                .Setup(guard => guard.AcquireCurrent(Request.Snapshot))
                .Returns(Lease.Object);

            Guard
                .Setup(guard => guard.ThrowIfStale(Request.Snapshot));

            Lease
                .Setup(lease => lease.Dispose());

            Authorization
                .Setup(authorization => authorization.AuthorizeAsync(
                    Request.Approval,
                    "Target",
                    Request.Confidence,
                    It.IsAny<Rectangle>(),
                    Request.ReviewPath,
                    Request.AnnotationPath,
                    Request.ScreenshotPath,
                    Request.TrustedReviewSha256,
                    Request.TrustedAnnotationSha256,
                    Request.TrustedScreenshotSha256,
                    It.IsAny<CancellationToken>()))
                .Returns(() =>
                {
                    onAuthorized?.Invoke();

                    return Task.FromResult((15d, 25d));
                });
        }

        internal void ConfigureExecutingGate(
            Func<Func<CancellationToken, Task>, CancellationToken, Task>
                execute)
        {
            Gate
                .Setup(gate => gate.ExecuteAsync(
                    Request.Approval,
                    Request.TrustedReviewSha256,
                    Request.TrustedAnnotationSha256,
                    Request.TrustedScreenshotSha256,
                    It.IsAny<Func<CancellationToken, Task>>(),
                    It.IsAny<CancellationToken>()))
                .Returns((
                    TrustedReviewApprovalEvidence _,
                    string _,
                    string _,
                    string _,
                    Func<CancellationToken, Task> operation,
                    CancellationToken cancellationToken) =>
                    execute(operation, cancellationToken));
        }

        internal void ConfigureGateFailure(Exception error)
        {
            Gate
                .Setup(gate => gate.ExecuteAsync(
                    Request.Approval,
                    Request.TrustedReviewSha256,
                    Request.TrustedAnnotationSha256,
                    Request.TrustedScreenshotSha256,
                    It.IsAny<Func<CancellationToken, Task>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(error);
        }

        internal void VerifyGateCalledOnce()
        {
            Gate.Verify(
                gate => gate.ExecuteAsync(
                    Request.Approval,
                    Request.TrustedReviewSha256,
                    Request.TrustedAnnotationSha256,
                    Request.TrustedScreenshotSha256,
                    It.IsAny<Func<CancellationToken, Task>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        internal void VerifyNoUnexpectedCalls(int expectedFreshnessChecks)
        {
            Guard.Verify(
                guard => guard.AcquireCurrent(Request.Snapshot),
                Times.Once);

            Guard.Verify(
                guard => guard.ThrowIfStale(Request.Snapshot),
                Times.Exactly(expectedFreshnessChecks));

            Authorization.Verify(
                authorization => authorization.AuthorizeAsync(
                    Request.Approval,
                    "Target",
                    Request.Confidence,
                    It.IsAny<Rectangle>(),
                    Request.ReviewPath,
                    Request.AnnotationPath,
                    Request.ScreenshotPath,
                    Request.TrustedReviewSha256,
                    Request.TrustedAnnotationSha256,
                    Request.TrustedScreenshotSha256,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            Guard.VerifyNoOtherCalls();
            Authorization.VerifyNoOtherCalls();
            Gate.VerifyNoOtherCalls();
            Click.VerifyNoOtherCalls();
            Lease.VerifyNoOtherCalls();
        }
    }
}