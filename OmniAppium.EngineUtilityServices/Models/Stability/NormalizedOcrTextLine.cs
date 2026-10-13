namespace OmniAppium.EngineUtilityServices.Models.Stability;

/// <summary>
/// Represents recognized text at a normalized screen location.
/// </summary>
public sealed record NormalizedOcrTextLine(
    string Text,
    double CenterX,
    double CenterY);