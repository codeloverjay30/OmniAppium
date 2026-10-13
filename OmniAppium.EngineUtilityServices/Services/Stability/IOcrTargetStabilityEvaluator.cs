using OmniAppium.EngineUtilityServices.Models.Stability;

namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Evaluates whether a target OCR text remains stable
/// across consecutive screen observations.
/// </summary>
public interface IOcrTargetStabilityEvaluator
{
    /// <summary>
    /// Evaluates the target against the current OCR observation.
    /// </summary>
    /// <param name="lines">
    /// OCR lines with normalized center coordinates.
    /// </param>
    /// <param name="requiredConsecutiveMatches">
    /// Number of consecutive matching comparisons required.
    /// </param>
    /// <returns>
    /// True when the target is stable; otherwise, false.
    /// </returns>
    bool Observe(
        IEnumerable<NormalizedOcrTextLine> lines,
        int requiredConsecutiveMatches);

    /// <summary>
    /// Clears the accumulated observation state.
    /// </summary>
    void Reset();
}