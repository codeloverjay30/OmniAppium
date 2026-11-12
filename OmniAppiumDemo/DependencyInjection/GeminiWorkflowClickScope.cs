using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Binds the protected Gemini click factory to the current asynchronous workflow.
/// </summary>
public sealed class GeminiWorkflowClickScope
{
    private readonly AsyncLocal<IServiceProvider?> _current = new();

    /// <summary>
    /// Resolves the protected click tool only within an active workflow scope.
    /// </summary>
    public OcrGroundedPlannerClickTool ResolveClickTool()
    {
        IServiceProvider provider = _current.Value
            ?? throw new InvalidOperationException("No active Gemini workflow scope.");
        return provider.GetRequiredService<OcrGroundedPlannerClickTool>();
    }

    /// <summary>
    /// Activates the specified workflow service scope for the current async flow.
    /// </summary>
    public IDisposable Enter(IServiceScope scope)
    {
        ArgumentNullException.ThrowIfNull(scope);
        if (_current.Value is not null)
        {
            throw new InvalidOperationException("A Gemini workflow scope is already active.");
        }

        _current.Value = scope.ServiceProvider;
        return new Lease(this, scope.ServiceProvider);
    }

    private sealed class Lease(GeminiWorkflowClickScope owner, IServiceProvider provider) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) return;
            if (!ReferenceEquals(owner._current.Value, provider))
            {
                throw new InvalidOperationException("Gemini workflow scope ownership mismatch.");
            }

            owner._current.Value = null;
            _disposed = true;
        }
    }
}