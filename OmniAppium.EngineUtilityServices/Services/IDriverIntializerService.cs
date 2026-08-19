using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityService.Utilities;

namespace OmniAppium.EngineUtilityService.Services
{
    public interface IDriverIntializerService : IDisposable
    {

        void Initialize(); // 根據組態初始化
    }
}
