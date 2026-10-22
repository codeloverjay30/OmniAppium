namespace OmniAppiumDemo.SmokeTests.Issue55;

/// <summary>
/// Represents the outcome of a controlled Issue 55 dispatch failure smoke test.
/// </summary>
public sealed record Issue55SmokeResult(
    Issue55SmokeStatus Status,
    string Reason,
    int ActionRequests,
    int InjectedFailures,
    int ForwardedActionRequests,
    string? ExceptionType = null)
{
    /// <summary>
    /// Gets whether every smoke-test invariant was satisfied.
    /// </summary>
    public bool Passed => Status == Issue55SmokeStatus.Passed;
}

/// <summary>
/// Defines the possible Issue 55 smoke-test outcomes.
/// </summary>
public enum Issue55SmokeStatus
{
    NotRun,
    Failed,
    Passed
}
