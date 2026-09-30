using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Mappers;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AiUtility.ToolKits.Abstractions;
using AiUtility.ToolKits.Consts;
using AiUtility.ToolKits.Services;
using CommonModels;
using EnumUtilityServices;
using FluentAssertions;
using JsonUtilityServices;
using Moq;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityService.Utilities;
using System.Collections.Concurrent;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace OmniAppium.EngineUtilityServices.Tests;

public class TestProgress : WorkflowProgress
{
}

[SupportedOSPlatform("windows")]
public class GeminiJobHandlerTests
{
    private Mock<IGeminiToolRegistry> _mockRegistry = null!;
    private Mock<IJsonUtilityService> _mockJsonUtilityService = null!;
    private Mock<IEnumUtilityService> _mockEnumUtilityService = null!;
    private Mock<GeminiToolConverter> _mockConverter = null!;
    private Mock<IGeminiSessionManager> _mockSessionManager = null!;
    private Mock<IScreenshotService> _mockScreenshotService = null!;
    private Mock<IProgress<TestProgress>> _mockProgress = null!;
    private AiExecutionSettings _settings = null!;
    private GeminiJobHandler<TestProgress> _handler = null!;

    private readonly Mock<IAiParameterSchemaGenerator>
        _parameterSchemaGeneratorMock;

    private readonly Mock<IGeminiParameterPropertyMapper>
        _parameterPropertyMapperMock;


    public GeminiJobHandlerTests()
    {
        _parameterSchemaGeneratorMock =
            new Mock<IAiParameterSchemaGenerator>(MockBehavior.Strict);

        _parameterPropertyMapperMock =
            new Mock<IGeminiParameterPropertyMapper>(
                MockBehavior.Strict);

        Setup();
    }

    private void Setup()
    {
        _mockRegistry = new Mock<IGeminiToolRegistry>();
        _mockJsonUtilityService = new Mock<IJsonUtilityService>();
        _mockEnumUtilityService = new Mock<IEnumUtilityService>();

        _mockConverter = new Mock<GeminiToolConverter>(
            _mockJsonUtilityService.Object,
            _mockEnumUtilityService.Object,
            _parameterSchemaGeneratorMock.Object,
            _parameterPropertyMapperMock.Object,
            AiToolConstants.DefaultDescription,
            AiToolConstants.DefaultParameterDescription)
        {
            CallBase = true
        };

        _mockSessionManager = new Mock<IGeminiSessionManager>();
        _mockScreenshotService = new Mock<IScreenshotService>();
        _mockProgress = new Mock<IProgress<TestProgress>>();

        _settings = new AiExecutionSettings
        {
            ToolExecutionTimeout = TimeSpan.FromSeconds(30)
        };

        _mockRegistry
            .Setup(r => r.GetAllTools())
            .Returns(new List<GeminiToolMetadata>());

        _mockScreenshotService
            .Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<ImageFormat>()))
            .Returns([0x01]);

        _handler = new GeminiJobHandler<TestProgress>(
            _settings,
            _mockRegistry.Object,
            _mockConverter.Object,
            _mockSessionManager.Object,
            _mockScreenshotService.Object,
            _mockProgress.Object);
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForGeminiJob()
    {
        // Arrange
        var job = new GeminiJob();

        // Act
        bool result = _handler.CanHandle(job);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrowException_WhenRequiredFieldsAreMissing()
    {
        // Arrange
        var invalidJob = new GeminiJob
        {
            UserTask = "Do something"
        };

        // Act
        Func<Task> act =
            () => _handler.AutoExecuteAsync(invalidJob);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithMessage("*Prompt*");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldCorrectlyInvokeSessionManager()
    {
        // Arrange
        var gJob = new GeminiJob
        {
            Prompt = "Find the login button",
            UserTask = "Login Task"
        };

        byte[] fakeScreenshot = [0x01, 0x02, 0x03];

        _mockScreenshotService
            .Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<ImageFormat>()))
            .Returns(fakeScreenshot);

        // Act
        await _handler.AutoExecuteAsync(gJob);

        // Assert
        _mockSessionManager.Verify(
            s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.Is<GeminiGenerateRequest>(r => r.Prompt == gJob.Prompt),
                gJob.UserTask,
                _settings,
                It.IsAny<CancellationToken>(),
                _mockProgress.Object),
            Times.Once);
    }

    [Fact]
    public void SetExecutionSettings_ShouldNotThrow_WhenSettingsAreValid()
    {
        // Arrange
        var newSettings = new AiExecutionSettings
        {
            ToolExecutionTimeout = TimeSpan.FromSeconds(60)
        };

        // Act
        Action act =
            () => _handler.SetExecutionSettings(newSettings);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldPropagateCancellation()
    {
        // Arrange
        var gJob = new GeminiJob
        {
            Prompt = "Test Prompt",
            UserTask = "Test Task"
        };

        _mockSessionManager
            .Setup(s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.IsAny<GeminiGenerateRequest>(),
                It.IsAny<string>(),
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()))
            .ThrowsAsync(new OperationCanceledException());

        // Act
        Func<Task> act =
            () => _handler.AutoExecuteAsync(gJob);

        // Assert
        await act.Should()
            .ThrowAsync<OperationCanceledException>()
            .WithMessage("*canceled*");

        _mockSessionManager.Verify(
            s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.IsAny<GeminiGenerateRequest>(),
                It.IsAny<string>(),
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldHandleEmptyToolRegistry_Successfully()
    {
        // Arrange
        var gJob = new GeminiJob
        {
            Prompt = "Simple Task",
            UserTask = "Task"
        };

        // Act
        await _handler.AutoExecuteAsync(gJob);

        // Assert
        _mockSessionManager.Verify(
            s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.Is<GeminiGenerateRequest>(r => r.Prompt == gJob.Prompt),
                It.IsAny<string>(),
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()),
            Times.Once);

        _mockConverter.Verify(
            c => c.ToToolDeclaration(It.IsAny<ToolMetadataBase>()),
            Times.Never);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrowException_WhenConverterFails()
    {
        // Arrange
        var gJob = new GeminiJob
        {
            Prompt = "Task with tools",
            UserTask = "Task"
        };

        string dummyName = "TestFunction";
        var dummyMethod =
            typeof(object).GetMethod(
                nameof(object.ToString),
                Type.EmptyTypes)!;

        var dummyParams =
            Array.Empty<System.Reflection.ParameterInfo>();

        var dummyAttrs =
            Enumerable.Empty<Attribute>();

        var tool = new GeminiToolMetadata(
            dummyName,
            dummyMethod,
            dummyParams,
            (Func<object?, object?[]?, object?>)((_, _) => null),
            (Func<object>)(() => new object()),
            dummyAttrs);

        _mockRegistry
            .Setup(r => r.GetAllTools())
            .Returns(new List<GeminiToolMetadata>
            {
                tool
            });

        _mockConverter
            .Setup(c => c.ToToolDeclaration(It.IsAny<ToolMetadataBase>()))
            .Throws(
                new InvalidOperationException(
                    "Converter failed"));

        // Act
        Func<Task> act =
            () => _handler.AutoExecuteAsync(gJob);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Converter failed");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrowException_WhenScreenshotIsEmpty()
    {
        // Arrange
        var gJob = new GeminiJob
        {
            Prompt = "Task without screen",
            UserTask = "Task"
        };

        _mockScreenshotService
            .Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<ImageFormat>()))
            .Returns(Array.Empty<byte>());

        // Act
        Func<Task> act =
            () => _handler.AutoExecuteAsync(gJob);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*empty image buffer*");
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldHandleEmptyTools_Successfully()
    {
        // Arrange
        var gJob = new GeminiJob
        {
            Prompt = "Simple Task",
            UserTask = "Task"
        };

        // Act
        await _handler.AutoExecuteAsync(gJob);

        // Assert
        _mockSessionManager.Verify(
            s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.IsAny<GeminiGenerateRequest>(),
                It.IsAny<string>(),
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldUseUpdatedSettings_AfterSetExecutionSettingsIsCalled()
    {
        // Arrange
        var gJob = new GeminiJob
        {
            Prompt = "Test",
            UserTask = "Task"
        };

        var newSettings = new AiExecutionSettings
        {
            ToolExecutionTimeout = TimeSpan.FromSeconds(99)
        };

        // Act
        _handler.SetExecutionSettings(newSettings);
        await _handler.AutoExecuteAsync(gJob);

        // Assert
        _mockSessionManager.Verify(
            s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.IsAny<GeminiGenerateRequest>(),
                It.IsAny<string>(),
                It.Is<AiExecutionSettings>(
                    settings =>
                        settings.ToolExecutionTimeout ==
                        TimeSpan.FromSeconds(99)),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()),
            Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AutoExecuteAsync_ShouldThrowException_WhenPromptIsWhiteSpace(
        string invalidPrompt)
    {
        // Arrange
        var invalidJob = new GeminiJob
        {
            Prompt = invalidPrompt,
            UserTask = "Valid Task"
        };

        // Act
        Func<Task> act =
            () => _handler.AutoExecuteAsync(invalidJob);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("*Prompt*");
    }

    [Theory]
    [InlineData("This is a prompt")]
    [InlineData("WTF")]
    public async Task AutoExecuteAsync_ShouldNotThrowException_WhenPromptIsNotWhiteSpace(
        string validPrompt)
    {
        // Arrange
        var validJob = new GeminiJob
        {
            Prompt = validPrompt,
            UserTask = "Valid Task"
        };

        // Act
        Func<Task> act =
            () => _handler.AutoExecuteAsync(validJob);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldHandleConcurrentRequests_Independently()
    {
        // Arrange
        var job1 = new GeminiJob
        {
            Prompt = "Prompt 1",
            UserTask = "Task 1"
        };

        var job2 = new GeminiJob
        {
            Prompt = "Prompt 2",
            UserTask = "Task 2"
        };

        // Act
        Task task1 =
            _handler.AutoExecuteAsync(job1);

        Task task2 =
            _handler.AutoExecuteAsync(job2);

        await Task.WhenAll(task1, task2);

        // Assert
        _mockSessionManager.Verify(
            s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.Is<GeminiGenerateRequest>(
                    r => r.Prompt == "Prompt 1"),
                "Task 1",
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()),
            Times.Once);

        _mockSessionManager.Verify(
            s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.Is<GeminiGenerateRequest>(
                    r => r.Prompt == "Prompt 2"),
                "Task 2",
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldPassProgressBarToSessionManager()
    {
        // Arrange
        var gJob = new GeminiJob
        {
            Prompt = "Test",
            UserTask = "Task"
        };

        // Act
        await _handler.AutoExecuteAsync(gJob);

        // Assert
        _mockSessionManager.Verify(
            s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.IsAny<GeminiGenerateRequest>(),
                It.IsAny<string>(),
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                _mockProgress.Object),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldOverwriteDataInStaticInstance()
    {
        // Arrange
        var job1 = new GeminiJob
        {
            Prompt = "Prompt A",
            UserTask = "Task A"
        };

        var job2 = new GeminiJob
        {
            Prompt = "Prompt B",
            UserTask = "Task B"
        };

        // Act
        await _handler.AutoExecuteAsync(job1);
        await _handler.AutoExecuteAsync(job2);

        // Assert
        _mockSessionManager.Verify(
            s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.Is<GeminiGenerateRequest>(
                    r => r.Prompt == "Prompt B"),
                "Task B",
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()),
            Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ConcurrencyTest_ShouldEnsureDataConsistency()
    {
        // Arrange
        const int concurrentExecutionCount = 2;

        using var semaphore = new SemaphoreSlim(
            initialCount: 0,
            maxCount: concurrentExecutionCount);

        var capturedRequests =
            new ConcurrentBag<(string? Prompt, string UserTask)>();

        _mockSessionManager
            .Setup(s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.IsAny<GeminiGenerateRequest>(),
                It.IsAny<string>(),
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()))
            .Returns(
                async (
                    GeminiGenerateRequest request,
                    string userTask,
                    AiExecutionSettings _,
                    CancellationToken cancellationToken,
                    IProgress<TestProgress> _) =>
                {
                    capturedRequests.Add(
                        (request.Prompt, userTask));

                    await semaphore.WaitAsync(
                        cancellationToken);

                    return new StatusJsonModels();
                });

        var job1 = new GeminiJob
        {
            Prompt = "CRITICAL_A",
            UserTask = "TASK_A"
        };

        var job2 = new GeminiJob
        {
            Prompt = "CRITICAL_B",
            UserTask = "TASK_B"
        };

        // Act
        Task task1 =
            _handler.AutoExecuteAsync(job1);

        Task task2 =
            _handler.AutoExecuteAsync(job2);

        semaphore.Release(
            concurrentExecutionCount);

        Func<Task> act =
            async () =>
                await Task.WhenAll(
                    task1,
                    task2);

        // Assert
        await act.Should().NotThrowAsync();

        capturedRequests.Should().BeEquivalentTo(
        [
            ("CRITICAL_A", "TASK_A"),
            ("CRITICAL_B", "TASK_B")
        ]);

        _mockSessionManager.Verify(
            s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.IsAny<GeminiGenerateRequest>(),
                It.IsAny<string>(),
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()),
            Times.Exactly(concurrentExecutionCount));
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldKeepConcurrentRequestContentsIndependent()
    {
        var requests = new ConcurrentQueue<GeminiGenerateRequest>();
        var release = new TaskCompletionSource<bool>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        _mockSessionManager
            .Setup(session => session.ExecuteWithToolSupportAsync<TestProgress>(
                It.IsAny<GeminiGenerateRequest>(),
                It.IsAny<string>(),
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()))
            .Returns(async (
                GeminiGenerateRequest request,
                string userTask,
                AiExecutionSettings settings,
                CancellationToken cancellationToken,
                IProgress<TestProgress> progress) =>
            {
                requests.Enqueue(request);
                await release.Task.WaitAsync(cancellationToken);
                return new StatusJsonModels
                {
                    StatusList = [new StatusJsonModel { IsSuccess = true }]
                };
            });

        Task firstTask = _handler.AutoExecuteAsync(new GeminiJob
        {
            Prompt = "First isolated prompt",
            UserTask = "First task"
        });
        Task secondTask = _handler.AutoExecuteAsync(new GeminiJob
        {
            Prompt = "Second isolated prompt",
            UserTask = "Second task"
        });
        release.SetResult(true);
        await Task.WhenAll(firstTask, secondTask);

        GeminiGenerateRequest[] captured = requests.ToArray();
        captured.Should().HaveCount(2);
        captured[0].Should().NotBeSameAs(captured[1]);
        captured[0].Contents.Should().NotBeSameAs(captured[1].Contents);
        captured[0].Contents.Should().ContainSingle();
        captured[1].Contents.Should().ContainSingle();
        captured[0].Contents[0].Parts.Should()
            .Contain(part => part.Text == "First isolated prompt")
            .And.NotContain(part => part.Text == "Second isolated prompt");
        captured[1].Contents[0].Parts.Should()
            .Contain(part => part.Text == "Second isolated prompt")
            .And.NotContain(part => part.Text == "First isolated prompt");

        GeminiInlineData firstImage = captured[0].Contents[0].Parts
            .Single(part => part.InlineData is not null).InlineData!;
        GeminiInlineData secondImage = captured[1].Contents[0].Parts
            .Single(part => part.InlineData is not null).InlineData!;
        firstImage.Should().NotBeSameAs(secondImage);
        string secondImageData = secondImage.Data;
        firstImage.Data = "changed first request";
        secondImage.Data.Should().Be(secondImageData);

        captured[0].Contents.Clear();
        await _handler.AutoExecuteAsync(new GeminiJob
        {
            Prompt = "Subsequent isolated prompt",
            UserTask = "Subsequent task"
        });
        GeminiGenerateRequest subsequent = requests.Last();
        subsequent.Contents.Should().ContainSingle();
        subsequent.Contents[0].Parts.Should()
            .Contain(part => part.Text == "Subsequent isolated prompt");
        captured[1].Contents.Should().ContainSingle();
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldSendJpegBytesWithMatchingMimeType()
    {
        byte[] screenshotBytes = [0xff, 0xd8, 0xff, 0xd9];
        GeminiGenerateRequest? captured = null;
        _mockScreenshotService
            .Setup(service => service.GetBytesOfCachedScreenshotBytes(
                It.Is<ImageFormat>(format => format.Guid == ImageFormat.Jpeg.Guid)))
            .Returns(screenshotBytes);
        _mockSessionManager
            .Setup(session => session.ExecuteWithToolSupportAsync<TestProgress>(
                It.IsAny<GeminiGenerateRequest>(),
                It.IsAny<string>(),
                It.IsAny<AiExecutionSettings>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<TestProgress>>()))
            .Returns((
                GeminiGenerateRequest request,
                string userTask,
                AiExecutionSettings settings,
                CancellationToken cancellationToken,
                IProgress<TestProgress> progress) =>
            {
                captured = request;
                return Task.FromResult(new StatusJsonModels
                {
                    StatusList = [new StatusJsonModel { IsSuccess = true }]
                });
            });

        await _handler.AutoExecuteAsync(new GeminiJob
        {
            Prompt = "Inspect the screenshot",
            UserTask = "Inspect the current screen"
        });

        _mockScreenshotService.Verify(service => service.TakeScreenshot(), Times.Once);
        _mockScreenshotService.Verify(
            service => service.GetBytesOfCachedScreenshotBytes(
                It.Is<ImageFormat>(format => format.Guid == ImageFormat.Jpeg.Guid)),
            Times.Once);
        captured.Should().NotBeNull();
        GeminiInlineData image = captured!.Contents
            .SelectMany(message => message.Parts)
            .Single(part => part.InlineData is not null)
            .InlineData!;
        image.MimeType.Should().Be("image/jpeg");
        image.Data.Should().Be(Convert.ToBase64String(screenshotBytes));
    }

}
