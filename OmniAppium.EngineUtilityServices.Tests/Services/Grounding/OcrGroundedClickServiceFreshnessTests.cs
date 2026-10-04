using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class OcrGroundedClickServiceFreshnessTests
{
    [Fact]
    public void Click_WhenSnapshotIsCurrent_ShouldClickTargetCenter()
    {
        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedActionLease> lease =
            new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot =
            new(
                "snapshot-42",
                [
                    new OcrGroundedTarget(
                    "ocr-0",
                    "Target",
                    Rectangle.FromXYWH(
                        10,
                        20,
                        20,
                        20))
                ]);

        freshnessGuard
            .Setup(guard => guard.AcquireCurrent(snapshot))
            .Returns(lease.Object);

        clickService
            .Setup(service => service.ClickAbsolute(20, 30));

        lease
            .Setup(currentLease => currentLease.Dispose());

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(snapshot, "ocr-0");

        act.Should().NotThrow();

        freshnessGuard.Verify(
            guard => guard.AcquireCurrent(snapshot),
            Times.Once);

        clickService.Verify(
            service => service.ClickAbsolute(20, 30),
            Times.Once);

        lease.Verify(
            currentLease => currentLease.Dispose(),
            Times.Once);
    }


    [Fact]
    public void Click_WhenSnapshotIsStale_ShouldRejectActionBeforeClicking()
    {
        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot =
            new(
                "snapshot-41",
                [
                    new OcrGroundedTarget(
                    "ocr-0",
                    "Target",
                    Rectangle.FromXYWH(
                        10,
                        20,
                        20,
                        20))
                ]);

        freshnessGuard
            .Setup(guard => guard.AcquireCurrent(snapshot))
            .Throws(
                new StaleOcrGroundedSnapshotException(
                    "snapshot-41",
                    "snapshot-42"));

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(snapshot, "ocr-0");

        act.Should()
            .Throw<StaleOcrGroundedSnapshotException>()
            .WithMessage(
                "*snapshot-41*stale*snapshot-42*");

        freshnessGuard.Verify(
            guard => guard.AcquireCurrent(snapshot),
            Times.Once);

        clickService.Verify(
            service => service.ClickAbsolute(
                It.IsAny<double>(),
                It.IsAny<double>()),
            Times.Never);
    }

    private static OcrGroundedSnapshot CreateSnapshot(
    string snapshotId,
    string targetId,
    int x,
    int y,
    int width,
    int height)
    {
        Rectangle bounds =
            Rectangle.FromXYWH(
                x,
                y,
                width,
                height);

        OcrGroundedTarget target =
            new(
                targetId,
                "Test target",
                bounds);

        return new OcrGroundedSnapshot(
            snapshotId,
            [target]);
    }
}
