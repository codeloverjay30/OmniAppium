using System.Runtime.CompilerServices;
using FluentAssertions;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class ReviewedGroundedClickWeakConsumptionTests
{
    [Fact]
    public void Consume_WhenCaptureRemainsAlive_ShouldRejectReplay()
    {
        var provider = new ReviewedGroundedClickEvidenceProvider(TimeProvider.System);
        var target = new OcrGroundedTarget("t", "Play",
            CoordinateUtilityServices.Rectangle.FromXYWH(1, 2, 20, 20));
        var snapshot = new OcrGroundedSnapshot("s", [target]);
        var evidence = (ReviewedGroundTruth)RuntimeHelpers.GetUninitializedObject(
            typeof(ReviewedGroundTruth));
        provider.Register(snapshot, target, evidence, 0.9f, TimeSpan.FromSeconds(30));
        provider.Consume(snapshot, target);

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Action replay = () => provider.Consume(snapshot, target);
        replay.Should().Throw<InvalidOperationException>().WithMessage("*consum*");
        Action reregister = () =>
            provider.Register(snapshot, target, evidence, 0.9f, TimeSpan.FromSeconds(30));
        reregister.Should().Throw<InvalidOperationException>().WithMessage("*consum*");
        GC.KeepAlive(snapshot);
    }

    [Fact]
    public void Consume_WhenManyCapturesAreReleased_ShouldNotRetainTheirKeys()
    {
        var provider = new ReviewedGroundedClickEvidenceProvider(TimeProvider.System);
        WeakReference[] references = Enumerable.Range(0, 128)
            .Select(_ => ConsumeAndRelease(provider)).ToArray();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        references.Count(reference => reference.IsAlive).Should().Be(0);
        GC.KeepAlive(provider);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference ConsumeAndRelease(
        ReviewedGroundedClickEvidenceProvider provider)
    {
        var target = new OcrGroundedTarget("t", "Play",
            CoordinateUtilityServices.Rectangle.FromXYWH(1, 2, 20, 20));
        var snapshot = new OcrGroundedSnapshot(Guid.NewGuid().ToString("N"), [target]);
        var evidence = (ReviewedGroundTruth)RuntimeHelpers.GetUninitializedObject(
            typeof(ReviewedGroundTruth));
        provider.Register(snapshot, target, evidence, 0.9f, TimeSpan.FromSeconds(30));
        provider.Consume(snapshot, target);
        return new WeakReference(snapshot);
    }
}
