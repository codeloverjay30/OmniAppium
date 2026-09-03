using OmniAppium.ConfigUtilityService.Models;

namespace OmniAppium.EngineUtilityService.Utilities;

/// <summary>
/// Handles automation jobs that perform click operations.
/// </summary>
public sealed class ClickJobHandler(
    IClickService clickService
) : IJobHandler
{
    /// <summary>
    /// Determines whether the specified job can be handled by this handler.
    /// </summary>
    /// <param name="job">The automation job.</param>
    /// <returns>
    /// <see langword="true"/> when the job is a <see cref="ClickJob"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool CanHandle(Job job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return job is ClickJob;
    }

    /// <summary>
    /// Executes the specified click job.
    /// </summary>
    /// <param name="job">The automation job to execute.</param>
    /// <returns>A completed task after the click operation finishes.</returns>
    public Task AutoExecuteAsync(Job job)
    {
        ArgumentNullException.ThrowIfNull(job);

        if (job is not ClickJob clickJob)
        {
            throw new ArgumentException(
                $"Expected {nameof(ClickJob)}, but received {job.GetType().Name}.",
                nameof(job));
        }

        clickService.Click(clickJob);

        return Task.CompletedTask;
    }
}