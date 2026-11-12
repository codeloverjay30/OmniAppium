using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Associates detected badges with OCR targets from the same observation.
/// </summary>
public interface INotificationNodeAssociator
{
    /// <summary>
    /// Builds conservative UI-node associations without guessing ambiguous owners.
    /// </summary>
    IReadOnlyList<GameUiNode> Associate(
        string observationId,
        IReadOnlyList<OcrGroundedTarget> targets,
        IReadOnlyList<RedDotEvidence> evidence);
}