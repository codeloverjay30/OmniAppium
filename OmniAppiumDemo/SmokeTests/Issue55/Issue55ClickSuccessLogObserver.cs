using Microsoft.Extensions.Logging;
using System.Threading;

namespace OmniAppiumDemo.SmokeTests.Issue55;

/// <summary>
/// Observes successful ClickService tap logs during an Issue 55 smoke test.
/// </summary>
public sealed class Issue55ClickSuccessLogObserver : ILoggerProvider
{
    private const string SuccessTemplate =
        "Successfully tap once at Position {Position}";

    private int _successCount;
    private int _disposed;

    /// <summary>
    /// Gets whether a successful tap log was emitted.
    /// </summary>
    public bool SuccessLogObserved =>
        Volatile.Read(ref _successCount) > 0;

    /// <summary>
    /// Gets the number of successful tap logs observed.
    /// </summary>
    public int SuccessLogCount =>
        Volatile.Read(ref _successCount);

    /// <summary>
    /// Creates a logger that observes successful tap events.
    /// </summary>
    public ILogger CreateLogger(string categoryName)
    {
        ObjectDisposedException.ThrowIf(
            Volatile.Read(ref _disposed) != 0,
            this);

        return new ObserverLogger(this);
    }

    /// <summary>
    /// Releases the observer.
    /// </summary>
    public void Dispose()
    {
        Interlocked.Exchange(ref _disposed, 1);
    }

    /// <summary>
    /// Records a matching success event.
    /// </summary>
    private void RecordSuccess()
    {
        Interlocked.Increment(ref _successCount);
    }

    /// <summary>
    /// Receives structured logging events from the test logger factory.
    /// </summary>
    private sealed class ObserverLogger(
        Issue55ClickSuccessLogObserver owner) : ILogger
    {
        /// <summary>
        /// Begins a logging scope.
        /// </summary>
        public IDisposable BeginScope<TState>(TState state)
            where TState : notnull
        {
            return NoopScope.Instance;
        }

        /// <summary>
        /// Determines whether the specified log level is enabled.
        /// </summary>
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Information;
        }

        /// <summary>
        /// Examines structured log events without storing sensitive arguments.
        /// </summary>
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (logLevel != LogLevel.Information ||
                Volatile.Read(ref owner._disposed) != 0 ||
                state is not IEnumerable<KeyValuePair<string, object?>> properties)
            {
                return;
            }

            foreach (var property in properties)
            {
                if (property.Key == "{OriginalFormat}" &&
                    property.Value is string template &&
                    string.Equals(
                        template,
                        SuccessTemplate,
                        StringComparison.Ordinal))
                {
                    owner.RecordSuccess();
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Represents an empty logging scope.
    /// </summary>
    private sealed class NoopScope : IDisposable
    {
        /// <summary>
        /// Gets the shared empty scope.
        /// </summary>
        public static NoopScope Instance { get; } = new();

        /// <summary>
        /// Completes the empty scope.
        /// </summary>
        public void Dispose()
        {
        }
    }
}