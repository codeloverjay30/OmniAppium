using CoordinateUtilityServices;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Executes validated clicks against OCR-grounded targets.
/// </summary>
public sealed class OcrGroundedClickService
    : IOcrGroundedClickService
{
    private readonly IClickService _clickService;
    private readonly IOcrGroundedSnapshotFreshnessGuard _freshnessGuard;
    private readonly IGroundedClickAuthorizationService _authorizationService;
    private readonly IGroundedClickEvidenceProvider _evidenceProvider;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="OcrGroundedClickService"/> class.
    /// </summary>
    /// <param name="clickService">
    /// The deterministic click service used to execute device clicks.
    /// </param>
    /// <param name="freshnessGuard">
    /// The guard used to authorize actions against current OCR-grounded snapshots.
    /// </param>
    public OcrGroundedClickService(
        IClickService clickService,
        IOcrGroundedSnapshotFreshnessGuard freshnessGuard,
        IGroundedClickAuthorizationService authorizationService,
        IGroundedClickEvidenceProvider evidenceProvider)
    {
        ArgumentNullException.ThrowIfNull(clickService);
        ArgumentNullException.ThrowIfNull(freshnessGuard);

        _clickService = clickService;
        _freshnessGuard = freshnessGuard;
        _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        _evidenceProvider = evidenceProvider ?? throw new ArgumentNullException(nameof(evidenceProvider));
    }

    /// <inheritdoc/>
    public void Click(
        OcrGroundedSnapshot snapshot,
        string targetId)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);

        if (snapshot.Targets is null)
        {
            throw new ArgumentException(
                "The OCR-grounded snapshot must contain a non-null target collection.",
                nameof(snapshot));
        }

        OcrGroundedTarget[] matches =
            snapshot.Targets
                .Where(
                    target =>
                        string.Equals(
                            target.Id,
                            targetId,
                            StringComparison.Ordinal))
                .Take(2)
                .ToArray();

        if (matches.Length == 0)
        {
            throw new InvalidOperationException(
                $"OCR-grounded target '{targetId}' was not found.");
        }

        if (matches.Length > 1)
        {
            throw new InvalidOperationException(
                $"OCR-grounded target '{targetId}' is ambiguous.");
        }

        OcrGroundedTarget target = matches[0];
        Rectangle bounds = target.Bounds;

        if (bounds.Width <= 0 ||
            bounds.Height <= 0)
        {
            throw new InvalidOperationException(
                $"OCR-grounded target '{targetId}' has zero-area bounds.");
        }

        using IOcrGroundedActionLease lease =
            _freshnessGuard.AcquireCurrent(snapshot);

        if (_evidenceProvider is not IAtomicGroundedClickEvidenceProvider atomicProvider)
            throw new InvalidOperationException(
                "Atomic reviewed click evidence provider is required.");

        ConsumedGroundedClickEvidence authorization = atomicProvider.Consume(snapshot, target);
        (double x, double y) = _authorizationService.Authorize(
            target.Text,
            authorization.Confidence,
            bounds,
            authorization.Evidence);

        _clickService.ClickAbsolute(x, y);
    }
}