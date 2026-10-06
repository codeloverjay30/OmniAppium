using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Mappers;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AiUtility.ToolKits.Services;
using EnumUtilityServices;
using FluentAssertions;
using JsonUtilityServices;
using Moq;
using OCRUtilityServices.Models;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityService.Utilities;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Observation;
using TypeUtilityServices;

namespace OmniAppium.EngineUtilityServices.Tests.Utilities;

public sealed class GeminiJobHandlerTests
{
    private readonly Mock<IGeminiToolRegistry> _registryMock;
    private readonly Mock<IGeminiSessionManager> _sessionManagerMock;
    private readonly Mock<IAndroidScreenObservationService> _observationServiceMock;
    private readonly Mock<IProgress<WorkflowProgress>> _progressMock;
    private readonly Mock<IAiParameterSchemaGenerator> _parameterSchemaGeneratorMock;
    private readonly Mock<IGeminiParameterPropertyMapper> _parameterPropertyMapperMock;

    private readonly GeminiToolConverter _converter;
    private readonly AiExecutionSettings _executionSettings;

    public GeminiJobHandlerTests()
    {
        _registryMock =
            new Mock<IGeminiToolRegistry>(MockBehavior.Strict);

        _sessionManagerMock =
            new Mock<IGeminiSessionManager>(MockBehavior.Strict);

        _observationServiceMock =
            new Mock<IAndroidScreenObservationService>(MockBehavior.Strict);

        _progressMock =
            new Mock<IProgress<WorkflowProgress>>(MockBehavior.Strict);

        /*
         * GeminiToolConverter is a concrete class.
         *
         * Reuse the same construction that already exists in this test project
         * if the project has shared Json/Enum utility fixtures.
         *
         * If not, replace these two helper calls with the project's existing
         * IJsonUtilityService / IEnumUtilityService construction.
         */
        _parameterSchemaGeneratorMock =
            new Mock<IAiParameterSchemaGenerator>(MockBehavior.Strict);

        _parameterPropertyMapperMock =
            new Mock<IGeminiParameterPropertyMapper>(MockBehavior.Strict);

        _converter = CreateGeminiToolConverter(
            _parameterSchemaGeneratorMock.Object,
            _parameterPropertyMapperMock.Object);

        _executionSettings = CreateValidExecutionSettings();

        /*
         * GeminiJobHandler enumerates tools before observing the screen.
         * An empty registry is sufficient for these handler tests.
         */
        _registryMock
            .Setup(registry => registry.GetAllTools())
            .Returns([]);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenAiExecutionSettingsIsNull()
    {
        Action act = () =>
            new GeminiJobHandler<WorkflowProgress>(
                null!,
                _registryMock.Object,
                _converter,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("aiExecutionSettings");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenRegistryIsNull()
    {
        Action act = () =>
            new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                null!,
                _converter,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("registry");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenConverterIsNull()
    {
        Action act = () =>
            new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                _registryMock.Object,
                null!,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("converter");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenSessionManagerIsNull()
    {
        Action act = () =>
            new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                _registryMock.Object,
                _converter,
                null!,
                _observationServiceMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("sessionManager");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenScreenObservationServiceIsNull()
    {
        Action act = () =>
            new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                _registryMock.Object,
                _converter,
                _sessionManagerMock.Object,
                null!,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("screenObservationService");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenProgressBarIsNull()
    {
        Action act = () =>
            new GeminiJobHandler<WorkflowProgress>(
                _executionSettings,
                _registryMock.Object,
                _converter,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("progressBar");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenToolExecutionTimeoutIsZero()
    {
        AiExecutionSettings invalidSettings =
            CreateValidExecutionSettings();

        invalidSettings.ToolExecutionTimeout =
            TimeSpan.Zero;

        Action act = () =>
            new GeminiJobHandler<WorkflowProgress>(
                invalidSettings,
                _registryMock.Object,
                _converter,
                _sessionManagerMock.Object,
                _observationServiceMock.Object,
                _progressMock.Object);

        act.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("*ToolExecutionTimeout*");
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForGeminiJob()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job = CreateValidGeminiJob();

        bool result =
            sut.CanHandle(job);

        result.Should().BeTrue();
    }

    [Fact]
    public void CanHandle_ShouldReturnFalse_ForNonGeminiJob()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Job job = new WaitJob();

        bool result =
            sut.CanHandle(job);

        result.Should().BeFalse();
    }

    [Fact]
    public void CanHandle_ShouldThrow_WhenJobIsNull()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Action act =
            () => sut.CanHandle(null!);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("job");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrow_WhenJobIsNull()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Func<Task> act =
            () => sut.AutoExecuteAsync((Job)null!);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("job");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrow_WhenJobIsNotGeminiJob()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        Job job = new WaitJob();

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("*GeminiJob*");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrow_WhenUserTaskIsEmpty()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        job.UserTask = string.Empty;

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithParameterName(nameof(GeminiJob.UserTask));

        _observationServiceMock.Verify(
            service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrow_WhenPromptIsEmpty()
    {
        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        job.Prompt = string.Empty;

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithParameterName(nameof(GeminiJob.Prompt));

        _observationServiceMock.Verify(
            service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldObserveAndroidScreenExactlyOnce()
    {
        IAndroidScreenObservation observation =
            CreateObservation(
                [0x01, 0x02, 0x03, 0x04]);

        _observationServiceMock
            .Setup(service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(observation);

        /*
         * We intentionally stop the workflow after observation.
         * This test owns only the observation-call-count invariant.
         */
        _sessionManagerMock
            .Setup(sessionManager =>
                sessionManager.ExecuteWithToolSupportAsync<WorkflowProgress>(
                    It.IsAny<
                        AiUtility.GeminiUtilityServices.Models.GeminiGenerateRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<AiExecutionSettings>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<IProgress<WorkflowProgress>>()))
            .ThrowsAsync(
                new InvalidOperationException(
                    "Stop after request construction."));

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Stop after request construction.");

        _observationServiceMock.Verify(
            service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldPropagateObservationFailure()
    {
        const string expectedMessage =
            "Android screen observation failed.";

        _observationServiceMock
            .Setup(service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new InvalidOperationException(
                    expectedMessage));

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldNotInvokeSessionManager_WhenObservationFails()
    {
        _observationServiceMock
            .Setup(service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new InvalidOperationException(
                    "Android screen observation failed."));

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Android screen observation failed.");

        _sessionManagerMock.Verify(
            sessionManager =>
                sessionManager.ExecuteWithToolSupportAsync<WorkflowProgress>(
                    It.IsAny<
                        AiUtility.GeminiUtilityServices.Models.GeminiGenerateRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<AiExecutionSettings>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<IProgress<WorkflowProgress>>()),
            Times.Never);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldPropagateCancellationFromObservation()
    {
        const string expectedMessage =
            "Android screen observation was canceled.";

        _observationServiceMock
            .Setup(service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new OperationCanceledException(
                    expectedMessage));

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        await act.Should()
            .ThrowAsync<OperationCanceledException>()
            .WithMessage(expectedMessage);

        _sessionManagerMock.Verify(
            sessionManager =>
                sessionManager.ExecuteWithToolSupportAsync<WorkflowProgress>(
                    It.IsAny<
                        AiUtility.GeminiUtilityServices.Models.GeminiGenerateRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<AiExecutionSettings>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<IProgress<WorkflowProgress>>()),
            Times.Never);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldPassObservationImageBytesToGeminiRequest()
    {
        // Arrange
        byte[] expectedImageBytes =
        [
            0x10,
        0x20,
        0x30,
        0x40,
        0x50
        ];

        IAndroidScreenObservation observation =
            CreateObservation(
                expectedImageBytes);

        _observationServiceMock
            .Setup(service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(observation);

        GeminiGenerateRequest? capturedRequest =
            null;

        _sessionManagerMock
            .Setup(sessionManager =>
                sessionManager.ExecuteWithToolSupportAsync<WorkflowProgress>(
                    It.IsAny<GeminiGenerateRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<AiExecutionSettings>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<IProgress<WorkflowProgress>>()))
            .Callback<
                GeminiGenerateRequest,
                string,
                AiExecutionSettings,
                CancellationToken,
                IProgress<WorkflowProgress>?>(
                (
                    request,
                    _,
                    _,
                    _,
                    _) =>
                {
                    capturedRequest = request;
                })
            .ThrowsAsync(
                new InvalidOperationException(
                    "Stop after request capture."));

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        // Act & Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Stop after request capture.");

        capturedRequest
            .Should()
            .NotBeNull();

        GeminiPart[] imageParts =
            capturedRequest!
                .Contents
                .SelectMany(message => message.Parts)
                .Where(part => part.InlineData is not null)
                .ToArray();

        imageParts
            .Should()
            .ContainSingle(
                "the request must contain the image from the Android screen observation");

        imageParts[0]
            .InlineData!
            .RawData
            .ToArray()
            .Should()
            .Equal(
                expectedImageBytes,
                "Gemini must receive the exact image bytes from the observation");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldIncludeObservationOcrTextInGeminiRequest()
    {
        // Arrange
        const string expectedOcrText =
            "主城 任務 商城";

        byte[] imageBytes =
        [
            0x01,
        0x02,
        0x03,
        0x04
        ];

        IAndroidScreenObservation observation =
            CreateObservation(
                imageBytes,
                expectedOcrText);

        _observationServiceMock
            .Setup(service =>
                service.ObserveAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(observation);

        GeminiGenerateRequest? capturedRequest =
            null;

        _sessionManagerMock
            .Setup(sessionManager =>
                sessionManager.ExecuteWithToolSupportAsync<WorkflowProgress>(
                    It.IsAny<GeminiGenerateRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<AiExecutionSettings>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<IProgress<WorkflowProgress>>()))
            .Callback<
                GeminiGenerateRequest,
                string,
                AiExecutionSettings,
                CancellationToken,
                IProgress<WorkflowProgress>?>(
                (
                    request,
                    _,
                    _,
                    _,
                    _) =>
                {
                    capturedRequest = request;
                })
            .ThrowsAsync(
                new InvalidOperationException(
                    "Stop after request capture."));

        GeminiJobHandler<WorkflowProgress> sut =
            CreateSut();

        GeminiJob job =
            CreateValidGeminiJob();

        Func<Task> act =
            () => sut.AutoExecuteAsync(job);

        // Act & Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "Stop after request capture.");

        capturedRequest
            .Should()
            .NotBeNull();

        string[] requestTexts =
            capturedRequest!
                .Contents
                .SelectMany(message => message.Parts)
                .Select(part => part.RawText.ToString())
                .Where(text =>
                    !string.IsNullOrEmpty(text))
                .ToArray();

        requestTexts
            .Should()
            .Contain(
                text =>
                    text.Contains(
                        expectedOcrText,
                        StringComparison.Ordinal),
                "the Gemini request must contain OCR text from the acquired observation");
    }

    private GeminiJobHandler<WorkflowProgress> CreateSut()
    {
        return new GeminiJobHandler<WorkflowProgress>(
            _executionSettings,
            _registryMock.Object,
            _converter,
            _sessionManagerMock.Object,
            _observationServiceMock.Object,
            _progressMock.Object);
    }

    private static AiExecutionSettings CreateValidExecutionSettings()
    {
        return new AiExecutionSettings
        {
            MaxSteps = 10,
            ToolExecutionTimeout =
                TimeSpan.FromSeconds(30)
        };
    }

    private static GeminiJob CreateValidGeminiJob()
    {
        return new GeminiJob
        {
            UserTask = "Execute the current automation task.",
            Prompt = "Observe the current Android screen."
        };
    }

    private static IAndroidScreenObservation CreateObservation(
        byte[] imageBytes,
        string ocrText = "")
    {
        ArgumentNullException.ThrowIfNull(imageBytes);

        Mock<IAndroidScreenObservation> observationMock =
            new(MockBehavior.Strict);

        OcrResult ocrResult =
            new(
                ocrText,
                Array.Empty<OcrTextLine>());

        observationMock
            .SetupGet(observation => observation.ImageBytes)
            .Returns(imageBytes);

        observationMock
            .SetupGet(observation => observation.OcrResult)
            .Returns(ocrResult);

        return observationMock.Object;
    }

    private static GeminiToolConverter CreateGeminiToolConverter(
        IAiParameterSchemaGenerator parameterSchemaGenerator,
        IGeminiParameterPropertyMapper parameterPropertyMapper)
    {
        ArgumentNullException.ThrowIfNull(parameterSchemaGenerator);
        ArgumentNullException.ThrowIfNull(parameterPropertyMapper);

        ITypeUtilityService typeUtilityService =
            new TypeUtilityService();

        IJsonUtilityService jsonUtilityService =
            new JsonUtilityService(typeUtilityService);

        IEnumUtilityService enumUtilityService =
            new EnumUtilityService();

        return new GeminiToolConverter(
            jsonUtilityService,
            enumUtilityService,
            parameterSchemaGenerator,
            parameterPropertyMapper);
    }
}