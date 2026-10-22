using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue50EngineCurrentBoundaryTests
{
    [Fact]
    public async Task ClickAsync_MissingTarget_ShouldRejectBeforeAuthorizationOrDeviceInput()
    {
        var fixture = new Fixture();
        var request = fixture.Request with { TargetId = "missing" };

        Func<Task> act = () => fixture.Sut.ClickAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Grounded target must exist exactly once*");
        fixture.Guard.VerifyNoOtherCalls();
        fixture.Authorization.VerifyNoOtherCalls();
        fixture.Click.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ClickAsync_StaleSnapshotAfterAuthorization_ShouldNeverClickAndReleaseLease()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorized();
        fixture.Guard.Setup(g => g.ThrowIfStale(fixture.Request.Snapshot))
            .Throws(new StaleOcrGroundedSnapshotException("snapshot", "new-snapshot"));

        Func<Task> act = () => fixture.Sut.ClickAsync(fixture.Request);

        await act.Should().ThrowAsync<StaleOcrGroundedSnapshotException>()
            .WithMessage("*snapshot*stale*new-snapshot*");
        fixture.Click.VerifyNoOtherCalls();
        fixture.Gate.VerifyNoOtherCalls();
        fixture.Lease.Verify(l => l.Dispose(), Times.Once);
    }

    [Theory]
    [InlineData(double.NaN, 25d)]
    [InlineData(double.PositiveInfinity, 25d)]
    [InlineData(15d, double.NegativeInfinity)]
    public async Task ClickAsync_NonFiniteAuthorizedCoordinate_ShouldNeverClick(
        double x, double y)
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorized(x, y);

        Func<Task> act = () => fixture.Sut.ClickAsync(fixture.Request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Authorized coordinates must be finite*");
        fixture.Click.VerifyNoOtherCalls();
        fixture.Gate.VerifyNoOtherCalls();
        fixture.Lease.Verify(l => l.Dispose(), Times.Once);
    }

    [Fact]
    public async Task ClickAsync_DeviceFailure_ShouldNotRetryAndShouldReleaseLease()
    {
        var fixture = new Fixture();
        fixture.ConfigureAuthorized();
        fixture.Gate.Setup(g => g.ExecuteAsync(
                fixture.Request.Approval, fixture.Request.TrustedReviewSha256,
                fixture.Request.TrustedAnnotationSha256, fixture.Request.TrustedScreenshotSha256,
                It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((TrustedReviewApprovalEvidence _, string _, string _, string _,
                Func<CancellationToken, Task> operation, CancellationToken ct) => operation(ct));
        fixture.Click.Setup(c => c.ClickAbsolute(15d, 25d))
            .Throws(new InvalidOperationException("Device dispatch failed."));

        Func<Task> act = () => fixture.Sut.ClickAsync(fixture.Request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Device dispatch failed*");
        fixture.Click.Verify(c => c.ClickAbsolute(15d, 25d), Times.Once);
        fixture.Gate.Verify(g => g.ExecuteAsync(
            fixture.Request.Approval, fixture.Request.TrustedReviewSha256,
            fixture.Request.TrustedAnnotationSha256, fixture.Request.TrustedScreenshotSha256,
            It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        fixture.Click.VerifyNoOtherCalls();
        fixture.Gate.VerifyNoOtherCalls();
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
            Sut = new TrustedGroundedClickService(
                Click.Object, Guard.Object, Authorization.Object, Gate.Object);
        }

        internal void ConfigureAuthorized(double x = 15d, double y = 25d)
        {
            Guard.Setup(g => g.AcquireCurrent(Request.Snapshot)).Returns(Lease.Object);
            Guard.Setup(g => g.ThrowIfStale(Request.Snapshot));
            Lease.Setup(l => l.Dispose());
            Authorization.Setup(a => a.AuthorizeAsync(
                Request.Approval, "Target", Request.Confidence, It.IsAny<Rectangle>(),
                Request.ReviewPath, Request.AnnotationPath, Request.ScreenshotPath,
                Request.TrustedReviewSha256, Request.TrustedAnnotationSha256,
                Request.TrustedScreenshotSha256, It.IsAny<CancellationToken>()))
                .ReturnsAsync((x, y));
        }
    }
}
