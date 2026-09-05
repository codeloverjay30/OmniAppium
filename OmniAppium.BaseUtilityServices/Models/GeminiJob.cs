using OmniAppium.ConfigUtilityService.Services;

namespace OmniAppium.ConfigUtilityService.Models;

/// <summary>
/// Represents an AI-driven automation job executed by Gemini.
/// </summary>
public sealed class GeminiJob : Job, IValidatable
{
    /// <summary>
    /// Gets or sets the logical user task supplied to the AI execution session.
    /// </summary>
    public string? UserTask { get; set; }

    /// <summary>
    /// Gets or sets the natural-language instruction supplied to the AI.
    /// </summary>
    public string? Prompt { get; set; }

    /// <summary>
    /// Validates the Gemini automation job.
    /// </summary>
    /// <returns>
    /// A validation result that indicates whether the job is valid.
    /// </returns>
    public (bool IsValid, string ErrorMessage) Validate()
    {
        if (string.IsNullOrWhiteSpace(UserTask))
        {
            return (
                false,
                "Gemini job must contain a non-empty UserTask.");
        }

        if (string.IsNullOrWhiteSpace(Prompt))
        {
            return (
                false,
                "Gemini job must contain a non-empty Prompt.");
        }

        if (Timeout < 0)
        {
            return (
                false,
                "Gemini job timeout must not be negative.");
        }

        return (true, string.Empty);
    }
}