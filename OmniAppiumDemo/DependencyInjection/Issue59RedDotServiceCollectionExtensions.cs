using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using OmniAppiumDemo.SmokeTests.Issue59;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Provides dependency injection registrations for Issue 59
/// red-dot observation integration and smoke testing.
/// </summary>
public static class Issue59RedDotServiceCollectionExtensions
{
    /// <summary>
    /// Registers red-dot detection, screenshot decoding,
    /// node association, and observation integration services.
    /// </summary>
    /// <param name="services">
    /// The service collection to configure.
    /// </param>
    /// <returns>
    /// The configured service collection.
    /// </returns>
    public static IServiceCollection AddIssue59RedDotExploration(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Fail closed on invalid configuration rather than silently widening matching.
        services.TryAddSingleton(_ =>
        {
            string path = Path.Combine(AppContext.BaseDirectory, "notification-matches.json5");
            if (!File.Exists(path))
                throw new FileNotFoundException("Notification association configuration is missing.", path);
            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path),
                new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
            if (!document.RootElement.TryGetProperty("association", out JsonElement association))
                throw new InvalidOperationException("Notification association configuration is missing.");
            NotificationAssociationOptions options = association.Deserialize<NotificationAssociationOptions>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Notification association configuration is null.");
            options.Validate();
            return options;
        });

        // Screenshot decoding: encoded image -> RGB24.
        services.TryAddSingleton<
            IRedDotFrameDecoder,
            OpenCvRedDotFrameDecoder>();

        services.TryAddSingleton<INotificationTemplateMatcher, OpenCvNotificationTemplateMatcher>();

        // Red-dot detection.
        services.TryAddSingleton<RedDotDetector>();

        // Red-dot evidence -> OCR node association.
        services.TryAddSingleton<
            INotificationNodeAssociator,
            NotificationNodeAssociator>();

        // Conservative spatial and temporal filtering.
        services.TryAddSingleton<INotificationEvidenceFilter, NotificationEvidenceFilter>();

        // Observation integration orchestration.
        services.TryAddSingleton<
            IRedDotObservationIntegrationService,
            RedDotObservationIntegrationService>();

        // Read-only real-device smoke test.
        services.TryAddTransient<Issue59RedDotSmokeTestRunner>();

        return services;
    }
}