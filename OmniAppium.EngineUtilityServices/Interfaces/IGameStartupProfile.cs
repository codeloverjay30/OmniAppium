namespace OmniAppium.EngineUtilityServices.Interfaces;

/// <summary>
/// Defines game-specific screen targets used by
/// the startup coordination process.
/// </summary>
public interface IGameStartupProfile
{
    /// <summary>
    /// Gets the text identifying the login button.
    /// </summary>
    string LoginButtonText { get; }

    /// <summary>
    /// Gets the text identifying the ready main screen.
    /// </summary>
    string MainScreenReadyText { get; }
}
