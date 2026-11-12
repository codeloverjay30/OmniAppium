namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Carries associated UI nodes and original red-dot evidence without authorizing clicks.
/// </summary>
public sealed record RedDotAssociationResult(
    string ObservationId,
    IReadOnlyList<GameUiNode> Nodes,
    IReadOnlyList<RedDotEvidence> Evidence){
    /// <summary>Gets template matches verified in the baseline observation.</summary>
    public IReadOnlyList<NotificationTemplateMatch> VerifiedTemplateMatches { get; init; } =
        Array.Empty<NotificationTemplateMatch>();

    /// <summary>Gets candidate counts collected during the same analysis.</summary>
    public RedDotFilteringStatistics? FilteringStatistics { get; init; }
}