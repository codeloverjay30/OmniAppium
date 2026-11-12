using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Exploration;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59TemporalIntegrationRegressionTests
{
    [Fact]
    public void Analyze_WithTwoFollowUpFrames_InvokesNotificationEvidenceFilter()
    {
        var decoder = new Mock<IRedDotFrameDecoder>(MockBehavior.Strict);
        var associator = new Mock<INotificationNodeAssociator>(MockBehavior.Strict);
        var filter = new Mock<INotificationEvidenceFilter>(MockBehavior.Strict);
        var templateMatcher = new Mock<INotificationTemplateMatcher>(MockBehavior.Strict);
        templateMatcher
            .Setup(m => m.Match(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<NotificationTemplateMatchOptions?>()))
            .Returns(Array.Empty<NotificationTemplateMatch>());
        var baseline = CreateObservation(1);
        var next1 = CreateObservation(2);
        var next2 = CreateObservation(3);
        decoder.Setup(d => d.Decode(It.IsAny<ReadOnlyMemory<byte>>()))
            .Returns(new RedDotFrame(8, 8, new byte[8 * 8 * 3]));
        filter.Setup(f => f.Filter(
                It.IsAny<IReadOnlyList<RedDotEvidence>>(),
                It.Is<IReadOnlyList<IReadOnlyList<RedDotEvidence>>>(frames => frames.Count == 2),
                8, 8,
                It.Is<NotificationEvidenceFilterOptions>(o => o.MinimumSubsequentFrames == 2)))
            .Returns(Array.Empty<RedDotEvidence>());
        associator.Setup(a => a.Associate("capture-1",
                It.IsAny<IReadOnlyList<OmniAppium.EngineUtilityServices.Services.Grounding.OcrGroundedTarget>>(),
                It.IsAny<IReadOnlyList<RedDotEvidence>>()))
            .Returns(Array.Empty<GameUiNode>());
        var service = new RedDotObservationIntegrationService(
            decoder.Object, new RedDotDetector(), associator.Object, filter.Object,
            templateMatcher.Object);
        var input = new RedDotObservationInput("capture-1", baseline, [])
        {
            NotificationTemplate = new byte[] { 1 },
            SubsequentObservations = [next1, next2]
        };

        var result = service.Analyze(input);

        result.Evidence.Should().BeEmpty();
        result.FilteringStatistics.Should().NotBeNull();
        result.FilteringStatistics!.RawCandidates.Should().BeGreaterThanOrEqualTo(0);
        result.FilteringStatistics.FilteredCandidates.Should().Be(0);
        result.FilteringStatistics.RejectedCandidates.Should()
            .Be(result.FilteringStatistics.RawCandidates);
        result.FilteringStatistics.SubsequentFrameCount.Should().Be(2);
        result.FilteringStatistics.AssociatedNodes.Should().Be(0);
        decoder.Verify(d => d.Decode(It.IsAny<ReadOnlyMemory<byte>>()), Times.Exactly(3));
        filter.VerifyAll();
        associator.VerifyAll();
        templateMatcher.Verify(m => m.Match(
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<NotificationTemplateMatchOptions?>()), Times.Exactly(3));
    }

    [Fact]
    public void Analyze_WithMismatchedFrameDimensions_FailsClosedBeforeAssociation()
    {
        var decoder = new Mock<IRedDotFrameDecoder>(MockBehavior.Strict);
        var associator = new Mock<INotificationNodeAssociator>(MockBehavior.Strict);
        var filter = new Mock<INotificationEvidenceFilter>(MockBehavior.Strict);
        var templateMatcher = new Mock<INotificationTemplateMatcher>(MockBehavior.Strict);
        templateMatcher
            .Setup(m => m.Match(
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<NotificationTemplateMatchOptions?>()))
            .Returns(Array.Empty<NotificationTemplateMatch>());
        decoder.SetupSequence(d => d.Decode(It.IsAny<ReadOnlyMemory<byte>>()))
            .Returns(new RedDotFrame(8, 8, new byte[8 * 8 * 3]))
            .Returns(new RedDotFrame(9, 8, new byte[9 * 8 * 3]));
        var service = new RedDotObservationIntegrationService(
            decoder.Object, new RedDotDetector(), associator.Object, filter.Object,
            templateMatcher.Object);
        var input = new RedDotObservationInput("capture-1", CreateObservation(1), [])
        {
            NotificationTemplate = new byte[] { 1 },
            SubsequentObservations = [CreateObservation(2), CreateObservation(3)]
        };

        Action act = () => service.Analyze(input);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Subsequent observation dimensions must match baseline*");
        associator.VerifyNoOtherCalls();
        filter.VerifyNoOtherCalls();
        templateMatcher.Verify(m => m.Match(
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<ReadOnlyMemory<byte>>(),
            It.IsAny<NotificationTemplateMatchOptions?>()), Times.Once);
    }

    [Fact]
    public void CapturedOriginalAndAnnotatedFixtures_DecodeToSameDimensions()
    {
        byte[] original = ReadFixture("original.png");
        byte[] annotated = ReadFixture("red-dot-annotated.png");
        var decoder = new OpenCvRedDotFrameDecoder();

        RedDotFrame originalFrame = decoder.Decode(original);
        RedDotFrame annotatedFrame = decoder.Decode(annotated);
        IReadOnlyList<RedDotEvidence> evidence = new RedDotDetector()
            .Detect(originalFrame, []);

        originalFrame.Width.Should().Be(2340);
        originalFrame.Height.Should().Be(1080);
        annotatedFrame.Width.Should().Be(originalFrame.Width);
        annotatedFrame.Height.Should().Be(originalFrame.Height);
        evidence.Should().NotBeEmpty();
    }

    private static IAndroidScreenObservation CreateObservation(byte value)
    {
        var mock = new Mock<IAndroidScreenObservation>(MockBehavior.Strict);
        mock.SetupGet(o => o.ImageBytes).Returns(new ReadOnlyMemory<byte>([value]));
        return mock.Object;
    }

    private static byte[] ReadFixture(string fileName)
    {
        var assembly = typeof(Issue59TemporalIntegrationRegressionTests).Assembly;
        string resourceName = $"Issue59Fixtures.{fileName}";
        using Stream stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"Embedded Issue 59 fixture '{resourceName}' is missing. Available: " +
                string.Join(", ", assembly.GetManifestResourceNames()));
        using var buffer = new MemoryStream();
        stream.CopyTo(buffer);
        return buffer.ToArray();
    }
}
