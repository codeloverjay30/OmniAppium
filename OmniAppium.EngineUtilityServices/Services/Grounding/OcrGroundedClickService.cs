using CoordinateUtilityServices;
using OmniAppium.EngineUtilityService.Services.Click;

namespace OmniAppium.EngineUtilityServices.Services.Grounding;

/// <summary>
/// Executes validated clicks against OCR-grounded targets.
/// </summary>
public sealed class OcrGroundedClickService
    : IOcrGroundedClickService
{
    private readonly IClickService _clickService;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="OcrGroundedClickService"/> class.
    /// </summary>
    /// <param name="clickService">
    /// The deterministic click service used to execute device clicks.
    /// </param>
    public OcrGroundedClickService(
        IClickService clickService)
    {
        ArgumentNullException.ThrowIfNull(clickService);

        _clickService = clickService;
    }

    /// <inheritdoc/>
    public void Click(
        OcrGroundedSnapshot snapshot,
        string targetId)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);

        if (snapshot.Targets is null)
        {
            throw new ArgumentException(
                "The OCR-grounded snapshot must contain a non-null target collection.",
                nameof(snapshot));
        }

        OcrGroundedTarget[] matches =
            snapshot.Targets
                .Where(
                    target =>
                        string.Equals(
                            target.Id,
                            targetId,
                            StringComparison.Ordinal))
                .Take(2)
                .ToArray();

        if (matches.Length == 0)
        {
            throw new InvalidOperationException(
                $"OCR-grounded target '{targetId}' was not found.");
        }

        if (matches.Length > 1)
        {
            throw new InvalidOperationException(
                $"OCR-grounded target '{targetId}' is ambiguous.");
        }

        OcrGroundedTarget target = matches[0];
        Rectangle bounds = target.Bounds;

        if (bounds.Width <= 0 ||
            bounds.Height <= 0)
        {
            throw new InvalidOperationException(
                $"OCR-grounded target '{targetId}' has zero-area bounds.");
        }

        Point center = bounds.Center;

        _clickService.ClickAbsolute(
            center.X,
            center.Y);
    }
}