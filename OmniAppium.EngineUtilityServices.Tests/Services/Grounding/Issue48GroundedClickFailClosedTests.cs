using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue48GroundedClickFailClosedTests
{
    [Fact]
    public void Click_WhenAtomicEvidenceIsUnavailable_ShouldNeverClickAndReleaseLease()
    {
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var guard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);
        var lease = new Mock<IOcrGroundedActionLease>(MockBehavior.Strict);
        var authorization = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);
        var evidence = new Mock<IGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var snapshot = Snapshot();
        guard.Setup(x => x.AcquireCurrent(snapshot)).Returns(lease.Object);
        lease.Setup(x => x.Dispose());

        var sut = new OcrGroundedClickService(
            click.Object, guard.Object, authorization.Object, evidence.Object);

        Action act = () => sut.Click(snapshot, "ocr-0");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Atomic reviewed click evidence provider is required*");
        click.Verify(x => x.ClickAbsolute(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        authorization.VerifyNoOtherCalls();
        lease.Verify(x => x.Dispose(), Times.Once);
    }

    [Fact]
    public void Click_WhenAuthorizationRejects_ShouldNeverClickAndReleaseLease()
    {
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var guard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);
        var lease = new Mock<IOcrGroundedActionLease>(MockBehavior.Strict);
        var authorization = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);
        var evidence = new Mock<IAtomicGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var snapshot = Snapshot();
        guard.Setup(x => x.AcquireCurrent(snapshot)).Returns(lease.Object);
        lease.Setup(x => x.Dispose());
        evidence.Setup(x => x.Consume(snapshot, snapshot.Targets[0]))
            .Returns(new ConsumedGroundedClickEvidence(null!, 0.1f));
        authorization.Setup(x => x.Authorize(
                It.IsAny<string>(), It.IsAny<float>(),
                It.IsAny<Rectangle>(), It.IsAny<ReviewedGroundTruth>()))
            .Throws(new InvalidOperationException("Reviewed evidence rejected"));

        var sut = new OcrGroundedClickService(
            click.Object, guard.Object, authorization.Object, evidence.Object);

        Action act = () => sut.Click(snapshot, "ocr-0");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Reviewed evidence rejected*");
        click.Verify(x => x.ClickAbsolute(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        lease.Verify(x => x.Dispose(), Times.Once);
    }

    [Fact]
    public void Click_WhenFreshnessLeaseFails_ShouldNotConsumeEvidenceOrClick()
    {
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var guard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);
        var authorization = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);
        var evidence = new Mock<IAtomicGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var snapshot = Snapshot();
        guard.Setup(x => x.AcquireCurrent(snapshot))
            .Throws(new InvalidOperationException("Snapshot is stale"));

        var sut = new OcrGroundedClickService(
            click.Object, guard.Object, authorization.Object, evidence.Object);

        Action act = () => sut.Click(snapshot, "ocr-0");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Snapshot is stale*");
        evidence.VerifyNoOtherCalls();
        authorization.VerifyNoOtherCalls();
        click.Verify(x => x.ClickAbsolute(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
    }

    private static OcrGroundedSnapshot Snapshot() =>
        new("snapshot-1", [
            new OcrGroundedTarget("ocr-0", "Target", Rectangle.FromXYWH(10, 20, 20, 20))
        ]);
}
