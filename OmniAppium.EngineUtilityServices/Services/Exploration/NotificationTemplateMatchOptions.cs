namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>Defines bounded multi-scale template matching thresholds.</summary>
public sealed record NotificationTemplateMatchOptions(
    double MinimumSimilarity = 0.90,
    double MinimumScale = 0.75,
    double MaximumScale = 1.25,
    double ScaleStep = 0.05,
    double MaximumOverlap = 0.30,
    int MaximumMatches = 128,
    bool RequireExclamationGlyph = false);