using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Coordinates same-observation red-dot processing without issuing device clicks.
/// </summary>
public sealed class RedDotObservationIntegrationService(
    IRedDotFrameDecoder decoder,
    RedDotDetector detector,
    INotificationNodeAssociator associator,
    INotificationEvidenceFilter filter,
    INotificationTemplateMatcher templateMatcher,
    NotificationAssociationOptions? associationOptions = null) : IRedDotObservationIntegrationService
{
    /// <summary>
    /// Decodes one observation, detects badges, and associates them to supplied same-frame OCR targets.
    /// </summary>
    public RedDotAssociationResult Analyze(RedDotObservationInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (string.IsNullOrWhiteSpace(input.ObservationId))
            throw new ArgumentException("Observation identity is required.", nameof(input));
        ArgumentNullException.ThrowIfNull(input.Observation);
        ArgumentNullException.ThrowIfNull(input.Targets);
        if (input.NotificationTemplate.IsEmpty)
            throw new InvalidOperationException(
                "A reviewed notification template is required before association.");

        // A caller must derive Targets from this exact Observation; no separate capture is performed.
        RedDotFrame frame = decoder.Decode(input.Observation.ImageBytes);
        var bounds = input.Targets.Select(t =>
        {
            if (t is null) throw new ArgumentException("OCR targets cannot contain null.", nameof(input));
            return new RedDotNodeBounds(t.Id, t.Bounds);
        }).ToArray();
        // The color detector is diagnostic only; it must never authorize a notification.
        IReadOnlyList<RedDotEvidence> rawEvidence = detector.Detect(frame, bounds);
        int rawCandidates = rawEvidence.Count;
        IReadOnlyList<NotificationTemplateMatch> matches = templateMatcher.Match(
            input.Observation.ImageBytes, input.NotificationTemplate, input.TemplateOptions);
        IReadOnlyList<RedDotEvidence> evidence = ToVerifiedEvidence(matches, rawEvidence);

        int subsequentFrameCount = 0;
        if (input.SubsequentObservations is not null)
        {
            subsequentFrameCount = input.SubsequentObservations.Count;

            NotificationEvidenceFilterOptions options = input.FilterOptions
                ?? new NotificationEvidenceFilterOptions();
            var subsequent = new List<IReadOnlyList<RedDotEvidence>>(
                input.SubsequentObservations.Count);
            foreach (IAndroidScreenObservation observation in input.SubsequentObservations)
            {
                if (observation is null)
                    throw new ArgumentException(
                        "Subsequent observations cannot contain null.", nameof(input));
                RedDotFrame nextFrame = decoder.Decode(observation.ImageBytes);
                if (nextFrame.Width != frame.Width || nextFrame.Height != frame.Height)
                    throw new InvalidOperationException(
                        "Subsequent observation dimensions must match baseline.");
                IReadOnlyList<NotificationTemplateMatch> nextMatches = templateMatcher.Match(
                    observation.ImageBytes, input.NotificationTemplate, input.TemplateOptions);
                IReadOnlyList<RedDotEvidence> nextRaw = detector.Detect(nextFrame, bounds);
                subsequent.Add(ToVerifiedEvidence(nextMatches, nextRaw));
            }
            evidence = filter.Filter(evidence, subsequent, frame.Width, frame.Height, options);
        }
        // Re-evaluate ownership only after reviewed template and temporal filtering.
        // Preserve all candidates for diagnostics; never choose among competing owners.
        NotificationAssociationOptions settings = associationOptions ?? new NotificationAssociationOptions();
        settings.Validate();
        if (input.IconTargets is not null)
        {
            var validTargetIds = input.Targets.Select(t => t.Id).ToHashSet(StringComparer.Ordinal);
            if (input.IconTargets.Any(icon => icon is null ||
                string.IsNullOrWhiteSpace(icon.TargetId) ||
                !validTargetIds.Contains(icon.TargetId)) ||
                input.IconTargets.GroupBy(icon => icon.TargetId, StringComparer.Ordinal)
                    .Any(group => group.Count() > 1))
                throw new ArgumentException(
                    "Icon targets must map uniquely to same-observation OCR targets.", nameof(input));
        }
        evidence = evidence.Select(badge => new RedDotEvidence(
            badge.Bounds, badge.Confidence,
            input.IconTargets is null
                ? NotificationAssociationGeometry.Resolve(badge, input.Targets, settings)
                : NotificationAssociationGeometry.ResolveViaIcons(
                    badge, input.Targets, input.IconTargets, settings))).ToArray();
        IReadOnlyList<GameUiNode> nodes = associator.Associate(input.ObservationId, input.Targets, evidence);
        if (nodes.Any(n => n.ObservationId != input.ObservationId))
            throw new InvalidOperationException("Associated node observation identity mismatch.");
        return new RedDotAssociationResult(
            input.ObservationId,
            nodes,
            evidence)
        {
            VerifiedTemplateMatches = matches,
            FilteringStatistics = new RedDotFilteringStatistics(
                RawCandidates: rawCandidates,
                FilteredCandidates: evidence.Count,
                RejectedCandidates: Math.Max(0, rawCandidates - evidence.Count),
                SubsequentFrameCount: subsequentFrameCount,
                AssociatedNodes: nodes.Count)
        };
    }    /// <summary>Converts reviewed visual badge matches into non-authorizing association evidence.</summary>
    private static IReadOnlyList<RedDotEvidence> ToVerifiedEvidence(
        IReadOnlyList<NotificationTemplateMatch> matches,
        IReadOnlyList<RedDotEvidence> rawEvidence)
    {
        ArgumentNullException.ThrowIfNull(matches);
        ArgumentNullException.ThrowIfNull(rawEvidence);
        return matches.Select(match =>
        {
            if (match.Width <= 0 || match.Height <= 0 ||
                !double.IsFinite(match.Similarity) ||
                match.Similarity is < 0 or > 1)
                throw new InvalidOperationException("Invalid notification template match.");
            var badge = CoordinateUtilityServices.Rectangle.FromXYWH(
                match.X, match.Y, match.Width, match.Height);
            // Only carry owner IDs from raw detections whose centers lie inside
            // the verified template rectangle. Ambiguous owners remain ambiguous.
            string[] owners = rawEvidence
                .Where(raw => raw.Bounds.TopLeft.X + raw.Bounds.Width / 2 >= match.X &&
                    raw.Bounds.TopLeft.X + raw.Bounds.Width / 2 <= match.X + match.Width &&
                    raw.Bounds.TopLeft.Y + raw.Bounds.Height / 2 >= match.Y &&
                    raw.Bounds.TopLeft.Y + raw.Bounds.Height / 2 <= match.Y + match.Height)
                .SelectMany(raw => raw.CandidateNodeIds)
                .Distinct(StringComparer.Ordinal)
                .ToArray();
            return new RedDotEvidence(badge, match.Similarity, owners);
        }).ToArray();
    }
}