using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityServices.Services.AiSecurity;

/// <summary>
/// Enforces an allowlist for AI-exposed click tools independently of registry order.
/// </summary>
public sealed class AiClickToolRegistrationGate
{
    private readonly AiClickExecutionMode _mode;

    /// <summary>
    /// Initializes a validated AI click registration boundary.
    /// </summary>
    public AiClickToolRegistrationGate(AiClickExecutionMode mode, bool isProduction)
    {
        AiClickExecutionPolicy.Validate(mode, isProduction);
        _mode = mode;
    }

    /// <summary>
    /// Rejects click tools that are not authorized for the selected AI mode.
    /// </summary>
    public void EnsureAllowed(Type toolType)
    {
        ArgumentNullException.ThrowIfNull(toolType);
        if (toolType == typeof(ClickService) && _mode != AiClickExecutionMode.Direct)
            throw new InvalidOperationException("Direct AI click tool is prohibited in Protected mode.");
        if (toolType == typeof(OcrGroundedPlannerClickTool) && _mode != AiClickExecutionMode.Protected)
            throw new InvalidOperationException("Protected AI planner tool is prohibited in Direct mode.");
    }
}
