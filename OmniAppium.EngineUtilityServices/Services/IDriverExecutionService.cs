using OmniAppium.ConfigUtilityServices.Models;

namespace OmniAppium.EngineUtilityService.Services
{
    public interface IDriverExecutionService
    {
        Task ExecuteJobsAsync(List<Job> steps);
    }
}
