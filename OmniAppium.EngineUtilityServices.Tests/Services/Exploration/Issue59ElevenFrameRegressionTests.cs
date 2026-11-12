using System.IO.Compression;
using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Exploration;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59ElevenFrameRegressionTests
{
    [Fact]
    public void CapturedElevenFrames_DecodeAndFilterWithoutIncreasingCandidateCount()
    {
        byte[][] screenshots = ReadFrames();
        var decoder = new OpenCvRedDotFrameDecoder();
        var detector = new RedDotDetector();
        var filter = new NotificationEvidenceFilter();

        RedDotFrame[] frames = screenshots.Select(bytes => decoder.Decode(bytes)).ToArray();
        frames.Should().HaveCount(11);
        frames.Should().OnlyContain(frame =>
            frame.Width == 2340 && frame.Height == 1080);

        IReadOnlyList<RedDotEvidence>[] candidates = frames
            .Select(frame => detector.Detect(frame, []))
            .ToArray();
        candidates[0].Should().NotBeEmpty();

        IReadOnlyList<RedDotEvidence> stable = filter.Filter(
            candidates[0], candidates.Skip(1).ToArray(),
            frames[0].Width, frames[0].Height,
            new NotificationEvidenceFilterOptions());

        stable.Count.Should().BeLessThanOrEqualTo(candidates[0].Count);
        stable.Should().OnlyContain(item => candidates[0].Contains(item));
    }

    [Fact]
    public void CapturedBaselineCandidate_StableEvidenceSurvivesAndTransientEvidenceIsRejected()
    {
        byte[][] screenshots = ReadFrames();
        var decoder = new OpenCvRedDotFrameDecoder();
        var detector = new RedDotDetector();
        RedDotFrame frame = decoder.Decode(screenshots[0]);
        RedDotEvidence candidate = detector.Detect(frame, []).First();
        var filter = new NotificationEvidenceFilter();
        var options = new NotificationEvidenceFilterOptions();

        IReadOnlyList<RedDotEvidence> retained = filter.Filter(
            [candidate], [[candidate], [candidate]],
            frame.Width, frame.Height, options);
        IReadOnlyList<RedDotEvidence> rejected = filter.Filter(
            [candidate], [[], []],
            frame.Width, frame.Height, options);

        retained.Should().ContainSingle().Which.Should().Be(candidate);
        rejected.Should().BeEmpty();
    }

    private static byte[][] ReadFrames()
    {
        const string resourceName = "Issue59Fixtures.20261027-134215.zip";
        using Stream stream = typeof(Issue59ElevenFrameRegressionTests)
            .Assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"Missing embedded Issue 59 capture: {resourceName}");
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        byte[][] frames = new byte[11][];
        for (int index = 0; index < frames.Length; index++)
        {
            string name = $"ocr-diagnostic-{index + 1:D6}.png";
            ZipArchiveEntry entry = archive.GetEntry(name)
                ?? throw new InvalidOperationException(
                    $"Missing Issue 59 frame: {name}");
            using Stream source = entry.Open();
            using var destination = new MemoryStream();
            source.CopyTo(destination);
            frames[index] = destination.ToArray();
        }
        return frames;
    }
}
