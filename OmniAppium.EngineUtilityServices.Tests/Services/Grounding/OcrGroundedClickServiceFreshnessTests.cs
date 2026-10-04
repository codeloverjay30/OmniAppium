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
        // Arrange
        var clickService = new Mock<IClickService>(MockBehavior.Strict);
        var freshnessGuard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(
            MockBehavior.Strict);

        var snapshot = CreateSnapshot(
            snapshotId: "snapshot-42",
            targetId: "ocr-0",
            x: 100,
            y: 200,
            width: 40,
            height: 20);

        freshnessGuard
            .Setup(x => x.ThrowIfStale(snapshot));

        clickService
            .Setup(x => x.ClickAbsolute(120, 210));

        var sut = new OcrGroundedClickService(
            clickService.Object,
            freshnessGuard.Object);

        // Act
        Action act = () => sut.Click(snapshot, "ocr-0");

        // Assert
        act.Should().NotThrow();

        freshnessGuard.Verify(
            x => x.ThrowIfStale(snapshot),
            Times.Once);

        clickService.Verify(
            x => x.ClickAbsolute(120, 210),
            Times.Once);
    }

    [Fact]
    public void Click_WhenSnapshotIsStale_ShouldRejectActionBeforeClicking()
    {
        // Arrange
        var clickService = new Mock<IClickService>(MockBehavior.Strict);
        var freshnessGuard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(
            MockBehavior.Strict);

        var snapshot = CreateSnapshot(
            snapshotId: "snapshot-41",
            targetId: "ocr-0",
            x: 100,
            y: 200,
            width: 40,
            height: 20);

        freshnessGuard
            .Setup(x => x.ThrowIfStale(snapshot))
            .Throws(
                new StaleOcrGroundedSnapshotException(
                    "snapshot-41",
                    "snapshot-42"
                )
            );

        var sut = new OcrGroundedClickService(
            clickService.Object,
            freshnessGuard.Object);

        // Act
        Action act = () => sut.Click(snapshot, "ocr-0");

        // Assert
        act.Should()
            .Throw<StaleOcrGroundedSnapshotException>()
            .WithMessage("*snapshot-41*stale*");

        clickService.Verify(
            x => x.ClickAbsolute(
                It.IsAny<double>(),
                It.IsAny<double>()),
            Times.Never);
    }


    [Fact]
    public void Click_WhenSnapshotIsStale_ShouldRejectBeforeResolvingTarget()
    {
        // Arrange
        var clickService = new Mock<IClickService>(MockBehavior.Strict);
        var freshnessGuard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(
            MockBehavior.Strict);

        var snapshot = new OcrGroundedSnapshot(
            "snapshot-41",
            []);

        freshnessGuard
            .Setup(x => x.ThrowIfStale(snapshot))
            .Throws(
                new StaleOcrGroundedSnapshotException(
                    "snapshot-41",
                    "snapshot-42"
                )
            );

        var sut = new OcrGroundedClickService(
            clickService.Object,
            freshnessGuard.Object);

        // Act
        Action act = () => sut.Click(snapshot, "ocr-does-not-exist");

        // Assert
        act.Should()
            .Throw<StaleOcrGroundedSnapshotException>()
            .WithMessage("*snapshot-41*stale*");

        clickService.VerifyNoOtherCalls();
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
