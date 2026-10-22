using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OpenQA.Selenium;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue54ApprovalDispatchFailureTests
{
    [Fact]
    public async Task ClickAsync_WhenDispatchFails_ShouldConsumeApprovalAndRejectReplay()
    {
        var fixture = new Fixture();
        var dispatchFailure = new WebDriverException("device dispatch rejected");
        fixture.Click.Setup(x => x.ClickAbsolute(15d, 25d)).Throws(dispatchFailure);

        Func<Task> first = () => fixture.Sut.ClickAsync(fixture.Request);
        await first.Should().ThrowAsync<WebDriverException>()
            .WithMessage("*device dispatch rejected*");

        fixture.ClaimCount.Should().Be(1);
        fixture.DispatchCount.Should().Be(1);

        Func<Task> replay = () => fixture.Sut.ClickAsync(fixture.Request);
        await replay.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already been claimed*");

        fixture.ClaimCount.Should().Be(1);
        fixture.DispatchCount.Should().Be(1);
        fixture.Click.Verify(x => x.ClickAbsolute(15d, 25d), Times.Once);
        fixture.Lease.Verify(x => x.Dispose(), Times.Exactly(2));
    }

    [Fact]
    public async Task ClickAsync_WhenDispatchSucceeds_ShouldStillRejectReplay()
    {
        var fixture = new Fixture();
        fixture.Click.Setup(x => x.ClickAbsolute(15d, 25d));

        Func<Task> first = () => fixture.Sut.ClickAsync(fixture.Request);
        await first.Should().NotThrowAsync();

        Func<Task> replay = () => fixture.Sut.ClickAsync(fixture.Request);
        await replay.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already been claimed*");

        fixture.ClaimCount.Should().Be(1);
        fixture.DispatchCount.Should().Be(1);
        fixture.Click.Verify(x => x.ClickAbsolute(15d, 25d), Times.Once);
        fixture.Lease.Verify(x => x.Dispose(), Times.Exactly(2));
    }

    private sealed class Fixture
    {
        private int _claimed;
        private int _claimCount;
        private int _dispatchCount;

        internal readonly Mock<IClickService> Click = new(MockBehavior.Strict);
        internal readonly Mock<IOcrGroundedSnapshotFreshnessGuard> Guard = new(MockBehavior.Strict);
        internal readonly Mock<IOcrGroundedActionLease> Lease = new(MockBehavior.Strict);
        internal readonly Mock<ITrustedReviewIntegrityAuthorizationService> Authorization = new(MockBehavior.Strict);
        internal readonly Mock<ITrustedApprovalExecutionGate> Gate = new(MockBehavior.Strict);

        internal readonly TrustedGroundedClickRequest Request;
        internal readonly TrustedGroundedClickService Sut;

        internal int ClaimCount => Volatile.Read(ref _claimCount);
        internal int DispatchCount => Volatile.Read(ref _dispatchCount);

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

            Guard.Setup(x => x.AcquireCurrent(Request.Snapshot)).Returns(Lease.Object);
            Guard.Setup(x => x.ThrowIfStale(Request.Snapshot));
            Lease.Setup(x => x.Dispose());

            Authorization.Setup(x => x.AuthorizeAsync(
                    Request.Approval, "Target", Request.Confidence, It.IsAny<Rectangle>(),
                    Request.ReviewPath, Request.AnnotationPath, Request.ScreenshotPath,
                    Request.TrustedReviewSha256, Request.TrustedAnnotationSha256,
                    Request.TrustedScreenshotSha256, It.IsAny<CancellationToken>()))
                .ReturnsAsync((15d, 25d));

            // Stateful test double: claims before dispatch and never restores the claim.
            Gate.Setup(x => x.ExecuteAsync(
                    Request.Approval, Request.TrustedReviewSha256,
                    Request.TrustedAnnotationSha256, Request.TrustedScreenshotSha256,
                    It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
                .Returns((TrustedReviewApprovalEvidence _, string _, string _, string _,
                    Func<CancellationToken, Task> operation, CancellationToken ct) =>
                {
                    ct.ThrowIfCancellationRequested();
                    if (Interlocked.CompareExchange(ref _claimed, 1, 0) != 0)
                        throw new InvalidOperationException("Approval has already been claimed.");

                    Interlocked.Increment(ref _claimCount);
                    Interlocked.Increment(ref _dispatchCount);
                    return operation(ct);
                });

            Sut = new TrustedGroundedClickService(
                Click.Object, Guard.Object, Authorization.Object, Gate.Object);
        }
    }
}
