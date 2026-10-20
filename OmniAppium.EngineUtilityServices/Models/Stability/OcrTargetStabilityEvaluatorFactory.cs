using OmniAppium.EngineUtilityServices.Services.Stability;

namespace OmniAppium.EngineUtilityServices.Models.Stability;

/// <summary>
/// Creates isolated, stateful OCR target stability evaluators.
/// </summary>
public sealed class OcrTargetStabilityEvaluatorFactory
    : IOcrTargetStabilityEvaluatorFactory
{
    /// <summary>
    /// Creates a new evaluator with independent observation state.
    /// </summary>
    public IOcrTargetStabilityEvaluator Create(
        string target,
        double maximumCenterMovement = 0.02)
    {
        return new OcrTargetStabilityEvaluator(
            target,
            maximumCenterMovement);
    }
}