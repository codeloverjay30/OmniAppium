using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Stability;
using System.Drawing;
using System.Drawing.Imaging;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Stability;

public sealed class ScreenFrameComparerTests
{
    private readonly ScreenFrameComparer _sut = new();

    [Fact]
    public void AreSimilar_WhenPixelsIdentical_ShouldReturnTrue()
    {
        byte[] image = Png(Color.Red);
        _sut.AreSimilar(image, image, 0).Should().BeTrue();
    }

    [Fact]
    public void AreSimilar_WhenPixelsDifferBeyondTolerance_ShouldReturnFalse()
    {
        _sut.AreSimilar(Png(Color.Red), Png(Color.Blue), 0).Should().BeFalse();
    }

    [Fact]
    public void AreSimilar_WhenImagesHaveDifferentDimensions_ShouldReturnFalse()
    {
        _sut.AreSimilar(Png(Color.Red, 2, 2), Png(Color.Red, 3, 2), 0)
            .Should().BeFalse();
    }

    [Fact]
    public void AreSimilar_WhenImageDataIsInvalid_ShouldThrowWithMeaningfulMessage()
    {
        Action act = () => _sut.AreSimilar(new byte[] { 1, 2 }, Png(Color.Red), 0);
        act.Should().Throw<ArgumentException>().WithMessage("*image*");
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void AreSimilar_WhenToleranceInvalid_ShouldThrow(double tolerance)
    {
        byte[] image = Png(Color.Red);
        Action act = () => _sut.AreSimilar(image, image, tolerance);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*tolerance*");
    }

    private static byte[] Png(Color color, int width = 2, int height = 2)
    {
        using var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(color);
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        return stream.ToArray();
    }
}
