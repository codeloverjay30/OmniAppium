using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Supplies an externally approved click request for the exact planner action.
/// </summary>
public interface IPlannerTrustedClickRequestProvider
{
    /// <summary>
    /// Obtains an independently approved request without issuing or signing approval.
    /// </summary>
    Task<TrustedGroundedClickRequest> GetApprovedRequestAsync(
        OcrGroundedSnapshot snapshot,
        string targetId,
        CancellationToken cancellationToken = default);
}
