namespace OmniAppium.EngineUtilityServices.Services.AiSecurity;

/// <summary>
/// Validates AI click execution configuration before application startup.
/// </summary>
public static class AiClickExecutionPolicy
{
    /// <summary>
    /// Rejects unsupported execution modes and Direct AI in production.
    /// </summary>
    public static void Validate(AiClickExecutionMode mode, bool isProduction)
    {
        if (!Enum.IsDefined(mode))
        {
            throw new ArgumentOutOfRangeException(nameof(mode), mode,
                "Unsupported AI click execution mode.");
        }

        if (isProduction && mode == AiClickExecutionMode.Direct)
        {
            throw new InvalidOperationException(
                "Direct AI click execution is prohibited in Production.");
        }
    }
}
