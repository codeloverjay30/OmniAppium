using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue50TrustedClickExecutionGateTests
{
    [Fact]
    public async Task ClickAsync_ValidApproval_ShouldDispatchOnlyInsideExecutionGate()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.Gate.Setup(g => g.ExecuteAsync(
                fixture.Request.Approval, fixture.Request.TrustedReviewSha256,
                fixture.Request.TrustedAnnotationSha256, fixture.Request.TrustedScreenshotSha256,
                It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((TrustedReviewApprovalEvidence _, string _, string _, string _,
                Func<CancellationToken, Task> operation, CancellationToken ct) =>
            {
                fixture.Click.VerifyNoOtherCalls();
                return operation(ct);
            });
        fixture.Click.Setup(c => c.ClickAbsolute(15d, 25d));

        await fixture.Sut.ClickAsync(fixture.Request);

        fixture.Gate.Verify(g => g.ExecuteAsync(
            fixture.Request.Approval, fixture.Request.TrustedReviewSha256,
            fixture.Request.TrustedAnnotationSha256, fixture.Request.TrustedScreenshotSha256,
            It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        fixture.Click.Verify(c => c.ClickAbsolute(15d, 25d), Times.Once);
        fixture.Lease.Verify(l => l.Dispose(), Times.Once);
    }

    [Fact]
    public async Task ClickAsync_ReplayedApproval_ShouldNeverDispatch()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureGateFailure(new InvalidOperationException("Approval has already been claimed."));

        Func<Task> act = () => fixture.Sut.ClickAsync(fixture.Request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already been claimed*");
        fixture.Click.VerifyNoOtherCalls();
        fixture.Lease.Verify(l => l.Dispose(), Times.Once);
    }

    [Fact]
    public async Task ClickAsync_ClaimStoreFailure_ShouldFailClosed()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureGateFailure(new IOException("Approval claim store unavailable."));

        Func<Task> act = () => fixture.Sut.ClickAsync(fixture.Request);

        await act.Should().ThrowAsync<IOException>()
            .WithMessage("*claim store unavailable*");
        fixture.Click.VerifyNoOtherCalls();
        fixture.Lease.Verify(l => l.Dispose(), Times.Once);
    }

    [Fact]
    public async Task ClickAsync_InvalidSignature_ShouldNeverDispatch()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.ConfigureGateFailure(new InvalidOperationException("Approval signature is invalid."));

        Func<Task> act = () => fixture.Sut.ClickAsync(fixture.Request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*signature is invalid*");
        fixture.Click.VerifyNoOtherCalls();
        fixture.Lease.Verify(l => l.Dispose(), Times.Once);
    }

    [Fact]
    public async Task ClickAsync_AmbiguousDeviceFailure_ShouldNotRetryGateOrClick()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorization();
        fixture.Gate.Setup(g => g.ExecuteAsync(
                fixture.Request.Approval, fixture.Request.TrustedReviewSha256,
                fixture.Request.TrustedAnnotationSha256, fixture.Request.TrustedScreenshotSha256,
                It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((TrustedReviewApprovalEvidence _, string _, string _, string _,
                Func<CancellationToken, Task> operation, CancellationToken ct) => operation(ct));
        fixture.Click.Setup(c => c.ClickAbsolute(15d, 25d))
            .Throws(new TimeoutException("Device click outcome is unknown."));

        Func<Task> act = () => fixture.Sut.ClickAsync(fixture.Request);

        await act.Should().ThrowAsync<TimeoutException>()
            .WithMessage("*outcome is unknown*");
        fixture.Gate.Verify(g => g.ExecuteAsync(
            fixture.Request.Approval, fixture.Request.TrustedReviewSha256,
            fixture.Request.TrustedAnnotationSha256, fixture.Request.TrustedScreenshotSha256,
            It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        fixture.Click.Verify(c => c.ClickAbsolute(15d, 25d), Times.Once);
        fixture.Lease.Verify(l => l.Dispose(), Times.Once);
    }

    private sealed class Fixture
    {
        internal readonly Mock<IClickService> Click = new(MockBehavior.Strict);
        internal readonly Mock<IOcrGroundedSnapshotFreshnessGuard> Guard = new(MockBehavior.Strict);
        internal readonly Mock<IOcrGroundedActionLease> Lease = new(MockBehavior.Strict);
        internal readonly Mock<ITrustedReviewIntegrityAuthorizationService> Authorization = new(MockBehavior.Strict);
        internal readonly Mock<ITrustedApprovalExecutionGate> Gate = new(MockBehavior.Strict);
        internal readonly TrustedGroundedClickRequest Request;
        internal readonly TrustedGroundedClickService Sut;

        internal Fixture()
        {
            const string digest = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            var approval = new TrustedReviewApprovalEvidence(
                "reviewer", "key", digest, digest, digest,
                DateTimeOffset.UtcNow.AddMinutes(-1),
                DateTimeOffset.UtcNow.AddMinutes(5), "signature");
            Request = new TrustedGroundedClickRequest(
                new OcrGroundedSnapshot("snapshot", [
                    new OcrGroundedTarget("target", "Target", Rectangle.FromXYWH(10, 20, 10, 10))]),
                "target", approval, "review.json", "annotation.json", "screen.png",
                digest, digest, digest, 0.99f);

            // Expected Phase 3 production contract: add a required gate dependency.
            Sut = new TrustedGroundedClickService(
                Click.Object, Guard.Object, Authorization.Object, Gate.Object);
        }

        internal void ConfigureAuthorization()
        {
            Guard.Setup(g => g.AcquireCurrent(Request.Snapshot)).Returns(Lease.Object);
            Guard.Setup(g => g.ThrowIfStale(Request.Snapshot));
            Lease.Setup(l => l.Dispose());
            Authorization.Setup(a => a.AuthorizeAsync(
                    Request.Approval, "Target", Request.Confidence, It.IsAny<Rectangle>(),
                    Request.ReviewPath, Request.AnnotationPath, Request.ScreenshotPath,
                    Request.TrustedReviewSha256, Request.TrustedAnnotationSha256,
                    Request.TrustedScreenshotSha256, It.IsAny<CancellationToken>()))
                .ReturnsAsync((15d, 25d));
        }

        internal void ConfigureGateFailure(Exception error)
        {
            Gate.Setup(g => g.ExecuteAsync(
                    Request.Approval, Request.TrustedReviewSha256,
                    Request.TrustedAnnotationSha256, Request.TrustedScreenshotSha256,
                    It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(error);
        }
    }
}
