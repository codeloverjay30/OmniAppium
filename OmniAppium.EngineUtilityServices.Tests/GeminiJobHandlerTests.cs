using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Mappers;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AiUtility.ToolKits.Services;
using CommonModels;
using EnumUtilityServices;
using FluentAssertions;
using JsonUtilityServices;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityService.Utilities;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.Planner;
using TypeUtilityServices;

namespace OmniAppium.EngineUtilityServices.Tests.Utilities;

public sealed class GeminiJobHandlerTests
{
    private readonly AiExecutionSettings _executionSettings;

    private readonly Mock<IGeminiToolRegistry> _registryMock;
    private readonly Mock<IGeminiSessionManager> _sessionManagerMock;

    private readonly Mock<IAndroidScreenObservationService> _observationServiceMock;

    private readonly Mock<IOcrGroundedPlannerExecutionScopeFactory> _plannerExecutionScopeFactoryMock;

    private readonly Mock<IAiParameterSchemaGenerator> _parameterSchemaGeneratorMock;
    private readonly Mock<IGeminiParameterPropertyMapper> _parameterPropertyMapperMock;

    private readonly Mock<IProgress<WorkflowProgress>> _progressMock;

    private readonly GeminiToolConverter _converter;

    public GeminiJobHandlerTests()
    {
        _executionSettings =
            new AiExecutionSettings
            {
                MaxSteps = 5,
                ToolExecutionTimeout =
                    TimeSpan.FromSeconds(30)
            };

        _registryMock =
            new Mock<IGeminiToolRegistry>(
                MockBehavior.Strict);

        _sessionManagerMock =
            new Mock<IGeminiSessionManager>(
                MockBehavior.Strict);

        _observationServiceMock =
            new Mock<IAndroidScreenObservationService>(
                MockBehavior.Strict);

        _plannerExecutionScopeFactoryMock =
            new Mock<IOcrGroundedPlannerExecutionScopeFactory>(
                MockBehavior.Strict);

        _progressMock =
            new Mock<IProgress<WorkflowProgress>>(
                MockBehavior.Strict);

        /*
         * IMPORTANT:
         * Keep your current compiling GeminiToolConverter initialization here.
         *
         * Do not restore the old parameterless constructor if the referenced
         * AiUtility version requires:
         *
         * IJsonUtilityService
         * IEnumUtilityService
         * IAiParameterSchemaGenerator
         * IGeminiParameterPropertyMapper
         */
        _parameterSchemaGeneratorMock =
            new Mock<IAiParameterSchemaGenerator>(
                MockBehavior.Strict);

        _parameterPropertyMapperMock =
            new Mock<IGeminiParameterPropertyMapper>(
                MockBehavior.Strict);

        _converter =
            CreateGeminiToolConverter(
                _parameterSchemaGeneratorMock.Object,
                _parameterPropertyMapperMock.Object);

        SetupEmptyToolRegistry();
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenExecutionSettingsIsNull()
    {
        Action act =
            () => new GeminiJobHandler<WorkflowProgress>(
                null!,
                _registryMock.Object,
                _converter,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                _plannerExecutionScopeFactoryMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*aiExecutionSettings*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenRegistryIsNull()
    {
        Action act =
            () => new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                null!,
                _converter,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                _plannerExecutionScopeFactoryMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*registry*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenConverterIsNull()
    {
        Action act =
            () => new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                _registryMock.Object,
                null!,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                _plannerExecutionScopeFactoryMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*converter*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenSessionManagerIsNull()
    {
        Action act =
            () => new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                _registryMock.Object,
                _converter,
                null!,
                _observationServiceMock.Object,
                _plannerExecutionScopeFactoryMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*sessionManager*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenObservationServiceIsNull()
    {
        Action act =
            () => new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                _registryMock.Object,
                _converter,
                _sessionManagerMock.Object,
                null!,
                _plannerExecutionScopeFactoryMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*screenObservationService*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenPlannerExecutionScopeFactoryIsNull()
    {
        Action act =
            () => new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                _registryMock.Object,
                _converter,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                null!,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*plannerExecutionScopeFactory*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenProgressBarIsNull()
    {
        Action act =
            () => new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                _registryMock.Object,
                _converter,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                _plannerExecutionScopeFactoryMock.Object,
                null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*progressBar*");
    }

[Fact]
public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenMaxStepsIsNegative()
{
    AiExecutionSettings settings =
        new()
        {
            MaxSteps = -1,
            ToolExecutionTimeout =
                TimeSpan.FromSeconds(30)
        };

    Action act =
        () => new GeminiJobHandler<WorkflowProgress>(
            settings,
            _registryMock.Object,
            _converter,
            _sessionManagerMock.Object,
            _observationServiceMock.Object,
            _plannerExecutionScopeFactoryMock.Object,
            _progressMock.Object);

    act.Should()
        .Throw<ArgumentOutOfRangeException>()
        .WithMessage("*MaxSteps*");
}

    [Fact]
    public void SetExecutionSettings_ShouldThrowArgumentOutOfRangeException_WhenMaxStepsIsNegative()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        AiExecutionSettings settings =
            new()
            {
                MaxSteps = -1,
                ToolExecutionTimeout =
                    TimeSpan.FromSeconds(30)
            };

        Action act =
            () => sut.SetExecutionSettings(settings);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*MaxSteps*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenToolExecutionTimeoutIsZero()
    {
        AiExecutionSettings settings =
            new()
            {
                MaxSteps = 5,
                ToolExecutionTimeout =
                    TimeSpan.Zero
            };

        Action act =
            () => new GeminiJobHandler<WorkflowProgress>(
                settings,
                _registryMock.Object,
                _converter,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                _plannerExecutionScopeFactoryMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*ToolExecutionTimeout*");
    }

    [Fact]
    public void SetExecutionSettings_ShouldThrowArgumentNullException_WhenSettingsIsNull()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Action act =
            () => sut.SetExecutionSettings(
                null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*aiExecutionSettings*");
    }

    [Fact]
    public void SetExecutionSettings_ShouldThrowArgumentOutOfRangeException_WhenTimeoutIsZero()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        AiExecutionSettings settings =
            new()
            {
                MaxSteps = 5,
                ToolExecutionTimeout =
                    TimeSpan.Zero
            };

        Action act =
            () => sut.SetExecutionSettings(
                settings);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*ToolExecutionTimeout*");
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_WhenJobIsGeminiJob()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        bool result =
            sut.CanHandle(
                CreateValidGeminiJob());

        result.Should()
            .BeTrue();
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_WhenJobIsNotGeminiJob()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Job job =
            new WaitJob
            {
                Timeout = 100
            };

        bool result =
            sut.CanHandle(job);

        result.Should()
            .BeFalse();
    }

    [Fact]
    public void CanHandle_ShouldThrowArgumentNullException_WhenJobIsNull()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Action act =
            () => sut.CanHandle(
                null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*job*");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrowArgumentException_WhenJobIsNotGeminiJob()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Job job =
            new WaitJob
            {
                Timeout = 100
            };

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage(
                "*Expected GeminiJob, but received WaitJob.*");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrowArgumentNullException_WhenGeminiJobIsNull()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Func<Task> act =
            () => sut.AutoExecuteAsync(
                (GeminiJob)null!);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithMessage("*gJob*");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrowArgumentException_WhenUserTaskIsEmpty()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        job.UserTask =
            string.Empty;

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("*UserTask*");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrowArgumentException_WhenPromptIsEmpty()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        job.Prompt =
            string.Empty;

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("*Prompt*");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldObserveScreenExactlyOnce()
    {
        IAndroidScreenObservation observation =
            CreateObservation(
                [0x01, 0x02, 0x03]);

        Mock<IOcrGroundedPlannerExecutionScope> scopeMock =
            CreatePlannerExecutionScopeMock();

        SetupObservation(observation);

        SetupPlannerScope(
            observation,
            scopeMock.Object);

        SetupSuccessfulGeminiExecution();

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        await sut.AutoExecuteAsync(
            CreateValidGeminiJob());

        _observationServiceMock.Verify(
            service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);

        scopeMock.Verify(
            scope =>
                scope.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldCreatePlannerExecutionScopeFromSameObservation()
    {
        IAndroidScreenObservation observation =
            CreateObservation(
                [0x01, 0x02, 0x03]);

        Mock<IOcrGroundedPlannerExecutionScope> scopeMock =
            CreatePlannerExecutionScopeMock();

        SetupObservation(observation);

        SetupPlannerScope(
            observation,
            scopeMock.Object);

        SetupSuccessfulGeminiExecution();

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        await sut.AutoExecuteAsync(
            CreateValidGeminiJob());

        _plannerExecutionScopeFactoryMock.Verify(
            factory =>
                factory.CreateAsync(
                    It.Is<IAndroidScreenObservation>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                observation)),
                    It.IsAny<CancellationToken>()),
            Times.Once);

        scopeMock.Verify(
            scope =>
                scope.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldKeepPlannerExecutionScopeAliveDuringGeminiExecution()
    {
        IAndroidScreenObservation observation =
            CreateObservation([0x01]);

        bool disposed = false;

        Mock<IOcrGroundedPlannerExecutionScope> scopeMock =
            new(MockBehavior.Strict);

        scopeMock
            .Setup(scope =>
                scope.Dispose())
            .Callback(
                () => disposed = true);

        SetupObservation(observation);

        SetupPlannerScope(
            observation,
            scopeMock.Object);

        _sessionManagerMock
            .Setup(sessionManager =>
                sessionManager
                    .ExecuteWithToolSupportAsync<WorkflowProgress>(
                        It.IsAny<GeminiGenerateRequest>(),
                        It.IsAny<ReadOnlyMemory<char>>(),
                        It.IsAny<AiExecutionSettings>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<IProgress<WorkflowProgress>>()))
            .Callback(
                () =>
                {
                    disposed.Should()
                        .BeFalse(
                            "planner execution state must remain available " +
                            "while Gemini tool execution is in progress");
                })
            .ReturnsAsync(
                CreateSuccessfulExecutionResult());

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        await sut.AutoExecuteAsync(
            CreateValidGeminiJob());

        disposed.Should()
            .BeTrue(
                "planner execution scope must be disposed " +
                "after Gemini execution");

        scopeMock.Verify(
            scope =>
                scope.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldDisposePlannerExecutionScope_WhenGeminiExecutionCompletes()
    {
        IAndroidScreenObservation observation =
            CreateObservation([0x01]);

        Mock<IOcrGroundedPlannerExecutionScope> scopeMock =
            CreatePlannerExecutionScopeMock();

        SetupObservation(observation);

        SetupPlannerScope(
            observation,
            scopeMock.Object);

        SetupSuccessfulGeminiExecution();

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        await sut.AutoExecuteAsync(
            CreateValidGeminiJob());

        scopeMock.Verify(
            scope =>
                scope.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldDisposePlannerExecutionScope_WhenGeminiExecutionThrows()
    {
        IAndroidScreenObservation observation =
            CreateObservation([0x01]);

        Mock<IOcrGroundedPlannerExecutionScope> scopeMock =
            CreatePlannerExecutionScopeMock();

        SetupObservation(observation);

        SetupPlannerScope(
            observation,
            scopeMock.Object);

        _sessionManagerMock
            .Setup(sessionManager =>
                sessionManager
                    .ExecuteWithToolSupportAsync<WorkflowProgress>(
                        It.IsAny<GeminiGenerateRequest>(),
                        It.IsAny<ReadOnlyMemory<char>>(),
                        It.IsAny<AiExecutionSettings>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<IProgress<WorkflowProgress>>()))
            .ThrowsAsync(
                new InvalidOperationException(
                    "Gemini execution failed."));

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Func<Task> act =
            () => sut.AutoExecuteAsync(
                CreateValidGeminiJob());

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Gemini execution failed.");

        scopeMock.Verify(
            scope =>
                scope.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldDisposePlannerExecutionScope_WhenGeminiExecutionIsCanceled()
    {
        IAndroidScreenObservation observation =
            CreateObservation([0x01]);

        Mock<IOcrGroundedPlannerExecutionScope> scopeMock =
            CreatePlannerExecutionScopeMock();

        SetupObservation(observation);

        SetupPlannerScope(
            observation,
            scopeMock.Object);

        _sessionManagerMock
            .Setup(sessionManager =>
                sessionManager
                    .ExecuteWithToolSupportAsync<WorkflowProgress>(
                        It.IsAny<GeminiGenerateRequest>(),
                        It.IsAny<ReadOnlyMemory<char>>(),
                        It.IsAny<AiExecutionSettings>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<IProgress<WorkflowProgress>>()))
            .ThrowsAsync(
                new OperationCanceledException(
                    "Gemini execution was canceled."));

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Func<Task> act =
            () => sut.AutoExecuteAsync(
                CreateValidGeminiJob());

        await act.Should()
            .ThrowAsync<OperationCanceledException>()
            .WithMessage(
                "Gemini execution was canceled.");

        scopeMock.Verify(
            scope =>
                scope.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldNotStartGeminiExecution_WhenPlannerExecutionScopeCreationFails()
    {
        IAndroidScreenObservation observation =
            CreateObservation([0x01]);

        SetupObservation(observation);

        _plannerExecutionScopeFactoryMock
            .Setup(factory =>
                factory.CreateAsync(
                    It.Is<IAndroidScreenObservation>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                observation)),
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new InvalidOperationException(
                    "Planner execution scope creation failed."));

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Func<Task> act =
            () => sut.AutoExecuteAsync(
                CreateValidGeminiJob());

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Planner execution scope creation failed.");

        _registryMock.Verify(
            registry =>
                registry.GetAllTools(),
            Times.Never);

        _sessionManagerMock.Verify(
            sessionManager =>
                sessionManager
                    .ExecuteWithToolSupportAsync<WorkflowProgress>(
                        It.IsAny<GeminiGenerateRequest>(),
                        It.IsAny<ReadOnlyMemory<char>>(),
                        It.IsAny<AiExecutionSettings>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<IProgress<WorkflowProgress>>()),
            Times.Never);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldNotCreatePlannerScope_WhenObservationFails()
    {
        _observationServiceMock
            .Setup(service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new InvalidOperationException(
                    "Screen observation failed."));

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Func<Task> act =
            () => sut.AutoExecuteAsync(
                CreateValidGeminiJob());

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Screen observation failed.");

        _plannerExecutionScopeFactoryMock.Verify(
            factory =>
                factory.CreateAsync(
                    It.IsAny<IAndroidScreenObservation>(),
                    It.IsAny<CancellationToken>()),
            Times.Never);

        _registryMock.Verify(
            registry =>
                registry.GetAllTools(),
            Times.Never);

        _sessionManagerMock.Verify(
            sessionManager =>
                sessionManager
                    .ExecuteWithToolSupportAsync<WorkflowProgress>(
                        It.IsAny<GeminiGenerateRequest>(),
                        It.IsAny<ReadOnlyMemory<char>>(),
                        It.IsAny<AiExecutionSettings>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<IProgress<WorkflowProgress>>()),
            Times.Never);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldPassConfiguredExecutionSettingsToGeminiSession()
    {
        IAndroidScreenObservation observation =
            CreateObservation([0x01]);

        Mock<IOcrGroundedPlannerExecutionScope> scopeMock =
            CreatePlannerExecutionScopeMock();

        SetupObservation(observation);

        SetupPlannerScope(
            observation,
            scopeMock.Object);

        AiExecutionSettings? capturedSettings =
            null;

        _sessionManagerMock
            .Setup(sessionManager =>
                sessionManager
                    .ExecuteWithToolSupportAsync<WorkflowProgress>(
                        It.IsAny<GeminiGenerateRequest>(),
                        It.IsAny<ReadOnlyMemory<char>>(),
                        It.IsAny<AiExecutionSettings>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<IProgress<WorkflowProgress>>()))
            .Callback<
                GeminiGenerateRequest,
                ReadOnlyMemory<char>,
                AiExecutionSettings,
                CancellationToken,
                IProgress<WorkflowProgress>?>(
                (_, _, settings, _, _) =>
                    capturedSettings = settings)
            .ReturnsAsync(
                CreateSuccessfulExecutionResult());

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        await sut.AutoExecuteAsync(
            CreateValidGeminiJob());

        capturedSettings.Should()
            .BeSameAs(_executionSettings);

        scopeMock.Verify(
            scope =>
                scope.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldUseUpdatedExecutionSettings()
    {
        IAndroidScreenObservation observation =
            CreateObservation([0x01]);

        Mock<IOcrGroundedPlannerExecutionScope> scopeMock =
            CreatePlannerExecutionScopeMock();

        SetupObservation(observation);

        SetupPlannerScope(
            observation,
            scopeMock.Object);

        AiExecutionSettings updatedSettings =
            new()
            {
                MaxSteps = 7,
                ToolExecutionTimeout =
                    TimeSpan.FromSeconds(45)
            };

        AiExecutionSettings? capturedSettings =
            null;

        _sessionManagerMock
            .Setup(sessionManager =>
                sessionManager
                    .ExecuteWithToolSupportAsync<WorkflowProgress>(
                        It.IsAny<GeminiGenerateRequest>(),
                        It.IsAny<ReadOnlyMemory<char>>(),
                        It.IsAny<AiExecutionSettings>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<IProgress<WorkflowProgress>>()))
            .Callback<
                GeminiGenerateRequest,
                ReadOnlyMemory<char>,
                AiExecutionSettings,
                CancellationToken,
                IProgress<WorkflowProgress>?>(
                (_, _, settings, _, _) =>
                    capturedSettings = settings)
            .ReturnsAsync(
                CreateSuccessfulExecutionResult());

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        sut.SetExecutionSettings(
            updatedSettings);

        await sut.AutoExecuteAsync(
            CreateValidGeminiJob());

        capturedSettings.Should()
            .BeSameAs(updatedSettings);

        scopeMock.Verify(
            scope =>
                scope.Dispose(),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldPassUserTaskToGeminiSession()
    {
        IAndroidScreenObservation observation =
            CreateObservation([0x01]);

        Mock<IOcrGroundedPlannerExecutionScope> scopeMock =
            CreatePlannerExecutionScopeMock();

        SetupObservation(observation);

        SetupPlannerScope(
            observation,
            scopeMock.Object);

        const string expectedUserTask =
            "Attack the visible target.";

        ReadOnlyMemory<char> capturedUserTask =
            ReadOnlyMemory<char>.Empty;

        _sessionManagerMock
            .Setup(sessionManager =>
                sessionManager
                    .ExecuteWithToolSupportAsync<WorkflowProgress>(
                        It.IsAny<GeminiGenerateRequest>(),
                        It.IsAny<ReadOnlyMemory<char>>(),
                        It.IsAny<AiExecutionSettings>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<IProgress<WorkflowProgress>>()))
            .Callback<
                GeminiGenerateRequest,
                ReadOnlyMemory<char>,
                AiExecutionSettings,
                CancellationToken,
                IProgress<WorkflowProgress>?>(
                (_, userTask, _, _, _) =>
                    capturedUserTask = userTask)
            .ReturnsAsync(
                CreateSuccessfulExecutionResult());

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        job.UserTask =
            expectedUserTask;

        await sut.AutoExecuteAsync(job);

        capturedUserTask
            .ToString()
            .Should()
            .Be(expectedUserTask);

        scopeMock.Verify(
            scope =>
                scope.Dispose(),
            Times.Once);
    }

    private GeminiJobHandler<WorkflowProgress> CreateSut()
    {
        return new GeminiJobHandler<WorkflowProgress>(
            _executionSettings,
            _registryMock.Object,
            _converter,
            _sessionManagerMock.Object,
            _observationServiceMock.Object,
            _plannerExecutionScopeFactoryMock.Object,
            _progressMock.Object);
    }

    private static GeminiJob CreateValidGeminiJob()
    {
        return new GeminiJob
        {
            Prompt =
                "Analyze the current game screen.",
            UserTask =
                "Continue the current game workflow."
        };
    }

    private static IAndroidScreenObservation CreateObservation(
        byte[] imageBytes)
    {
        OcrResult ocrResult =
            new(
                "Start Battle",
                Array.Empty<OcrTextLine>());

        return new AndroidScreenObservation(
            imageBytes,
            ocrResult);
    }

    private void SetupEmptyToolRegistry()
    {
        _registryMock
            .Setup(registry =>
                registry.GetAllTools())
            .Returns([]);
    }

    private void SetupObservation(
        IAndroidScreenObservation observation)
    {
        _observationServiceMock
            .Setup(service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(observation);
    }

    private void SetupPlannerScope(
        IAndroidScreenObservation observation,
        IOcrGroundedPlannerExecutionScope scope)
    {
        _plannerExecutionScopeFactoryMock
            .Setup(factory =>
                factory.CreateAsync(
                    It.Is<IAndroidScreenObservation>(
                        candidate =>
                            ReferenceEquals(
                                candidate,
                                observation)),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(scope);
    }

    private void SetupSuccessfulGeminiExecution()
    {
        _sessionManagerMock
            .Setup(sessionManager =>
                sessionManager
                    .ExecuteWithToolSupportAsync<WorkflowProgress>(
                        It.IsAny<GeminiGenerateRequest>(),
                        It.IsAny<ReadOnlyMemory<char>>(),
                        It.IsAny<AiExecutionSettings>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<IProgress<WorkflowProgress>>()))
            .ReturnsAsync(
                CreateSuccessfulExecutionResult());
    }

    private static Mock<IOcrGroundedPlannerExecutionScope>
        CreatePlannerExecutionScopeMock()
    {
        Mock<IOcrGroundedPlannerExecutionScope> scopeMock =
            new(MockBehavior.Strict);

        scopeMock
            .Setup(scope =>
                scope.Dispose());

        return scopeMock;
    }

    private static StatusJsonModels
        CreateSuccessfulExecutionResult()
    {
        return new StatusJsonModels
        {
            StatusList =
            [
                new StatusJsonModel
                {
                    IsSuccess = true
                }
            ]
        };
    }

    private static GeminiToolConverter CreateGeminiToolConverter(
    IAiParameterSchemaGenerator parameterSchemaGenerator,
    IGeminiParameterPropertyMapper parameterPropertyMapper)
    {
        ITypeUtilityService typeUtilityService =
            new TypeUtilityService();

        IJsonUtilityService jsonUtilityService =
            new JsonUtilityService(
                typeUtilityService);

        IEnumUtilityService enumUtilityService =
            new EnumUtilityService();

        return new GeminiToolConverter(
            jsonUtilityService,
            enumUtilityService,
            parameterSchemaGenerator,
            parameterPropertyMapper);
    }
}