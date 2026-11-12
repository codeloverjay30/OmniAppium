namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Describes an RGB24 screenshot whose pixels are stored in row-major order.
/// </summary>
public sealed record RedDotFrame(int Width, int Height, ReadOnlyMemory<byte> Rgb24);
