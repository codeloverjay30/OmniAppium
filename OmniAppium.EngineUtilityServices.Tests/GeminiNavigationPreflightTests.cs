using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Mappers;
using AiUtility.GeminiKits.Models;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AiUtility.ToolKits.Services;
using CommonModels;
using EnumUtilityServices;
using FluentAssertions;
using JsonUtilityServices;
using Moq;
using OmniAppium.ConfigUtilityServices.Models;
using OmniAppium.EngineUtilityService.Services.Wait;
using OmniAppium.EngineUtilityService.Utilities;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Navigation;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.Planner;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.AiSecurity;

public sealed class GeminiNavigationPreflightTests
{
    [Fact]
    public async Task VerifiedNavigation_WithOnlyWaitTool_RejectsBeforeObservationOrGemini()
    {
        var waitMethod = typeof(WaitService).GetMethod(
            nameof(WaitService.Wait), [typeof(TimeSpan)])!;
        var waitTool = new GeminiToolMetadata(
            waitMethod.Name,
            waitMethod,
            waitMethod.GetParameters(),
            (_, _) => throw new InvalidOperationException("Wait must not execute."),
            null,
            Array.Empty<Attribute>());
        var registry = new Mock<IGeminiToolRegistry>();
        registry.Setup(x => x.GetAllTools()).Returns(new[] { waitTool });

        var observation = new Mock<IAndroidScreenObservationService>(MockBehavior.Strict);
        var session = new Mock<IGeminiSessionManager>(MockBehavior.Strict);
        var planner = new Mock<IOcrGroundedPlannerExecutionScopeFactory>(MockBehavior.Strict);
        var handler = new GeminiJobHandler<WorkflowProgress>(
            new AiExecutionSettings
            {
                LastTokenCountNeededToBeKept = 1,
                ToolExecutionTimeout = TimeSpan.FromSeconds(5)
            },
            registry.Object,
            new GeminiToolConverter(
                Mock.Of<IJsonUtilityService>(),
                Mock.Of<IEnumUtilityService>(),
                Mock.Of<IAiParameterSchemaGenerator>(),
                Mock.Of<IGeminiParameterPropertyMapper>()),
            session.Object,
            observation.Object,
            planner.Object,
            Mock.Of<IProgress<WorkflowProgress>>(),
            Mock.Of<ITrustedNavigationEvidenceCollector>(),
            Mock.Of<INavigationCompletionGuard>());

        Func<Task> act = () => handler.AutoExecuteAsync(new GeminiJob
        {
            Prompt = "Navigate to the target.",
            UserTask = "Open the target screen.",
            RequiresNavigationVerification = true
        });

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Verified navigation requires the registered OCR-grounded planner click tool.");

        observation.Invocations.Should().BeEmpty();
        session.Invocations.Should().BeEmpty();
        planner.Invocations.Should().BeEmpty();
    }

    public enum SessionOutcome
    {
        DestinationUnverified,
        DestinationVerified,
        TimedOut,
        Canceled
    }

    [Theory]
    [InlineData(SessionOutcome.DestinationUnverified)]
    [InlineData(SessionOutcome.DestinationVerified)]
    [InlineData(SessionOutcome.TimedOut)]
    [InlineData(SessionOutcome.Canceled)]
    public async Task VerifiedNavigation_RequiresSessionSuccessAndVerifiedDestination(
        SessionOutcome outcome)
    {
        var clickMethod = typeof(OcrGroundedPlannerClickTool).GetMethod(
            nameof(OcrGroundedPlannerClickTool.Click))!;
        var clickTool = new GeminiToolMetadata(
            clickMethod.Name,
            clickMethod,
            clickMethod.GetParameters(),
            (_, _) => throw new InvalidOperationException("This test uses captured navigation evidence."),
            null,
            Array.Empty<Attribute>());

        var registry = new Mock<IGeminiToolRegistry>();
        registry.Setup(x => x.GetAllTools()).Returns(new[] { clickTool });

        var converter = new Mock<GeminiToolConverter>(
            Mock.Of<IJsonUtilityService>(),
            Mock.Of<IEnumUtilityService>(),
            Mock.Of<IAiParameterSchemaGenerator>(),
            Mock.Of<IGeminiParameterPropertyMapper>(),
            "Test tool",
            "Test parameter");
        converter.Setup(x => x.ToToolDeclaration(clickTool))
            .Returns(new GeminiToolDeclaration { Name = clickMethod.Name });

        var frame = Mock.Of<IAndroidScreenObservation>();
        var observation = new Mock<IAndroidScreenObservationService>(MockBehavior.Strict);
        observation.Setup(x => x.ObserveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(frame);

        var planner = new Mock<IOcrGroundedPlannerExecutionScopeFactory>(MockBehavior.Strict);
        planner.Setup(x => x.CreateAsync(frame, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IOcrGroundedPlannerExecutionScope>());

        var sessionResult = new StatusJsonModels();
        sessionResult.StatusList.Add(new StatusJsonModel { IsSuccess = true });
        var session = new Mock<IGeminiSessionManager>(MockBehavior.Strict);
        var execution = session.Setup(x => x.ExecuteWithToolSupportAsync<WorkflowProgress>(
                It.IsAny<GeminiGenerateRequest>(),
                It.IsAny<ReadOnlyMemory<char>>(),
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<WorkflowProgress>>()));

        switch (outcome)
        {
            case SessionOutcome.TimedOut:
                execution.ThrowsAsync(new TimeoutException("Gemini session timed out."));
                break;
            case SessionOutcome.Canceled:
                execution.ThrowsAsync(new OperationCanceledException("Gemini session was canceled."));
                break;
            default:
                execution.ReturnsAsync(sessionResult);
                break;
        }

        var collector = new Mock<ITrustedNavigationEvidenceCollector>(MockBehavior.Strict);
        collector.Setup(x => x.GetEvidence()).Returns(new[]
        {
            new NavigationActionEvidence(
                NavigationActionKind.TrustedGroundedClick,
                Executed: true,
                Succeeded: true,
                PostActionScreenVerified: true,
                ExpectedDestinationVerified: outcome == SessionOutcome.DestinationVerified)
        });

        var handler = new GeminiJobHandler<WorkflowProgress>(
            new AiExecutionSettings
            {
                LastTokenCountNeededToBeKept = 1,
                ToolExecutionTimeout = TimeSpan.FromSeconds(5)
            },
            registry.Object,
            converter.Object,
            session.Object,
            observation.Object,
            planner.Object,
            Mock.Of<IProgress<WorkflowProgress>>(),
            collector.Object,
            new NavigationCompletionGuard());

        Func<Task> act = () => handler.AutoExecuteAsync(new GeminiJob
        {
            Prompt = "Navigate to the target.",
            UserTask = "Open the target screen.",
            RequiresNavigationVerification = true
        });

        switch (outcome)
        {
            case SessionOutcome.TimedOut:
                await act.Should().ThrowAsync<TimeoutException>()
                    .WithMessage("Gemini session timed out.");
                collector.Invocations.Should().BeEmpty();
                break;
            case SessionOutcome.Canceled:
                await act.Should().ThrowAsync<OperationCanceledException>()
                    .WithMessage("Gemini session was canceled.");
                collector.Invocations.Should().BeEmpty();
                break;
            case SessionOutcome.DestinationVerified:
                await act.Should().NotThrowAsync();
                collector.Invocations.Should().ContainSingle();
                break;
            default:
                await act.Should().ThrowAsync<InvalidOperationException>()
                    .WithMessage(
                        "The navigation action is missing post-action screen or destination verification.");
                collector.Invocations.Should().ContainSingle();
                break;
        }

        session.Invocations.Should().ContainSingle();
    }
}