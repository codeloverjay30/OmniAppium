using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue48TrustedDeviceClickBoundaryTests
{
    [Fact]
    public async Task ClickAsync_WhenAuthorizationFails_ShouldNeverClick()
    {
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var guard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);
        var lease = new Mock<IOcrGroundedActionLease>(MockBehavior.Strict);
        var auth = new Mock<ITrustedReviewIntegrityAuthorizationService>(MockBehavior.Strict);
        TrustedGroundedClickRequest request = CreateRequest();
        guard.Setup(g => g.AcquireCurrent(request.Snapshot)).Returns(lease.Object);
        lease.Setup(l => l.Dispose());
        auth.Setup(a => a.AuthorizeAsync(
            request.Approval, "Target", request.Confidence, It.IsAny<Rectangle>(),
            request.ReviewPath, request.AnnotationPath, request.ScreenshotPath,
            request.TrustedReviewSha256, request.TrustedAnnotationSha256,
            request.TrustedScreenshotSha256, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Approval signature is invalid."));

        var sut = new TrustedGroundedClickService(click.Object, guard.Object, auth.Object);
        Func<Task> act = () => sut.ClickAsync(request);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Approval signature is invalid*");
        click.Verify(c => c.ClickAbsolute(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        click.VerifyNoOtherCalls();
        lease.Verify(l => l.Dispose(), Times.Once);
    }

    [Fact]
    public async Task ClickAsync_WhenAlreadyCancelled_ShouldNeverTouchDevice()
    {
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var guard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);
        var auth = new Mock<ITrustedReviewIntegrityAuthorizationService>(MockBehavior.Strict);
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var sut = new TrustedGroundedClickService(click.Object, guard.Object, auth.Object);
        Func<Task> act = () => sut.ClickAsync(CreateRequest(), cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
        click.VerifyNoOtherCalls();
        guard.VerifyNoOtherCalls();
        auth.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ClickAsync_WhenAuthorizationSucceeds_ShouldClickOnceAtAuthorizedCoordinates()
    {
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var guard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);
        var lease = new Mock<IOcrGroundedActionLease>(MockBehavior.Strict);
        var auth = new Mock<ITrustedReviewIntegrityAuthorizationService>(MockBehavior.Strict);
        TrustedGroundedClickRequest request = CreateRequest();
        guard.Setup(g => g.AcquireCurrent(request.Snapshot)).Returns(lease.Object);
        guard.Setup(g => g.ThrowIfStale(request.Snapshot));
        lease.Setup(l => l.Dispose());
        auth.Setup(a => a.AuthorizeAsync(
            request.Approval, "Target", request.Confidence, It.IsAny<Rectangle>(),
            request.ReviewPath, request.AnnotationPath, request.ScreenshotPath,
            request.TrustedReviewSha256, request.TrustedAnnotationSha256,
            request.TrustedScreenshotSha256, It.IsAny<CancellationToken>()))
            .ReturnsAsync((15d, 25d));
        click.Setup(c => c.ClickAbsolute(15d, 25d));
        var sut = new TrustedGroundedClickService(click.Object, guard.Object, auth.Object);
        await sut.ClickAsync(request);
        click.Verify(c => c.ClickAbsolute(15d, 25d), Times.Once);
        click.VerifyNoOtherCalls();
        lease.Verify(l => l.Dispose(), Times.Once);
    }

    private static TrustedGroundedClickRequest CreateRequest()
    {
        const string digest = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
        var approval = new TrustedReviewApprovalEvidence(
            "reviewer", "key", digest, digest, digest,
            DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddMinutes(5),
            "signature");
        return new TrustedGroundedClickRequest(
            new OcrGroundedSnapshot("snapshot", [
                new OcrGroundedTarget("target", "Target", Rectangle.FromXYWH(10, 20, 10, 10))]),
            "target", approval, "review.json", "annotation.json", "screen.png",
            digest, digest, digest, 0.99f);
    }
}
