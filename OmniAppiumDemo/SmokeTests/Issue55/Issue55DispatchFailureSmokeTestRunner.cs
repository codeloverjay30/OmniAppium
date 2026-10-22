using OpenQA.Selenium;

namespace OmniAppiumDemo.SmokeTests.Issue55;

/// <summary>
/// Verifies that a real ClickService call propagates one injected W3C dispatch failure.
/// </summary>
public sealed class Issue55DispatchFailureSmokeTestRunner
{
    /// <summary>
    /// Runs the diagnostic against an already-established Appium session routed through the proxy.
    /// The caller must pass the real ClickService.TapAt or ClickAbsolute operation, not a stub.
    /// </summary>
    /// <param name="proxy">The running proxy used by the driver's command executor.</param>
    /// <param name="sessionId">The established real Appium session identifier.</param>
    /// <param name="tap">The real ClickService operation, invoked exactly once.</param>
    /// <param name="confirmed">Whether the operator explicitly approved this diagnostic.</param>
    /// <param name="successLogObserved">Optional observer for the ClickService success log, scoped to this operation.</param>
    /// <returns>The diagnostic outcome.</returns>
    public Issue55SmokeResult Run(
        Issue55AppiumFaultProxy proxy,
        string sessionId,
        Action tap,
        bool confirmed,
        Func<bool>? successLogObserved = null)
    {
        ArgumentNullException.ThrowIfNull(proxy);
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);
        ArgumentNullException.ThrowIfNull(tap);

        if (!confirmed)
            return new(Issue55SmokeStatus.NotRun, "Explicit operator confirmation is required.", 0, 0, 0);

        if (!proxy.IsRunning)
            return new(Issue55SmokeStatus.NotRun, "The proxy is not running.", 0, 0, 0);

        if (proxy.ActionRequests != 0)
            return new(Issue55SmokeStatus.NotRun, "Proxy counters are not clean.", proxy.ActionRequests,
                proxy.InjectedFailures, proxy.ForwardedActionRequests);

        try
        {
            proxy.Arm(sessionId);
        }
        catch (InvalidOperationException ex)
        {
            return new(Issue55SmokeStatus.NotRun, ex.Message, proxy.ActionRequests,
                proxy.InjectedFailures, proxy.ForwardedActionRequests);
        }

        WebDriverException? observed = null;
        Exception? unexpected = null;
        try
        {
            tap();
        }
        catch (WebDriverException ex)
        {
            observed = ex;
        }
        catch (Exception ex)
        {
            unexpected = ex;
        }
        finally
        {
            proxy.Disarm();
        }

        var actions = proxy.ActionRequests;
        var injected = proxy.InjectedFailures;
        var forwarded = proxy.ForwardedActionRequests;

        if (unexpected is not null)
            return new(Issue55SmokeStatus.Failed, $"Unexpected exception: {unexpected.GetType().Name}.",
                actions, injected, forwarded, unexpected.GetType().FullName);

        if (observed is null)
            return new(Issue55SmokeStatus.Failed, "ClickService returned without propagating WebDriverException.",
                actions, injected, forwarded);

        if (!observed.Message.Contains(Issue55AppiumFaultProxy.FailureMarker, StringComparison.Ordinal))
            return new(Issue55SmokeStatus.Failed, "WebDriverException did not preserve the injected error marker.",
                actions, injected, forwarded, observed.GetType().FullName);

        if (actions != 1 || injected != 1 || forwarded != 0)
            return new(Issue55SmokeStatus.Failed, "Dispatch count or no-forwarding invariant failed.",
                actions, injected, forwarded, observed.GetType().FullName);

        if (successLogObserved is null)
            return new(Issue55SmokeStatus.NotRun,
                "Success-log observation was not configured; false-success validation is incomplete.",
                actions, injected, forwarded, observed.GetType().FullName);

        bool successLogged;
        try { successLogged = successLogObserved(); }
        catch (Exception ex)
        {
            return new(Issue55SmokeStatus.Failed, $"Success-log observer failed: {ex.GetType().Name}.",
                actions, injected, forwarded, observed.GetType().FullName);
        }

        if (successLogged)
            return new(Issue55SmokeStatus.Failed, "ClickService emitted a success log after dispatch failure.",
                actions, injected, forwarded, observed.GetType().FullName);

        return new(Issue55SmokeStatus.Passed, "Original exception propagated; no success log; one rejected dispatch; no forwarding.",
            actions, injected, forwarded, observed.GetType().FullName);
    }
}
