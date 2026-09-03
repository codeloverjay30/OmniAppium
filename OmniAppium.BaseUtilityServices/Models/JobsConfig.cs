using OmniAppium.ConfigUtilityService.Services;

namespace OmniAppium.ConfigUtilityService.Models;

/// <summary>
/// Represents the root configuration for an automation job sequence.
/// </summary>
public sealed class JobsConfig : IValidatable
{
    /// <summary>
    /// Gets or sets the ordered automation jobs.
    /// </summary>
    public List<Job>? Jobs { get; init; } = [];

    /// <summary>
    /// Validates the jobs configuration.
    /// </summary>
    /// <returns>
    /// A validation result that indicates whether the configuration is valid.
    /// </returns>
    public (bool IsValid, string ErrorMessage) Validate()
    {
        if (Jobs is null)
        {
            return (
                false,
                "Jobs configuration must contain a Jobs collection.");
        }

        if (Jobs.Count == 0)
        {
            return (
                false,
                "Jobs configuration must contain at least one job.");
        }

        for (var index = 0; index < Jobs.Count; index++)
        {
            if (Jobs[index] is null)
            {
                return (
                    false,
                    $"Jobs configuration contains a null job at index {index}.");
            }
        }

        return (true, string.Empty);
    }
}