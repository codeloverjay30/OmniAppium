using OmniAppium.ConfigUtilityService.Models;

namespace OmniAppium.EngineUtilityService.Utilities;

/// <summary>
/// Dispatches automation jobs to their corresponding job handlers.
/// </summary>
public sealed class AutoTaskExecutionUtilityService
    : IAutoTaskExecutionUtilityService
{
    private readonly IReadOnlyList<IJobHandler> _handlers;

    /// <summary>
    /// Initializes a new automation job execution service.
    /// </summary>
    /// <param name="handlers">The registered job handlers.</param>
    public AutoTaskExecutionUtilityService(
        IEnumerable<IJobHandler> handlers)
    {
        ArgumentNullException.ThrowIfNull(handlers);

        _handlers = handlers.ToArray();

        if (_handlers.Count == 0)
        {
            throw new ArgumentException(
                "At least one job handler must be registered.",
                nameof(handlers));
        }

        if (_handlers.Any(static handler => handler is null))
        {
            throw new ArgumentException(
                "The job handler collection must not contain null entries.",
                nameof(handlers));
        }
    }

    /// <summary>
    /// Executes a single automation job.
    /// </summary>
    /// <param name="job">The automation job.</param>
    /// <returns>A task representing the execution.</returns>
    public async Task ExecuteAsync(Job job)
    {
        ArgumentNullException.ThrowIfNull(job);

        var handler = ResolveHandler(job);

        await handler.AutoExecuteAsync(job);
    }

    /// <summary>
    /// Executes automation jobs sequentially in the supplied order.
    /// </summary>
    /// <param name="jobs">The ordered automation jobs.</param>
    /// <returns>A task representing the sequence execution.</returns>
    public async Task ExecuteSequenceAsync(
        IEnumerable<Job> jobs)
    {
        ArgumentNullException.ThrowIfNull(jobs);

        foreach (var job in jobs)
        {
            ArgumentNullException.ThrowIfNull(job);

            await ExecuteAsync(job);
        }
    }

    /// <summary>
    /// Resolves exactly one handler for the specified automation job.
    /// </summary>
    /// <param name="job">The automation job.</param>
    /// <returns>The unique matching handler.</returns>
    private IJobHandler ResolveHandler(Job job)
    {
        var matchingHandlers = _handlers
            .Where(handler => handler.CanHandle(job))
            .Take(2)
            .ToArray();

        return matchingHandlers.Length switch
        {
            0 => throw new NotSupportedException(
                $"No handler is registered for job type '{job.GetType().Name}'."),

            1 => matchingHandlers[0],

            _ => throw new InvalidOperationException(
                $"Multiple handlers are registered for job type '{job.GetType().Name}'.")
        };
    }
}