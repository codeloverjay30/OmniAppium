using CoordinateUtilityServices;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Click;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Executes a grounded device click only through a single-use trusted approval gate.
/// </summary>
public sealed class TrustedGroundedClickService : ITrustedGroundedClickService
{
    private readonly IClickService _click;
    private readonly IOcrGroundedSnapshotFreshnessGuard _freshness;
    private readonly ITrustedReviewIntegrityAuthorizationService _authorization;
    private readonly ITrustedApprovalExecutionGate _executionGate;

    /// <summary>
    /// Initializes the trusted device-input boundary with a mandatory execution gate.
    /// </summary>
    public TrustedGroundedClickService(
        IClickService click,
        IOcrGroundedSnapshotFreshnessGuard freshness,
        ITrustedReviewIntegrityAuthorizationService authorization,
        ITrustedApprovalExecutionGate executionGate)
    {
        _click = click ?? throw new ArgumentNullException(nameof(click));
        _freshness = freshness ?? throw new ArgumentNullException(nameof(freshness));
        _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
        _executionGate = executionGate ?? throw new ArgumentNullException(nameof(executionGate));
    }

    /// <summary>
    /// Validates the grounded target and dispatches a click only after atomic approval consumption.
    /// </summary>
    public Task ClickAsync(
        TrustedGroundedClickRequest request,
        CancellationToken cancellationToken = default) =>
        ExecuteCoreAsync(request, null, cancellationToken);

    /// <summary>
    /// Executes a trusted click and returns a sanitized outcome without exposing approval evidence.
    /// </summary>
    public async Task<TrustedClickExecutionOutcome> ExecuteWithOutcomeAsync(
        TrustedGroundedClickRequest request,
        CancellationToken cancellationToken = default)
    {
        var state = new DispatchState();
        try
        {
            await ExecuteCoreAsync(request, state, cancellationToken).ConfigureAwait(false);
            return new TrustedClickExecutionOutcome(
                TrustedClickExecutionStatus.DispatchCompleted,
                TrustedClickFailureCategory.None,
                DispatchAttempted: true);
        }
        catch (Exception exception)
        {
            bool dispatched = state.Attempted;
            TrustedClickExecutionStatus status = dispatched
                ? TrustedClickExecutionStatus.DispatchFailedOrAmbiguous
                : state.GateEntered
                    ? TrustedClickExecutionStatus.RejectedOrConsumedUndetermined
                    : TrustedClickExecutionStatus.RejectedBeforeConsumption;

            if (!dispatched && state.GateEntered &&
                exception is InvalidOperationException &&
                string.Equals(exception.Message, "Approval has already been claimed.", StringComparison.Ordinal))
            {
                status = TrustedClickExecutionStatus.RejectedAlreadyConsumed;
            }

            TrustedClickFailureCategory category = exception switch
            {
                OperationCanceledException => TrustedClickFailureCategory.Cancellation,
                TimeoutException => TrustedClickFailureCategory.Timeout,
                ArgumentException => TrustedClickFailureCategory.InvalidRequest,
                InvalidOperationException => TrustedClickFailureCategory.AuthorizationOrClaim,
                _ => TrustedClickFailureCategory.Unexpected
            };

            return new TrustedClickExecutionOutcome(status, category, dispatched);
        }
    }

    /// <summary>
    /// Tracks dispatch transitions without storing approval or device details.
    /// </summary>
    private sealed class DispatchState
    {
        internal bool GateEntered;
        internal bool Attempted;
    }

    /// <summary>
    /// Performs the original fail-closed approval and device-dispatch workflow.
    /// </summary>
    private async Task ExecuteCoreAsync(
        TrustedGroundedClickRequest request,
        DispatchState? state,
        CancellationToken cancellationToken)
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
            .Where(target => target is not null &&
                string.Equals(target.Id, request.TargetId, StringComparison.Ordinal))
            .Take(2)
            .ToArray();

        if (matches.Length != 1)
            throw new InvalidOperationException("Grounded target must exist exactly once.");

        OcrGroundedTarget target = matches[0];
        Rectangle bounds = target.Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0)
            throw new InvalidOperationException("Grounded target bounds are invalid.");

        // The lease covers authorization, approval consumption and device dispatch.
        using IOcrGroundedActionLease lease = _freshness.AcquireCurrent(snapshot);

        (double x, double y) = await _authorization.AuthorizeAsync(
            request.Approval,
            target.Text,
            request.Confidence,
            bounds,
            request.ReviewPath,
            request.AnnotationPath,
            request.ScreenshotPath,
            request.TrustedReviewSha256,
            request.TrustedAnnotationSha256,
            request.TrustedScreenshotSha256,
            cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();
        _freshness.ThrowIfStale(snapshot);

        if (!double.IsFinite(x) || !double.IsFinite(y))
            throw new InvalidOperationException("Authorized coordinates must be finite.");

        if (state is not null)
            state.GateEntered = true;

        await _executionGate.ExecuteAsync(
            request.Approval,
            request.TrustedReviewSha256,
            request.TrustedAnnotationSha256,
            request.TrustedScreenshotSha256,
            operationToken =>
            {
                operationToken.ThrowIfCancellationRequested();
                _freshness.ThrowIfStale(snapshot);
                if (state is not null)
                    state.Attempted = true;

                _click.ClickAbsolute(x, y);
                return Task.CompletedTask;
            },
            cancellationToken).ConfigureAwait(false);
    }
}
