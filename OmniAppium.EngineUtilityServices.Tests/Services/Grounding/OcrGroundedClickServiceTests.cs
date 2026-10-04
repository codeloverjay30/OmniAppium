using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class OcrGroundedClickServiceTests
{
    [Fact]
    public void Constructor_WhenClickServiceIsNull_ShouldThrowArgumentNullException()
    {
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        Action act = () =>
            _ = new OcrGroundedClickService(
                null!,
                freshnessGuard.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*clickService*");
    }

    [Fact]
    public void Constructor_WhenFreshnessGuardIsNull_ShouldThrowArgumentNullException()
    {
        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Action act = () =>
            _ = new OcrGroundedClickService(
                clickService.Object,
                null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*freshnessGuard*");
    }

    [Fact]
    public void Click_WhenSnapshotIsNull_ShouldThrowArgumentNullException()
    {
        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(
                null!,
                "ocr-0");

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*snapshot*");

        freshnessGuard.Verify(
            guard =>
                guard.ThrowIfStale(
                    It.IsAny<OcrGroundedSnapshot>()),
            Times.Never);

        clickService.VerifyNoOtherCalls();
        freshnessGuard.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Click_WhenTargetIdIsNullOrWhiteSpace_ShouldThrowArgumentException(
        string? targetId)
    {
        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "snapshot-1",
                CreateTarget(
                    "ocr-0",
                    100,
                    200,
                    40,
                    20));

        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(
                snapshot,
                targetId!);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*targetId*");

        freshnessGuard.Verify(
            guard =>
                guard.ThrowIfStale(
                    It.IsAny<OcrGroundedSnapshot>()),
            Times.Never);

        clickService.VerifyNoOtherCalls();
        freshnessGuard.VerifyNoOtherCalls();
    }

[Fact]
public void Click_WhenTargetsCollectionIsNull_ShouldThrowArgumentException()
{
    // Arrange
    Mock<IClickService> clickService =
        new(MockBehavior.Strict);

    Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
        new(MockBehavior.Strict);

    OcrGroundedSnapshot snapshot =
        new(
            "snapshot-1",
            null!);

    OcrGroundedClickService sut =
        new(
            clickService.Object,
            freshnessGuard.Object);

    // Act
    Action act = () =>
        sut.Click(
            snapshot,
            "ocr-0");

    // Assert
    act.Should()
        .Throw<ArgumentException>()
        .WithMessage(
            "*OCR-grounded snapshot must contain a non-null target collection*");

    freshnessGuard.Verify(
        guard => guard.AcquireCurrent(
            It.IsAny<OcrGroundedSnapshot>()),
        Times.Never);

    clickService.Verify(
        service => service.ClickAbsolute(
            It.IsAny<double>(),
            It.IsAny<double>()),
        Times.Never);
}

[Fact]
public void Click_WhenTargetDoesNotExist_ShouldThrowInvalidOperationException()
{
    // Arrange
    Mock<IClickService> clickService =
        new(MockBehavior.Strict);

    Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
        new(MockBehavior.Strict);

    OcrGroundedSnapshot snapshot =
        new(
            "snapshot-1",
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

    OcrGroundedClickService sut =
        new(
            clickService.Object,
            freshnessGuard.Object);

    // Act
    Action act = () =>
        sut.Click(
            snapshot,
            "ocr-does-not-exist");

    // Assert
    act.Should()
        .Throw<InvalidOperationException>()
        .WithMessage(
            "*OCR-grounded target 'ocr-does-not-exist' was not found*");

    freshnessGuard.Verify(
        guard => guard.AcquireCurrent(
            It.IsAny<OcrGroundedSnapshot>()),
        Times.Never);

    clickService.Verify(
        service => service.ClickAbsolute(
            It.IsAny<double>(),
            It.IsAny<double>()),
        Times.Never);
}

[Fact]
public void Click_WhenTargetIdDiffersOnlyByCase_ShouldThrowInvalidOperationException()
{
    // Arrange
    Mock<IClickService> clickService =
        new(MockBehavior.Strict);

    Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
        new(MockBehavior.Strict);

    OcrGroundedSnapshot snapshot =
        new(
            "snapshot-1",
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

    OcrGroundedClickService sut =
        new(
            clickService.Object,
            freshnessGuard.Object);

    // Act
    Action act = () =>
        sut.Click(
            snapshot,
            "OCR-0");

    // Assert
    act.Should()
        .Throw<InvalidOperationException>()
        .WithMessage(
            "*OCR-grounded target 'OCR-0' was not found*");

    freshnessGuard.Verify(
        guard => guard.AcquireCurrent(
            It.IsAny<OcrGroundedSnapshot>()),
        Times.Never);

    clickService.Verify(
        service => service.ClickAbsolute(
            It.IsAny<double>(),
            It.IsAny<double>()),
        Times.Never);
}

[Fact]
public void Click_WhenTargetIdIsAmbiguous_ShouldThrowInvalidOperationException()
{
    // Arrange
    Mock<IClickService> clickService =
        new(MockBehavior.Strict);

    Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
        new(MockBehavior.Strict);

    OcrGroundedSnapshot snapshot =
        new(
            "snapshot-1",
            [
                new OcrGroundedTarget(
                    "ocr-0",
                    "First Target",
                    Rectangle.FromXYWH(
                        10,
                        20,
                        20,
                        20)),
                new OcrGroundedTarget(
                    "ocr-0",
                    "Second Target",
                    Rectangle.FromXYWH(
                        40,
                        50,
                        20,
                        20))
            ]);

    OcrGroundedClickService sut =
        new(
            clickService.Object,
            freshnessGuard.Object);

    // Act
    Action act = () =>
        sut.Click(
            snapshot,
            "ocr-0");

    // Assert
    act.Should()
        .Throw<InvalidOperationException>()
        .WithMessage(
            "*OCR-grounded target 'ocr-0' is ambiguous*");

    freshnessGuard.Verify(
        guard => guard.AcquireCurrent(
            It.IsAny<OcrGroundedSnapshot>()),
        Times.Never);

    clickService.Verify(
        service => service.ClickAbsolute(
            It.IsAny<double>(),
            It.IsAny<double>()),
        Times.Never);
}

[Fact]
public void Click_WhenTargetWidthIsZero_ShouldThrowInvalidOperationException()
{
    // Arrange
    Mock<IClickService> clickService =
        new(MockBehavior.Strict);

    Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
        new(MockBehavior.Strict);

    OcrGroundedSnapshot snapshot =
        new(
            "snapshot-1",
            [
                new OcrGroundedTarget(
                    "ocr-0",
                    "Target",
                    Rectangle.FromXYWH(
                        10,
                        20,
                        0,
                        20))
            ]);

    OcrGroundedClickService sut =
        new(
            clickService.Object,
            freshnessGuard.Object);

    // Act
    Action act = () =>
        sut.Click(
            snapshot,
            "ocr-0");

    // Assert
    act.Should()
        .Throw<InvalidOperationException>()
        .WithMessage(
            "*OCR-grounded target 'ocr-0' has zero-area bounds*");

    freshnessGuard.Verify(
        guard => guard.AcquireCurrent(
            It.IsAny<OcrGroundedSnapshot>()),
        Times.Never);

    clickService.Verify(
        service => service.ClickAbsolute(
            It.IsAny<double>(),
            It.IsAny<double>()),
        Times.Never);
}

    [Fact]
    public void Click_WhenTargetHeightIsZero_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot =
            new(
                "snapshot-1",
                [
                    new OcrGroundedTarget(
                    "ocr-0",
                    "Target",
                    Rectangle.FromXYWH(
                        10,
                        20,
                        20,
                        0))
                ]);

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*OCR-grounded target 'ocr-0' has zero-area bounds*");

        freshnessGuard.Verify(
            guard => guard.AcquireCurrent(
                It.IsAny<OcrGroundedSnapshot>()),
            Times.Never);

        clickService.Verify(
            service => service.ClickAbsolute(
                It.IsAny<double>(),
                It.IsAny<double>()),
            Times.Never);
    }

    [Fact]
    public void Click_WhenTargetIsValid_ShouldClickAbsoluteAtBoundsCenter()
    {
        // Arrange
        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedActionLease> lease =
            new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot =
            new(
                "snapshot-1",
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
            .Setup(
                guard =>
                    guard.AcquireCurrent(snapshot))
            .Returns(lease.Object);

        clickService
            .Setup(
                service =>
                    service.ClickAbsolute(
                        20,
                        30));

        lease
            .Setup(
                currentLease =>
                    currentLease.Dispose());

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        // Assert
        act.Should().NotThrow();

        freshnessGuard.Verify(
            guard =>
                guard.AcquireCurrent(snapshot),
            Times.Once);

        clickService.Verify(
            service =>
                service.ClickAbsolute(
                    20,
                    30),
            Times.Once);

        lease.Verify(
            currentLease =>
                currentLease.Dispose(),
            Times.Once);
    }

    private static Mock<IOcrGroundedSnapshotFreshnessGuard>
        CreatePermissiveFreshnessGuard(
            OcrGroundedSnapshot snapshot)
    {
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        freshnessGuard
            .Setup(
                guard =>
                    guard.ThrowIfStale(snapshot));

        return freshnessGuard;
    }

    private static OcrGroundedSnapshot CreateSnapshot(
        string snapshotId,
        params OcrGroundedTarget[] targets)
    {
        return new OcrGroundedSnapshot(
            snapshotId,
            targets);
    }

    private static OcrGroundedTarget CreateTarget(
        string targetId,
        double x,
        double y,
        double width,
        double height)
    {
        Rectangle bounds =
            Rectangle.FromXYWH(
                x,
                y,
                width,
                height);

        return new OcrGroundedTarget(
            targetId,
            "Test target",
            bounds);
    }
}