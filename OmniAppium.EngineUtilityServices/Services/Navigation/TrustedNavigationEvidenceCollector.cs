using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityServices.Services.Navigation;

/// <summary>
/// Records authorized click outcomes and independently verified destination facts.
/// </summary>
/// <remarks>
/// Create one instance per workflow. This collector does not itself authenticate a verifier:
/// production composition must supply a trusted implementation that observes a fresh screen.
/// </remarks>
public sealed class TrustedNavigationEvidenceCollector : ITrustedNavigationEvidenceCollector
{
    private readonly ITrustedPlannerClickExecutor _clickExecutor;
    private readonly ITrustedNavigationDestinationVerifier _destinationVerifier;
    private readonly object _sync = new();
    private readonly List<NavigationActionEvidence> _actions = [];
    private readonly SemaphoreSlim _executionGate = new(1, 1);

    /// <summary>
    /// Initializes a collector with trusted execution and destination-verification boundaries.
    /// </summary>
    public TrustedNavigationEvidenceCollector(
        ITrustedPlannerClickExecutor clickExecutor,
        ITrustedNavigationDestinationVerifier destinationVerifier)
    {
        _clickExecutor = clickExecutor ?? throw new ArgumentNullException(nameof(clickExecutor));
        _destinationVerifier = destinationVerifier ?? throw new ArgumentNullException(nameof(destinationVerifier));
    }

    /// <inheritdoc/>
    public async Task ExecuteAsync(
        OcrGroundedSnapshot snapshot,
        string targetId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        cancellationToken.ThrowIfCancellationRequested();

        await _executionGate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await _clickExecutor.ClickAsync(snapshot, targetId, cancellationToken)
                .ConfigureAwait(false);

            // A completed trusted click is not equivalent to a verified destination.
            NavigationActionEvidence action = new(
                NavigationActionKind.TrustedGroundedClick,
                Executed: true,
                Succeeded: true,
                PostActionScreenVerified: false,
                ExpectedDestinationVerified: false);
            lock (_sync)
            {
                _actions.Add(action);
            }

            NavigationDestinationVerification verification =
                await _destinationVerifier.VerifyAsync(snapshot, targetId, cancellationToken)
                    .ConfigureAwait(false)
                ?? throw new InvalidOperationException(
                    "Post-action destination verification returned no result.");

            // A destination cannot be verified without a verified post-action screen.
            bool screenVerified = verification.PostActionScreenVerified;
            bool destinationVerified = screenVerified && verification.ExpectedDestinationVerified;
            lock (_sync)
            {
                _actions[^1] = action with
                {
                    PostActionScreenVerified = screenVerified,
                    ExpectedDestinationVerified = destinationVerified
                };
            }
        }
        finally
        {
            _executionGate.Release();
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<NavigationActionEvidence> GetEvidence()
    {
        lock (_sync)
        {
            return Array.AsReadOnly(_actions.ToArray());
        }
    }
}