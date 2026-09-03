namespace OmniAppium.ConfigUtilityService.Models;

/// <summary>
/// Represents the configuration root for an automation job sequence.
/// </summary>
public sealed class JobsConfig
{
    /// <summary>
    /// Gets the ordered automation jobs.
    /// </summary>
    public List<Job> Jobs { get; init; } = [];
}
