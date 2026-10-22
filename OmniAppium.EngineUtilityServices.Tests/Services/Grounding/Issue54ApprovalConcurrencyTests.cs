using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OpenQA.Selenium;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue54ApprovalConcurrencyTests
{
    [Fact]
    public async Task ClickAsync_ConcurrentSameApproval_ShouldAllowOnlyOneDispatch()
    {
        var fixture = new Fixture();
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();

        fixture.Click.Setup(x => x.ClickAbsolute(15d, 25d)).Callback(() =>
        {
            entered.Set();
            if (!release.Wait(TimeSpan.FromSeconds(10)))
                throw new TimeoutException("Test dispatch release timed out.");
        });

        Task first = Task.Run(() => fixture.First.ClickAsync(fixture.Request));
        try
        {
            entered.Wait(TimeSpan.FromSeconds(10)).Should().BeTrue(
                "the first click must enter device dispatch before replay is attempted");

            Func<Task> competing = () => fixture.Second.ClickAsync(fixture.Request);
            await competing.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*already been claimed*");
        }
        finally
        {
            release.Set();
        }

        Func<Task> completeFirst = () => first;
        await completeFirst.Should().NotThrowAsync();

        fixture.ClaimCount.Should().Be(1);
        fixture.DispatchCount.Should().Be(1);
        fixture.Click.Verify(x => x.ClickAbsolute(15d, 25d), Times.Once);
        fixture.Lease.Verify(x => x.Dispose(), Times.Exactly(2));
    }

    [Fact]
    public async Task ClickAsync_ConcurrentDispatchFailure_ShouldNotRestoreApproval()
    {
        var fixture = new Fixture();
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();

        fixture.Click.Setup(x => x.ClickAbsolute(15d, 25d)).Callback(() =>
        {
            entered.Set();
            if (!release.Wait(TimeSpan.FromSeconds(10)))
                throw new TimeoutException("Test dispatch release timed out.");

            throw new WebDriverException("device dispatch outcome is unknown");
        });

        Task first = Task.Run(() => fixture.First.ClickAsync(fixture.Request));
        try
        {
            entered.Wait(TimeSpan.FromSeconds(10)).Should().BeTrue(
                "the first click must claim approval before the competing request");

            Func<Task> competing = () => fixture.Second.ClickAsync(fixture.Request);
            await competing.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*already been claimed*");
        }
        finally
        {
            release.Set();
        }

        Func<Task> firstFailure = () => first;
        await firstFailure.Should().ThrowAsync<WebDriverException>()
            .WithMessage("*device dispatch outcome is unknown*");

        Func<Task> replay = () => fixture.Second.ClickAsync(fixture.Request);
        await replay.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already been claimed*");

        fixture.ClaimCount.Should().Be(1);
        fixture.DispatchCount.Should().Be(1);
        fixture.Click.Verify(x => x.ClickAbsolute(15d, 25d), Times.Once);
        fixture.Lease.Verify(x => x.Dispose(), Times.Exactly(3));
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
        internal readonly TrustedGroundedClickService First;
        internal readonly TrustedGroundedClickService Second;

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

            First = new TrustedGroundedClickService(
                Click.Object, Guard.Object, Authorization.Object, Gate.Object);
            Second = new TrustedGroundedClickService(
                Click.Object, Guard.Object, Authorization.Object, Gate.Object);
        }
    }
}
