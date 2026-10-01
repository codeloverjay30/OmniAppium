using OmniAppium.EngineUtilityServices.Services.OCR;

namespace OmniAppium.EngineUtilityServices.Workflows;

/// <summary>
/// Executes declarative game workflow steps by composing existing OCR
/// interaction and page-verification services.
/// </summary>
public sealed class GameWorkflowStepExecutor(
    IOcrClickService ocrClickService,
    IOcrPageVerificationService ocrPageVerificationService)
    : IGameWorkflowStepExecutor
{
    private readonly IOcrClickService _ocrClickService =
        ocrClickService ??
        throw new ArgumentNullException(nameof(ocrClickService));

    private readonly IOcrPageVerificationService
        _ocrPageVerificationService =
            ocrPageVerificationService ??
            throw new ArgumentNullException(
                nameof(ocrPageVerificationService));

    /// <inheritdoc/>
    public async Task ExecuteAsync(
        GameWorkflowStep step,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(step);

        Validate(step);

        cancellationToken.ThrowIfCancellationRequested();

        await _ocrClickService
            .ClickTextAsync(
                step.ClickText,
                step.ClickTimeout,
                cancellationToken)
            .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        await _ocrPageVerificationService
            .WaitForAllTextAsync(
                step.VerificationTexts,
                step.VerificationTimeout,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Validates the structural definition of a game workflow step.
    /// </summary>
    /// <param name="step">The workflow step to validate.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when required text values are missing.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when a timeout is not greater than zero.
    /// </exception>
    private static void Validate(GameWorkflowStep step)
    {
        if (string.IsNullOrWhiteSpace(step.ClickText))
        {
            throw new ArgumentException(
                "Workflow step ClickText must not be empty.",
                nameof(step));
        }

        if (step.VerificationTexts is null ||
            step.VerificationTexts.Count == 0)
        {
            throw new ArgumentException(
                "Workflow step must contain at least one verification text.",
                nameof(step));
        }

        foreach (string text in step.VerificationTexts)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Workflow verification text must not be empty.",
                    nameof(step));
            }
        }

        if (step.ClickTimeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(step),
                step.ClickTimeout,
                "Workflow click timeout must be greater than zero.");
        }

        if (step.VerificationTimeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(step),
                step.VerificationTimeout,
                "Workflow verification timeout must be greater than zero.");
        }
    }
}