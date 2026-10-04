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
        OcrGroundedSnapshot snapshot =
            new(
                "snapshot-1",
                null!);

        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            CreatePermissiveFreshnessGuard(snapshot);

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*non-null target collection*");

        freshnessGuard.Verify(
            guard =>
                guard.ThrowIfStale(snapshot),
            Times.Once);

        clickService.VerifyNoOtherCalls();
        freshnessGuard.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetDoesNotExist_ShouldThrowInvalidOperationException()
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
            CreatePermissiveFreshnessGuard(snapshot);

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-999");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*ocr-999*not found*");

        freshnessGuard.Verify(
            guard =>
                guard.ThrowIfStale(snapshot),
            Times.Once);

        clickService.VerifyNoOtherCalls();
        freshnessGuard.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetIdDiffersOnlyByCase_ShouldThrowInvalidOperationException()
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
            CreatePermissiveFreshnessGuard(snapshot);

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(
                snapshot,
                "OCR-0");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*OCR-0*not found*");

        freshnessGuard.Verify(
            guard =>
                guard.ThrowIfStale(snapshot),
            Times.Once);

        clickService.VerifyNoOtherCalls();
        freshnessGuard.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetIdIsAmbiguous_ShouldThrowInvalidOperationException()
    {
        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "snapshot-1",
                CreateTarget(
                    "ocr-0",
                    100,
                    200,
                    40,
                    20),
                CreateTarget(
                    "ocr-0",
                    300,
                    400,
                    40,
                    20));

        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            CreatePermissiveFreshnessGuard(snapshot);

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*ocr-0*ambiguous*");

        freshnessGuard.Verify(
            guard =>
                guard.ThrowIfStale(snapshot),
            Times.Once);

        clickService.VerifyNoOtherCalls();
        freshnessGuard.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetWidthIsZero_ShouldThrowInvalidOperationException()
    {
        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "snapshot-1",
                CreateTarget(
                    "ocr-0",
                    100,
                    200,
                    0,
                    20));

        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            CreatePermissiveFreshnessGuard(snapshot);

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*ocr-0*zero-area bounds*");

        freshnessGuard.Verify(
            guard =>
                guard.ThrowIfStale(snapshot),
            Times.Once);

        clickService.VerifyNoOtherCalls();
        freshnessGuard.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetHeightIsZero_ShouldThrowInvalidOperationException()
    {
        OcrGroundedSnapshot snapshot =
            CreateSnapshot(
                "snapshot-1",
                CreateTarget(
                    "ocr-0",
                    100,
                    200,
                    40,
                    0));

        Mock<IClickService> clickService =
            new(MockBehavior.Strict);

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            CreatePermissiveFreshnessGuard(snapshot);

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*ocr-0*zero-area bounds*");

        freshnessGuard.Verify(
            guard =>
                guard.ThrowIfStale(snapshot),
            Times.Once);

        clickService.VerifyNoOtherCalls();
        freshnessGuard.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetIsValid_ShouldClickAbsoluteAtBoundsCenter()
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

        clickService
            .Setup(
                service =>
                    service.ClickAbsolute(
                        120,
                        210));

        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            CreatePermissiveFreshnessGuard(snapshot);

        OcrGroundedClickService sut =
            new(
                clickService.Object,
                freshnessGuard.Object);

        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        act.Should()
            .NotThrow();

        freshnessGuard.Verify(
            guard =>
                guard.ThrowIfStale(snapshot),
            Times.Once);

        clickService.Verify(
            service =>
                service.ClickAbsolute(
                    120,
                    210),
            Times.Once);

        clickService.VerifyNoOtherCalls();
        freshnessGuard.VerifyNoOtherCalls();
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