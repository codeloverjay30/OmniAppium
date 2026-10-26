using System.Security.Cryptography;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using System.IO.Abstractions;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Enforces exact screenshot and target binding between a review run and the current trusted observation.
/// </summary>
public sealed class TrustedRunSnapshotBinding : ITrustedRunSnapshotBinding
{
    private const string ScreenshotFileName = "task-home-original.png";
    private const int MaximumScreenshotBytes = 32 * 1024 * 1024;
    private readonly IFileSystem _files;
    private readonly ITrustedSnapshotEvidenceSource _source;
    private readonly TimeProvider _clock;
    private readonly TimeSpan _maximumAge;

    /// <summary>
    /// Creates a fail-closed binding validator with an injectable clock and trusted evidence source.
    /// </summary>
    public TrustedRunSnapshotBinding(
        IFileSystem files,
        ITrustedSnapshotEvidenceSource source,
        TimeProvider clock,
        TimeSpan maximumAge)
    {
        _files = files ?? throw new ArgumentNullException(nameof(files));
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        if (maximumAge <= TimeSpan.Zero || maximumAge > TimeSpan.FromMinutes(1))
            throw new ArgumentOutOfRangeException(nameof(maximumAge),
                "Maximum observation age must be greater than zero and at most one minute.");
        _maximumAge = maximumAge;
    }

    /// <summary>
    /// Verifies that trusted live evidence exactly matches the archived screenshot and requested target.
    /// </summary>
    public async Task VerifyAsync(
        string runDirectory,
        OcrGroundedSnapshot snapshot,
        string targetId,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(runDirectory);
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        cancellationToken.ThrowIfCancellationRequested();

        if (snapshot.Targets is null)
            throw new InvalidOperationException("Snapshot targets are missing.");

        var matches = snapshot.Targets.Where(target =>
            target is not null && string.Equals(target.Id, targetId, StringComparison.Ordinal)).ToArray();
        if (matches.Length != 1)
            throw new InvalidOperationException("Snapshot must contain exactly one requested target.");

        var target = matches[0];
        string directory = _files.Path.GetFullPath(runDirectory);
        if (!_files.Directory.Exists(directory) ||
            (_files.File.GetAttributes(directory) & FileAttributes.ReparsePoint) != 0)
            throw new InvalidOperationException("Trusted review run directory is missing or linked.");

        string screenshotPath = _files.Path.Combine(directory, ScreenshotFileName);
        if (!_files.File.Exists(screenshotPath) ||
            (_files.File.GetAttributes(screenshotPath) & FileAttributes.ReparsePoint) != 0)
            throw new InvalidOperationException("Trusted review screenshot is missing or linked.");

        TrustedSnapshotEvidence evidence = await _source.GetEvidenceAsync(
            snapshot, targetId, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Trusted snapshot evidence is unavailable.");

        cancellationToken.ThrowIfCancellationRequested();
        if (!ReferenceEquals(evidence.Snapshot, snapshot) ||
            !string.Equals(evidence.TargetId, targetId, StringComparison.Ordinal) ||
            !string.Equals(evidence.TargetText, target.Text, StringComparison.Ordinal) ||
            !evidence.TargetBounds.Equals(target.Bounds))
            throw new InvalidOperationException("Trusted snapshot target binding mismatch.");

        DateTimeOffset now = _clock.GetUtcNow();
        if (evidence.CapturedAtUtc > now ||
            now - evidence.CapturedAtUtc > _maximumAge)
            throw new InvalidOperationException("Trusted snapshot evidence is stale or future-dated.");

        if (evidence.ScreenshotBytes is not { Length: > 0 and <= MaximumScreenshotBytes })
            throw new InvalidOperationException("Trusted snapshot screenshot size is invalid.");

        byte[] observedHash = SHA256.HashData(evidence.ScreenshotBytes);
        byte[] reviewedHash;
        using (Stream stream = _files.File.OpenRead(screenshotPath))
        {
            if (stream.Length <= 0 || stream.Length > MaximumScreenshotBytes)
                throw new InvalidOperationException("Reviewed screenshot size is invalid.");
            reviewedHash = SHA256.HashData(stream);
        }

        if (!CryptographicOperations.FixedTimeEquals(observedHash, reviewedHash))
            throw new InvalidOperationException("Current screenshot differs from the reviewed screenshot.");
    }
}
