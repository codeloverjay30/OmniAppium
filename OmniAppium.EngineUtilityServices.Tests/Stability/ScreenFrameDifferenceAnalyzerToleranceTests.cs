using FluentAssertions;
using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Stability;

public sealed class ScreenFrameDifferenceAnalyzerToleranceTests
{
    [Fact]
    public void Analyze_WhenChannelDeltaIsWithinTolerance_ShouldReportNoDifferences()
    {
        var sut = new ScreenFrameDifferenceAnalyzer();
        byte[] first = CreatePng(100);
        byte[] second = CreatePng(107);

        ScreenFrameDifference result = sut.Analyze(first, second, channelTolerance: 8);

        result.DifferenceRatio.Should().Be(0);
        result.DifferentPixels.Should().Be(0);
        result.TotalPixels.Should().Be(4);
    }

    [Fact]
    public void Analyze_WhenChannelDeltaExceedsTolerance_ShouldReportDifferences()
    {
        var sut = new ScreenFrameDifferenceAnalyzer();
        byte[] first = CreatePng(100);
        byte[] second = CreatePng(109);

        ScreenFrameDifference result = sut.Analyze(first, second, channelTolerance: 8);

        result.DifferenceRatio.Should().Be(1);
        result.DifferentPixels.Should().Be(4);
    }

    [Fact]
    public void CreateDifferenceMask_WhenDifferenceExceedsTolerance_ShouldReturnWhitePixels()
    {
        var sut = new ScreenFrameDifferenceAnalyzer();
        byte[] first = CreatePng(100);
        byte[] second = CreatePng(109);

        byte[] png = sut.CreateDifferenceMask(first, second, channelTolerance: 8);
        using Mat mask = Cv2.ImDecode(png, ImreadModes.Grayscale);

        mask.Width.Should().Be(2);
        mask.Height.Should().Be(2);
        Cv2.CountNonZero(mask).Should().Be(4);
    }

    /// <summary>
    /// Creates a small lossless PNG image with identical channel values.
    /// </summary>
    private static byte[] CreatePng(byte value)
    {
        using Mat image = new(2, 2, MatType.CV_8UC3, new Scalar(value, value, value));
        if (!Cv2.ImEncode(".png", image, out byte[] encoded) || encoded.Length == 0)
        {
            throw new InvalidOperationException(
                "Failed to encode the test image as PNG.");
        }
        return encoded;
    }
}
