using System.Text.Json.Serialization;

namespace OmniAppium.ConfigUtilityService.Models;

/// <summary>
/// Represents the base model for an automation job.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
[JsonDerivedType(typeof(ClickJob), typeDiscriminator: "Click")]
[JsonDerivedType(typeof(WaitJob), typeDiscriminator: "Wait")]
[JsonDerivedType(typeof(ScreenshotJob), typeDiscriminator: "TakeScreenShot")]
[JsonDerivedType(typeof(InputJob), typeDiscriminator: "Input")]
[JsonDerivedType(typeof(GeminiJob), typeDiscriminator: "Gemini")]
public abstract class Job
{
    /// <summary>
    /// Gets or sets the logical name of the automation job.
    /// </summary>
    public string? JobName { get; set; }

    /// <summary>
    /// Gets or sets the human-readable description of the automation job.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the execution timeout in milliseconds.
    /// </summary>
    public int Timeout { get; set; }
}