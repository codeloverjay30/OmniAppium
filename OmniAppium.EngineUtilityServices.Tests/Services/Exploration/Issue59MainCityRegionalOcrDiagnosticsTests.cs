using CoordinateUtilityServices;
using FluentAssertions;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59MainCityRegionalOcrDiagnosticsTests
{
    private static readonly Rectangle Region = Rectangle.FromXYWH(125, 375, 180, 85);

    [Fact]
    public void Diagnose_ExactMainCityLabel_ShouldKeepOriginalFrameBounds()
    {
        Rectangle bounds = Rectangle.FromXYWH(164, 394, 70, 29);
        OcrResult result = new("主 城", [new OcrTextLine("主 城", bounds)]);

        var diagnostic = NotificationRegionalOcrCandidateSelector.Diagnose(
            result, Region, 2340, 1080, "主城", "issue59-diagnostic-main-city");

        diagnostic.Reason.Should().Be(NotificationRegionalOcrRejectionReason.None);
        diagnostic.Candidate.Should().NotBeNull();
        diagnostic.Candidate!.Bounds.Should().Be(bounds);
        diagnostic.Candidate.Id.Should().Be("issue59-diagnostic-main-city");
    }

    [Fact]
    public void Diagnose_UnrelatedNearbyLabel_ShouldFailClosed()
    {
        OcrResult result = new("VIP 9", [
            new OcrTextLine("VIP 9", Rectangle.FromXYWH(160, 395, 65, 25))]);

        var diagnostic = NotificationRegionalOcrCandidateSelector.Diagnose(
            result, Region, 2340, 1080, "主城", "issue59-diagnostic-main-city");

        diagnostic.Candidate.Should().BeNull();
        diagnostic.Reason.Should().Be(NotificationRegionalOcrRejectionReason.LabelMismatch);
    }

    [Fact]
    public void Diagnose_DuplicateMainCityLabels_ShouldFailClosed()
    {
        OcrResult result = new("主城 主 城", [
            new OcrTextLine("主城", Rectangle.FromXYWH(160, 395, 65, 25)),
            new OcrTextLine("主 城", Rectangle.FromXYWH(162, 396, 65, 25))]);

        var diagnostic = NotificationRegionalOcrCandidateSelector.Diagnose(
            result, Region, 2340, 1080, "主城", "issue59-diagnostic-main-city");

        diagnostic.Candidate.Should().BeNull();
        diagnostic.Reason.Should().Be(NotificationRegionalOcrRejectionReason.DuplicateLabel);
    }

    [Fact]
    public void Diagnose_EmptyRegion_ShouldFailClosed()
    {
        OcrResult result = new("", Array.Empty<OcrTextLine>());

        var diagnostic = NotificationRegionalOcrCandidateSelector.Diagnose(
            result, Region, 2340, 1080, "主城", "issue59-diagnostic-main-city");

        diagnostic.Candidate.Should().BeNull();
        diagnostic.Reason.Should().Be(NotificationRegionalOcrRejectionReason.NoTextDetected);
    }

    [Fact]
    public void Diagnose_IncorrectFrameSize_ShouldFailClosed()
    {
        OcrResult result = new("主城", [
            new OcrTextLine("主城", Rectangle.FromXYWH(160, 395, 65, 25))]);

        var diagnostic = NotificationRegionalOcrCandidateSelector.Diagnose(
            result, Region, 1920, 360, "主城", "issue59-diagnostic-main-city");

        diagnostic.Candidate.Should().BeNull();
        diagnostic.Reason.Should().Be(NotificationRegionalOcrRejectionReason.InvalidRegion);
    }
}