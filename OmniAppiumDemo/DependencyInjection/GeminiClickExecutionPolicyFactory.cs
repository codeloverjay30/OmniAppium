using System.Reflection;
using AiUtility.GeminiKits.Attributes;
using AiUtility.GeminiKits.Security;
using OmniAppium.EngineUtilityService.Services.Wait;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Creates the runtime authorization policy for AI-exposed device tools.
/// </summary>
public static class GeminiClickExecutionPolicyFactory
{
    /// <summary>
    /// Creates a method-identity allowlist for the selected execution mode.
    /// </summary>
    public static GeminiToolExecutionPolicy Create(AiClickExecutionMode mode, bool isProduction)
    {
        AiClickExecutionPolicy.Validate(mode, isProduction);

        Type clickToolType = mode switch
        {
            AiClickExecutionMode.Protected => typeof(OcrGroundedPlannerClickTool),
            AiClickExecutionMode.Direct => typeof(ClickService),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unsupported AI click execution mode.")
        };

        MethodInfo[] allowedMethods = new[] { clickToolType, typeof(WaitService) }
            .SelectMany(static type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            .Where(static method => method.IsDefined(typeof(GeminiToolAttribute), inherit: true))
            .ToArray();

        if (!allowedMethods.Any(method => method.DeclaringType == clickToolType))
        {
            throw new InvalidOperationException($"No Gemini click method was found on '{clickToolType.FullName}'.");
        }

        return GeminiToolExecutionPolicy.Protected(allowedMethods);
    }
}
