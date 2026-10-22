using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Appium.Android;

namespace OmniAppiumDemo.SmokeTests.Issue55;

/// <summary>
/// Coordinates a real Appium session and a controlled Issue 55 dispatch failure test.
/// </summary>
public static class Issue55SmokeBootstrapper
{
    /// <summary>
    /// Runs the Issue 55 smoke test with an Appium driver connected through the fault proxy.
    /// </summary>
    /// <param name="upstreamEndpoint">
    /// The real local Appium server endpoint.
    /// </param>
    /// <param name="createDriver">
    /// Creates a real AndroidDriver using the supplied proxy endpoint.
    /// </param>
    /// <param name="createTapAction">
    /// Creates a delegate that invokes the production ClickService.
    /// </param>
    /// <param name="successLogObserved">
    /// Observes whether ClickService emitted a success log during the test operation.
    /// </param>
    /// <param name="confirmed">
    /// Indicates whether the operator explicitly confirmed the diagnostic.
    /// </param>
    /// <returns>The Issue 55 smoke test outcome.</returns>
    public static async Task<Issue55SmokeResult> RunIssue55SmokeAsync(
        Uri upstreamEndpoint,
        Func<Uri, AndroidDriver> createDriver,
        Func<AndroidDriver, Action> createTapAction,
        Func<bool> successLogObserved,
        bool confirmed,
        ILogger<Issue55AppiumFaultProxy> proxyLogger
    )
    {
        ArgumentNullException.ThrowIfNull(upstreamEndpoint);
        ArgumentNullException.ThrowIfNull(createDriver);
        ArgumentNullException.ThrowIfNull(createTapAction);
        ArgumentNullException.ThrowIfNull(successLogObserved);
        ArgumentNullException.ThrowIfNull(proxyLogger);

        if (!confirmed)
        {
            return new Issue55SmokeResult(
                Issue55SmokeStatus.NotRun,
                "Operator confirmation was not provided.",
                0,
                0,
                0);
        }

        await using var proxy =
            new Issue55AppiumFaultProxy(
                upstreamEndpoint,
                proxyLogger);

        AndroidDriver? driver = null;

        try
        {
            await proxy.StartAsync().ConfigureAwait(false);

            Uri proxyEndpoint = proxy.Endpoint
                ?? throw new InvalidOperationException(
                    "Issue55 proxy endpoint is unavailable.");

            // The driver must connect to the proxy, not directly to Appium.
            driver = createDriver(proxyEndpoint);

            ArgumentNullException.ThrowIfNull(driver);

            string sessionId = driver.SessionId?.ToString()
                ?? throw new InvalidOperationException(
                    "Appium session was not established.");

            Action tap = createTapAction(driver);

            ArgumentNullException.ThrowIfNull(tap);

            var runner =
                new Issue55DispatchFailureSmokeTestRunner();

            return runner.Run(
                proxy,
                sessionId,
                tap,
                confirmed: true,
                successLogObserved: successLogObserved);
        }
        catch (Exception ex)
        {
            return new Issue55SmokeResult(
                Issue55SmokeStatus.NotRun,
                $"Smoke setup failed: {ex.GetType().Name}: {ex.Message}",
                proxy.ActionRequests,
                proxy.InjectedFailures,
                proxy.ForwardedActionRequests,
                ex.GetType().FullName);
        }
        finally
        {
            driver?.Dispose();
        }
    }
}