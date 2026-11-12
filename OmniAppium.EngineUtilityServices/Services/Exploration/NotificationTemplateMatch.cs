namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>Represents a verified visual template match without click authorization.</summary>
public sealed record NotificationTemplateMatch(
    int X,
    int Y,
    int Width,
    int Height,
    double Similarity);
