using System.Runtime.CompilerServices;
using FluentAssertions;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class ReviewedGroundedClickEvidenceConsumptionTests
{
    [Fact]
    public void Resolve_AfterFirstAuthorization_ShouldRejectReplay()
    {
        var (provider, snapshot, target) = CreateRegisteredSnapshot();

        provider.ResolveConfidence(snapshot, target).Should().Be(0.99f);
        provider.Resolve(snapshot, target).Should().NotBeNull();

        Action replay = () => provider.Resolve(snapshot, target);
        replay.Should().Throw<InvalidOperationException>()
            .WithMessage("*consum*");
    }

    [Fact]
    public async Task Resolve_ConcurrentCallers_ShouldAuthorizeExactlyOnce()
    {
        var (provider, snapshot, target) = CreateRegisteredSnapshot();
        using var start = new ManualResetEventSlim(false);

        Task<bool>[] attempts = Enumerable.Range(0, 32)
            .Select(_ => Task.Run(() =>
            {
                start.Wait();
                try
                {
                    provider.Resolve(snapshot, target);
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }))
            .ToArray();

        start.Set();
        bool[] results = await Task.WhenAll(attempts);

        results.Count(authorized => authorized).Should().Be(1);
        results.Count(authorized => !authorized).Should().Be(31);
    }

    [Fact]
    public void Resolve_AfterRevoke_ShouldReject()
    {
        var (provider, snapshot, target) = CreateRegisteredSnapshot();
        provider.Revoke(snapshot);

        Action act = () => provider.Resolve(snapshot, target);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*unavailable*");
    }

    private static (
        ReviewedGroundedClickEvidenceProvider Provider,
        OcrGroundedSnapshot Snapshot,
        OcrGroundedTarget Target) CreateRegisteredSnapshot()
    {
        var provider = new ReviewedGroundedClickEvidenceProvider(TimeProvider.System);
        var target = new OcrGroundedTarget(
            "target-1", "Play",
            CoordinateUtilityServices.Rectangle.FromXYWH(10, 20, 30, 40));
        var snapshot = new OcrGroundedSnapshot("frame-1", [target]);

        // Opaque sentinel: the provider must only store/return this instance.
        // Do not use this test object in OCR validation or a real click.
        var evidence = (ReviewedGroundTruth)RuntimeHelpers.GetUninitializedObject(
            typeof(ReviewedGroundTruth));

        provider.Register(snapshot, target, evidence, 0.99f, TimeSpan.FromSeconds(30));
        return (provider, snapshot, target);
    }
}
