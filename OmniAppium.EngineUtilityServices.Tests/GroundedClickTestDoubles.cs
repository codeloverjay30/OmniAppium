using Moq;
using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests;

/// <summary>
/// Provides isolated authorization doubles for legacy behavior tests.
/// These doubles do not establish production authorization correctness.
/// </summary>
internal static class GroundedClickTestDoubles
{
    /// <summary>
    /// Creates a strict evidence provider for tests that exercise other behavior.
    /// </summary>
    internal static IGroundedClickEvidenceProvider Evidence()
    {
        var mock = new Mock<IGroundedClickEvidenceProvider>(MockBehavior.Strict);
        mock.Setup(x => x.Resolve(It.IsAny<OcrResult>(), It.IsAny<string>()))
            .Returns((ReviewedGroundTruth)null!);
        mock.Setup(x => x.ResolveConfidence(It.IsAny<OcrResult>(), It.IsAny<OcrTextMatch>()))
            .Returns(1.0f);
        mock.Setup(x => x.Resolve(It.IsAny<OcrGroundedSnapshot>(), It.IsAny<OcrGroundedTarget>()))
            .Returns((ReviewedGroundTruth)null!);
        mock.Setup(x => x.ResolveConfidence(It.IsAny<OcrGroundedSnapshot>(), It.IsAny<OcrGroundedTarget>()))
            .Returns(1.0f);
        return mock.Object;
    }

    /// <summary>
    /// Creates a strict authorization double that returns the matched center.
    /// </summary>
    internal static IGroundedClickAuthorizationService Authorization()
    {
        var mock = new Mock<IGroundedClickAuthorizationService>(MockBehavior.Strict);
        mock.Setup(x => x.Authorize(
                It.IsAny<string>(), It.IsAny<float>(),
                It.IsAny<CoordinateUtilityServices.Rectangle>(),
                It.IsAny<ReviewedGroundTruth>()))
            .Returns((string _, float _, CoordinateUtilityServices.Rectangle bounds, ReviewedGroundTruth _) =>
                (bounds.Center.X, bounds.Center.Y));
        return mock.Object;
    }
}
