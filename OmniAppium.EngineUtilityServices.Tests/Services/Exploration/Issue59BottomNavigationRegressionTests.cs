using CoordinateUtilityServices;
using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59BottomNavigationRegressionTests
{
    private const string ObservationId = "8d9316ebc61b4503b0eb508237594f0e";
    private const string GeneralId = "issue59-ocr-000018";
    private const string BagId = "issue59-ocr-000019";
    private const string EquipmentId = "issue59-ocr-000020";

    [Theory]
    [InlineData(GeneralId, "一 武 將", 1418, 1015, 89, 28, 1492, 907, 0.970)]
    [InlineData(EquipmentId, "裝 備", 1620, 1016, 56, 27, 1662, 907, 0.980)]
    [InlineData(BagId, "背 包", 1789, 1015, 57, 28, 1832, 907, 0.980)]
    public void CapturedBottomNavigationBadge_ShouldResolveToItsOcrLabel(
        string id, string text, int x, int y, int width, int height,
        int badgeX, int badgeY, double confidence)
    {
        OcrGroundedTarget target = Target(id, text, x, y, width, height);
        RedDotEvidence badge = Badge(badgeX, badgeY, confidence);

        IReadOnlyList<string> owners = NotificationAssociationGeometry.Resolve(
            badge, [target], new NotificationAssociationOptions());

        owners.Should().ContainSingle().Which.Should().Be(id);
    }

    [Fact]
    public void CapturedThreeBadgesTogether_ShouldAssociateEachWithExactlyOneDistinctTarget()
    {
        IReadOnlyList<GameUiNode> nodes = new NotificationNodeAssociator()
            .Associate(ObservationId, CapturedTargets(), CapturedBadges());

        nodes.Should().HaveCount(3);
        nodes.Should().OnlyContain(node =>
            node.HasRedDot &&
            !node.IsRedDotAssociationAmbiguous &&
            node.ObservationId == ObservationId &&
            node.Kind == GameUiNodeKind.Unknown &&
            node.Confidence == 0);

        nodes.Should().ContainSingle(node =>
            node.Id == GeneralId && node.Text == "一 武 將");
        nodes.Should().ContainSingle(node =>
            node.Id == EquipmentId && node.Text == "裝 備");
        nodes.Should().ContainSingle(node =>
            node.Id == BagId && node.Text == "背 包");

        IReadOnlyList<OcrGroundedTarget> targets = CapturedTargets();
        foreach (RedDotEvidence badge in CapturedBadges())
        {
            IReadOnlyList<string> owners = NotificationAssociationGeometry.Resolve(
                badge, targets, new NotificationAssociationOptions());
            owners.Should().ContainSingle();
        }

        NotificationAssociationGeometry.Resolve(
            CapturedBadges()[0], targets, new NotificationAssociationOptions())
            .Should().Equal(GeneralId);
        NotificationAssociationGeometry.Resolve(
            CapturedBadges()[1], targets, new NotificationAssociationOptions())
            .Should().Equal(EquipmentId);
        NotificationAssociationGeometry.Resolve(
            CapturedBadges()[2], targets, new NotificationAssociationOptions())
            .Should().Equal(BagId);
    }

    [Fact]
    public void CapturedGeneralBadge_WhenGeneralOcrTargetMissing_ShouldNotBorrowEquipmentOrBag()
    {
        IReadOnlyList<OcrGroundedTarget> targets =
            CapturedTargets().Where(target => target.Id != GeneralId).ToArray();

        NotificationAssociationGeometry.Resolve(
            CapturedBadges()[0], targets, new NotificationAssociationOptions())
            .Should().BeEmpty();

        IReadOnlyList<GameUiNode> nodes = new NotificationNodeAssociator()
            .Associate(ObservationId, targets, [CapturedBadges()[0]]);

        nodes.Should().HaveCount(2);
        nodes.Should().OnlyContain(node => !node.HasRedDot);
    }

    [Fact]
    public void CapturedGeneralBadge_WithDuplicatePlausibleOcrOwner_ShouldRemainAmbiguous()
    {
        OcrGroundedTarget duplicate = Target(
            "duplicate-general", "武 將", 1419, 1015, 88, 28);
        IReadOnlyList<OcrGroundedTarget> targets =
            [.. CapturedTargets(), duplicate];

        NotificationAssociationGeometry.Resolve(
            CapturedBadges()[0], targets, new NotificationAssociationOptions())
            .Should().BeEquivalentTo([GeneralId, "duplicate-general"]);

        IReadOnlyList<GameUiNode> nodes = new NotificationNodeAssociator()
            .Associate(ObservationId, targets, [CapturedBadges()[0]]);

        nodes.Where(node => node.Id is GeneralId or "duplicate-general")
            .Should().OnlyContain(node =>
                !node.HasRedDot && node.IsRedDotAssociationAmbiguous);
        nodes.Where(node => node.Id is EquipmentId or BagId)
            .Should().OnlyContain(node =>
                !node.HasRedDot && !node.IsRedDotAssociationAmbiguous);
    }

    [Fact]
    public void CapturedThreeBadges_WhenIconAboveLabelDisabled_ShouldNotAssociate()
    {
        var options = new NotificationAssociationOptions
        {
            EnableIconAboveLabel = false
        };

        IReadOnlyList<GameUiNode> nodes = new NotificationNodeAssociator(options)
            .Associate(ObservationId, CapturedTargets(), CapturedBadges());

        nodes.Should().HaveCount(3);
        nodes.Should().OnlyContain(node => !node.HasRedDot);
    }

    [Fact]
    public void CapturedGeneralBadge_WithConflictingPreexistingCandidate_ShouldFailClosed()
    {
        RedDotEvidence original = CapturedBadges()[0];
        RedDotEvidence conflicting = original with
        {
            CandidateNodeIds = [EquipmentId]
        };

        IReadOnlyList<GameUiNode> nodes = new NotificationNodeAssociator()
            .Associate(ObservationId, CapturedTargets(), [conflicting]);

        nodes.Where(node => node.Id is GeneralId or EquipmentId)
            .Should().OnlyContain(node =>
                !node.HasRedDot && node.IsRedDotAssociationAmbiguous);
        nodes.Should().ContainSingle(node =>
            node.Id == BagId && !node.HasRedDot);
    }

    private static IReadOnlyList<OcrGroundedTarget> CapturedTargets() =>
    [
        Target(GeneralId, "一 武 將", 1418, 1015, 89, 28),
        Target(EquipmentId, "裝 備", 1620, 1016, 56, 27),
        Target(BagId, "背 包", 1789, 1015, 57, 28)
    ];

    private static IReadOnlyList<RedDotEvidence> CapturedBadges() =>
    [
        Badge(1492, 907, 0.970),
        Badge(1662, 907, 0.980),
        Badge(1832, 907, 0.980)
    ];

    private static OcrGroundedTarget Target(
        string id, string text, int x, int y, int width, int height) =>
        new(id, text, Rectangle.FromXYWH(x, y, width, height));

    private static RedDotEvidence Badge(int x, int y, double confidence) =>
        new(Rectangle.FromXYWH(x, y, 29, 31), confidence, []);
}
