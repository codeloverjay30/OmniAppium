using FluentAssertions;
using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Services.Exploration;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59NotificationDefinitionCandidateTests
{
    [Fact]
    public void TemplateMatch_WithPureRedDot_ShouldNotConfirmNotification()
    {
        using Mat badge = CreateBadge();
        using Mat screen = CreateScreen();
        Cv2.Circle(screen, new Point(64, 44), 11, new Scalar(0, 0, 240), -1);

        IReadOnlyList<NotificationTemplateMatch> matches = Match(screen, badge);

        matches.Should().BeEmpty("a solid red dot lacks the required white exclamation mark");
    }

    [Fact]
    public void TemplateMatch_WithTwoValidBadges_ShouldLocateBoth()
    {
        using Mat badge = CreateBadge();
        using Mat screen = CreateScreen();
        CopyBadge(screen, badge, 25, 30);
        CopyBadge(screen, badge, 105, 70);

        IReadOnlyList<NotificationTemplateMatch> matches = Match(screen, badge);

        matches.Should().HaveCount(2);
        matches.Should().ContainSingle(m => m.X == 25 && m.Y == 30);
        matches.Should().ContainSingle(m => m.X == 105 && m.Y == 70);
    }

    [Fact]
    public void TemplateMatch_WithOneValidBadge_ShouldSuppressDuplicateCandidates()
    {
        using Mat badge = CreateBadge();
        using Mat screen = CreateScreen();
        CopyBadge(screen, badge, 55, 45);

        IReadOnlyList<NotificationTemplateMatch> matches = Match(screen, badge);

        matches.Should().ContainSingle();
        matches[0].X.Should().Be(55);
        matches[0].Y.Should().Be(45);
    }

    [Fact]
    public void Planner_WithAmbiguousNotificationOwner_ShouldStopWithoutSelectingTarget()
    {
        var planner = new RedDotExplorationPlanner();
        var nodes = new[]
        {
            new GameUiNode("entry", "capture-1", GameUiNodeKind.Entry,
                "超值活動", true, true, 0.99)
        };

        ExplorationDecision decision = planner.Plan(
            "capture-1", nodes, new HashSet<string>(), 0, 3);

        decision.Kind.Should().Be(ExplorationDecisionKind.Stop);
        decision.TargetId.Should().BeNull();
    }

    private static IReadOnlyList<NotificationTemplateMatch> Match(Mat screen, Mat badge)
    {
        Cv2.ImEncode(".png", screen, out byte[] screenBytes)
            .Should().BeTrue();
        Cv2.ImEncode(".png", badge, out byte[] badgeBytes)
            .Should().BeTrue();

        return new OpenCvNotificationTemplateMatcher().Match(
            screenBytes,
            badgeBytes,
            new NotificationTemplateMatchOptions(
                MinimumSimilarity: 0.99,
                MinimumScale: 1,
                MaximumScale: 1,
                MaximumMatches: 32));
    }

    private static Mat CreateScreen() =>
        new(160, 180, MatType.CV_8UC3, new Scalar(30, 30, 30));

    private static Mat CreateBadge()
    {
        var badge = new Mat(24, 24, MatType.CV_8UC3, new Scalar(30, 30, 30));
        Cv2.Circle(badge, new Point(12, 12), 11, new Scalar(0, 0, 240), -1);
        Cv2.Line(badge, new Point(12, 5), new Point(12, 14), Scalar.White, 2);
        Cv2.Circle(badge, new Point(12, 18), 1, Scalar.White, -1);
        return badge;
    }

    private static void CopyBadge(Mat screen, Mat badge, int x, int y)
    {
        using Mat region = new(screen, new Rect(x, y, badge.Width, badge.Height));
        badge.CopyTo(region);
    }
}
