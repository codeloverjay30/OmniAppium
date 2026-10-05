using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class OcrGroundedPlannerActionExecutorTests
{
    private readonly Mock<IOcrGroundedClickService> _clickServiceMock;

    public OcrGroundedPlannerActionExecutorTests()
    {
        _clickServiceMock =
            new Mock<IOcrGroundedClickService>(
                MockBehavior.Strict);
    }

    [Fact]
    public void Constructor_WithNullClickService_ShouldThrow()
    {
        // Act
        Action act = () =>
            new OcrGroundedPlannerActionExecutor(null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*clickService*");
    }

    [Fact]
    public void Click_WithValidGroundedAction_ShouldDelegateExactSnapshotAndTargetId()
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        const string targetId = "ocr-0";

        _clickServiceMock
            .Setup(
                service => service.Click(
                    It.Is<OcrGroundedSnapshot>(
                        actual =>
                            ReferenceEquals(
                                actual,
                                snapshot)),
                    targetId));

        var sut =
            CreateSut();

        // Act
        sut.Click(
            snapshot,
            targetId);

        // Assert
        _clickServiceMock.Verify(
            service => service.Click(
                It.Is<OcrGroundedSnapshot>(
                    actual =>
                        ReferenceEquals(
                            actual,
                            snapshot)),
                targetId),
            Times.Once);

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WithValidGroundedAction_ShouldDelegateExactlyOnce()
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        const string targetId = "ocr-0";

        _clickServiceMock
            .Setup(
                service => service.Click(
                    snapshot,
                    targetId));

        var sut =
            CreateSut();

        // Act
        sut.Click(
            snapshot,
            targetId);

        // Assert
        _clickServiceMock.Verify(
            service => service.Click(
                snapshot,
                targetId),
            Times.Once);

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WithTargetIdHavingDifferentCasing_ShouldPreserveExactTargetId()
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        const string targetId = "OCR-0";

        _clickServiceMock
            .Setup(
                service => service.Click(
                    snapshot,
                    targetId));

        var sut =
            CreateSut();

        // Act
        sut.Click(
            snapshot,
            targetId);

        // Assert
        _clickServiceMock.Verify(
            service => service.Click(
                snapshot,
                "OCR-0"),
            Times.Once);

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WithTargetIdContainingSurroundingWhitespace_ShouldPreserveExactTargetId()
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        const string targetId = " ocr-0 ";

        _clickServiceMock
            .Setup(
                service => service.Click(
                    snapshot,
                    targetId));

        var sut =
            CreateSut();

        // Act
        sut.Click(
            snapshot,
            targetId);

        // Assert
        _clickServiceMock.Verify(
            service => service.Click(
                snapshot,
                " ocr-0 "),
            Times.Once);

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WithDifferentSnapshotContainingSameTargetId_ShouldPreserveSelectedSnapshotInstance()
    {
        // Arrange
        OcrGroundedSnapshot selectedSnapshot =
            CreateSnapshot("snapshot-selected");

        OcrGroundedSnapshot otherSnapshot =
            CreateSnapshot("snapshot-other");

        const string targetId = "ocr-0";

        _clickServiceMock
            .Setup(
                service => service.Click(
                    selectedSnapshot,
                    targetId));

        var sut =
            CreateSut();

        // Act
        sut.Click(
            selectedSnapshot,
            targetId);

        // Assert
        _clickServiceMock.Verify(
            service => service.Click(
                It.Is<OcrGroundedSnapshot>(
                    actual =>
                        ReferenceEquals(
                            actual,
                            selectedSnapshot)),
                targetId),
            Times.Once);

        _clickServiceMock.Verify(
            service => service.Click(
                It.Is<OcrGroundedSnapshot>(
                    actual =>
                        ReferenceEquals(
                            actual,
                            otherSnapshot)),
                It.IsAny<string>()),
            Times.Never);

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenGroundedClickServiceThrowsInvalidOperationException_ShouldPropagateExactFailure()
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        const string targetId = "ocr-0";

        _clickServiceMock
            .Setup(
                service => service.Click(
                    snapshot,
                    targetId))
            .Throws(
                new InvalidOperationException(
                    "grounded click failed"));

        var sut =
            CreateSut();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                targetId);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("grounded click failed");

        _clickServiceMock.Verify(
            service => service.Click(
                snapshot,
                targetId),
            Times.Once);

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenGroundedClickServiceReportsMissingTarget_ShouldPropagateFailure()
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        const string targetId = "ocr-missing";

        _clickServiceMock
            .Setup(
                service => service.Click(
                    snapshot,
                    targetId))
            .Throws(
                new InvalidOperationException(
                    $"OCR-grounded target '{targetId}' was not found."));

        var sut =
            CreateSut();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                targetId);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*OCR-grounded target 'ocr-missing' was not found*");

        _clickServiceMock.Verify(
            service => service.Click(
                snapshot,
                targetId),
            Times.Once);

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenGroundedClickServiceReportsAmbiguousTarget_ShouldPropagateFailure()
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        const string targetId = "ocr-0";

        _clickServiceMock
            .Setup(
                service => service.Click(
                    snapshot,
                    targetId))
            .Throws(
                new InvalidOperationException(
                    $"OCR-grounded target '{targetId}' is ambiguous."));

        var sut =
            CreateSut();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                targetId);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*OCR-grounded target 'ocr-0' is ambiguous*");

        _clickServiceMock.Verify(
            service => service.Click(
                snapshot,
                targetId),
            Times.Once);

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenGroundedClickServiceReportsStaleSnapshot_ShouldPropagateFailure()
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-stale");

        const string targetId = "ocr-0";

        _clickServiceMock
            .Setup(
                service => service.Click(
                    snapshot,
                    targetId))
            .Throws(
                new StaleOcrGroundedSnapshotException(
                    "snapshot-stale",
                    "snapshot-current"));

        var sut =
            CreateSut();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                targetId);

        // Assert
        act.Should()
            .Throw<StaleOcrGroundedSnapshotException>()
            .WithMessage(
                "*snapshot-stale*stale*snapshot-current*");

        _clickServiceMock.Verify(
            service => service.Click(
                snapshot,
                targetId),
            Times.Once);

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenGroundedClickServiceThrowsArgumentException_ShouldPropagateFailure()
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        const string targetId = "ocr-0";

        _clickServiceMock
            .Setup(
                service => service.Click(
                    snapshot,
                    targetId))
            .Throws(
                new ArgumentException(
                    "grounded target is invalid",
                    "targetId"));

        var sut =
            CreateSut();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                targetId);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "*grounded target is invalid*targetId*");

        _clickServiceMock.Verify(
            service => service.Click(
                snapshot,
                targetId),
            Times.Once);

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WithNullSnapshot_ShouldThrowBeforeDelegation()
    {
        // Arrange
        var sut =
            CreateSut();

        // Act
        Action act = () =>
            sut.Click(
                null!,
                "ocr-0");

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*snapshot*");

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WithNullTargetId_ShouldThrowBeforeDelegation()
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        var sut =
            CreateSut();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*targetId*");

        _clickServiceMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\r")]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Click_WithEmptyOrWhitespaceTargetId_ShouldThrowBeforeDelegation(
        string targetId)
    {
        // Arrange
        OcrGroundedSnapshot snapshot =
            CreateSnapshot("snapshot-1");

        var sut =
            CreateSut();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                targetId);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*targetId*");

        _clickServiceMock.VerifyNoOtherCalls();
    }

    private OcrGroundedPlannerActionExecutor CreateSut()
    {
        return new OcrGroundedPlannerActionExecutor(
            _clickServiceMock.Object);
    }

    private static OcrGroundedSnapshot CreateSnapshot(
        string snapshotId)
    {
        return new OcrGroundedSnapshot(
            snapshotId,
            Array.Empty<OcrGroundedTarget>());
    }
}