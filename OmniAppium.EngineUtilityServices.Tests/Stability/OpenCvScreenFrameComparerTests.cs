using FluentAssertions;
using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Services.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Stability;

public sealed class OpenCvScreenFrameComparerTests
{
    private readonly ScreenFrameComparer _sut = new();

    [Fact]
    public void AreSimilar_WhenImagesAreIdentical_ShouldReturnTrue()
    {
        byte[] image = CreatePng(new Scalar(10, 20, 30, 255));
        _sut.AreSimilar(image, image, 0).Should().BeTrue();
    }

    [Fact]
    public void AreSimilar_WhenAllPixelsDiffer_ShouldRespectTolerance()
    {
        byte[] first = CreatePng(new Scalar(10, 20, 30, 255));
        byte[] second = CreatePng(new Scalar(11, 20, 30, 255));

        _sut.AreSimilar(first, second, 0.99).Should().BeFalse();
        _sut.AreSimilar(first, second, 1).Should().BeTrue();
    }

    [Fact]
    public void AreSimilar_WhenAlphaDiffers_ShouldReturnFalse()
    {
        byte[] first = CreatePng(new Scalar(10, 20, 30, 255));
        byte[] second = CreatePng(new Scalar(10, 20, 30, 128));
        _sut.AreSimilar(first, second, 0).Should().BeFalse();
    }

    [Fact]
    public void AreSimilar_WhenDimensionsDiffer_ShouldReturnFalse()
    {
        byte[] first = CreatePng(new Scalar(1, 2, 3, 255), 10);
        byte[] second = CreatePng(new Scalar(1, 2, 3, 255), 11);
        _sut.AreSimilar(first, second, 0).Should().BeFalse();
    }

    [Fact]
    public void AreSimilar_WhenImageIsEmpty_ShouldThrowMeaningfulException()
    {
        byte[] valid = CreatePng(new Scalar(1, 2, 3, 255));
        Action act = () => _sut.AreSimilar(ReadOnlyMemory<byte>.Empty, valid, 0);
        act.Should().Throw<ArgumentException>().WithMessage("*image cannot be empty*");
    }

    [Fact]
    public void AreSimilar_WhenImageIsCorrupt_ShouldThrowMeaningfulException()
    {
        byte[] valid = CreatePng(new Scalar(1, 2, 3, 255));
        Action act = () => _sut.AreSimilar(new byte[] { 1, 2, 3 }, valid, 0);
        act.Should().Throw<ArgumentException>().WithMessage("*invalid or unsupported*");
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void AreSimilar_WhenToleranceIsInvalid_ShouldThrow(double tolerance)
    {
        Action act = () => _sut.AreSimilar(ReadOnlyMemory<byte>.Empty, ReadOnlyMemory<byte>.Empty, tolerance);
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*tolerance*");
    }

    private static byte[] CreatePng(Scalar color, int side = 10)
    {
        using var image = new Mat(side, side, MatType.CV_8UC4, color);
        Cv2.ImEncode(".png", image, out byte[] encoded).Should().BeTrue();
        return encoded;
    }
}
