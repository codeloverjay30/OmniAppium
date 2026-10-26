using AiUtility.ToolKits.Abstractions;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Planner;

public sealed class OcrGroundedPlannerClickToolTests
{
    [Fact]
    public async Task Click_WhenApproved_ShouldPassExactSnapshotAndTargetIdToTrustedBoundary()
    {
        OcrGroundedSnapshot snapshot = CreateSnapshot("snapshot-A");
        const string targetId = "ocr-0";
        TrustedGroundedClickRequest approved = CreateApprovedRequest(snapshot, targetId);
        var state = ActiveState(snapshot);
        var provider = new Mock<IPlannerTrustedClickRequestProvider>(MockBehavior.Strict);
        var trustedClick = new Mock<ITrustedGroundedClickService>(MockBehavior.Strict);
        provider.Setup(x => x.GetApprovedRequestAsync(
                It.Is<OcrGroundedSnapshot>(s => ReferenceEquals(s, snapshot)),
                targetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(approved);
        trustedClick.Setup(x => x.ClickAsync(
                It.Is<TrustedGroundedClickRequest>(r => ReferenceEquals(r, approved)),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        OcrGroundedPlannerClickTool sut = CreateTool(state, provider, trustedClick);
        Func<Task> act = () => sut.Click(targetId);

        await act.Should().NotThrowAsync();
        state.VerifyGet(x => x.Current, Times.Once);
        provider.Verify(x => x.GetApprovedRequestAsync(snapshot, targetId, It.IsAny<CancellationToken>()), Times.Once);
        trustedClick.Verify(x => x.ClickAsync(approved, It.IsAny<CancellationToken>()), Times.Once);
        state.VerifyNoOtherCalls();
        provider.VerifyNoOtherCalls();
        trustedClick.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Click_WhenTargetIdContainsWhitespace_ShouldPreserveTargetIdExactly()
    {
        OcrGroundedSnapshot snapshot = CreateSnapshot("snapshot-A");
        const string targetId = "  ocr-0  ";
        TrustedGroundedClickRequest approved = CreateApprovedRequest(snapshot, targetId);
        var state = ActiveState(snapshot);
        var provider = new Mock<IPlannerTrustedClickRequestProvider>(MockBehavior.Strict);
        var trustedClick = new Mock<ITrustedGroundedClickService>(MockBehavior.Strict);
        provider.Setup(x => x.GetApprovedRequestAsync(snapshot, targetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(approved);
        trustedClick.Setup(x => x.ClickAsync(approved, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        OcrGroundedPlannerClickTool sut = CreateTool(state, provider, trustedClick);
        Func<Task> act = () => sut.Click(targetId);

        await act.Should().NotThrowAsync();
        state.VerifyGet(x => x.Current, Times.Once);
        provider.Verify(x => x.GetApprovedRequestAsync(snapshot, targetId, It.IsAny<CancellationToken>()), Times.Once);
        trustedClick.Verify(x => x.ClickAsync(approved, It.IsAny<CancellationToken>()), Times.Once);
        state.VerifyNoOtherCalls();
        provider.VerifyNoOtherCalls();
        trustedClick.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Click_WhenExecutionStateIsMissing_ShouldNeverRequestApprovalOrClick()
    {
        var state = new Mock<IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>>(MockBehavior.Strict);
        state.SetupGet(x => x.Current)
            .Throws(new InvalidOperationException("No active AI tool execution state is available."));
        var provider = new Mock<IPlannerTrustedClickRequestProvider>(MockBehavior.Strict);
        var trustedClick = new Mock<ITrustedGroundedClickService>(MockBehavior.Strict);

        OcrGroundedPlannerClickTool sut = CreateTool(state, provider, trustedClick);
        Func<Task> act = () => sut.Click("ocr-0");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*No active AI tool execution state is available*");
        state.VerifyGet(x => x.Current, Times.Once);
        state.VerifyNoOtherCalls();
        provider.VerifyNoOtherCalls();
        trustedClick.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Click_WhenApprovalIsMissing_ShouldNeverClick()
    {
        OcrGroundedSnapshot snapshot = CreateSnapshot("snapshot-A");
        var state = ActiveState(snapshot);
        var provider = new Mock<IPlannerTrustedClickRequestProvider>(MockBehavior.Strict);
        var trustedClick = new Mock<ITrustedGroundedClickService>(MockBehavior.Strict);
        provider.Setup(x => x.GetApprovedRequestAsync(snapshot, "ocr-0", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TrustedGroundedClickRequest)null!);

        OcrGroundedPlannerClickTool sut = CreateTool(state, provider, trustedClick);
        Func<Task> act = () => sut.Click("ocr-0");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Trusted review approval is required*");
        state.VerifyGet(x => x.Current, Times.Once);
        provider.Verify(x => x.GetApprovedRequestAsync(snapshot, "ocr-0", It.IsAny<CancellationToken>()), Times.Once);
        state.VerifyNoOtherCalls();
        provider.VerifyNoOtherCalls();
        trustedClick.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Click_WhenApprovalDoesNotMatchCurrentAction_ShouldNeverClick(bool mismatchSnapshot)
    {
        OcrGroundedSnapshot snapshot = CreateSnapshot("snapshot-A");
        TrustedGroundedClickRequest approved = CreateApprovedRequest(
            mismatchSnapshot ? CreateSnapshot("snapshot-A") : snapshot,
            mismatchSnapshot ? "ocr-0" : "ocr-other");
        var state = ActiveState(snapshot);
        var provider = new Mock<IPlannerTrustedClickRequestProvider>(MockBehavior.Strict);
        var trustedClick = new Mock<ITrustedGroundedClickService>(MockBehavior.Strict);
        provider.Setup(x => x.GetApprovedRequestAsync(snapshot, "ocr-0", It.IsAny<CancellationToken>()))
            .ReturnsAsync(approved);

        OcrGroundedPlannerClickTool sut = CreateTool(state, provider, trustedClick);
        Func<Task> act = () => sut.Click("ocr-0");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*does not match the current planner action*");
        provider.Verify(x => x.GetApprovedRequestAsync(snapshot, "ocr-0", It.IsAny<CancellationToken>()), Times.Once);
        state.VerifyGet(x => x.Current, Times.Once);
        state.VerifyNoOtherCalls();
        provider.VerifyNoOtherCalls();
        trustedClick.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Click_WhenTrustedClickFails_ShouldPropagateOriginalFailure()
    {
        OcrGroundedSnapshot snapshot = CreateSnapshot("snapshot-A");
        TrustedGroundedClickRequest approved = CreateApprovedRequest(snapshot, "ocr-0");
        var state = ActiveState(snapshot);
        var provider = new Mock<IPlannerTrustedClickRequestProvider>(MockBehavior.Strict);
        var trustedClick = new Mock<ITrustedGroundedClickService>(MockBehavior.Strict);
        provider.Setup(x => x.GetApprovedRequestAsync(snapshot, "ocr-0", It.IsAny<CancellationToken>()))
            .ReturnsAsync(approved);
        trustedClick.Setup(x => x.ClickAsync(approved, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Trusted click failed."));

        OcrGroundedPlannerClickTool sut = CreateTool(state, provider, trustedClick);
        Func<Task> act = () => sut.Click("ocr-0");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Trusted click failed*");
        
        state.VerifyGet(x => x.Current, Times.Once);

        provider.Verify(
            x => x.GetApprovedRequestAsync(
                snapshot,
                "ocr-0",
                It.IsAny<CancellationToken>()),
            Times.Once);
        provider.VerifyNoOtherCalls();
        trustedClick.Verify(x => x.ClickAsync(approved, It.IsAny<CancellationToken>()), Times.Once);
        trustedClick.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Click_WhenInvokedTwice_ShouldResolveStateAndApprovalForEachInvocation()
    {
        OcrGroundedSnapshot first = CreateSnapshot("snapshot-A");
        OcrGroundedSnapshot second = CreateSnapshot("snapshot-B");
        TrustedGroundedClickRequest firstRequest = CreateApprovedRequest(first, "ocr-0");
        TrustedGroundedClickRequest secondRequest = CreateApprovedRequest(second, "ocr-1");
        var state = new Mock<IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>>(MockBehavior.Strict);
        state.SetupSequence(x => x.Current)
            .Returns(new OcrGroundedPlannerExecutionState(first))
            .Returns(new OcrGroundedPlannerExecutionState(second));
        var provider = new Mock<IPlannerTrustedClickRequestProvider>(MockBehavior.Strict);
        var trustedClick = new Mock<ITrustedGroundedClickService>(MockBehavior.Strict);
        provider.Setup(x => x.GetApprovedRequestAsync(first, "ocr-0", It.IsAny<CancellationToken>()))
            .ReturnsAsync(firstRequest);
        provider.Setup(x => x.GetApprovedRequestAsync(second, "ocr-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(secondRequest);
        trustedClick.Setup(x => x.ClickAsync(firstRequest, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        trustedClick.Setup(x => x.ClickAsync(secondRequest, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        OcrGroundedPlannerClickTool sut = CreateTool(state, provider, trustedClick);
        await sut.Click("ocr-0");
        await sut.Click("ocr-1");

        state.VerifyGet(x => x.Current, Times.Exactly(2));
        provider.Verify(x => x.GetApprovedRequestAsync(first, "ocr-0", It.IsAny<CancellationToken>()), Times.Once);
        provider.Verify(x => x.GetApprovedRequestAsync(second, "ocr-1", It.IsAny<CancellationToken>()), Times.Once);
        trustedClick.Verify(x => x.ClickAsync(firstRequest, It.IsAny<CancellationToken>()), Times.Once);
        trustedClick.Verify(x => x.ClickAsync(secondRequest, It.IsAny<CancellationToken>()), Times.Once);
        state.VerifyNoOtherCalls();
        provider.VerifyNoOtherCalls();
        trustedClick.VerifyNoOtherCalls();
    }

    private static OcrGroundedPlannerClickTool CreateTool(
        Mock<IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>> state,
        Mock<IPlannerTrustedClickRequestProvider> provider,
        Mock<ITrustedGroundedClickService> trustedClick) =>
        new(state.Object, new TrustedPlannerClickExecutor(provider.Object, trustedClick.Object));

    private static Mock<IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>> ActiveState(
        OcrGroundedSnapshot snapshot)
    {
        var state = new Mock<IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>>(MockBehavior.Strict);
        state.SetupGet(x => x.Current).Returns(new OcrGroundedPlannerExecutionState(snapshot));
        return state;
    }

    private static OcrGroundedSnapshot CreateSnapshot(string id) =>
        new(id, Array.Empty<OcrGroundedTarget>());

    // The approval is an opaque test fixture. The trusted device service is strictly mocked,
    // so no unverified evidence is ever sent to a production authorization boundary.
    private static TrustedGroundedClickRequest CreateApprovedRequest(
        OcrGroundedSnapshot snapshot, string targetId) =>
        new(snapshot, targetId, null!, "review", "annotation", "screenshot",
            "review-sha", "annotation-sha", "screenshot-sha", 1f);
}