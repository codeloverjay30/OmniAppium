using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Stability;

public sealed class AndroidScreenStabilityGuardTests
{
    private static ScreenStabilityOptions Options => new()
    {
        RequiredConsecutiveMatches = 2,
        SampleInterval = TimeSpan.FromMilliseconds(1),
        Timeout = TimeSpan.FromMilliseconds(150),
        MaximumDifferenceRatio = 0.01
    };

    [Fact]
    public async Task WaitForStabilityAsync_WhenThreeFramesMatch_ShouldReturnStable()
    {
        var capture = Sequence([1], [1], [1]);
        var comparer = SameBytesComparer();
        var sut = new AndroidScreenStabilityGuard(capture.Object, comparer.Object);
        var result = await sut.WaitForStabilityAsync(Options, TestContext.Current.CancellationToken);
        result.IsStable.Should().BeTrue();
        result.SampleCount.Should().Be(3);
        capture.Verify(x => x.CaptureScreenshotBytes(null), Times.Exactly(3));
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenMismatchOccurs_ShouldResetCounter()
    {
        var capture = Sequence([1], [1], [2], [2], [2]);
        var sut = new AndroidScreenStabilityGuard(capture.Object, SameBytesComparer().Object);
        var result = await sut.WaitForStabilityAsync(Options, TestContext.Current.CancellationToken);
        result.IsStable.Should().BeTrue();
        result.SampleCount.Should().Be(5);
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenFramesAlternate_ShouldTimeOutWithoutStableResult()
    {
        var capture = new Mock<IScreenshotService>(MockBehavior.Strict);
        int count = 0;
        capture.Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(() => [unchecked((byte)Interlocked.Increment(ref count))]);
        var sut = new AndroidScreenStabilityGuard(capture.Object, SameBytesComparer().Object);
        var result = await sut.WaitForStabilityAsync(Options, TestContext.Current.CancellationToken);
        result.IsStable.Should().BeFalse();
        result.TimedOut.Should().BeTrue();
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenCanceledBeforeCapture_ShouldNotCapture()
    {
        var capture = new Mock<IScreenshotService>(MockBehavior.Strict);
        var sut = new AndroidScreenStabilityGuard(capture.Object, SameBytesComparer().Object);
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        Func<Task> act = () => sut.WaitForStabilityAsync(Options, cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
        capture.Verify(x => x.CaptureScreenshotBytes(null), Times.Never);
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenCaptureThrows_ShouldPreserveExceptionMessage()
    {
        var capture = new Mock<IScreenshotService>(MockBehavior.Strict);
        capture.Setup(x => x.CaptureScreenshotBytes(null))
            .Throws(new InvalidOperationException("device capture failed"));
        var sut = new AndroidScreenStabilityGuard(capture.Object, SameBytesComparer().Object);
        Func<Task> act = () => sut.WaitForStabilityAsync(Options, TestContext.Current.CancellationToken);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("device capture failed");
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenEmptyFrameCaptured_ShouldNeverReportStable()
    {
        var capture = new Mock<IScreenshotService>(MockBehavior.Strict);
        capture.Setup(x => x.CaptureScreenshotBytes(null)).Returns([]);
        var sut = new AndroidScreenStabilityGuard(capture.Object, SameBytesComparer().Object);
        var result = await sut.WaitForStabilityAsync(Options, TestContext.Current.CancellationToken);
        result.IsStable.Should().BeFalse();
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenCallsOverlap_ShouldNotShareMatchCounter()
    {
        var capture = new Mock<IScreenshotService>(MockBehavior.Strict);
        capture.Setup(x => x.CaptureScreenshotBytes(null)).Returns([1]);
        var sut = new AndroidScreenStabilityGuard(capture.Object, SameBytesComparer().Object);
        var results = await Task.WhenAll(
            sut.WaitForStabilityAsync(Options, TestContext.Current.CancellationToken),
            sut.WaitForStabilityAsync(Options, TestContext.Current.CancellationToken));
        results.Should().OnlyContain(r => r.IsStable && r.SampleCount == 3);
        capture.Verify(x => x.CaptureScreenshotBytes(null), Times.Exactly(6));
    }

    private static Mock<IScreenshotService> Sequence(params byte[][] frames)
    {
        var capture = new Mock<IScreenshotService>(MockBehavior.Strict);
        var sequence = capture.SetupSequence(x => x.CaptureScreenshotBytes(null));
        foreach (var frame in frames)
            sequence = sequence.Returns(frame);
        return capture;
    }

    private static Mock<IScreenFrameComparer> SameBytesComparer()
    {
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);
        comparer.Setup(x => x.AreSimilar(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<double>()))
            .Returns((ReadOnlyMemory<byte> a, ReadOnlyMemory<byte> b, double _) =>
                a.Span.SequenceEqual(b.Span));
        return comparer;
    }
}
