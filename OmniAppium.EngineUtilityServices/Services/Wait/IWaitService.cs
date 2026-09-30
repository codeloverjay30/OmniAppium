namespace OmniAppium.EngineUtilityService.Services.Wait
{
    /// <summary>
    /// Defines blocking wait operations for automation jobs.
    /// </summary>
    public interface IWaitService
    {
        /// <summary>
        /// Waits using the specified duration.
        /// </summary>
        /// <param name="timeout">The requested duration.</param>
        void Wait(TimeSpan timeout);
        /// <summary>
        /// Waits for the specified number of milliseconds.
        /// </summary>
        /// <param name="timeout">The duration in milliseconds.</param>
        void Wait(int timeout);
    }
}
