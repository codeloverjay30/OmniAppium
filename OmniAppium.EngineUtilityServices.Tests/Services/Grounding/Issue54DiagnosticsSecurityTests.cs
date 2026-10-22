using System.Text.Json;
using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue54DiagnosticsSecurityTests
{
    private const string SecretSignature = "PRIVATE_SIGNATURE_ISSUE54";
    private const string SecretDigest = "PRIVATE_DIGEST_ISSUE54";
    private const string SecretPath = "PRIVATE_PATH_ISSUE54";
    private const string SecretException = "PRIVATE_EXCEPTION_MESSAGE_ISSUE54";

    [Fact]
    public async Task ExecuteWithOutcomeAsync_TransportFailure_ShouldExposeOnlyBoundedDiagnostics()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureExecutingGate((operation, ct) => operation(ct));
        fixture.Click.Setup(click => click.ClickAbsolute(15d, 25d))
            .Throws(new TimeoutException(
                $"{SecretSignature} {SecretDigest} {SecretPath} {SecretException}"));

        TrustedClickExecutionOutcome outcome =
            await fixture.Sut.ExecuteWithOutcomeAsync(fixture.Request, TestContext.Current.CancellationToken);

        outcome.Status.Should().Be(TrustedClickExecutionStatus.DispatchFailedOrAmbiguous);
        outcome.FailureCategory.Should().Be(TrustedClickFailureCategory.Timeout);
        outcome.DispatchAttempted.Should().BeTrue();
        VerifySanitized(outcome);
        fixture.Click.Verify(click => click.ClickAbsolute(15d, 25d), Times.Once);
        fixture.VerifyGateCalledOnce();
        fixture.Lease.Verify(lease => lease.Dispose(), Times.Once);
        fixture.VerifyNoUnexpectedCalls(2);
    }

    [Fact]
    public async Task ExecuteWithOutcomeAsync_ClaimStoreFailure_ShouldNeverClaimPreConsumptionCertainty()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureGateFailure(new IOException(
            $"{SecretSignature} {SecretDigest} {SecretPath} {SecretException}"));

        TrustedClickExecutionOutcome outcome =
            await fixture.Sut.ExecuteWithOutcomeAsync(fixture.Request, TestContext.Current.CancellationToken);

        outcome.Status.Should().Be(TrustedClickExecutionStatus.RejectedOrConsumedUndetermined);
        outcome.FailureCategory.Should().Be(TrustedClickFailureCategory.Unexpected);
        outcome.DispatchAttempted.Should().BeFalse();
        VerifySanitized(outcome);
        fixture.Click.VerifyNoOtherCalls();
        fixture.VerifyGateCalledOnce();
        fixture.Lease.Verify(lease => lease.Dispose(), Times.Once);
        fixture.VerifyNoUnexpectedCalls(1);
    }

    [Fact]
    public async Task ExecuteWithOutcomeAsync_UnknownGateFailure_ShouldNotReportApprovalUnconsumed()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureGateFailure(new InvalidOperationException(
            $"{SecretSignature} {SecretException}"));

        TrustedClickExecutionOutcome outcome =
            await fixture.Sut.ExecuteWithOutcomeAsync(fixture.Request, TestContext.Current.CancellationToken);

        outcome.Status.Should().Be(TrustedClickExecutionStatus.RejectedOrConsumedUndetermined);
        outcome.FailureCategory.Should().Be(TrustedClickFailureCategory.AuthorizationOrClaim);
        outcome.DispatchAttempted.Should().BeFalse();
        VerifySanitized(outcome);
        fixture.Click.VerifyNoOtherCalls();
        fixture.VerifyGateCalledOnce();
        fixture.Lease.Verify(lease => lease.Dispose(), Times.Once);
        fixture.VerifyNoUnexpectedCalls(1);
    }

    [Fact]
    public async Task ExecuteWithOutcomeAsync_Replay_ShouldReportConsumedWithoutSensitivePayload()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureGateFailure(new InvalidOperationException("Approval has already been claimed."));

        TrustedClickExecutionOutcome outcome =
            await fixture.Sut.ExecuteWithOutcomeAsync(fixture.Request, TestContext.Current.CancellationToken);

        outcome.Status.Should().Be(TrustedClickExecutionStatus.RejectedAlreadyConsumed);
        outcome.FailureCategory.Should().Be(TrustedClickFailureCategory.AuthorizationOrClaim);
        outcome.DispatchAttempted.Should().BeFalse();
        VerifySanitized(outcome);
        fixture.Click.VerifyNoOtherCalls();
        fixture.VerifyGateCalledOnce();
        fixture.Lease.Verify(lease => lease.Dispose(), Times.Once);
        fixture.VerifyNoUnexpectedCalls(1);
    }

    [Fact]
    public async Task ExecuteWithOutcomeAsync_PreCancelled_ShouldNotEnterGateOrDispatch()
    {
        var fixture = new Fixture();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        TrustedClickExecutionOutcome outcome =
            await fixture.Sut.ExecuteWithOutcomeAsync(fixture.Request, cancellation.Token);

        outcome.Status.Should().Be(TrustedClickExecutionStatus.RejectedBeforeConsumption);
        outcome.FailureCategory.Should().Be(TrustedClickFailureCategory.Cancellation);
        outcome.DispatchAttempted.Should().BeFalse();
        VerifySanitized(outcome);
        fixture.Guard.VerifyNoOtherCalls();
        fixture.Authorization.VerifyNoOtherCalls();
        fixture.Gate.VerifyNoOtherCalls();
        fixture.Click.VerifyNoOtherCalls();
        fixture.Lease.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ExecuteWithOutcomeAsync_DeviceCancellation_ShouldRemainAmbiguousAndNeverRetry()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureExecutingGate((operation, ct) => operation(ct));
        fixture.Click.Setup(click => click.ClickAbsolute(15d, 25d))
            .Throws(new OperationCanceledException(SecretException));

        TrustedClickExecutionOutcome outcome =
            await fixture.Sut.ExecuteWithOutcomeAsync(fixture.Request, TestContext.Current.CancellationToken);

        outcome.Status.Should().Be(TrustedClickExecutionStatus.DispatchFailedOrAmbiguous);
        outcome.FailureCategory.Should().Be(TrustedClickFailureCategory.Cancellation);
        outcome.DispatchAttempted.Should().BeTrue();
        VerifySanitized(outcome);
        fixture.Click.Verify(click => click.ClickAbsolute(15d, 25d), Times.Once);
        fixture.VerifyGateCalledOnce();
        fixture.Lease.Verify(lease => lease.Dispose(), Times.Once);
        fixture.VerifyNoUnexpectedCalls(2);
    }

    private static void VerifySanitized(TrustedClickExecutionOutcome outcome)
    {
        string[] publicPropertyNames = typeof(TrustedClickExecutionOutcome)
            .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
            .Select(property => property.Name)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        publicPropertyNames.Should().BeEquivalentTo(
            new[] { "Status", "FailureCategory", "DispatchAttempted" });

        string representation = outcome.ToString() + JsonSerializer.Serialize(outcome);
        foreach (string secret in new[] {
            SecretSignature, SecretDigest, SecretPath, SecretException,
            "signature", "review.json", "annotation.json", "screen.png",
            "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
        })
        {
            representation.Should().NotContain(secret);
        }
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
