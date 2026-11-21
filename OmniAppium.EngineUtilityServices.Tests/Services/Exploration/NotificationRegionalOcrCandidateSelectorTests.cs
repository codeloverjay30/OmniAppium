using CoordinateUtilityServices;
using FluentAssertions;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class NotificationRegionalOcrCandidateSelectorTests
{
    private static readonly Rectangle Region = Rectangle.FromXYWH(2030, 272, 250, 85);

    [Fact]
    public void Select_IsolatedPrimaryLabel_ShouldPreserveSourceBounds()
    {
        Rectangle bounds = Rectangle.FromXYWH(2083, 277, 109, 34);
        OcrResult result = Result(
            new OcrTextLine("限 時 禮 包", bounds),
            new OcrTextLine("05 : 55 : 10", Rectangle.FromXYWH(2096, 318, 84, 20)));

        var candidate = NotificationRegionalOcrCandidateSelector.Select(
            result, Region, 2340, 1080, "限時禮包", "regional-gift");

        candidate.Should().NotBeNull();
        candidate!.Id.Should().Be("regional-gift");
        candidate.Text.Should().Be("限 時 禮 包");
        candidate.Bounds.Should().Be(bounds);
    }

    [Fact]
    public void Select_CountdownOnly_ShouldReject()
    {
        OcrResult result = Result(
            new OcrTextLine("05 : 55 : 10", Rectangle.FromXYWH(2096, 318, 84, 20)));

        var candidate = NotificationRegionalOcrCandidateSelector.Select(
            result, Region, 2340, 1080, "限時禮包", "regional-gift");

        candidate.Should().BeNull();
    }

    [Fact]
    public void Select_MergedLabel_ShouldReject()
    {
        OcrResult result = Result(
            new OcrTextLine("活動日曆 限 時 禮 包", Rectangle.FromXYWH(2035, 277, 210, 34)));

        var candidate = NotificationRegionalOcrCandidateSelector.Select(
            result, Region, 2340, 1080, "限時禮包", "regional-gift");

        candidate.Should().BeNull();
    }

    [Fact]
    public void Select_DuplicatePrimaryLabels_ShouldReject()
    {
        OcrResult result = Result(
            new OcrTextLine("限時禮包", Rectangle.FromXYWH(2083, 277, 109, 34)),
            new OcrTextLine("限 時 禮 包", Rectangle.FromXYWH(2083, 279, 109, 34)));

        var candidate = NotificationRegionalOcrCandidateSelector.Select(
            result, Region, 2340, 1080, "限時禮包", "regional-gift");

        candidate.Should().BeNull();
    }

    [Fact]
    public void Select_OutOfRegionBounds_ShouldReject()
    {
        OcrResult result = Result(
            new OcrTextLine("限時禮包", Rectangle.FromXYWH(2000, 277, 109, 34)));

        var candidate = NotificationRegionalOcrCandidateSelector.Select(
            result, Region, 2340, 1080, "限時禮包", "regional-gift");

        candidate.Should().BeNull();
    }

    [Fact]
    public void Select_OutOfFrameRegion_ShouldReject()
    {
        OcrResult result = Result(
            new OcrTextLine("限時禮包", Rectangle.FromXYWH(2083, 277, 109, 34)));

        var candidate = NotificationRegionalOcrCandidateSelector.Select(
            result, Region, 1920, 1080, "限時禮包", "regional-gift");

        candidate.Should().BeNull();
    }

    [Fact]
    public void Diagnose_MissingCharacter_ShouldReportLabelMismatch()
    {
        OcrResult result = Result(
            new OcrTextLine("限 時 包", Rectangle.FromXYWH(2083, 277, 109, 34)),
            new OcrTextLine("05 : 19 : 32", Rectangle.FromXYWH(2096, 318, 84, 20)));

        var diagnostic = NotificationRegionalOcrCandidateSelector.Diagnose(
            result, Region, 2340, 1080, "限時禮包", "regional-gift");

        diagnostic.Candidate.Should().BeNull();
        diagnostic.Reason.Should().Be(NotificationRegionalOcrRejectionReason.LabelMismatch);
        diagnostic.ObservedText.Should().Be("限時包");
    }

    [Fact]
    public void Diagnose_DuplicateLabels_ShouldReportDuplicateLabel()
    {
        OcrResult result = Result(
            new OcrTextLine("限時禮包", Rectangle.FromXYWH(2083, 277, 109, 34)),
            new OcrTextLine("限 時 禮 包", Rectangle.FromXYWH(2083, 279, 109, 34)));

        var diagnostic = NotificationRegionalOcrCandidateSelector.Diagnose(
            result, Region, 2340, 1080, "限時禮包", "regional-gift");

        diagnostic.Candidate.Should().BeNull();
        diagnostic.Reason.Should().Be(NotificationRegionalOcrRejectionReason.DuplicateLabel);
    }

    [Fact]
    public void Diagnose_OutsideRegion_ShouldReportOutOfRegion()
    {
        OcrResult result = Result(
            new OcrTextLine("限時禮包", Rectangle.FromXYWH(2000, 277, 109, 34)));

        var diagnostic = NotificationRegionalOcrCandidateSelector.Diagnose(
            result, Region, 2340, 1080, "限時禮包", "regional-gift");

        diagnostic.Candidate.Should().BeNull();
        diagnostic.Reason.Should().Be(NotificationRegionalOcrRejectionReason.OutOfRegion);
    }

    [Fact]
    public void Diagnose_EmptyOcr_ShouldReportNoTextDetected()
    {
        var diagnostic = NotificationRegionalOcrCandidateSelector.Diagnose(
            Result(), Region, 2340, 1080, "限時禮包", "regional-gift");

        diagnostic.Candidate.Should().BeNull();
        diagnostic.Reason.Should().Be(NotificationRegionalOcrRejectionReason.NoTextDetected);
    }

    [Fact]
    public void Diagnose_ValidLabel_ShouldPreserveCandidateAndBounds()
    {
        Rectangle bounds = Rectangle.FromXYWH(2083, 277, 109, 34);
        var diagnostic = NotificationRegionalOcrCandidateSelector.Diagnose(
            Result(new OcrTextLine("限 時 禮 包", bounds)),
            Region, 2340, 1080, "限時禮包", "regional-gift");

        diagnostic.Reason.Should().Be(NotificationRegionalOcrRejectionReason.None);
        diagnostic.Candidate.Should().NotBeNull();
        diagnostic.Candidate!.Bounds.Should().Be(bounds);
    }

    private static OcrResult Result(params OcrTextLine[] lines) =>
        new(string.Join(" ", lines.Select(line => line.Text)), lines);
}