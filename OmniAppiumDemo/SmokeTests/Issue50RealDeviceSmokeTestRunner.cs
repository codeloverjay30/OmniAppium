using System.Globalization;
using OpenQA.Selenium;
using AiUtility.GeminiKits.Executor;
using AiUtility.GeminiKits.Registry;
using ExpressionTreeUtilityServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniAppium.ConfigUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppiumDemo.DependencyInjection;
using ReflectionUtilityServices;
using TypeUtilityServices;

namespace OmniAppiumDemo.SmokeTests;

/// <summary>
/// Runs explicitly requested Issue 50 device smoke scenarios without weakening production registrations.
/// </summary>
public static class Issue50RealDeviceSmokeTestRunner
{
    /// <summary>
    /// Executes a selected smoke scenario. The caller must invoke this before resolving legacy workflow services.
    /// </summary>
    public static async Task<int> RunAsync(
        string[] args,
        ClickService clickService,
        IServiceProvider productionServices,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(args);
        ArgumentNullException.ThrowIfNull(clickService);
        ArgumentNullException.ThrowIfNull(productionServices);
        ArgumentNullException.ThrowIfNull(logger);

        int optionIndex = Array.IndexOf(args, "--issue50-smoke");
        if (optionIndex < 0 || optionIndex + 1 >= args.Length)
            throw new ArgumentException("Specify --issue50-smoke direct|direct-coordinate|protected-valid|protected-invalid.", nameof(args));

        string scenario = args[optionIndex + 1];
        switch (scenario)
        {
            case "protected-invalid":
            {
                var guardedClick = productionServices.GetRequiredService<IOcrGroundedClickService>();
                var snapshot = new OcrGroundedSnapshot(
                    "issue50-invalid-evidence",
                    Array.Empty<OcrGroundedTarget>());
                try
                {
                    guardedClick.Click(snapshot, "missing-target");
                    logger.LogError("FAIL: production grounded click unexpectedly accepted invalid evidence.");
                    return 1;
                }
                catch (InvalidOperationException ex) when (
                    ex.Message.Contains("Trusted signed review approval is required", StringComparison.Ordinal))
                {
                    logger.LogInformation("PASS: production rejected unauthorized grounded click. Category=AuthorizationOrClaim.");
                    logger.LogWarning("Device zero-tap verification requires independent Appium/device event monitoring.");
                    return 0;
                }
            }
            case "protected-valid":
            {
                if (!args.Contains("--issue54-protected-confirmed", StringComparer.Ordinal))
                {
                    logger.LogWarning("NOT RUN: protected-valid requires --issue54-protected-confirmed.");
                    return 2;
                }

                // Evidence must come from an independently trusted review workflow.
                // Never create or sign approvals in the smoke runner.
                TrustedGroundedClickRequest? request =
                    productionServices.GetService<TrustedGroundedClickRequest>();
                ITrustedGroundedClickService? trustedClick =
                    productionServices.GetService<ITrustedGroundedClickService>();

                if (request is null || trustedClick is null)
                {
                    logger.LogWarning(
                        "NOT RUN: signed approval request or trusted click service is not provisioned.");
                    return 2;
                }

                if (Console.IsInputRedirected)
                {
                    logger.LogWarning("NOT RUN: interactive operator confirmation is required.");
                    return 2;
                }

                Console.Write("Type ISSUE54-PROTECTED-TAP to authorize one protected device tap: ");
                if (!string.Equals(
                    Console.ReadLine(), "ISSUE54-PROTECTED-TAP", StringComparison.Ordinal))
                {
                    logger.LogWarning("NOT RUN: operator did not confirm protected device input.");
                    return 2;
                }

                // A completed Appium call is not proof of physical screen state change.
                TrustedClickExecutionOutcome first =
                    await trustedClick.ExecuteWithOutcomeAsync(request, cancellationToken)
                        .ConfigureAwait(false);

                logger.LogInformation(
                    "Protected first attempt: Status={Status}; FailureCategory={Category}; DispatchAttempted={Attempted}",
                    first.Status, first.FailureCategory, first.DispatchAttempted);

                if (first.Status != TrustedClickExecutionStatus.DispatchCompleted ||
                    !first.DispatchAttempted)
                {
                    logger.LogError(
                        "FAILED: protected execution was not completed. Do not retry ambiguous dispatch.");
                    return 1;
                }

                // The same approval must never cause a second dispatch.
                // Freshness may reject a changed screen before the claim store is reached.
                TrustedClickExecutionOutcome replay =
                    await trustedClick.ExecuteWithOutcomeAsync(request, cancellationToken)
                        .ConfigureAwait(false);

                logger.LogInformation(
                    "Protected replay: Status={Status}; FailureCategory={Category}; DispatchAttempted={Attempted}",
                    replay.Status, replay.FailureCategory, replay.DispatchAttempted);

                if (replay.DispatchAttempted)
                {
                    logger.LogError("FAILED: replay attempted a second device dispatch.");
                    return 1;
                }

                if (replay.Status != TrustedClickExecutionStatus.RejectedAlreadyConsumed)
                {
                    logger.LogWarning(
                        "INCONCLUSIVE: replay did not dispatch, but claim rejection was not observable. " +
                        "A changed screen may fail freshness before reaching the claim store.");
                    return 2;
                }

                logger.LogInformation(
                    "PASS: protected dispatch completed and replay was rejected. " +
                    "Verify physical tap count and target response independently.");
                return 0;
            }

            case "direct-coordinate":
            {
                if (!args.Contains("--issue50-development-confirmed", StringComparer.Ordinal))
                    throw new InvalidOperationException(
                        "Direct coordinate smoke requires --issue50-development-confirmed.");

                int x = ReadCoordinate(args, "--tap-x");
                int y = ReadCoordinate(args, "--tap-y");
                var size = clickService.Driver.Manage().Window.Size;

                if (x < 0 || x >= size.Width || y < 0 || y >= size.Height)
                    throw new ArgumentOutOfRangeException(
                        nameof(args),
                        $"Absolute tap ({x},{y}) must be within {size.Width}x{size.Height}.");

                logger.LogWarning(
                    "DEVELOPMENT ONLY. Absolute device tap at ({X},{Y}); viewport {Width}x{Height}. " +
                    "This bypasses Gemini tool selection for coordinate diagnostics only.",
                    x, y, size.Width, size.Height);

                Console.Write($"Type ISSUE50-TAP-{x}-{y} to authorize exactly one tap: ");
                if (!string.Equals(
                        Console.ReadLine(),
                        $"ISSUE50-TAP-{x}-{y}",
                        StringComparison.Ordinal))
                {
                    logger.LogWarning("CANCELLED: coordinate tap was not authorized.");
                    return 2;
                }

                var before = clickService.Driver.GetScreenshot().AsByteArray;
                logger.LogInformation(
                    "BEFORE: screenshot SHA256={Hash}; bytes={Length}; viewport={Width}x{Height}",
                    Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(before)),
                    before.Length, size.Width, size.Height);

                clickService.ClickAbsolute(x, y);
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

                var after = clickService.Driver.GetScreenshot().AsByteArray;
                bool changed = !before.AsSpan().SequenceEqual(after);
                logger.LogInformation(
                    "AFTER: screenshot SHA256={Hash}; bytes={Length}; screenshotChanged={Changed}",
                    Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(after)),
                    after.Length, changed);
                logger.LogWarning(
                    "INCONCLUSIVE: screenshot differences can be animation; ClickService may swallow " +
                    "Appium failures. Confirm actual target response and exactly one tap independently.");
                return 0;
            }

            case "direct":
            {
                if (!args.Contains("--issue50-development-confirmed", StringComparer.Ordinal))
                    throw new InvalidOperationException(
                        "Direct smoke requires explicit --issue50-development-confirmed; do not use in production.");

                logger.LogWarning(
                    "DEVELOPMENT ONLY. The current Click(Target) implementation taps the fixed diagnostic " +
                    "position (584,884) in reference coordinates, subject to device scaling. " +
                    "Use only a safe disposable test screen.");
                Console.Write("Type ISSUE50-DIRECT-TAP to authorize one physical tap: ");
                string? confirmation = Console.ReadLine();
                if (!string.Equals(confirmation, "ISSUE50-DIRECT-TAP", StringComparison.Ordinal))
                {
                    logger.LogWarning("CANCELLED: operator did not authorize device input.");
                    return 2;
                }

                var registry = new GeminiToolRegistry(
                    new ReflectionUtilityService(new ExpressionTreeUtilityService()));
                registry.RegisterAiClickTools(
                    AiClickExecutionMode.Direct,
                    isProduction: false,
                    () => clickService,
                    () => throw new InvalidOperationException("Protected planner is unavailable in Direct mode."));

                var executor = new GeminiToolExecutor(
                    registry,
                    new TypeUtilityService(),
                    GeminiClickExecutionPolicyFactory.Create(
                        AiClickExecutionMode.Direct, isProduction: false));

                await executor.ExecuteAsync(
                    nameof(ClickService.Click),
                    new Dictionary<string, object>
                    {
                        ["target"] = new ByAccessibilityId
                        {
                            Description = "Issue 50 operator-approved development device smoke",
                            Value = "issue50-diagnostic-target"
                        }
                    },
                    cancellationToken);

                logger.LogInformation(
                    "EXECUTED: Direct tool invocation returned. Verify exactly one tap using " +
                    "device/Appium logs; ClickService currently swallows underlying tap exceptions.");
                return 0;
            }
            default:
                throw new ArgumentException(
                    $"Unsupported Issue 50 smoke scenario: {scenario}", nameof(args));
        }
    }

    /// <summary>
    /// Reads a required nonnegative integer command-line coordinate.
    /// </summary>
    private static int ReadCoordinate(string[] args, string name)
    {
        int index = Array.IndexOf(args, name);
        if (index < 0 || index + 1 >= args.Length ||
            !int.TryParse(args[index + 1], NumberStyles.None,
                CultureInfo.InvariantCulture, out int value))
        {
            throw new ArgumentException(
                $"Specify {name} followed by a nonnegative integer.", nameof(args));
        }

        return value;
    }
}