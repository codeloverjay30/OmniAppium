namespace OmniAppium.EngineUtilityServices.Services.Stability;

/// <summary>
/// Compares decoded screen images for visual similarity.
/// </summary>
public interface IScreenFrameComparer
{
    /// <summary>
    /// Determines whether two encoded images differ by no more than the specified pixel ratio.
    /// </summary>
    bool AreSimilar(ReadOnlyMemory<byte> first, ReadOnlyMemory<byte> second, double tolerance);
}
