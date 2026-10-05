using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Owns the execution-state lease for one OCR-grounded planner execution.
/// </summary>
public sealed class OcrGroundedPlannerExecutionScope
    : IOcrGroundedPlannerExecutionScope
{
    private IDisposable? _executionStateLease;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="OcrGroundedPlannerExecutionScope"/> class.
    /// </summary>
    /// <param name="snapshot">
    /// The immutable OCR-grounded snapshot owned by this scope.
    /// </param>
    /// <param name="executionStateLease">
    /// The lease that restores the previous AI tool execution state.
    /// </param>
    public OcrGroundedPlannerExecutionScope(
        OcrGroundedSnapshot snapshot,
        IDisposable executionStateLease)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(executionStateLease);

        Snapshot = snapshot;
        _executionStateLease = executionStateLease;
    }

    /// <inheritdoc/>
    public OcrGroundedSnapshot Snapshot { get; }

    /// <summary>
    /// Releases the execution-state lease exactly once.
    /// </summary>
    public void Dispose()
    {
        IDisposable? lease =
            Interlocked.Exchange(
                ref _executionStateLease,
                null);

        lease?.Dispose();
    }
}
