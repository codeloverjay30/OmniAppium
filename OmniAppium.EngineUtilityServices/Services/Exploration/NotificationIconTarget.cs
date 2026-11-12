using CoordinateUtilityServices;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Represents a same-observation, independently detected icon and its OCR-grounded label identity.
/// </summary>
public sealed record NotificationIconTarget(string TargetId, Rectangle IconBounds);
