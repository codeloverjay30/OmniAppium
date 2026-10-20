using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue48RuntimeRegressionAdditionalTests
{
    [Fact]
    public async Task Click_WhenConcurrentCallersShareEvidence_ShouldClickAtMostOnce()
    {
        var snapshot = CreateSnapshot();
        var target = snapshot.Targets[0];
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var guard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);
        var authorization = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);
        var evidence = new Mock<IAtomicGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var lease = new Mock<IOcrGroundedActionLease>(MockBehavior.Strict);
        int consumed = 0;
        int clicks = 0;

        guard.Setup(x => x.AcquireCurrent(snapshot)).Returns(lease.Object);
        lease.Setup(x => x.Dispose());
        evidence.Setup(x => x.Consume(snapshot, target)).Returns(() =>
        {
            if (Interlocked.Increment(ref consumed) != 1)
                throw new InvalidOperationException("Evidence already consumed");
            return new ConsumedGroundedClickEvidence(null!, 0.99f);
        });
        authorization.Setup(x => x.Authorize(
                It.IsAny<string>(), It.IsAny<float>(),
                It.IsAny<Rectangle>(), It.IsAny<ReviewedGroundTruth>()))
            .Returns((15d, 25d));
        click.Setup(x => x.ClickAbsolute(15d, 25d))
            .Callback(() => Interlocked.Increment(ref clicks));

        var sut = new OcrGroundedClickService(
            click.Object, guard.Object, authorization.Object, evidence.Object);

        using var start = new ManualResetEventSlim(false);
        Task<Exception?>[] tasks = Enumerable.Range(0, 32).Select(_ =>
            Task.Run(() =>
            {
                start.Wait();
                try
                {
                    sut.Click(snapshot, "ocr-0");
                    return (Exception?)null;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            })).ToArray();

        start.Set();
        Exception?[] results = await Task.WhenAll(tasks);

        results.Count(x => x is null).Should().Be(1);
        results.Where(x => x is not null).Should().OnlyContain(x =>
            x is InvalidOperationException &&
            x.Message == "Evidence already consumed");
        clicks.Should().Be(1);
        click.Verify(x => x.ClickAbsolute(15d, 25d), Times.Once);
        lease.Verify(x => x.Dispose(), Times.Exactly(32));
    }

    [Fact]
    public void Click_WhenDeviceClickThrows_ShouldReleaseLeaseAndRejectEvidenceReplay()
    {
        var snapshot = CreateSnapshot();
        var target = snapshot.Targets[0];
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var guard = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);
        var authorization = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);
        var evidence = new Mock<IAtomicGroundedClickEvidenceProvider>(MockBehavior.Strict);
        var lease = new Mock<IOcrGroundedActionLease>(MockBehavior.Strict);
        int consumed = 0;

        guard.Setup(x => x.AcquireCurrent(snapshot)).Returns(lease.Object);
        lease.Setup(x => x.Dispose());
        evidence.Setup(x => x.Consume(snapshot, target)).Returns(() =>
        {
            if (Interlocked.Increment(ref consumed) != 1)
                throw new InvalidOperationException("Evidence already consumed");
            return new ConsumedGroundedClickEvidence(null!, 0.99f);
        });
        authorization.Setup(x => x.Authorize(
                It.IsAny<string>(), It.IsAny<float>(),
                It.IsAny<Rectangle>(), It.IsAny<ReviewedGroundTruth>()))
            .Returns((15d, 25d));
        click.Setup(x => x.ClickAbsolute(15d, 25d))
            .Throws(new InvalidOperationException("Device click failed"));

        var sut = new OcrGroundedClickService(
            click.Object, guard.Object, authorization.Object, evidence.Object);

        Action first = () => sut.Click(snapshot, "ocr-0");
        Action replay = () => sut.Click(snapshot, "ocr-0");

        first.Should().Throw<InvalidOperationException>()
            .WithMessage("*Device click failed*");
        replay.Should().Throw<InvalidOperationException>()
            .WithMessage("*Evidence already consumed*");
        click.Verify(x => x.ClickAbsolute(15d, 25d), Times.Once);
        authorization.Verify(x => x.Authorize(
            It.IsAny<string>(), It.IsAny<float>(),
            It.IsAny<Rectangle>(), It.IsAny<ReviewedGroundTruth>()), Times.Once);
        lease.Verify(x => x.Dispose(), Times.Exactly(2));
    }

    private static OcrGroundedSnapshot CreateSnapshot() =>
        new("snapshot-1", [
            new OcrGroundedTarget("ocr-0", "Target", Rectangle.FromXYWH(10, 20, 20, 20))
        ]);
}
