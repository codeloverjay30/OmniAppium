namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Defines bounded navigation planning for a single observation.
/// </summary>
public sealed class RedDotExplorationPlanner
{
    private static readonly string[] FinancialIndicators =
    [
        "充值", "首儲", "首储", "累儲", "累储", "儲值", "储值",
        "NT$", "NTD", "購買", "购买", "課金", "课金"
    ];

    /// <summary>
    /// Selects a candidate without performing or authorizing device interaction.
    /// </summary>
    public ExplorationDecision Plan(
        string observationId,
        IReadOnlyList<GameUiNode> nodes,
        IReadOnlySet<string> visitedIds,
        int actionsTaken,
        int maxActions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(observationId);
        ArgumentNullException.ThrowIfNull(nodes);
        ArgumentNullException.ThrowIfNull(visitedIds);
        if (maxActions <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxActions), "Maximum actions must be positive.");
        if (actionsTaken < 0)
            throw new ArgumentOutOfRangeException(nameof(actionsTaken), "Action count cannot be negative.");
        if (actionsTaken >= maxActions)
            return new(ExplorationDecisionKind.Stop, null, "Action budget exhausted.");

        foreach (GameUiNode node in nodes)
        {
            if (node is null ||
                string.IsNullOrWhiteSpace(node.Id) ||
                node.ObservationId != observationId ||
                node.Kind == GameUiNodeKind.Unknown ||
                !node.HasRedDot ||
                node.IsRedDotAssociationAmbiguous ||
                !double.IsFinite(node.Confidence) ||
                node.Confidence < 0.85 ||
                visitedIds.Contains(node.Id) ||
                string.IsNullOrWhiteSpace(node.Text) ||
                FinancialIndicators.Any(term =>
                    node.Text.Contains(term, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            return new(ExplorationDecisionKind.SelectTarget, node.Id,
                "Candidate selected for independent authorization and grounding.");
        }

        return new(ExplorationDecisionKind.Stop, null, "No eligible safe navigation target.");
    }
}
