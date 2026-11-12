using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Maps verified badge evidence to OCR-grounded targets using fail-closed association rules.
/// </summary>
public sealed class NotificationNodeAssociator : INotificationNodeAssociator
{
    private readonly NotificationAssociationOptions _options;

    /// <summary>Creates an associator with validated geometric settings.</summary>
    public NotificationNodeAssociator(NotificationAssociationOptions? options = null)
    {
        _options = options ?? new NotificationAssociationOptions();
        _options.Validate();
    }

    /// <summary>Creates nodes without assigning confidence or semantic click authorization.</summary>
    public IReadOnlyList<GameUiNode> Associate(
        string observationId,
        IReadOnlyList<OcrGroundedTarget> targets,
        IReadOnlyList<RedDotEvidence> evidence)
    {
        if (string.IsNullOrWhiteSpace(observationId))
            throw new ArgumentException("Observation identity is required.", nameof(observationId));
        ArgumentNullException.ThrowIfNull(targets);
        ArgumentNullException.ThrowIfNull(evidence);
        if (targets.Any(t => t is null || string.IsNullOrWhiteSpace(t.Id)) ||
            targets.GroupBy(t => t.Id, StringComparer.Ordinal).Any(g => g.Count() > 1))
            throw new ArgumentException("OCR target identifiers must be unique and non-empty.", nameof(targets));

        var ids = targets.Select(t => t.Id).ToHashSet(StringComparer.Ordinal);
        var owners = new List<IReadOnlyList<string>>(evidence.Count);
        foreach (RedDotEvidence item in evidence)
        {
            if (item is null || item.CandidateNodeIds is null ||
                !double.IsFinite(item.Confidence) || item.Confidence is < 0 or > 1 ||
                item.CandidateNodeIds.Any(id => !ids.Contains(id)))
                throw new ArgumentException("Red-dot evidence contains invalid candidate identifiers or confidence.", nameof(evidence));
            owners.Add(NotificationAssociationGeometry.Resolve(item, targets, _options));
        }

        var nodes = new List<GameUiNode>(targets.Count);
        foreach (OcrGroundedTarget target in targets)
        {
            var matches = owners.Where(o => o.Contains(target.Id, StringComparer.Ordinal)).ToArray();
            bool ambiguous = matches.Length > 1 || matches.Any(o => o.Count != 1);
            nodes.Add(new GameUiNode(target.Id, observationId, GameUiNodeKind.Unknown,
                target.Text, matches.Length == 1 && !ambiguous, ambiguous, 0));
        }
        return nodes.AsReadOnly();
    }
}