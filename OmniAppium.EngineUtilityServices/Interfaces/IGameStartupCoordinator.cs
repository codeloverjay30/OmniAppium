namespace OmniAppium.EngineUtilityServices.Interfaces;

/// <summary>
/// Defines the contract for ensuring that a game
/// reaches its main screen.
/// </summary>
public interface IGameStartupCoordinator
{
    /// <summary>
    /// Ensures that the game is ready for main-screen operations.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels startup verification.
    /// </param>
    /// <returns>A task representing startup completion.</returns>
    Task EnsureMainScreenReadyAsync(
        CancellationToken cancellationToken = default);
}
