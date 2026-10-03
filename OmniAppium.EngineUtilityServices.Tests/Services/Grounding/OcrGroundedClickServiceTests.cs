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
        // Act
        Action act = () =>
            _ = new OcrGroundedClickService(
                null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*clickService*");
    }

    [Fact]
    public void Click_WhenSnapshotIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Mock<IClickService> clickServiceMock =
            new(MockBehavior.Strict);

        var sut =
            new OcrGroundedClickService(
                clickServiceMock.Object);

        // Act
        Action act = () =>
            sut.Click(
                null!,
                "ocr-0");

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*snapshot*");

        clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetIdIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Mock<IClickService> clickServiceMock =
            new(MockBehavior.Strict);

        var sut =
            new OcrGroundedClickService(
                clickServiceMock.Object);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*targetId*");

        clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetIdIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        Mock<IClickService> clickServiceMock =
            new(MockBehavior.Strict);

        var sut =
            new OcrGroundedClickService(
                clickServiceMock.Object);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                string.Empty);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*targetId*");

        clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetIdIsWhiteSpace_ShouldThrowArgumentException()
    {
        // Arrange
        Mock<IClickService> clickServiceMock =
            new(MockBehavior.Strict);

        var sut =
            new OcrGroundedClickService(
                clickServiceMock.Object);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                "   ");

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*targetId*");

        clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetsAreNull_ShouldThrowArgumentException()
    {
        // Arrange
        Mock<IClickService> clickServiceMock =
            new(MockBehavior.Strict);

        var sut =
            new OcrGroundedClickService(
                clickServiceMock.Object);

        OcrGroundedSnapshot snapshot =
            new(
                "screen-42",
                null!);

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

        clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetDoesNotExist_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Mock<IClickService> clickServiceMock =
            new(MockBehavior.Strict);

        var sut =
            new OcrGroundedClickService(
                clickServiceMock.Object);

        OcrGroundedSnapshot snapshot =
            CreateSnapshot();

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                "missing-target");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*missing-target*was not found*");

        clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetIdIsDuplicated_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Mock<IClickService> clickServiceMock =
            new(MockBehavior.Strict);

        var sut =
            new OcrGroundedClickService(
                clickServiceMock.Object);

        OcrGroundedSnapshot snapshot =
            new(
                "screen-42",
                [
                    new OcrGroundedTarget(
                        "ocr-0",
                        "前往",
                        Rectangle.FromXYWH(
                            100,
                            200,
                            120,
                            40)),

                    new OcrGroundedTarget(
                        "ocr-0",
                        "前往",
                        Rectangle.FromXYWH(
                            100,
                            400,
                            120,
                            40))
                ]);

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*ocr-0*ambiguous*");

        clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetBoundsHaveZeroWidth_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Mock<IClickService> clickServiceMock =
            new(MockBehavior.Strict);

        var sut =
            new OcrGroundedClickService(
                clickServiceMock.Object);

        OcrGroundedSnapshot snapshot =
            new(
                "screen-42",
                [
                    new OcrGroundedTarget(
                        "ocr-0",
                        "前往",
                        Rectangle.FromXYWH(
                            100,
                            200,
                            0,
                            40))
                ]);

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*ocr-0*zero-area bounds*");

        clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetBoundsHaveZeroHeight_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Mock<IClickService> clickServiceMock =
            new(MockBehavior.Strict);

        var sut =
            new OcrGroundedClickService(
                clickServiceMock.Object);

        OcrGroundedSnapshot snapshot =
            new(
                "screen-42",
                [
                    new OcrGroundedTarget(
                        "ocr-0",
                        "前往",
                        Rectangle.FromXYWH(
                            100,
                            200,
                            120,
                            0))
                ]);

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*ocr-0*zero-area bounds*");

        clickServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetExists_ShouldClickCenterOfBounds()
    {
        // Arrange
        Rectangle bounds =
            Rectangle.FromXYWH(
                100,
                200,
                120,
                40);

        OcrGroundedSnapshot snapshot =
            new(
                "screen-42",
                [
                    new OcrGroundedTarget(
                        "ocr-0",
                        "前往",
                        bounds)
                ]);

        Mock<IClickService> clickServiceMock =
            new(MockBehavior.Strict);

        clickServiceMock
            .Setup(service =>
                service.ClickAbsolute(
                    160,
                    220));

        var sut =
            new OcrGroundedClickService(
                clickServiceMock.Object);

        // Act
        Action act = () =>
            sut.Click(
                snapshot,
                "ocr-0");

        // Assert
        act.Should()
            .NotThrow();

        clickServiceMock.Verify(
            service =>
                service.ClickAbsolute(
                    160,
                    220),
            Times.Once);

        clickServiceMock.VerifyNoOtherCalls();
    }

    private static OcrGroundedSnapshot CreateSnapshot()
    {
        return new OcrGroundedSnapshot(
            "screen-42",
            [
                new OcrGroundedTarget(
                    "ocr-0",
                    "前往",
                    Rectangle.FromXYWH(
                        100,
                        200,
                        120,
                        40))
            ]);
    }
}