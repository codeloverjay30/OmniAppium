using OmniAppium.EngineUtilityServices.Interfaces;

namespace OmniAppiumDemo.Games.BeautyThreeKingdoms.Startup;

/// <summary>
/// Defines the startup screen targets for
/// Beauty Three Kingdoms.
/// </summary>
public sealed class BeautyThreeKingdomsStartupProfile
    : IGameStartupProfile
{
    /// <inheritdoc />
    public string LoginButtonText => "登入遊戲";

    /// <inheritdoc />
    public string MainScreenReadyText => "任務";
}
