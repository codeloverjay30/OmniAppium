using System.Runtime.CompilerServices;
using FluentAssertions;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class ReviewedGroundedClickAtomicConsumptionTests
{
    [Fact]
    public void Consume_ShouldReturnEvidenceAndConfidenceTogether()
    {
        var (provider, snapshot, target, evidence) = CreateRegistered();
        ConsumedGroundedClickEvidence result = provider.Consume(snapshot, target);
        result.Evidence.Should().BeSameAs(evidence);
        result.Confidence.Should().Be(0.99f);
    }

    [Fact]
    public void Consume_AfterConsumption_ShouldRejectReplay()
    {
        var (provider, snapshot, target, _) = CreateRegistered();
        provider.Consume(snapshot, target);
        Action act = () => provider.Consume(snapshot, target);
        act.Should().Throw<InvalidOperationException>().WithMessage("*consum*");
    }

    [Fact]
    public void Register_AfterConsumption_ShouldRejectSameCapture()
    {
        var (provider, snapshot, target, evidence) = CreateRegistered();
        provider.Consume(snapshot, target);
        Action act = () => provider.Register(snapshot, target, evidence, 0.9f, TimeSpan.FromSeconds(10));
        act.Should().Throw<InvalidOperationException>().WithMessage("*consum*");
    }

    [Fact]
    public void Register_WhileActive_ShouldRejectReplacement()
    {
        var (provider, snapshot, target, evidence) = CreateRegistered();
        Action act = () => provider.Register(snapshot, target, evidence, 0.5f, TimeSpan.FromSeconds(10));
        act.Should().Throw<InvalidOperationException>().WithMessage("*registered*");
    }

    [Fact]
    public async Task Consume_ConcurrentCallers_ShouldSucceedExactlyOnce()
    {
        var (provider, snapshot, target, _) = CreateRegistered();
        using var start = new ManualResetEventSlim(false);
        Task<bool>[] attempts = Enumerable.Range(0, 32).Select(_ => Task.Run(() =>
        {
            start.Wait();
            try
            {
                provider.Consume(snapshot, target);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        })).ToArray();
        start.Set();
        bool[] results = await Task.WhenAll(attempts);
        results.Count(success => success).Should().Be(1);
    }

    [Fact]
    public void Consume_AfterRevoke_ShouldReject()
    {
        var (provider, snapshot, target, _) = CreateRegistered();
        provider.Revoke(snapshot);
        Action act = () => provider.Consume(snapshot, target);
        act.Should().Throw<InvalidOperationException>().WithMessage("*unavailable*");
    }

    [Fact]
    public void Consume_WrongTarget_ShouldNotConsumeValidEvidence()
    {
        var (provider, snapshot, target, evidence) = CreateRegistered();
        var other = new OcrGroundedTarget("other", "Other",
            CoordinateUtilityServices.Rectangle.FromXYWH(10, 20, 30, 40));
        Action act = () => provider.Consume(snapshot, other);
        act.Should().Throw<InvalidOperationException>().WithMessage("*target*");
        provider.Consume(snapshot, target).Evidence.Should().BeSameAs(evidence);
    }

    private static (ReviewedGroundedClickEvidenceProvider Provider,
        OcrGroundedSnapshot Snapshot, OcrGroundedTarget Target,
        ReviewedGroundTruth Evidence) CreateRegistered()
    {
        var provider = new ReviewedGroundedClickEvidenceProvider(TimeProvider.System);
        var target = new OcrGroundedTarget("target-1", "Play",
            CoordinateUtilityServices.Rectangle.FromXYWH(10, 20, 30, 40));
        var snapshot = new OcrGroundedSnapshot("frame-1", [target]);
        var evidence = (ReviewedGroundTruth)RuntimeHelpers.GetUninitializedObject(
            typeof(ReviewedGroundTruth));
        provider.Register(snapshot, target, evidence, 0.99f, TimeSpan.FromSeconds(30));
        return (provider, snapshot, target, evidence);
    }
}
