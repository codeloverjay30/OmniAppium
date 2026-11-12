namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Filters red-dot evidence using configurable spatial and temporal checks.
/// </summary>
public interface INotificationEvidenceFilter
{
    /// <summary>
    /// Returns baseline evidence supported by subsequent observations.
    /// </summary>
    IReadOnlyList<RedDotEvidence> Filter(
        IReadOnlyList<RedDotEvidence> baseline,
        IReadOnlyList<IReadOnlyList<RedDotEvidence>> subsequentFrames,
        int imageWidth,
        int imageHeight,
        NotificationEvidenceFilterOptions options);
}