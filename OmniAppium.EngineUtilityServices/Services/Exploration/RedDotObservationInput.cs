using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Binds one screenshot observation to an explicit identity and its same-frame OCR targets.
/// </summary>
public sealed record RedDotObservationInput(
    string ObservationId,
    IAndroidScreenObservation Observation,
    IReadOnlyList<OcrGroundedTarget> Targets)
{
    /// <summary>
    /// Gets optional same-frame icon bounds already linked to OCR-grounded target identifiers.
    /// Missing OCR targets cannot be synthesized from icon annotations.
    /// </summary>
    public IReadOnlyList<NotificationIconTarget>? IconTargets { get; init; }

    /// <summary>Gets the required, reviewed notification badge PNG/JPEG template.</summary>
    public ReadOnlyMemory<byte> NotificationTemplate { get; init; }

    /// <summary>Gets optional bounded template matching options.</summary>
    public NotificationTemplateMatchOptions? TemplateOptions { get; init; }

    /// <summary>Gets optional subsequent observations used to verify temporal stability.</summary>
    public IReadOnlyList<IAndroidScreenObservation>? SubsequentObservations { get; init; }

    /// <summary>Gets the filtering options for subsequent observations.</summary>
    public NotificationEvidenceFilterOptions? FilterOptions { get; init; }
}