using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class ReviewedGroundedClickEvidenceProviderTests
{
    [Fact]
    public void Resolve_WithoutReviewedRegistration_ShouldReject()
    {
        var provider = new ReviewedGroundedClickEvidenceProvider(TimeProvider.System);
        var target = new OcrGroundedTarget(
            "target-1", "Play",
            CoordinateUtilityServices.Rectangle.FromXYWH(10, 20, 30, 40));
        var snapshot = new OcrGroundedSnapshot("frame-1", [target]);

        Action act = () => provider.Resolve(snapshot, target);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Reviewed click evidence is unavailable*");
    }

    [Fact]
    public void ResolveConfidence_WithoutReviewedRegistration_ShouldReject()
    {
        var provider = new ReviewedGroundedClickEvidenceProvider(TimeProvider.System);
        var target = new OcrGroundedTarget(
            "target-1", "Play",
            CoordinateUtilityServices.Rectangle.FromXYWH(10, 20, 30, 40));
        var snapshot = new OcrGroundedSnapshot("frame-1", [target]);

        Action act = () => provider.ResolveConfidence(snapshot, target);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Reviewed click evidence is unavailable*");
    }

    [Fact]
    public void Register_WithNullReviewedEvidence_ShouldReject()
    {
        var provider = new ReviewedGroundedClickEvidenceProvider(TimeProvider.System);
        var target = new OcrGroundedTarget(
            "target-1", "Play",
            CoordinateUtilityServices.Rectangle.FromXYWH(10, 20, 30, 40));
        var snapshot = new OcrGroundedSnapshot("frame-1", [target]);

        Action act = () => provider.Register(
            snapshot, target, null!, 0.99f, TimeSpan.FromSeconds(5));

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*evidence*");
    }
}
