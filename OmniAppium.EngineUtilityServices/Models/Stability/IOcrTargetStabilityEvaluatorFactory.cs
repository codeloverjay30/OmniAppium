namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Creates independent OCR target stability evaluators.
/// </summary>
public interface IOcrTargetStabilityEvaluatorFactory
{
    /// <summary>
    /// Creates an evaluator for the specified target.
    /// </summary>
    IOcrTargetStabilityEvaluator Create(
        string target,
        double maximumCenterMovement = 0.02);
}