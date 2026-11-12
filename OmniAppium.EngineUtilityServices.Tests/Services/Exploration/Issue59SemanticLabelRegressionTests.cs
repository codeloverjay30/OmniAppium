using CoordinateUtilityServices;
using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59SemanticLabelRegressionTests
{
    [Theory]
    [InlineData("03 : 51 : 43")]
    [InlineData("03:51:43")]
    [InlineData("00：27：37")]
    [InlineData("5")]
    [InlineData("剩餘時間：2天")]
    [InlineData("倒數 03:51:43")]
    [InlineData(" ")]
    public void SecondaryText_ShouldNotBePrimaryLabel(string value)
    {
        NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(value).Should().BeFalse();
    }

    [Theory]
    [InlineData("武將")]
    [InlineData("裝 備")]
    [InlineData("背包")]
    [InlineData("限時禮包")]
    [InlineData("強者之路")]
    public void NavigationText_ShouldRemainPrimaryLabelCandidate(string value)
    {
        NotificationOcrLabelClassifier.IsPrimaryLabelCandidate(value).Should().BeTrue();
    }

    [Fact]
    public void TimerBelowBadge_ShouldNotAcquireOwnershipEvenWhenGeometryMatches()
    {
        var badge = new RedDotEvidence(Rectangle.FromXYWH(2159, 216, 29, 31), 0.976, []);
        var targets = new[]
        {
            new OcrGroundedTarget("timer", "03 : 51 : 43",
                Rectangle.FromXYWH(2095, 319, 84, 17))
        };
        var options = new NotificationAssociationOptions();

        NotificationAssociationGeometry.Resolve(badge, targets, options)
            .Should().BeEmpty();
    }

    [Fact]
    public void ExistingTimerCandidate_ShouldNotBypassSemanticGate()
    {
        var badge = new RedDotEvidence(Rectangle.FromXYWH(2159, 216, 29, 31),
            0.976, ["timer"]);
        var targets = new[]
        {
            new OcrGroundedTarget("timer", "03 : 51 : 43",
                Rectangle.FromXYWH(2095, 319, 84, 17))
        };

        NotificationAssociationGeometry.Resolve(badge, targets, new NotificationAssociationOptions())
            .Should().BeEmpty();
    }

    [Theory]
    [InlineData("warrior", "武將", 1492, 907, 1418, 1015, 89, 28)]
    [InlineData("equipment", "裝備", 1662, 907, 1620, 1016, 56, 27)]
    [InlineData("backpack", "背包", 1832, 907, 1789, 1015, 57, 28)]
    public void ReviewedBottomNavigation_ShouldRetainUniqueOwnership(
        string id, string label, int badgeX, int badgeY,
        int labelX, int labelY, int labelWidth, int labelHeight)
    {
        var badge = new RedDotEvidence(Rectangle.FromXYWH(badgeX, badgeY, 29, 31),
            0.98, []);
        var targets = new[]
        {
            new OcrGroundedTarget(id, label,
                Rectangle.FromXYWH(labelX, labelY, labelWidth, labelHeight))
        };

        NotificationAssociationGeometry.Resolve(badge, targets, new NotificationAssociationOptions())
            .Should().ContainSingle().Which.Should().Be(id);
    }

    [Fact]
    public void MissingPrimaryLabel_ShouldRemainUnownedEvenWhenIconReferencesTimer()
    {
        var badge = new RedDotEvidence(Rectangle.FromXYWH(2159, 216, 29, 31), 0.976, []);
        var targets = new[]
        {
            new OcrGroundedTarget("timer", "03:51:43",
                Rectangle.FromXYWH(2095, 319, 84, 17))
        };
        var icons = new[]
        {
            new NotificationIconTarget("timer",
                Rectangle.FromXYWH(2092, 219, 88, 88))
        };

        NotificationAssociationGeometry.ResolveViaIcons(
            badge, targets, icons, new NotificationAssociationOptions())
            .Should().BeEmpty();
    }
}
