using CoordinateUtilityServices;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Represents a detected red component and its potential owning UI nodes.
/// </summary>
public sealed record RedDotEvidence(Rectangle Bounds, double Confidence, IReadOnlyList<string> CandidateNodeIds)
{
    /// <summary>
    /// Gets whether the component cannot be uniquely associated with a UI node.
    /// </summary>
    public bool IsAmbiguous => CandidateNodeIds.Count != 1;
}
