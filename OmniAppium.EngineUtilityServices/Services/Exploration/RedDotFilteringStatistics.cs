namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Records red-dot candidate counts from a single observation analysis.
/// </summary>
/// <param name="RawCandidates">Candidates detected in the baseline frame.</param>
/// <param name="FilteredCandidates">Candidates retained after temporal filtering.</param>
/// <param name="RejectedCandidates">Candidates rejected by filtering.</param>
/// <param name="SubsequentFrameCount">Number of subsequent frames analyzed.</param>
/// <param name="AssociatedNodes">Number of associated UI nodes.</param>
public sealed record RedDotFilteringStatistics(
    int RawCandidates,
    int FilteredCandidates,
    int RejectedCandidates,
    int SubsequentFrameCount,
    int AssociatedNodes);