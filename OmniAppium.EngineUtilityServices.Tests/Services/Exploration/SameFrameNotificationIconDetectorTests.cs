using CoordinateUtilityServices;
using FluentAssertions;
using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using Xunit;
using Point = OpenCvSharp.Point;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class SameFrameNotificationIconDetectorTests
{
    [Fact]
    public void Detect_WithCircleAndUniqueLabel_ShouldReturnOneIcon()
    {
        using Mat image = CreateCircleImage();
        byte[] screenshot = Encode(image);
        var targets = new[] { new OcrGroundedTarget("activity", "活動",
            Rectangle.FromXYWH(92, 165, 56, 24)) };

        var result = new SameFrameNotificationIconDetector().Detect(screenshot, targets);

        result.Should().ContainSingle();
        result[0].TargetId.Should().Be("activity");
    }

    [Fact]
    public void Detect_WithoutOcr_ShouldRemainUnowned()
    {
        using Mat image = CreateCircleImage();

        var result = new SameFrameNotificationIconDetector().Detect(
            Encode(image), Array.Empty<OcrGroundedTarget>());

        result.Should().BeEmpty();
    }

    [Fact]
    public void Detect_WithCompetingLabels_ShouldFailClosed()
    {
        using Mat image = CreateCircleImage();
        var targets = new[]
        {
            new OcrGroundedTarget("a", "活動", Rectangle.FromXYWH(92, 165, 56, 24)),
            new OcrGroundedTarget("b", "活動", Rectangle.FromXYWH(94, 165, 56, 24))
        };

        var result = new SameFrameNotificationIconDetector().Detect(Encode(image), targets);

        result.Should().BeEmpty();
    }

    [Fact]
    public void Detect_WithInvalidImage_ShouldThrowActualMessage()
    {
        Action act = () => new SameFrameNotificationIconDetector().Detect(
            new byte[] { 1, 2, 3 }, Array.Empty<OcrGroundedTarget>());

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Cannot decode same-frame icon screenshot*");
    }

    private static Mat CreateCircleImage()
    {
        var image = new Mat(new Size(240, 220), MatType.CV_8UC3, Scalar.Black);
        Cv2.Circle(image, new Point(120, 105), 46, Scalar.White, 4, LineTypes.AntiAlias);
        return image;
    }

    private static byte[] Encode(Mat image)
    {
        Cv2.ImEncode(".png", image, out byte[] buffer);
        return buffer;
    }
}
