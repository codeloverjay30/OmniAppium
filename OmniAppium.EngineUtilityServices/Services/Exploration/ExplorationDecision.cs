namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Represents a navigation decision without granting execution authorization.
/// </summary>
public sealed record ExplorationDecision(
    ExplorationDecisionKind Kind,
    string? TargetId,
    string Reason);
