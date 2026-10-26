using CoordinateUtilityServices;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using Moq;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Planner;
using System.IO.Abstractions.TestingHelpers;
using Rectangle = CoordinateUtilityServices.Rectangle;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Planner;

public sealed class TrustedRunSnapshotBindingTests
{
    private const string RunDirectory = @"C:\\reviews\\run-1";
    private const string ScreenshotPath = @"C:\\reviews\\run-1\\task-home-original.png";
    private static readonly byte[] ReviewedImage = [1, 2, 3, 4, 5];

    [Fact]
    public async Task VerifyAsync_WhenEvidenceMatches_ShouldSucceed()
    {
        var fixture = CreateFixture();
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, "target-1", CancellationToken.None);
        await act.Should().NotThrowAsync();
    }

    [Theory]
    [InlineData("different-target")]
    [InlineData("")]
    public async Task VerifyAsync_WhenTargetIsNotUnique_ShouldReject(string targetId)
    {
        var fixture = CreateFixture();
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, targetId, CancellationToken.None);
        if (targetId.Length == 0)
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("*targetId*");
        else
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*exactly one requested target*");
        fixture.Source.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task VerifyAsync_WhenScreenshotDiffers_ShouldReject()
    {
        var fixture = CreateFixture(image: [9, 8, 7]);
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, "target-1", CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*Current screenshot differs*");
    }

    [Fact]
    public async Task VerifyAsync_WhenEvidenceIsStale_ShouldReject()
    {
        var fixture = CreateFixture(age: TimeSpan.FromSeconds(11));
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, "target-1", CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*stale or future-dated*");
    }

    [Fact]
    public async Task VerifyAsync_WhenEvidenceIsFutureDated_ShouldReject()
    {
        var fixture = CreateFixture(age: TimeSpan.FromSeconds(-1));
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, "target-1", CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*stale or future-dated*");
    }

    [Fact]
    public async Task VerifyAsync_WhenSnapshotInstanceDiffers_ShouldReject()
    {
        var fixture = CreateFixture(useDifferentSnapshot: true);
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, "target-1", CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*target binding mismatch*");
    }

    [Fact]
    public async Task VerifyAsync_WhenTargetTextDiffers_ShouldReject()
    {
        var fixture = CreateFixture(text: "unexpected");
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, "target-1", CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*target binding mismatch*");
    }

    [Fact]
    public async Task VerifyAsync_WhenTargetBoundsDiffer_ShouldReject()
    {
        var fixture = CreateFixture(changeBounds: true);
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, "target-1", CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*target binding mismatch*");
    }

    [Fact]
    public async Task VerifyAsync_WhenReviewedScreenshotIsMissing_ShouldRejectWithoutReadingSource()
    {
        var fixture = CreateFixture(includeScreenshot: false);
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, "target-1", CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*screenshot is missing or linked*");
        fixture.Source.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task VerifyAsync_WhenEvidenceIsUnavailable_ShouldReject()
    {
        var fixture = CreateFixture(returnNull: true);
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, "target-1", CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*evidence is unavailable*");
    }

    [Fact]
    public async Task VerifyAsync_WhenCancelled_ShouldNotReadEvidence()
    {
        var fixture = CreateFixture();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Func<Task> act = () => fixture.Sut.VerifyAsync(RunDirectory, fixture.Snapshot, "target-1", cancellation.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
        fixture.Source.VerifyNoOtherCalls();
    }

    private static Fixture CreateFixture(
        byte[]? image = null,
        TimeSpan? age = null,
        bool useDifferentSnapshot = false,
        string text = "Play",
        bool changeBounds = false,
        bool includeScreenshot = true,
        bool returnNull = false)
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(RunDirectory);
        if (includeScreenshot)
            fileSystem.AddFile(ScreenshotPath, new MockFileData(ReviewedImage));

        var bounds = Rectangle.FromXYWH(10, 20, 30, 40);
        var target = new OcrGroundedTarget("target-1", "Play", bounds);
        var snapshot = new OcrGroundedSnapshot("snapshot-1", [target]);
        var evidenceSnapshot = useDifferentSnapshot
            ? new OcrGroundedSnapshot("snapshot-1", [target])
            : snapshot;

        var clock = new FakeTimeProvider(new DateTimeOffset(2026, 10, 25, 8, 0, 0, TimeSpan.Zero));
        var evidence = new TrustedSnapshotEvidence(
            evidenceSnapshot, "target-1", text,
            changeBounds ? Rectangle.FromXYWH(11, 20, 30, 40) : bounds,
            image ?? ReviewedImage, clock.GetUtcNow() - (age ?? TimeSpan.FromSeconds(1)));

        var source = new Mock<ITrustedSnapshotEvidenceSource>(MockBehavior.Strict);
        if (!returnNull)
            source.Setup(x => x.GetEvidenceAsync(snapshot, "target-1", It.IsAny<CancellationToken>()))
                .ReturnsAsync(evidence);
        else
            source.Setup(x => x.GetEvidenceAsync(snapshot, "target-1", It.IsAny<CancellationToken>()))
                .ReturnsAsync((TrustedSnapshotEvidence)null!);

        var sut = new TrustedRunSnapshotBinding(fileSystem, source.Object, clock, TimeSpan.FromSeconds(10));
        return new Fixture(sut, snapshot, source);
    }

    private sealed record Fixture(
        TrustedRunSnapshotBinding Sut,
        OcrGroundedSnapshot Snapshot,
        Mock<ITrustedSnapshotEvidenceSource> Source);
}
