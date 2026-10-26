using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Executes a planner click exclusively through the trusted authorization boundary.
/// </summary>
public sealed class TrustedPlannerClickExecutor : ITrustedPlannerClickExecutor
{
    private readonly IPlannerTrustedClickRequestProvider _provider;
    private readonly ITrustedGroundedClickService _trustedClick;

    /// <summary>
    /// Initializes the trusted planner execution boundary.
    /// </summary>
    public TrustedPlannerClickExecutor(
        IPlannerTrustedClickRequestProvider provider,
        ITrustedGroundedClickService trustedClick)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _trustedClick = trustedClick ?? throw new ArgumentNullException(nameof(trustedClick));
    }

    /// <inheritdoc/>
    public async Task ClickAsync(
        OcrGroundedSnapshot snapshot,
        string targetId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        cancellationToken.ThrowIfCancellationRequested();

        TrustedGroundedClickRequest request =
            await _provider.GetApprovedRequestAsync(snapshot, targetId, cancellationToken)
                .ConfigureAwait(false)
            ?? throw new InvalidOperationException(
                "Trusted review approval is required for protected planner clicks.");

        if (!ReferenceEquals(request.Snapshot, snapshot) ||
            !string.Equals(request.TargetId, targetId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Trusted review approval does not match the current planner action.");
        }

        await _trustedClick.ClickAsync(request, cancellationToken).ConfigureAwait(false);
    }
}

