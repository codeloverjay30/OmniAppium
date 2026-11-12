using CoordinateUtilityServices;
using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59OcrSegmentationDiagnosticsTests
{
    [Fact]
    public void CountdownNearGiftBadge_ShouldBeReportedButNotClassifiedAsPrimary()
    {
        var badges = new[] { new RedDotEvidence(Rectangle.FromXYWH(2159, 216, 29, 31), 0.97, []) };
        var targets = new[]
        {
            new OcrGroundedTarget("timer", "03 : 51 : 43", Rectangle.FromXYWH(2095, 319, 84, 17))
        };

        var findings = NotificationOcrSegmentationDiagnostics.Inspect(badges, targets);

        findings.Should().ContainSingle()
            .Which.IsPrimaryLabelCandidate.Should().BeFalse();
        badges[0].CandidateNodeIds.Should().BeEmpty();
    }

    [Fact]
    public void MergedLookingText_ShouldBeFlaggedForReviewWithoutSynthesizingCharacterBounds()
    {
        var badges = new[] { new RedDotEvidence(Rectangle.FromXYWH(2159, 216, 29, 31), 0.97, []) };
        var targets = new[]
        {
            new OcrGroundedTarget("merged", "限時禮包 強者之路",
                Rectangle.FromXYWH(2020, 250, 180, 35))
        };

        var findings = NotificationOcrSegmentationDiagnostics.Inspect(badges, targets);

        findings.Should().ContainSingle()
            .Which.ContainsMultipleWhitespaceSeparatedTokens.Should().BeTrue();
        targets.Should().ContainSingle();
        badges[0].CandidateNodeIds.Should().BeEmpty();
    }

    [Fact]
    public void FarAwayText_ShouldNotBeReported()
    {
        var badges = new[] { new RedDotEvidence(Rectangle.FromXYWH(2159, 216, 29, 31), 0.97, []) };
        var targets = new[]
        {
            new OcrGroundedTarget("far", "武將", Rectangle.FromXYWH(1400, 1000, 60, 30))
        };

        NotificationOcrSegmentationDiagnostics.Inspect(badges, targets)
            .Should().BeEmpty();
    }

    [Fact]
    public void InvalidDistance_ShouldBeRejectedWithMessage()
    {
        Action act = () => NotificationOcrSegmentationDiagnostics.Inspect(
            Array.Empty<RedDotEvidence>(), Array.Empty<OcrGroundedTarget>(), double.NaN);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*finite and positive*");
    }
}