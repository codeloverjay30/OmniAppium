using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class OcrGroundedSnapshotFreshnessGuardTests
{
    [Fact]
    public void ThrowIfStale_WhenSnapshotIdMatchesCurrentSnapshot_ShouldNotThrow()
    {
        var sut = new OcrGroundedSnapshotFreshnessGuard();

        sut.MarkCurrent("snapshot-42");

        var snapshot = new OcrGroundedSnapshot(
            "snapshot-42",
            []);

        Action act = () => sut.ThrowIfStale(snapshot);

        act.Should().NotThrow();
    }

    [Fact]
    public void ThrowIfStale_WhenNewerSnapshotHasBeenMarkedCurrent_ShouldThrow()
    {
        var sut = new OcrGroundedSnapshotFreshnessGuard();

        sut.MarkCurrent("snapshot-41");
        sut.MarkCurrent("snapshot-42");

        var staleSnapshot = new OcrGroundedSnapshot(
            "snapshot-41",
            []);

        Action act = () => sut.ThrowIfStale(staleSnapshot);

        act.Should()
            .Throw<StaleOcrGroundedSnapshotException>()
            .WithMessage("*snapshot-41*stale*");
    }

    [Fact]
    public void ThrowIfStale_WhenNoSnapshotHasBeenMarkedCurrent_ShouldFailClosed()
    {
        var sut = new OcrGroundedSnapshotFreshnessGuard();

        var snapshot = new OcrGroundedSnapshot(
            "snapshot-42",
            []);

        Action act = () => sut.ThrowIfStale(snapshot);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*current OCR-grounded snapshot*");
    }
}
