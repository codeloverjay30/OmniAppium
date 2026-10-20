using CoordinateUtilityServices;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityService.Services.Click;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Executes a device click only after signed review authorization succeeds.
/// </summary>
public sealed class TrustedGroundedClickService : ITrustedGroundedClickService
{
    private readonly IClickService _click;
    private readonly IOcrGroundedSnapshotFreshnessGuard _freshness;
    private readonly ITrustedReviewIntegrityAuthorizationService _authorization;

    /// <summary>
    /// Initializes the trusted device-input boundary.
    /// </summary>
    public TrustedGroundedClickService(
        IClickService click,
        IOcrGroundedSnapshotFreshnessGuard freshness,
        ITrustedReviewIntegrityAuthorizationService authorization)
    {
        _click = click ?? throw new ArgumentNullException(nameof(click));
        _freshness = freshness ?? throw new ArgumentNullException(nameof(freshness));
        _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
    }

    /// <inheritdoc/>
    public async Task ClickAsync(TrustedGroundedClickRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        OcrGroundedSnapshot snapshot = request.Snapshot
            ?? throw new ArgumentException("Snapshot is required.", nameof(request));
        ArgumentException.ThrowIfNullOrWhiteSpace(request.TargetId);
        ArgumentNullException.ThrowIfNull(request.Approval);
        if (snapshot.Targets is null)
            throw new InvalidOperationException("Snapshot targets are missing.");

        OcrGroundedTarget[] matches = snapshot.Targets
            .Where(t => t is not null && string.Equals(t.Id, request.TargetId, StringComparison.Ordinal))
            .Take(2).ToArray();
        if (matches.Length != 1)
            throw new InvalidOperationException("Grounded target must exist exactly once.");

        OcrGroundedTarget target = matches[0];
        Rectangle bounds = target.Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0)
            throw new InvalidOperationException("Grounded target bounds are invalid.");

        // Hold the freshness lease across authorization and the synchronous device input.
        using IOcrGroundedActionLease lease = _freshness.AcquireCurrent(snapshot);
        (double x, double y) = await _authorization.AuthorizeAsync(
            request.Approval, target.Text, request.Confidence, bounds,
            request.ReviewPath, request.AnnotationPath, request.ScreenshotPath,
            request.TrustedReviewSha256, request.TrustedAnnotationSha256,
            request.TrustedScreenshotSha256, cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();
        _freshness.ThrowIfStale(snapshot);
        if (!double.IsFinite(x) || !double.IsFinite(y))
            throw new InvalidOperationException("Authorized coordinates must be finite.");

        _click.ClickAbsolute(x, y);
    }
}
