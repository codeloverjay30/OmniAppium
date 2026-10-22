using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue54ProductionExecutionOutcomeTests
{
    [Fact]
    public async Task ExecuteWithOutcomeAsync_Completed_ShouldReportDispatchCompleted()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureExecutingGate((operation, ct) => operation(ct));
        fixture.Click.Setup(click => click.ClickAbsolute(15d, 25d));

        TrustedClickExecutionOutcome result =
            await fixture.Sut.ExecuteWithOutcomeAsync(fixture.Request);

        result.Status.Should().Be(TrustedClickExecutionStatus.DispatchCompleted);
        result.FailureCategory.Should().Be(TrustedClickFailureCategory.None);
        result.DispatchAttempted.Should().BeTrue();
        fixture.Click.Verify(click => click.ClickAbsolute(15d, 25d), Times.Once);
        fixture.VerifyGateCalledOnce();
        fixture.Lease.Verify(lease => lease.Dispose(), Times.Once);
        fixture.VerifyNoUnexpectedCalls(2);
    }

    [Fact]
    public async Task ExecuteWithOutcomeAsync_Replay_ShouldReportConsumedWithoutDispatch()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureGateFailure(new InvalidOperationException(
            "Approval has already been claimed."));

        TrustedClickExecutionOutcome result =
            await fixture.Sut.ExecuteWithOutcomeAsync(fixture.Request);

        result.Status.Should().Be(TrustedClickExecutionStatus.RejectedAlreadyConsumed);
        result.DispatchAttempted.Should().BeFalse();
        fixture.Click.VerifyNoOtherCalls();
        fixture.VerifyGateCalledOnce();
        fixture.Lease.Verify(lease => lease.Dispose(), Times.Once);
        fixture.VerifyNoUnexpectedCalls(1);
    }

    [Fact]
    public async Task ExecuteWithOutcomeAsync_DeviceFailure_ShouldNotLeakOrRetry()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureExecutingGate((operation, ct) => operation(ct));
        fixture.Click.Setup(click => click.ClickAbsolute(15d, 25d))
            .Throws(new TimeoutException("PRIVATE_APPROVAL_SIGNATURE"));

        TrustedClickExecutionOutcome result =
            await fixture.Sut.ExecuteWithOutcomeAsync(fixture.Request);

        result.Status.Should().Be(TrustedClickExecutionStatus.DispatchFailedOrAmbiguous);
        result.FailureCategory.Should().Be(TrustedClickFailureCategory.Timeout);
        result.DispatchAttempted.Should().BeTrue();
        result.ToString().Should().NotContain("PRIVATE_APPROVAL_SIGNATURE");
        fixture.Click.Verify(click => click.ClickAbsolute(15d, 25d), Times.Once);
        fixture.VerifyGateCalledOnce();
        fixture.Lease.Verify(lease => lease.Dispose(), Times.Once);
        fixture.VerifyNoUnexpectedCalls(2);
    }

    [Fact]
    public async Task ExecuteWithOutcomeAsync_ClaimFailure_ShouldRemainConservative()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureGateFailure(new IOException("PRIVATE_CLAIM_PATH"));

        TrustedClickExecutionOutcome result =
            await fixture.Sut.ExecuteWithOutcomeAsync(fixture.Request);

        result.Status.Should().Be(TrustedClickExecutionStatus.RejectedOrConsumedUndetermined);
        result.DispatchAttempted.Should().BeFalse();
        result.ToString().Should().NotContain("PRIVATE_CLAIM_PATH");
        fixture.VerifyGateCalledOnce();
        fixture.Lease.Verify(lease => lease.Dispose(), Times.Once);
        fixture.VerifyNoUnexpectedCalls(1);
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
