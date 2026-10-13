
using FluentAssertions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityServices.Models.Stability;
using OmniAppium.EngineUtilityServices.Services.Stability;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Stability;

public sealed class AndroidScreenStabilityGuardTests
{
    private static AndroidScreenStabilityGuard CreateSut(
        Mock<IScreenshotService> screenshots,
        Mock<IScreenFrameComparer> comparer,
        TimeProvider? timeProvider = null)
    {
        var logger = new LoggerFactoryBaseUtilityService(
            NullLoggerFactory.Instance,
            nameof(AndroidScreenStabilityGuardTests));

        return new AndroidScreenStabilityGuard(
            screenshots.Object,
            comparer.Object,
            logger,
            timeProvider);
    }

    private static ScreenStabilityOptions CreateOptions(
        int requiredMatches = 2,
        int intervalMs = 1,
        int timeoutMs = 1000)
    {
        return new ScreenStabilityOptions
        {
            RequiredConsecutiveMatches = requiredMatches,
            SampleInterval = TimeSpan.FromMilliseconds(intervalMs),
            Timeout = TimeSpan.FromMilliseconds(timeoutMs),
            MaximumDifferenceRatio = 0.01
        };
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenFramesMatch_ShouldReturnStable()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        screenshots
            .SetupSequence(x => x.CaptureScreenshotBytes(null))
            .Returns(new byte[] { 1 })
            .Returns(new byte[] { 1 })
            .Returns(new byte[] { 1 });

        comparer
            .Setup(x => x.AreSimilar(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                0.01))
            .Returns(true);

        var sut = CreateSut(screenshots, comparer);

        ScreenStabilityResult result =
            await sut.WaitForStabilityAsync(CreateOptions());

        result.IsStable.Should().BeTrue();
        result.TimedOut.Should().BeFalse();
        result.SampleCount.Should().Be(3);

        screenshots.Verify(
            x => x.CaptureScreenshotBytes(null),
            Times.Exactly(3));

        comparer.Verify(
            x => x.AreSimilar(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                0.01),
            Times.Exactly(2));
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenFramesNeverMatch_ShouldTimeout()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(new byte[] { 1, 2, 3 });

        comparer
            .Setup(x => x.AreSimilar(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<double>()))
            .Returns(false);

        var sut = CreateSut(screenshots, comparer);

        ScreenStabilityResult result =
            await sut.WaitForStabilityAsync(
                CreateOptions(timeoutMs: 30));

        result.IsStable.Should().BeFalse();
        result.TimedOut.Should().BeTrue();
        result.SampleCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenCaptureIsEmpty_ShouldNotCompare()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(Array.Empty<byte>());

        var sut = CreateSut(screenshots, comparer);

        ScreenStabilityResult result =
            await sut.WaitForStabilityAsync(
                CreateOptions(timeoutMs: 30));

        result.IsStable.Should().BeFalse();
        result.TimedOut.Should().BeTrue();

        comparer.Verify(
            x => x.AreSimilar(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<double>()),
            Times.Never);
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenMatchSequenceBreaks_ShouldResetCount()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        screenshots
            .SetupSequence(x => x.CaptureScreenshotBytes(null))
            .Returns(new byte[] { 1 })
            .Returns(new byte[] { 2 })
            .Returns(new byte[] { 3 })
            .Returns(new byte[] { 4 })
            .Returns(new byte[] { 5 });

        comparer
            .SetupSequence(x => x.AreSimilar(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<double>()))
            .Returns(true)
            .Returns(false)
            .Returns(true)
            .Returns(true);

        var sut = CreateSut(screenshots, comparer);

        ScreenStabilityResult result =
            await sut.WaitForStabilityAsync(CreateOptions());

        result.IsStable.Should().BeTrue();
        result.TimedOut.Should().BeFalse();
        result.SampleCount.Should().Be(5);
    }

    [Fact]
    public void WaitForStabilityAsync_WhenAlreadyCancelled_ShouldThrow()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        var sut = CreateSut(screenshots, comparer);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        Action act = () =>
            sut.WaitForStabilityAsync(
                CreateOptions(),
                cts.Token).GetAwaiter().GetResult();

        act.Should()
            .Throw<OperationCanceledException>()
            .WithMessage("*canceled*");

        screenshots.Verify(
            x => x.CaptureScreenshotBytes(null),
            Times.Never);
    }

    [Fact]
    public void WaitForStabilityAsync_WhenCaptureThrows_ShouldPropagateException()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Throws(new InvalidOperationException(
                "Screenshot capture failed."));

        var sut = CreateSut(screenshots, comparer);

        Action act = () =>
            sut.WaitForStabilityAsync(
                CreateOptions()).GetAwaiter().GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Screenshot capture failed*");
    }

    [Fact]
    public void WaitForStabilityAsync_WhenComparerThrows_ShouldPropagateException()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(new byte[] { 1 });

        comparer
            .Setup(x => x.AreSimilar(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<double>()))
            .Throws(new InvalidOperationException(
                "Frame comparison failed."));

        var sut = CreateSut(screenshots, comparer);

        Action act = () =>
            sut.WaitForStabilityAsync(
                CreateOptions()).GetAwaiter().GetResult();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Frame comparison failed*");
    }

    [Fact]
    public void WaitForStabilityAsync_WhenOptionsAreNull_ShouldThrow()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        var sut = CreateSut(screenshots, comparer);

        Action act = () =>
            sut.WaitForStabilityAsync(null!)
                .GetAwaiter().GetResult();

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*options*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void WaitForStabilityAsync_WhenRequiredMatchesInvalid_ShouldThrow(
        int requiredMatches)
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        var sut = CreateSut(screenshots, comparer);

        Action act = () =>
            sut.WaitForStabilityAsync(
                CreateOptions(requiredMatches: requiredMatches))
                .GetAwaiter().GetResult();

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*RequiredConsecutiveMatches must be positive*");
    }

    [Fact]
    public async Task WaitForStabilityAsync_WhenScreenshotBufferIsReused_ShouldCopyPreviousFrame()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        byte[] sharedBuffer = { 1 };
        int captureCount = 0;

        screenshots
            .Setup(x => x.CaptureScreenshotBytes(null))
            .Returns(() =>
            {
                captureCount++;
                sharedBuffer[0] = (byte)captureCount;
                return sharedBuffer;
            });

        comparer
            .Setup(x => x.AreSimilar(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<double>()))
            .Returns((
                ReadOnlyMemory<byte> previous,
                ReadOnlyMemory<byte> current,
                double _) =>
            {
                previous.Span[0].Should().Be(1);
                current.Span[0].Should().Be(2);
                return true;
            });

        var sut = CreateSut(screenshots, comparer);

        ScreenStabilityResult result =
            await sut.WaitForStabilityAsync(
                CreateOptions(requiredMatches: 1));

        result.IsStable.Should().BeTrue();
        result.SampleCount.Should().Be(2);
    }

    [Fact]
    public void Constructor_WhenLoggerFactoryIsNull_ShouldThrow()
    {
        var screenshots = new Mock<IScreenshotService>(MockBehavior.Strict);
        var comparer = new Mock<IScreenFrameComparer>(MockBehavior.Strict);

        Action act = () => new AndroidScreenStabilityGuard(
            screenshots.Object,
            comparer.Object,
            null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*loggerFactoryBaseUtilityService*");
    }
}
