using CoordinateUtilityServices;
using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppiumDemo.SmokeTests.Issue59;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59OcrProvenanceDiagnosticsTests
{
    [Fact]
    public void Inspect_ShouldReportMissingWarriorAsRawOcrAbsence()
    {
        var lines = new (string? Text, Rectangle Bounds)[]
        {
            ("裝 備", Rectangle.FromXYWH(1620, 1016, 56, 27))
        };

        var result = Issue59OcrProvenanceDiagnostics.Inspect(
            lines, Array.Empty<OcrGroundedTarget>(),
            Rectangle.FromXYWH(1350, 850, 250, 230));

        result.Should().BeEmpty();
    }

    [Fact]
    public void Inspect_ShouldPreserveRawLineIndexAndRecognizeRetainedTarget()
    {
        var lines = new (string? Text, Rectangle Bounds)[]
        {
            ("裝 備", Rectangle.FromXYWH(1620, 1016, 56, 27)),
            ("武 將", Rectangle.FromXYWH(1430, 1015, 77, 28))
        };
        var targets = new[]
        {
            new OcrGroundedTarget("issue59-ocr-000001", "武 將",
                Rectangle.FromXYWH(1430, 1015, 77, 28))
        };

        var result = Issue59OcrProvenanceDiagnostics.Inspect(
            lines, targets, Rectangle.FromXYWH(1350, 850, 250, 230));

        result.Should().ContainSingle()
            .Which.Should().Match<Issue59OcrProvenanceDiagnostics.Entry>(
                item => item.RawIndex == 1 &&
                        item.Retained &&
                        item.Reason == "Retained");
    }

    [Fact]
    public void Inspect_ShouldReportNonBlankRawLineNotRetained()
    {
        var lines = new (string? Text, Rectangle Bounds)[]
        {
            ("武 將", Rectangle.FromXYWH(1430, 1015, 77, 28))
        };

        var result = Issue59OcrProvenanceDiagnostics.Inspect(
            lines, Array.Empty<OcrGroundedTarget>(),
            Rectangle.FromXYWH(1350, 850, 250, 230));

        result.Should().ContainSingle()
            .Which.Reason.Should().Be("NonBlankRawLineNotRetained");
    }

    [Fact]
    public void Inspect_ShouldRejectInvalidRegion()
    {
        Action act = () => Issue59OcrProvenanceDiagnostics.Inspect(
            Array.Empty<(string? Text, Rectangle Bounds)>(),
            Array.Empty<OcrGroundedTarget>(),
            Rectangle.FromXYWH(0, 0, 0, 10));

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Diagnostic region*");
    }
}
