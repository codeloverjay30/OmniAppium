using System.IO.Abstractions;
using System.Text.Json;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Reads externally signed planner click requests without issuing approvals.
/// </summary>
public sealed class JsonPlannerTrustedClickRequestProvider : IPlannerTrustedClickRequestProvider
{
    private readonly IFileSystem _fileSystem;
    private readonly string _requestPath;

    /// <summary>
    /// Initializes an external JSON approval reader.
    /// </summary>
    public JsonPlannerTrustedClickRequestProvider(IFileSystem fileSystem, string requestPath)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        ArgumentException.ThrowIfNullOrWhiteSpace(requestPath);
        _requestPath = requestPath;
    }

    /// <summary>
    /// Loads an approved request and binds it to the exact current planner snapshot.
    /// </summary>
    public async Task<TrustedGroundedClickRequest> GetApprovedRequestAsync(
        OcrGroundedSnapshot snapshot,
        string targetId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        cancellationToken.ThrowIfCancellationRequested();

        if (!_fileSystem.File.Exists(_requestPath))
            throw new InvalidOperationException("External planner approval request is unavailable.");

        TrustedGroundedClickRequest request;
        try
        {
            await using Stream stream = _fileSystem.File.OpenRead(_requestPath);
            request = await JsonSerializer.DeserializeAsync<TrustedGroundedClickRequest>(
                stream, cancellationToken: cancellationToken).ConfigureAwait(false)
                ?? throw new InvalidOperationException("External planner approval request is empty.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("External planner approval JSON is invalid.", ex);
        }

        if (request.Approval is null ||
            request.Snapshot is null ||
            request.Snapshot.Targets is null ||
            snapshot.Targets is null ||
            !string.Equals(request.TargetId, targetId, StringComparison.Ordinal) ||
            !string.Equals(request.Snapshot.SnapshotId, snapshot.SnapshotId, StringComparison.Ordinal) ||
            request.Snapshot.Targets.Count != snapshot.Targets.Count ||
            !request.Snapshot.Targets.SequenceEqual(snapshot.Targets))
        {
            throw new InvalidOperationException(
                "External planner approval does not match the current snapshot and target.");
        }

        // The trusted click service must still verify the signature, evidence digests,
        // freshness, and irreversible single-use claim before dispatch.
        return request with { Snapshot = snapshot };
    }
}
