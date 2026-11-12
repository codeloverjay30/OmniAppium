namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>Matches an encoded visual badge template against an encoded screenshot.</summary>
public interface INotificationTemplateMatcher
{
    /// <summary>Returns spatially deduplicated matches or fails on invalid inputs.</summary>
    IReadOnlyList<NotificationTemplateMatch> Match(
        ReadOnlyMemory<byte> screenshot,
        ReadOnlyMemory<byte> template,
        NotificationTemplateMatchOptions? options = null);
}
