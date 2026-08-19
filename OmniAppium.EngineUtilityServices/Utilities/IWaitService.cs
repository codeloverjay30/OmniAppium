namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IWaitService
    {
        void Wait(TimeSpan timeout);
        void Wait(int timeout);
    }
}
