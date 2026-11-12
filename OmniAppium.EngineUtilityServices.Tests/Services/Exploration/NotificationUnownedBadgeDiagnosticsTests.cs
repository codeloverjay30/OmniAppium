using CoordinateUtilityServices;
using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class NotificationUnownedBadgeDiagnosticsTests
{
    [Fact]
    public void Inspect_WithCapturedBottomNavigationBadge_ShouldNotReportOwnedBadge()
    {
        RedDotEvidence badge = Badge(1492, 907);
        OcrGroundedTarget target = Target("general", "一 武 將", 1418, 1015, 89, 28);

        IReadOnlyList<NotificationUnownedBadgeDiagnostic> diagnostics =
            NotificationUnownedBadgeDiagnostics.Inspect(
                [badge], [target], new NotificationAssociationOptions(), 2340, 1080);

        diagnostics.Should().BeEmpty();
    }

    [Fact]
    public void Inspect_WithUnownedBadge_ShouldReportRegionAndNearestRejectionWithoutChangingEvidence()
    {
        RedDotEvidence badge = Badge(816, 989);
        OcrGroundedTarget target = Target("battle", "三國巔峰戰", 800, 880, 110, 25);

        IReadOnlyList<NotificationUnownedBadgeDiagnostic> diagnostics =
            NotificationUnownedBadgeDiagnostics.Inspect(
                [badge], [target], new NotificationAssociationOptions(), 2340, 1080);

        diagnostics.Should().ContainSingle();
        diagnostics[0].BadgeIndex.Should().Be(0);
        diagnostics[0].Region.Should().Be("BottomCenter");
        diagnostics[0].NearestTargetId.Should().Be("battle");
        diagnostics[0].NearestRejectionReason.Should().NotBeNullOrWhiteSpace();
        badge.CandidateNodeIds.Should().BeEmpty();
        NotificationAssociationGeometry.Resolve(
            badge, [target], new NotificationAssociationOptions()).Should().BeEmpty();
    }

    [Fact]
    public void Inspect_WithoutOcrTargets_ShouldExplainMissingTargets()
    {
        IReadOnlyList<NotificationUnownedBadgeDiagnostic> diagnostics =
            NotificationUnownedBadgeDiagnostics.Inspect(
                [Badge(500, 500)], [], new NotificationAssociationOptions(),
                2340, 1080);

        diagnostics.Should().ContainSingle();
        diagnostics[0].Hint.Should().Be("NoOcrTargets");
        diagnostics[0].NearestTargetId.Should().BeNull();
    }

    [Fact]
    public void Inspect_WithInvalidImageDimensions_ShouldRejectInput()
    {
        Action act = () => NotificationUnownedBadgeDiagnostics.Inspect(
            [Badge(500, 500)], [], new NotificationAssociationOptions(), 0, 1080);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Image dimensions must be positive*");
    }

    [Fact]
    public void Inspect_WithMergedOcrRectangle_ShouldReportAllNearbyTargetsWithoutInventingGroundTruth()
    {
        RedDotEvidence badge = Badge(1659, 216);
        OcrGroundedTarget merged = Target("merged", "演練 探險 活動日曆", 1208, 282, 734, 27);
        OcrGroundedTarget other = Target("other", "福利", 2050, 282, 80, 27);

        NotificationUnownedBadgeDiagnostic diagnostic =
            NotificationUnownedBadgeDiagnostics.Inspect(
                [badge], [merged, other], new NotificationAssociationOptions(),
                2340, 1080).Single();

        diagnostic.NearbyTargets.Should().HaveCount(2);
        diagnostic.NearbyTargets.Should().Contain(x => x.TargetId == "merged");
        diagnostic.PossibleMergedOcr.Should().BeTrue();
        diagnostic.NearbyTargets.Single(x => x.TargetId == "merged")
            .OcrTargetWidthRatio.Should().BeApproximately(734d / 2340d, 0.000001);
        diagnostic.CandidateVisualType.Should().Be("Unknown");
        diagnostic.GroundTruthStatus.Should().Be("Unreviewed");
        diagnostic.RequiresManualReview.Should().BeTrue();
        badge.CandidateNodeIds.Should().BeEmpty();
    }

    [Fact]
    public void Inspect_WithLabelAboveBadge_ShouldOnlySuggestLayout()
    {
        RedDotEvidence badge = Badge(816, 989);
        OcrGroundedTarget target = Target("battle", "三國巔峰戰", 658, 912, 167, 32);

        NotificationUnownedBadgeDiagnostic diagnostic =
            NotificationUnownedBadgeDiagnostics.Inspect(
                [badge], [target], new NotificationAssociationOptions(),
                2340, 1080).Single();

        diagnostic.SuggestedLayout.Should().Be("LabelAboveIcon");
        diagnostic.NearbyTargets.Should().ContainSingle();
        diagnostic.CandidateVisualType.Should().Be("Unknown");
        badge.CandidateNodeIds.Should().BeEmpty();
    }

    [Fact]
    public void Inspect_WithNoOcr_ShouldKeepAllReviewFieldsConservative()
    {
        NotificationUnownedBadgeDiagnostic diagnostic =
            NotificationUnownedBadgeDiagnostics.Inspect(
                [Badge(500, 500)], [], new NotificationAssociationOptions(),
                2340, 1080).Single();

        diagnostic.NearbyTargets.Should().BeEmpty();
        diagnostic.OcrTargetWidthRatio.Should().BeNull();
        diagnostic.PossibleMergedOcr.Should().BeFalse();
        diagnostic.SuggestedLayout.Should().Be("Unknown");
        diagnostic.RequiresManualReview.Should().BeTrue();
    }

    private static OcrGroundedTarget Target(
        string id, string text, int x, int y, int width, int height) =>
        new(id, text, Rectangle.FromXYWH(x, y, width, height));

    private static RedDotEvidence Badge(int x, int y) =>
        new(Rectangle.FromXYWH(x, y, 29, 31), 0.98, []);
}
