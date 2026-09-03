using OmniAppium.ConfigUtilityService.Models;

namespace OmniAppium.ConfigUtilityService.Validators;

/// <summary>
/// Validates the jobs configuration and ensures that its required invariants are satisfied.
/// </summary>
public sealed class JobsConfigValidator(
    JobsConfig config
) : IConfigValidator
{
    private readonly JobsConfig _config =
        config ?? throw new ArgumentNullException(nameof(config));

    /// <summary>
    /// Ensures that the jobs configuration is valid.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the jobs configuration violates a required invariant.
    /// </exception>
    public void EnsureConfig()
    {
        if (_config.Jobs is null)
        {
            throw new InvalidOperationException(
                "Jobs configuration must contain a Jobs collection.");
        }

        if (_config.Jobs.Count == 0)
        {
            throw new InvalidOperationException(
                "Jobs configuration must contain at least one job.");
        }

        for (var index = 0; index < _config.Jobs.Count; index++)
        {
            if (_config.Jobs[index] is null)
            {
                throw new InvalidOperationException(
                    $"Jobs configuration contains a null job at index {index}.");
            }
        }
    }
}