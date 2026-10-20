using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;

namespace OmniAppiumDemo.SmokeTests;

/// <summary>
/// Verifies that an unreviewed live OCR snapshot cannot authorize a device click.
/// </summary>
public sealed class Issue48ClickSmokeTestRunner
{
    private readonly IAndroidScreenOcrService _ocr;
    private readonly IOcrGroundedSnapshotFactory _snapshotFactory;
    private readonly IGroundedClickAuthorizationService _authorization;

    /// <summary>
    /// Initializes the read-only smoke test with production OCR and authorization services.
    /// </summary>
    public Issue48ClickSmokeTestRunner(
        IAndroidScreenOcrService ocr,
        IOcrGroundedSnapshotFactory snapshotFactory,
        IGroundedClickAuthorizationService authorization)
    {
        _ocr = ocr ?? throw new ArgumentNullException(nameof(ocr));
        _snapshotFactory = snapshotFactory ?? throw new ArgumentNullException(nameof(snapshotFactory));
        _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
    }

    /// <summary>
    /// Captures live OCR and verifies that missing reviewed evidence is rejected before any click.
    /// </summary>
    public async Task<OcrGroundedSnapshot> VerifyReadOnlyAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        OcrResult observation = await _ocr
            .RecognizeCurrentScreenAsync(cancellationToken)
            .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        OcrGroundedSnapshot snapshot = _snapshotFactory.Create(
            Guid.NewGuid().ToString("N"), observation);

        OcrGroundedTarget target = snapshot.Targets.FirstOrDefault(
            candidate => candidate.Bounds.Width > 0 && candidate.Bounds.Height > 0)
            ?? throw new InvalidOperationException(
                "Issue48 Phase 1 cannot verify authorization: no positive-area OCR target was found.");

        var clickGuard = new RejectingCountingClickService();
        var freshnessGuard = new OcrGroundedSnapshotFreshnessGuard();
        freshnessGuard.MarkCurrent(snapshot.SnapshotId);

        // This production provider is deliberately empty: no reviewer has registered evidence.
        var evidenceProvider = new ReviewedGroundedClickEvidenceProvider(TimeProvider.System);
        var groundedClick = new OcrGroundedClickService(
            clickGuard, freshnessGuard, _authorization, evidenceProvider);

        bool rejectedForMissingEvidence = false;
        try
        {
            groundedClick.Click(snapshot, target.Id);
        }
        catch (InvalidOperationException ex) when (
            string.Equals(
                ex.Message,
                "Reviewed click evidence is unavailable.",
                StringComparison.Ordinal))
        {
            rejectedForMissingEvidence = true;
        }

        if (!rejectedForMissingEvidence)
        {
            throw new InvalidOperationException(
                "Issue48 Phase 1 FAIL: missing reviewed evidence was not rejected.");
        }

        if (clickGuard.Attempts != 0)
        {
            throw new InvalidOperationException(
                $"Issue48 Phase 1 FAIL: {clickGuard.Attempts} unauthorized click attempts were intercepted.");
        }

        return snapshot;
    }

    /// <summary>
    /// Verifies that independently reviewed evidence permits exactly one real device click.
    /// </summary>
    /// <remarks>
    /// The caller must acquire fresh evidence from a trusted reviewer and explicitly
    /// authorize the real-device action. This method never manufactures evidence.
    /// </remarks>
    public void VerifyApprovedSingleClick(
        OcrGroundedSnapshot snapshot,
        string targetId,
        ReviewedGroundTruth reviewedEvidence,
        float reviewedConfidence,
        IClickService realClickService,
        bool operatorConfirmedDeviceClick,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        ArgumentNullException.ThrowIfNull(reviewedEvidence);
        ArgumentNullException.ThrowIfNull(realClickService);

        if (!operatorConfirmedDeviceClick)
            throw new InvalidOperationException(
                "Issue48 Phase 2 requires explicit operator confirmation.");

        cancellationToken.ThrowIfCancellationRequested();

        OcrGroundedTarget[] matches = snapshot.Targets
            .Where(candidate => string.Equals(candidate.Id, targetId, StringComparison.Ordinal))
            .Take(2)
            .ToArray();

        if (matches.Length != 1 ||
            matches[0].Bounds.Width <= 0 ||
            matches[0].Bounds.Height <= 0)
            throw new InvalidOperationException(
                "Issue48 Phase 2 requires exactly one positive-area reviewed target.");

        var evidenceProvider = new ReviewedGroundedClickEvidenceProvider(TimeProvider.System);
        var freshnessGuard = new OcrGroundedSnapshotFreshnessGuard();
        freshnessGuard.MarkCurrent(snapshot.SnapshotId);

        var auditedClick = new SingleClickAuditingService(realClickService);
        var groundedClick = new OcrGroundedClickService(
            auditedClick, freshnessGuard, _authorization, evidenceProvider);

        evidenceProvider.Register(
            snapshot, matches[0], reviewedEvidence, reviewedConfidence,
            TimeSpan.FromSeconds(30));

        // Cancellation is checked before authorization and before any real-device action.
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            groundedClick.Click(snapshot, targetId);

            if (auditedClick.Attempts != 1)
                throw new InvalidOperationException(
                    "Issue48 Phase 2 FAIL: expected exactly one real click attempt.");

            bool replayRejected = false;
            try
            {
                groundedClick.Click(snapshot, targetId);
            }
            catch (InvalidOperationException ex) when (
                string.Equals(
                    ex.Message,
                    "Reviewed click evidence has already been consumed.",
                    StringComparison.Ordinal))
            {
                replayRejected = true;
            }

            if (!replayRejected)
                throw new InvalidOperationException(
                    "Issue48 Phase 2 FAIL: evidence replay was not rejected.");

            if (auditedClick.Attempts != 1)
                throw new InvalidOperationException(
                    "Issue48 Phase 2 FAIL: replay triggered another device click.");
        }
        finally
        {
            evidenceProvider.Revoke(snapshot);
        }
    }

    /// <summary>
    /// Audits a single allowed absolute click and blocks all other device interactions.
    /// </summary>
    private sealed class SingleClickAuditingService : IClickService
    {
        private readonly IClickService _inner;
        private int _attempts;

        /// <summary>
        /// Initializes an audited click service.
        /// </summary>
        public SingleClickAuditingService(IClickService inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <summary>
        /// Gets the number of absolute click attempts.
        /// </summary>
        public int Attempts => Volatile.Read(ref _attempts);

        /// <inheritdoc/>
        public void ClickAbsolute(double x, double y)
        {
            if (Interlocked.Increment(ref _attempts) != 1)
                throw new InvalidOperationException(
                    "Issue48 Phase 2 blocked a second device click.");

            _inner.ClickAbsolute(x, y);
        }

        /// <inheritdoc/>
        public void Click(ClickJob clickJob) =>
            throw new InvalidOperationException(
                "Issue48 Phase 2 forbids job-based clicks.");

        /// <inheritdoc/>
        public void ClickScaled(double x, double y) =>
            throw new InvalidOperationException(
                "Issue48 Phase 2 forbids scaled clicks.");

        /// <inheritdoc/>
        public void TapAt(int x, int y, TimeSpan duration = default) =>
            throw new InvalidOperationException(
                "Issue48 Phase 2 forbids direct taps.");
    }

    /// <summary>
    /// Records forbidden click calls without forwarding them to the Android device.
    /// </summary>
    private sealed class RejectingCountingClickService : IClickService
    {
        private int _attempts;

        /// <summary>
        /// Gets the number of intercepted click calls.
        /// </summary>
        public int Attempts => Volatile.Read(ref _attempts);

        /// <inheritdoc/>
        public void ClickAbsolute(double x, double y) => Reject();

        /// <inheritdoc/>
        public void Click(ClickJob clickJob) => Reject();

        /// <inheritdoc/>
        public void ClickScaled(double x, double y) => Reject();

        /// <inheritdoc/>
        public void TapAt(int x, int y, TimeSpan duration = default) => Reject();

        /// <summary>
        /// Records and blocks any attempted device interaction.
        /// </summary>
        private void Reject()
        {
            Interlocked.Increment(ref _attempts);
            throw new InvalidOperationException(
                "Issue48 Phase 1 blocked an unauthorized device click.");
        }
    }
}
