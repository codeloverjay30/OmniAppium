using CoordinateUtilityServices;
using FluentAssertions;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class OcrGroundedSnapshotFactoryTests
{
    [Fact]
    public void Create_WhenSnapshotIdIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var sut = new OcrGroundedSnapshotFactory();

        OcrResult ocrResult =
            new(
                string.Empty,
                Array.Empty<OcrTextLine>());

        // Act
        Action act = () =>
            sut.Create(
                null!,
                ocrResult);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*snapshotId*");
    }

    [Fact]
    public void Create_WhenSnapshotIdIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var sut = new OcrGroundedSnapshotFactory();

        OcrResult ocrResult =
            new(
                string.Empty,
                Array.Empty<OcrTextLine>());

        // Act
        Action act = () =>
            sut.Create(
                string.Empty,
                ocrResult);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*snapshotId*");
    }

    [Fact]
    public void Create_WhenSnapshotIdIsWhiteSpace_ShouldThrowArgumentException()
    {
        // Arrange
        var sut = new OcrGroundedSnapshotFactory();

        OcrResult ocrResult =
            new(
                string.Empty,
                Array.Empty<OcrTextLine>());

        // Act
        Action act = () =>
            sut.Create(
                "   ",
                ocrResult);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*snapshotId*");
    }

    [Fact]
    public void Create_WhenOcrResultIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var sut = new OcrGroundedSnapshotFactory();

        // Act
        Action act = () =>
            sut.Create(
                "screen-42",
                null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*ocrResult*");
    }

    [Fact]
    public void Create_WhenLinesAreNull_ShouldThrowArgumentException()
    {
        // Arrange
        var sut = new OcrGroundedSnapshotFactory();

        OcrResult ocrResult =
            new(
                string.Empty,
                null!);

        // Act
        Action act = () =>
            sut.Create(
                "screen-42",
                ocrResult);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage(
                "*OCR result must contain a non-null line collection*");
    }

    [Fact]
    public void Create_WhenLinesAreEmpty_ShouldReturnEmptyTargets()
    {
        // Arrange
        var sut = new OcrGroundedSnapshotFactory();

        OcrResult ocrResult =
            new(
                string.Empty,
                Array.Empty<OcrTextLine>());

        // Act
        OcrGroundedSnapshot result =
            sut.Create(
                "screen-42",
                ocrResult);

        // Assert
        result.SnapshotId
            .Should()
            .Be("screen-42");

        result.Targets
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void Create_WhenLinesExist_ShouldPreserveTextAndBounds()
    {
        // Arrange
        var sut = new OcrGroundedSnapshotFactory();

        Rectangle firstBounds =
            Rectangle.FromXYWH(
                100,
                200,
                120,
                40);

        Rectangle secondBounds =
            Rectangle.FromXYWH(
                300,
                400,
                160,
                50);

        OcrResult ocrResult =
            new(
                "任務 前往",
                [
                    new OcrTextLine(
                        "任務",
                        firstBounds),

                    new OcrTextLine(
                        "前往",
                        secondBounds)
                ]);

        // Act
        OcrGroundedSnapshot result =
            sut.Create(
                "screen-42",
                ocrResult);

        // Assert
        result.SnapshotId
            .Should()
            .Be("screen-42");

        result.Targets
            .Should()
            .HaveCount(2);

        result.Targets[0].Id
            .Should()
            .Be("ocr-0");

        result.Targets[0].Text
            .Should()
            .Be("任務");

        result.Targets[0].Bounds
            .Should()
            .Be(firstBounds);

        result.Targets[1].Id
            .Should()
            .Be("ocr-1");

        result.Targets[1].Text
            .Should()
            .Be("前往");

        result.Targets[1].Bounds
            .Should()
            .Be(secondBounds);
    }

    [Fact]
    public void Create_WhenDuplicateTextsExist_ShouldAssignUniqueTargetIds()
    {
        // Arrange
        var sut = new OcrGroundedSnapshotFactory();

        Rectangle firstBounds =
            Rectangle.FromXYWH(
                100,
                200,
                120,
                40);

        Rectangle secondBounds =
            Rectangle.FromXYWH(
                100,
                400,
                120,
                40);

        OcrResult ocrResult =
            new(
                "前往 前往",
                [
                    new OcrTextLine(
                        "前往",
                        firstBounds),

                    new OcrTextLine(
                        "前往",
                        secondBounds)
                ]);

        // Act
        OcrGroundedSnapshot result =
            sut.Create(
                "screen-42",
                ocrResult);

        // Assert
        result.Targets
            .Should()
            .HaveCount(2);

        result.Targets
            .Should()
            .OnlyContain(
                target =>
                    target.Text == "前往");

        result.Targets
            .Should()
            .OnlyHaveUniqueItems(
                target =>
                    target.Id);

        result.Targets[0].Id
            .Should()
            .Be("ocr-0");

        result.Targets[1].Id
            .Should()
            .Be("ocr-1");

        result.Targets[0].Bounds
            .Should()
            .Be(firstBounds);

        result.Targets[1].Bounds
            .Should()
            .Be(secondBounds);
    }
}