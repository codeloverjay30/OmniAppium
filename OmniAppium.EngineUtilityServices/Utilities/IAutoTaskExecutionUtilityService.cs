using OmniAppium.ConfigUtilityService.Models;

namespace OmniAppium.EngineUtilityService.Utilities;

/// <summary>
/// Defines operations for executing automation jobs.
/// </summary>
public interface IAutoTaskExecutionUtilityService
{
    /// <summary>
    /// Executes a single automation job.
    /// </summary>
    /// <param name="job">The automation job.</param>
    /// <returns>A task representing the execution.</returns>
    Task ExecuteAsync(Job job);

    /// <summary>
    /// Executes automation jobs sequentially.
    /// </summary>
    /// <param name="jobs">The ordered automation jobs.</param>
    /// <returns>A task representing the sequence execution.</returns>
    Task ExecuteSequenceAsync(IEnumerable<Job> jobs);
}