namespace OmniAppium.EngineUtilityServices.Workflows;

/// <summary>
/// Defines a declarative game workflow step that clicks visible text and
/// verifies the resulting page by using OCR markers.
/// </summary>
public sealed record GameWorkflowStep(
    string ClickText,
    IReadOnlyCollection<string> VerificationTexts,
    TimeSpan ClickTimeout,
    TimeSpan VerificationTimeout);