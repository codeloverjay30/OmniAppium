
using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Evaluates whether a unique OCR target remains at a stable
/// normalized screen location across consecutive observations.
/// </summary>
/// <remarks>
/// Instances are stateful and must not be shared between
/// independent or concurrent observation sequences.
/// </remarks>
public sealed class OcrTargetStabilityEvaluator
    : IOcrTargetStabilityEvaluator
{
    private readonly string _target;
    private readonly double _maximumCenterMovement;

    private (double X, double Y)? _previousCenter;
    private int _consecutiveMatches;

    /// <summary>
    /// Initializes an evaluator for a specific OCR target.
    /// </summary>
    /// <param name="target">
    /// The required target text.
    /// </param>
    /// <param name="maximumCenterMovement">
    /// Maximum permitted movement on each normalized axis.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the target is empty or whitespace.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the movement tolerance is invalid.
    /// </exception>
    public OcrTargetStabilityEvaluator(
        string target,
        double maximumCenterMovement = 0.02)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(target);

        if (!double.IsFinite(maximumCenterMovement) ||
            maximumCenterMovement is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maximumCenterMovement),
                "Maximum center movement must be between 0 and 1.");
        }

        _target = NormalizeText(target);
        _maximumCenterMovement = maximumCenterMovement;
    }

    /// <inheritdoc/>
    public bool Observe(
        IEnumerable<NormalizedOcrTextLine> lines,
        int requiredConsecutiveMatches)
    {
        ArgumentNullException.ThrowIfNull(lines);

        if (requiredConsecutiveMatches <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(requiredConsecutiveMatches),
                "Required consecutive matches must be positive.");
        }

        NormalizedOcrTextLine? targetLine = null;

        foreach (NormalizedOcrTextLine line in lines)
        {
            if (line is null ||
                string.IsNullOrWhiteSpace(line.Text))
            {
                continue;
            }

            if (!IsValidCoordinate(
                    line.CenterX,
                    line.CenterY))
            {
                continue;
            }

            if (!IsTargetMatch(line.Text))
            {
                continue;
            }

            // Multiple matching targets are ambiguous.
            // Never select an arbitrary target.
            if (targetLine is not null)
            {
                Reset();
                return false;
            }

            targetLine = line;
        }

        // The target is absent or its location is invalid.
        if (targetLine is null)
        {
            Reset();
            return false;
        }

        var currentCenter = (
            X: targetLine.CenterX,
            Y: targetLine.CenterY);

        // The first observation establishes the baseline.
        if (_previousCenter is null)
        {
            _previousCenter = currentCenter;
            _consecutiveMatches = 0;

            return false;
        }

        (double X, double Y) previousCenter =
            _previousCenter.Value;

        bool isStable =
            Math.Abs(
                previousCenter.X - currentCenter.X)
                <= _maximumCenterMovement &&
            Math.Abs(
                previousCenter.Y - currentCenter.Y)
                <= _maximumCenterMovement;

        // Update the baseline after every valid observation.
        _previousCenter = currentCenter;

        if (!isStable)
        {
            _consecutiveMatches = 0;
            return false;
        }

        // Prevent integer overflow during long-running sessions.
        if (_consecutiveMatches < requiredConsecutiveMatches)
        {
            _consecutiveMatches++;
        }

        return _consecutiveMatches >=
            requiredConsecutiveMatches;
    }

    /// <inheritdoc/>
    public void Reset()
    {
        _previousCenter = null;
        _consecutiveMatches = 0;
    }

    /// <summary>
    /// Determines whether the recognized text contains the target.
    /// </summary>
    private bool IsTargetMatch(string recognizedText)
    {
        string normalized = NormalizeText(recognizedText);

        return normalized.Contains(
            _target,
            StringComparison.Ordinal);
    }

    /// <summary>
    /// Validates normalized screen coordinates.
    /// </summary>
    private static bool IsValidCoordinate(
        double centerX,
        double centerY)
    {
        return double.IsFinite(centerX) &&
               double.IsFinite(centerY) &&
               centerX is >= 0 and <= 1 &&
               centerY is >= 0 and <= 1;
    }

    /// <summary>
    /// Removes whitespace from OCR text without altering
    /// character casing or non-whitespace characters.
    /// </summary>
    private static string NormalizeText(string text)
    {
        ReadOnlySpan<char> source = text.AsSpan();

        int nonWhitespaceCount = 0;

        foreach (char character in source)
        {
            if (!char.IsWhiteSpace(character))
            {
                nonWhitespaceCount++;
            }
        }

        return string.Create(
            nonWhitespaceCount,
            text,
            static (destination, value) =>
            {
                int index = 0;

                foreach (char character in value.AsSpan())
                {
                    if (!char.IsWhiteSpace(character))
                    {
                        destination[index++] = character;
                    }
                }
            });
    }
}
