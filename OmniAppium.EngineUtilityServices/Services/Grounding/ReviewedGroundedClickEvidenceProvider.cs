using OCRUtilityServices.Models;
using System.Runtime.CompilerServices;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Stores explicitly reviewed click evidence bound to exact in-memory OCR captures.
/// Registrations are supplied by a trusted reviewer, never inferred from OCR text.
/// </summary>
public sealed class ReviewedGroundedClickEvidenceProvider : IAtomicGroundedClickEvidenceProvider
{
    private readonly object _gate = new();
    private readonly TimeProvider _clock;
    private readonly ConditionalWeakTable<object, ConsumedMarker> _consumed = new();
    private readonly Dictionary<object, Entry> _entries =
        new(ReferenceEqualityComparer.Instance);

    /// <summary>
    /// Creates a provider using an injectable clock.
    /// </summary>
    public ReviewedGroundedClickEvidenceProvider(TimeProvider clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    /// <summary>
    /// Registers an independently reviewed OCR observation and its exact match.
    /// </summary>
    public void Register(
        OcrResult observation,
        string targetText,
        OcrTextMatch match,
        ReviewedGroundTruth evidence,
        float confidence,
        TimeSpan lifetime)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetText);
        ArgumentNullException.ThrowIfNull(match);
        ArgumentNullException.ThrowIfNull(evidence);
        ValidateRegistration(confidence, lifetime);
        lock (_gate)
        {
            EnsureNotRegistered(observation);
            _entries[observation] = new Entry(
                targetText, match.Text, match.Bounds, evidence, confidence,
                _clock.GetUtcNow().Add(lifetime), null);
        }
    }

    /// <summary>
    /// Registers independently reviewed evidence for an exact snapshot target.
    /// </summary>
    public void Register(
        OcrGroundedSnapshot snapshot,
        OcrGroundedTarget target,
        ReviewedGroundTruth evidence,
        float confidence,
        TimeSpan lifetime)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(evidence);
        ValidateRegistration(confidence, lifetime);
        if (snapshot.Targets is null ||
            !snapshot.Targets.Any(item => ReferenceEquals(item, target)) ||
            snapshot.Targets.Count(item => item.Id == target.Id) != 1)
        {
            throw new InvalidOperationException(
                "Reviewed target must belong uniquely to the exact snapshot.");
        }

        lock (_gate)
        {
            EnsureNotRegistered(snapshot);
            _entries[snapshot] = new Entry(
                target.Text, target.Text, target.Bounds, evidence, confidence,
                _clock.GetUtcNow().Add(lifetime), target);
        }
    }

    /// <inheritdoc/>
    public ConsumedGroundedClickEvidence Consume(
        OcrResult observation, string targetText, OcrTextMatch match)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetText);
        ArgumentNullException.ThrowIfNull(match);
        lock (_gate)
        {
            Entry entry = GetCurrent(observation);
            if (!string.Equals(entry.RequestedText, targetText, StringComparison.Ordinal) ||
                !string.Equals(entry.MatchedText, match.Text, StringComparison.Ordinal) ||
                !Equals(entry.Bounds, match.Bounds))
                throw new InvalidOperationException("Reviewed OCR target or match geometry does not match.");
            MarkConsumed(observation);
            return new ConsumedGroundedClickEvidence(entry.Evidence, entry.Confidence);
        }
    }

    /// <inheritdoc/>
    public ConsumedGroundedClickEvidence Consume(
        OcrGroundedSnapshot snapshot, OcrGroundedTarget target)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(target);
        lock (_gate)
        {
            Entry entry = GetCurrent(snapshot);
            ValidateTarget(snapshot, target, entry);
            MarkConsumed(snapshot);
            return new ConsumedGroundedClickEvidence(entry.Evidence, entry.Confidence);
        }
    }

    /// <summary>
    /// Rejects reuse of an active or previously consumed capture.
    /// </summary>
    private void EnsureNotRegistered(object capture)
    {
        if (_entries.ContainsKey(capture) || _consumed.TryGetValue(capture, out _))
            throw new InvalidOperationException("Reviewed capture is already registered or consumed.");
    }

    /// <summary>
    /// Atomically transitions an active capture to consumed.
    /// </summary>
    private void MarkConsumed(object capture)
    {
        _entries.Remove(capture);
        _consumed.Add(capture, new ConsumedMarker());
    }

    /// <inheritdoc/>
    public ReviewedGroundTruth Resolve(OcrResult observation, string targetText)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetText);
        lock (_gate)
        {
            Entry entry = GetCurrent(observation);
            if (!string.Equals(entry.RequestedText, targetText, StringComparison.Ordinal))
                throw new InvalidOperationException("Reviewed OCR target does not match.");
            MarkConsumed(observation);
            return entry.Evidence;
        }
    }

    /// <inheritdoc/>
    public float ResolveConfidence(OcrResult observation, OcrTextMatch match)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(match);
        lock (_gate)
        {
            Entry entry = GetCurrent(observation);
            if (!string.Equals(entry.MatchedText, match.Text, StringComparison.Ordinal) ||
                !Equals(entry.Bounds, match.Bounds))
                throw new InvalidOperationException("Reviewed OCR match geometry does not match.");
            return entry.Confidence;
        }
    }

    /// <inheritdoc/>
    public ReviewedGroundTruth Resolve(OcrGroundedSnapshot snapshot, OcrGroundedTarget target)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(target);
        lock (_gate)
        {
            Entry entry = GetCurrent(snapshot);
            ValidateTarget(snapshot, target, entry);
            MarkConsumed(snapshot);
            return entry.Evidence;
        }
    }

    /// <inheritdoc/>
    public float ResolveConfidence(OcrGroundedSnapshot snapshot, OcrGroundedTarget target)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(target);
        lock (_gate)
        {
            Entry entry = GetCurrent(snapshot);
            ValidateTarget(snapshot, target, entry);
            return entry.Confidence;
        }
    }

    /// <summary>
    /// Revokes evidence for an exact capture object.
    /// </summary>
    public void Revoke(object capture)
    {
        ArgumentNullException.ThrowIfNull(capture);
        lock (_gate)
        {
            _entries.Remove(capture);
        }
    }

    /// <summary>
    /// Ensures confidence and expiry are safe before registration.
    /// </summary>
    private static void ValidateRegistration(float confidence, TimeSpan lifetime)
    {
        if (!float.IsFinite(confidence) || confidence < 0 || confidence > 1)
            throw new ArgumentOutOfRangeException(nameof(confidence),
                "Reviewed confidence must be finite and between zero and one.");
        if (lifetime <= TimeSpan.Zero || lifetime > TimeSpan.FromMinutes(5))
            throw new ArgumentOutOfRangeException(nameof(lifetime),
                "Reviewed evidence lifetime must be positive and no more than five minutes.");
    }

    /// <summary>
    /// Retrieves evidence only while its registration remains current.
    /// </summary>
    private Entry GetCurrent(object capture)
    {
        if (!_entries.TryGetValue(capture, out Entry? entry))
        {
            if (_consumed.TryGetValue(capture, out _))
                throw new InvalidOperationException("Reviewed click evidence has already been consumed.");
            throw new InvalidOperationException("Reviewed click evidence is unavailable.");
        }
        if (_clock.GetUtcNow() >= entry.ExpiresAt)
        {
            _entries.Remove(capture);
            throw new InvalidOperationException("Reviewed click evidence has expired.");
        }
        return entry;
    }

    /// <summary>
    /// Verifies exact snapshot membership and immutable reviewed target attributes.
    /// </summary>
    private static void ValidateTarget(
        OcrGroundedSnapshot snapshot, OcrGroundedTarget target, Entry entry)
    {
        if (!ReferenceEquals(entry.Target, target) ||
            snapshot.Targets is null ||
            !snapshot.Targets.Any(item => ReferenceEquals(item, target)) ||
            !string.Equals(entry.RequestedText, target.Text, StringComparison.Ordinal) ||
            !Equals(entry.Bounds, target.Bounds))
            throw new InvalidOperationException("Reviewed grounded target does not match.");
    }

    /// <summary>
    /// Identifies a consumed capture without retaining its key strongly.
    /// </summary>
    private sealed class ConsumedMarker { }

    /// <summary>
    /// Holds a single reviewed authorization record.
    /// </summary>
    private sealed record Entry(
        string RequestedText,
        string MatchedText,
        CoordinateUtilityServices.Rectangle Bounds,
        ReviewedGroundTruth Evidence,
        float Confidence,
        DateTimeOffset ExpiresAt,
        OcrGroundedTarget? Target);
}