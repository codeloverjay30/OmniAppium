using OCRUtilityServices.Services;
using System.Security.Cryptography;
using System.Text.Json;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using System.IO.Abstractions;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Loads signed Issue50 run evidence without issuing approvals or accessing private keys.
/// </summary>
public sealed class TrustedRunPlannerClickRequestProvider : IPlannerTrustedClickRequestProvider
{
    private readonly IFileSystem _files;
    private readonly string _runDirectory;
    private readonly ITrustedRunSnapshotBinding _binding;
    private readonly float _confidence;

    /// <summary>
    /// Creates a provider for a single immutable review run.
    /// </summary>
    public TrustedRunPlannerClickRequestProvider(IFileSystem files, string runDirectory,
        ITrustedRunSnapshotBinding binding, float confidence)
    {
        _files = files ?? throw new ArgumentNullException(nameof(files));
        ArgumentException.ThrowIfNullOrWhiteSpace(runDirectory);
        _binding = binding ?? throw new ArgumentNullException(nameof(binding));
        if (!float.IsFinite(confidence) || confidence < 0f || confidence > 1f)
            throw new ArgumentOutOfRangeException(nameof(confidence));
        _runDirectory = _files.Path.GetFullPath(runDirectory);
        _confidence = confidence;
    }

    /// <summary>
    /// Reads immutable evidence, checks its hashes and expiry, and binds it to the live snapshot.
    /// </summary>
    public async Task<TrustedGroundedClickRequest> GetApprovedRequestAsync(
        OcrGroundedSnapshot snapshot, string targetId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        cancellationToken.ThrowIfCancellationRequested();
        if (snapshot.Targets is null || !snapshot.Targets.Any(t =>
            t is not null && string.Equals(t.Id, targetId, StringComparison.Ordinal)))
            throw new InvalidOperationException("Current snapshot does not contain the approved target.");

        string manifestPath = Resolve("run-manifest.json");
        string approvalPath = Resolve("task-home.approval.json");
        string reviewPath = Resolve("task-home.review.json");
        string annotationPath = Resolve("task-home.ground-truth.reviewed.json");
        string screenshotPath = Resolve("task-home-original.png");

        Manifest manifest = await ReadAsync<Manifest>(manifestPath, cancellationToken);
        TrustedReviewApprovalEvidence approval =
            await ReadAsync<TrustedReviewApprovalEvidence>(approvalPath, cancellationToken);

        if (manifest.SchemaVersion != "1.0" ||
            manifest.ReviewStatus != "IndependentReviewImported" ||
            manifest.RunId != _files.Path.GetFileName(_runDirectory) ||
            manifest.ReviewFile != "task-home.review.json" ||
            manifest.AnnotationFile != "task-home.ground-truth.reviewed.json" ||
            manifest.ScreenshotFile != "task-home-original.png" ||
            manifest.ApprovalFile is not null ||
            !string.Equals(manifest.ReviewerId, approval.ReviewerId, StringComparison.Ordinal))
            throw new InvalidOperationException("Trusted review run manifest is invalid.");

        VerifyDigest(reviewPath, manifest.ReviewSha256, approval.ReviewSha256);
        VerifyDigest(annotationPath, manifest.AnnotationSha256, approval.AnnotationSha256);
        VerifyDigest(screenshotPath, manifest.ScreenshotSha256, approval.ScreenshotSha256);

        DateTimeOffset now = DateTimeOffset.UtcNow;
        if (approval.ApprovedAtUtc > now || approval.ExpiresAtUtc <= now ||
            approval.ExpiresAtUtc <= approval.ApprovedAtUtc ||
            string.IsNullOrWhiteSpace(approval.SignatureBase64))
            throw new InvalidOperationException("Trusted review approval is missing or expired.");

        // The archive alone cannot prove that the current device screen is the reviewed screen.
        await _binding.VerifyAsync(_runDirectory, snapshot, targetId, cancellationToken)
            .ConfigureAwait(false);

        // The downstream trusted-click authorization must still verify the ECDSA signature,
        // freshness, exact target geometry and one-time claim before dispatch.
        return new TrustedGroundedClickRequest(snapshot, targetId, approval,
            reviewPath, annotationPath, screenshotPath,
            approval.ReviewSha256, approval.AnnotationSha256, approval.ScreenshotSha256,
            _confidence);
    }

    /// <summary>
    /// Resolves a fixed evidence filename while rejecting linked files and directories.
    /// </summary>
    private string Resolve(string fileName)
    {
        string path = _files.Path.Combine(_runDirectory, fileName);
        if (!_files.File.Exists(path))
            throw new InvalidOperationException($"Trusted run evidence is missing: {fileName}");
        if ((_files.File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0)
            throw new InvalidOperationException($"Trusted run evidence is a link: {fileName}");
        return path;
    }

    /// <summary>
    /// Deserializes an evidence document with a bounded input size.
    /// </summary>
    private async Task<T> ReadAsync<T>(string path, CancellationToken cancellationToken)
    {
        const long maxJsonBytes = 1024 * 1024;
        await using Stream stream = _files.File.OpenRead(path);
        if (stream.Length > maxJsonBytes)
            throw new InvalidOperationException("Trusted run JSON exceeds the size limit.");
        try
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken)
                .ConfigureAwait(false)
                ?? throw new InvalidOperationException("Trusted run JSON is empty.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Trusted run JSON is invalid.", ex);
        }
    }

    /// <summary>
    /// Checks the file digest against both the manifest and signed approval payload.
    /// </summary>
    private void VerifyDigest(string path, string? manifestHash, string? approvalHash)
    {
        if (!IsDigest(manifestHash) || !IsDigest(approvalHash))
            throw new InvalidOperationException("Trusted run SHA-256 is invalid.");
        using Stream stream = _files.File.OpenRead(path);
        byte[] actual = SHA256.HashData(stream);
        byte[] expectedManifest = Convert.FromHexString(manifestHash!);
        byte[] expectedApproval = Convert.FromHexString(approvalHash!);
        if (!CryptographicOperations.FixedTimeEquals(actual, expectedManifest) ||
            !CryptographicOperations.FixedTimeEquals(actual, expectedApproval))
            throw new InvalidOperationException("Trusted run evidence SHA-256 mismatch.");
    }

    /// <summary>
    /// Validates an uppercase or lowercase SHA-256 hexadecimal digest.
    /// </summary>
    private static bool IsDigest(string? value)
    {
        if (value is null || value.Length != 64) return false;
        foreach (char c in value.AsSpan())
            if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') ||
                  (c >= 'A' && c <= 'F'))) return false;
        return true;
    }

    /// <summary>
    /// Represents only the manifest fields required by the authorization boundary.
    /// </summary>
    private sealed record Manifest(
        string? SchemaVersion, string? RunId, string? ReviewerId, string? ReviewStatus,
        string? ScreenshotFile, string? ScreenshotSha256,
        string? AnnotationFile, string? AnnotationSha256,
        string? ReviewFile, string? ReviewSha256, string? ApprovalFile);
}

