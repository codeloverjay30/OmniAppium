using OmniAppium.ConfigUtilityService.Models;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IClickService
    {
        void Click(ClickJob clickJob);
        void Click(double rx, double ry,bool usePreloadScreenSize = true);
        void TapAt(
            int x ,
            int y ,
            TimeSpan duration = default // 編譯器會將其視為 0 毫秒的 TimeSpan
        );
    }
}
