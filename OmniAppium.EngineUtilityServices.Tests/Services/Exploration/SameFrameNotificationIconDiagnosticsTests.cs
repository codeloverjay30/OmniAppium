using CoordinateUtilityServices;
using FluentAssertions;
using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using Xunit;
using Point = OpenCvSharp.Point;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class SameFrameNotificationIconDiagnosticsTests
{
    [Fact]
    public void Diagnose_WithUniqueAlignedLabel_ShouldReportDiagnosticOnly()
    {
        using Mat image = CreateImage();
        var targets = new[]
        {
            new OcrGroundedTarget("main", "主城",
                Rectangle.FromXYWH(92, 165, 56, 24))
        };

        var result = new SameFrameNotificationIconDiagnostics().Diagnose(
            Encode(image), targets, 120, 105);

        result.Should().Contain(x =>
            x.Reason == "UniqueAlignedLabelDiagnosticOnly" &&
            x.AlignedLabelCount == 1);
    }

    [Fact]
    public void Diagnose_WithoutOcr_ShouldNotInventAnOwner()
    {
        using Mat image = CreateImage();

        var result = new SameFrameNotificationIconDiagnostics().Diagnose(
            Encode(image), Array.Empty<OcrGroundedTarget>(), 120, 105);

        result.Should().OnlyContain(x => x.AlignedLabelCount == 0);
    }

    [Fact]
    public void Diagnose_InvalidImage_ShouldThrowActualMessage()
    {
        Action act = () => new SameFrameNotificationIconDiagnostics().Diagnose(
            new byte[] { 1, 2, 3 }, Array.Empty<OcrGroundedTarget>(), 120, 105);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Cannot decode same-frame icon screenshot*");
    }

    [Fact]
    public void Diagnose_EmptyScreenshot_ShouldThrowActualMessage()
    {
        Action act = () => new SameFrameNotificationIconDiagnostics().Diagnose(
            ReadOnlyMemory<byte>.Empty, Array.Empty<OcrGroundedTarget>(), 120, 105);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Screenshot bytes are required*");
    }

    private static Mat CreateImage()
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