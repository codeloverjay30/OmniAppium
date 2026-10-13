using FluentAssertions;
using OCRUtilityServices.Models;
using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Services.Observation;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Observation;

public sealed class OcrObservationLineNormalizerTests
{
    private readonly OcrObservationLineNormalizer _sut = new();

    private static OcrResult EmptyOcrResult() =>
        new(string.Empty, Array.Empty<OcrTextLine>());

    private static byte[] ValidPng()
    {
        using var image = new Mat(
            16, 16, MatType.CV_8UC3, new Scalar(30, 80, 120));

        bool encoded = Cv2.ImEncode(".png", image, out byte[] buffer);

        encoded.Should().BeTrue(
            "the test fixture must produce a valid PNG image");

        buffer.Should().NotBeEmpty();

        return buffer;
    }

    [Fact]
    public void Normalize_WithValidPngAndNoLines_ShouldReturnEmptyCollection()
    {
        IReadOnlyList<OmniAppium.EngineUtilityServices.Models.Stability.NormalizedOcrTextLine>
            result = _sut.Normalize(EmptyOcrResult(), ValidPng());

        result.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Normalize_WithNullResult_ShouldThrow()
    {
        Action act = () => _sut.Normalize(null!, ValidPng());
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*result*");
    }

    [Fact]
    public void Normalize_WithEmptyImage_ShouldThrow()
    {
        Action act = () => _sut.Normalize(
            EmptyOcrResult(), ReadOnlyMemory<byte>.Empty);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*The image cannot be empty*");
    }

    [Fact]
    public void Normalize_WithImageExceedingEncodedLimit_ShouldThrow()
    {
        byte[] oversized = new byte[32 * 1024 * 1024 + 1];

        Action act = () => _sut.Normalize(EmptyOcrResult(), oversized);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*The image exceeds the encoded size limit*");
    }

    [Fact]
    public void Normalize_WithCorruptedImage_ShouldThrow()
    {
        byte[] corrupted = [0x01, 0x02, 0x03, 0x04, 0x05];

        Action act = () => _sut.Normalize(EmptyOcrResult(), corrupted);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*The image data is invalid or unsupported*");
    }

    [Fact]
    public void Normalize_WithValidImage_ShouldNotMutateImageBytes()
    {
        byte[] png = ValidPng();
        byte[] original = png.ToArray();

        _sut.Normalize(EmptyOcrResult(), png);

        png.Should().Equal(original);
    }
}
