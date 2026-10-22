using AiUtility.GeminiKits.Abstractions;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Registers only the AI click tool authorized for the selected execution mode.
/// </summary>
public static class GeminiClickToolRegistryExtensions
{
    /// <summary>
    /// Validates the runtime policy before registering any AI click tool.
    /// </summary>
    public static void RegisterAiClickTools(
        this IGeminiToolRegistry registry,
        AiClickExecutionMode mode,
        bool isProduction,
        Func<ClickService> directClickFactory,
        Func<OcrGroundedPlannerClickTool> protectedClickFactory)
    {
        ArgumentNullException.ThrowIfNull(registry);
        ArgumentNullException.ThrowIfNull(directClickFactory);
        ArgumentNullException.ThrowIfNull(protectedClickFactory);

        var gate = new AiClickToolRegistrationGate(mode, isProduction);

        switch (mode)
        {
            case AiClickExecutionMode.Protected:
                gate.EnsureAllowed(typeof(OcrGroundedPlannerClickTool));
                registry.Register<OcrGroundedPlannerClickTool>(protectedClickFactory);
                break;
            case AiClickExecutionMode.Direct:
                gate.EnsureAllowed(typeof(ClickService));
                registry.Register<ClickService>(directClickFactory);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode,
                    "Unsupported AI click execution mode.");
        }
    }
}
