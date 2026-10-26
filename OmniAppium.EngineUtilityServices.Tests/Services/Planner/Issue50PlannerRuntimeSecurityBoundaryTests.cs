using AiUtility.ToolKits.Abstractions;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Planner;

public sealed class Issue50PlannerRuntimeSecurityBoundaryTests
{
    [Fact]
    public async Task Click_WhenPlannerScopeIsMissing_ShouldNeverRequestApprovalOrTouchDevice()
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
    public async Task Click_WhenTargetIsNotApproved_ShouldNeverTouchDevice()
    {
        OcrGroundedSnapshot snapshot = CreateSnapshot();
        var state = ActiveState(snapshot);
        var provider = new Mock<IPlannerTrustedClickRequestProvider>(MockBehavior.Strict);
        var trustedClick = new Mock<ITrustedGroundedClickService>(MockBehavior.Strict);
        provider.Setup(x => x.GetApprovedRequestAsync(snapshot, "ocr-missing", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TrustedGroundedClickRequest)null!);
        OcrGroundedPlannerClickTool sut = CreateTool(state, provider, trustedClick);

        Func<Task> act = () => sut.Click("ocr-missing");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Trusted review approval is required*");
        state.VerifyGet(x => x.Current, Times.Once);
        provider.Verify(x => x.GetApprovedRequestAsync(snapshot, "ocr-missing", It.IsAny<CancellationToken>()), Times.Once);
        state.VerifyNoOtherCalls();
        provider.VerifyNoOtherCalls();
        trustedClick.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Click_WhenSnapshotIsStale_ShouldNeverTouchDeviceOrAuthorize()
    {
        OcrGroundedSnapshot snapshot = CreateSnapshot();
        var state = ActiveState(snapshot);
        var provider = new Mock<IPlannerTrustedClickRequestProvider>(MockBehavior.Strict);
        var trustedClick = new Mock<ITrustedGroundedClickService>(MockBehavior.Strict);
        provider.Setup(x => x.GetApprovedRequestAsync(snapshot, "ocr-0", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("OCR-grounded snapshot is stale."));
        OcrGroundedPlannerClickTool sut = CreateTool(state, provider, trustedClick);

        Func<Task> act = () => sut.Click("ocr-0");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*snapshot is stale*");
        state.VerifyGet(x => x.Current, Times.Once);
        provider.Verify(x => x.GetApprovedRequestAsync(snapshot, "ocr-0", It.IsAny<CancellationToken>()), Times.Once);
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

    private static OcrGroundedSnapshot CreateSnapshot() =>
        new("snapshot-issue50", [
            new OcrGroundedTarget("ocr-0", "Target",
                CoordinateUtilityServices.Rectangle.FromXYWH(10, 20, 20, 20))
        ]);
}