using FluentAssertions;
using OmniAppium.ConfigUtilityServices.Extensions;
using LegacyPoint = OmniAppium.ConfigUtilityServices.Models.Point;
using LegacyRectangle = OmniAppium.ConfigUtilityServices.Models.Rectangle;

namespace OmniAppium.EngineUtilityServices.Tests.Extensions;

public class CoordinateRectangleMigrationExtensionsTests
{
    [Fact]
    public void ToCoordinateRectangle_ValidRectangle_PreservesCoordinates()
    {
        var source = new LegacyRectangle
        {
            LeftTop = new LegacyPoint { X = 10, Y = 20 },
            Width = 100,
            Height = 50
        };

        var result = source.ToCoordinateRectangle();

        result.TopLeft.X.Should().Be(10);
        result.TopLeft.Y.Should().Be(20);
        result.Width.Should().Be(100);
        result.Height.Should().Be(50);
    }

    [Fact]
    public void ToCoordinateRectangle_NegativeWidth_Throws()
    {
        var source = new LegacyRectangle
        {
            LeftTop = new LegacyPoint { X = 10, Y = 20 },
            Width = -1,
            Height = 50
        };

        Action act = () => source.ToCoordinateRectangle();

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*width cannot be negative*");
    }

    [Fact]
    public void ToCoordinateRectangle_NullOrigin_Throws()
    {
        var source = new LegacyRectangle
        {
            LeftTop = null!,
            Width = 100,
            Height = 50
        };

        Action act = () => source.ToCoordinateRectangle();

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*origin cannot be null*");
    }

    [Fact]
    public void ToCoordinateRectangle_NullSource_Throws()
    {
        LegacyRectangle source = null!;

        Action act = () => source.ToCoordinateRectangle();

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*source*");
    }

    [Fact]
    public void ToCoordinateRectangle_NegativeHeight_Throws()
    {
        var source = new LegacyRectangle
        {
            LeftTop = new LegacyPoint
            {
                X = 10,
                Y = 20
            },
            Width = 100,
            Height = -1
        };

        Action act = () => source.ToCoordinateRectangle();

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Rectangle height cannot be negative.*");
    }
    [Fact]
    public void ToCoordinateRectangle_NegativeOrigin_PreservesCoordinates()
    {
        var source = new LegacyRectangle
        {
            LeftTop = new LegacyPoint
            {
                X = -10,
                Y = -20
            },
            Width = 100,
            Height = 50
        };

        var result = source.ToCoordinateRectangle();

        result.TopLeft.X.Should().Be(-10);
        result.TopLeft.Y.Should().Be(-20);
        result.Width.Should().Be(100);
        result.Height.Should().Be(50);
    }
}