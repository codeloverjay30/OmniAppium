using CoordinateUtilityServices;

namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Describes a candidate navigation element and its pixel-space bounds.
/// </summary>
public sealed record RedDotNodeBounds(string Id, Rectangle Bounds);
