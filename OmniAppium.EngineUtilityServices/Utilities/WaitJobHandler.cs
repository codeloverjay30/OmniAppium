using OmniAppium.ConfigUtilityService.Models;

namespace OmniAppium.EngineUtilityService.Utilities;

/// <summary>
/// Handles wait jobs.
/// </summary>
public sealed class WaitJobHandler(
    IWaitService waitService
) : IJobHandler
{
    private readonly IWaitService _waitService =
        waitService ?? throw new ArgumentNullException(nameof(waitService));

    /// <summary>
    /// Determines whether this handler supports the specified job.
    /// </summary>
    public bool CanHandle(Job job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return job is WaitJob;
    }

    /// <summary>
    /// Executes the specified wait job.
    /// </summary>
    public Task AutoExecuteAsync(Job job)
    {
        ArgumentNullException.ThrowIfNull(job);

        if (job is not WaitJob waitJob)
        {
            throw new ArgumentException(
                $"Expected {nameof(WaitJob)}, but received {job.GetType().Name}.",
                nameof(job));
        }

        ArgumentOutOfRangeException.ThrowIfNegative(
            waitJob.Timeout,
            nameof(waitJob.Timeout));

        _waitService.Wait(waitJob.Timeout);

        return Task.CompletedTask;
    }
}