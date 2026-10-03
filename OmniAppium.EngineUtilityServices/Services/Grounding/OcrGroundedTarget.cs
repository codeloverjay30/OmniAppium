using CoordinateUtilityServices;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Represents an OCR-grounded target that can be selected by an AI planner.
/// </summary>
public sealed record OcrGroundedTarget(
    string Id,
    string Text,
    Rectangle Bounds);
