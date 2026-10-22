using AiUtility.ToolKits.Abstractions;
using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Planner;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Planner;

public sealed class Issue50PlannerRuntimeSecurityBoundaryTests
{
    [Fact]
    public void Click_WhenPlannerScopeIsMissing_ShouldNeverTouchDevice()
    {
        var state = new Mock<IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>>(MockBehavior.Strict);
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var freshness = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);
        state.SetupGet(x => x.Current)
            .Throws(new InvalidOperationException("No active AI tool execution state is available."));

        OcrGroundedPlannerClickTool sut = CreateTool(state, click, freshness);
        Action act = () => sut.Click("ocr-0");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*No active AI tool execution state is available*");
        click.Verify(x => x.ClickAbsolute(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        click.VerifyNoOtherCalls();
        freshness.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenTargetIsNotInSnapshot_ShouldNeverTouchDevice()
    {
        OcrGroundedSnapshot snapshot = CreateSnapshot();
        var state = CreateActiveState(snapshot);
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var freshness = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);

        OcrGroundedPlannerClickTool sut = CreateTool(state, click, freshness);
        Action act = () => sut.Click("ocr-missing");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*OCR-grounded target 'ocr-missing' was not found*");
        click.Verify(x => x.ClickAbsolute(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        click.VerifyNoOtherCalls();
        freshness.VerifyNoOtherCalls();
    }

    [Fact]
    public void Click_WhenSnapshotIsStale_ShouldNeverTouchDeviceOrAuthorize()
    {
        OcrGroundedSnapshot snapshot = CreateSnapshot();
        var state = CreateActiveState(snapshot);
        var click = new Mock<IClickService>(MockBehavior.Strict);
        var freshness = new Mock<IOcrGroundedSnapshotFreshnessGuard>(MockBehavior.Strict);
        freshness.Setup(x => x.AcquireCurrent(snapshot))
            .Throws(new InvalidOperationException("OCR-grounded snapshot is stale."));

        OcrGroundedPlannerClickTool sut = CreateTool(state, click, freshness);
        Action act = () => sut.Click("ocr-0");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*snapshot is stale*");
        freshness.Verify(x => x.AcquireCurrent(snapshot), Times.Once);
        freshness.VerifyNoOtherCalls();
        click.Verify(x => x.ClickAbsolute(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        click.VerifyNoOtherCalls();
    }

    private static Mock<IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>>
        CreateActiveState(OcrGroundedSnapshot snapshot)
    {
        var state = new Mock<IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>>(MockBehavior.Strict);
        state.SetupGet(x => x.Current).Returns(new OcrGroundedPlannerExecutionState(snapshot));
        return state;
    }

    private static OcrGroundedSnapshot CreateSnapshot() =>
        new("snapshot-issue50", [
            new OcrGroundedTarget("ocr-0", "Target", Rectangle.FromXYWH(10, 20, 20, 20))
        ]);

    private static OcrGroundedPlannerClickTool CreateTool(
        Mock<IAiToolExecutionStateAccessor<OcrGroundedPlannerExecutionState>> state,
        Mock<IClickService> click,
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshness)
    {
        var groundedClick = new OcrGroundedClickService(
            click.Object,
            freshness.Object,
            GroundedClickTestDoubles.Authorization(),
            GroundedClickTestDoubles.Evidence());
        var executor = new OcrGroundedPlannerActionExecutor(groundedClick);
        return new OcrGroundedPlannerClickTool(state.Object, executor);
    }
}
