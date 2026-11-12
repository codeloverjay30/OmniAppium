namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Represents an observed UI candidate bound to one screenshot identity.
/// </summary>
public sealed record GameUiNode(
    string Id,
    string ObservationId,
    GameUiNodeKind Kind,
    string? Text,
    bool HasRedDot,
    bool IsRedDotAssociationAmbiguous,
    double Confidence);
