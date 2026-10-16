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
        var mock = new Mock<IAtomicGroundedClickEvidenceProvider>(MockBehavior.Strict);
        mock.Setup(x => x.Consume(
                It.IsAny<OcrResult>(), It.IsAny<string>(), It.IsAny<OcrTextMatch>()))
            .Returns(new ConsumedGroundedClickEvidence(null!, 1.0f));
        mock.Setup(x => x.Consume(
                It.IsAny<OcrGroundedSnapshot>(), It.IsAny<OcrGroundedTarget>()))
            .Returns(new ConsumedGroundedClickEvidence(null!, 1.0f));
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