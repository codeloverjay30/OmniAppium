using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Models;
using AiUtility.GeminiUtilityServices.DataAnnotations;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AiUtility.ToolKits.Abstractions;
using CommonModels;
using FluentAssertions;
using Moq;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityService.Utilities;
using System.Drawing.Imaging;
using System.Timers;

namespace OmniAppium.EngineUtilityServices.Tests;

public class TestProgress : WorkflowProgress { }

public class GeminiJobHandlerTests
{
    private Mock<IToolRegistry<ToolMetadataBase , GeminiToolAttribute>> _mockRegistry;
    private Mock<IToolDispatcher<ToolMetadataBase , GeminiToolAttribute>> _mockDispatcher;
    private Mock<IAiToolConverter<GeminiToolDeclaration>> _mockConverter;
    private Mock<IGeminiSessionManager> _mockSessionManager;
    private Mock<IScreenshotService> _mockScreenshotService;
    private Mock<IProgress<TestProgress>> _mockProgress;
    private AiExecutionSettings _settings;
    private GeminiJobHandler<TestProgress> _handler;

    public GeminiJobHandlerTests()
    {
        Setup();
    }

    private void Setup()
    {
        _mockRegistry = new Mock<IToolRegistry<ToolMetadataBase , GeminiToolAttribute>>();
        _mockDispatcher = new Mock<IToolDispatcher<ToolMetadataBase , GeminiToolAttribute>>();
        _mockConverter = new Mock<IAiToolConverter<AiUtility.GeminiKits.Models.GeminiToolDeclaration>>();
        _mockSessionManager = new Mock<IGeminiSessionManager>();
        _mockScreenshotService = new Mock<IScreenshotService>();
        _mockProgress = new Mock<IProgress<TestProgress>>();

        _settings = new AiExecutionSettings
        {
            ToolExecutionTimeout = TimeSpan.FromSeconds(30)
        };

        _handler = new GeminiJobHandler<TestProgress>(
            _settings ,
            _mockRegistry.Object ,
            _mockDispatcher.Object ,
            _mockConverter.Object ,
            _mockSessionManager.Object ,
            _mockScreenshotService.Object ,
            _mockProgress.Object
        );
    }

    [Fact]
    public void CanHandle_ShouldReturnTrue_ForGeminiJob()
    {
        Setup();
        // Arrange
        var job = new GeminiJob();

        // Act
        var result = _handler.CanHandle(job);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrowException_WhenRequiredFieldsAreMissing()
    {
        Setup();
        // Arrange: 缺少 Prompt 的 Job
        var invalidJob = new GeminiJob { UserTask = "Do something" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.AutoExecuteAsync(invalidJob));
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldCorrectilyInvokeSessionManager()
    {
        Setup();
        // Arrange
        var gJob = new GeminiJob
        {
            Prompt = "Find the login button" ,
            UserTask = "Login Task"
        };

        var fakeScreenshot = new byte [ ] { 0x01 , 0x02 , 0x03 };
        _mockScreenshotService
            .Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<ImageFormat>()))
            .Returns(fakeScreenshot);

        _mockRegistry
            .Setup(r => r.GetAllTools())
            .Returns(new List<ToolMetadataBase>());

        // Act
        await _handler.AutoExecuteAsync(gJob);

        // Assert: 驗證是否有呼叫 SessionManager 執行 AI 邏輯
        _mockSessionManager.Verify(s => s.ExecuteWithToolSupportAsync<TestProgress>(
            It.Is<GeminiGenerateRequest>(r => r.Prompt == gJob.Prompt) ,
            gJob.UserTask ,
            _settings ,
            It.IsAny<CancellationToken>() ,
            _mockProgress.Object
        ) , Times.Once);
    }

    [Fact]
    public void SetExecutionSettings_ShouldUpdateSettings_WhenValid()
    {
        Setup();
        // Arrange
        var newSettings = new AiExecutionSettings
        {
            ToolExecutionTimeout = TimeSpan.FromSeconds(60)
        };

        // Act
        _handler.SetExecutionSettings(newSettings);

        // Assert
        Assert.Equal(newSettings.ToolExecutionTimeout , _handler._aiExecutionSettings.ToolExecutionTimeout);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldPropagateCancellation()
    {
        // Arrange
        var gJob = new GeminiJob
        {
            Prompt = "Test Prompt" ,
            UserTask = "Test Task"
        };

        // 模擬截圖回傳
        _mockScreenshotService
            .Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<ImageFormat>()))
            .Returns(new byte [ ] { 0x01 });

        // 模擬工具註冊
        _mockRegistry.Setup(r => r.GetAllTools()).Returns(new List<ToolMetadataBase>());

        // 重點：模擬 SessionManager 在被呼叫時拋出取消異常
        // 因為 GeminiJobHandler 內部使用的是基於 Timeout 的 CTS.Token，
        // 我們透過 Mock 確保當該方法執行時，會反應出取消狀態。
        _mockSessionManager
            .Setup(s => s.ExecuteWithToolSupportAsync<TestProgress>(
                It.IsAny<GeminiGenerateRequest>() ,
                It.IsAny<string>() ,
                It.IsAny<AiExecutionSettings>() ,
                It.IsAny<CancellationToken>() ,
                It.IsAny<IProgress<TestProgress>>()
            ))
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        // 驗證 Handler 是否將來自 SessionManager 的取消異常正確向上拋出
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            _handler.AutoExecuteAsync(gJob));

        _mockSessionManager.Verify(s => s.ExecuteWithToolSupportAsync<TestProgress>(
            It.IsAny<GeminiGenerateRequest>() ,
            It.IsAny<string>() ,
            It.IsAny<AiExecutionSettings>() ,
            It.IsAny<CancellationToken>() ,
            It.IsAny<IProgress<TestProgress>>()
        ) , Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldHandleEmptyToolRegistry_Successfully()
    {
        Setup();
        // Arrange: 模擬工具註冊表回傳空清單
        var gJob = new GeminiJob { Prompt = "Simple Task" , UserTask = "Task" };

        _mockRegistry
            .Setup(r => r.GetAllTools())
            .Returns(new List<ToolMetadataBase>()); // 回傳空清單

        _mockScreenshotService
            .Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<ImageFormat>()))
            .Returns(new byte [ ] { 0x01 });

        // Act
        await _handler.AutoExecuteAsync(gJob);

        // Assert: 驗證即使沒有工具，SessionManager 仍應被呼叫且 tools 參數為空
        _mockSessionManager.Verify(s => s.ExecuteWithToolSupportAsync<TestProgress>(
            It.Is<GeminiGenerateRequest>(r => r.Prompt == gJob.Prompt) ,
            It.IsAny<string>() ,
            It.IsAny<AiExecutionSettings>() ,
            It.IsAny<CancellationToken>() ,
            It.IsAny<IProgress<TestProgress>>()
        ) , Times.Once);

        _mockConverter.Verify(c => c.ToToolDeclaration(It.IsAny<ToolMetadataBase>()) , Times.Never);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldThrowException_WhenConverterFails()
    {
        Setup();
        // Arrange
        var gJob = new GeminiJob { Prompt = "Task with tools" , UserTask = "Task" };

        // 建立 Dummy 參數以符合 ToolMetadataBase 的 protected constructor 要求
        var dummyName = "TestFunction";
        var dummyMethod = typeof(object).GetMethod(nameof(object.ToString));
        var dummyParams = Array.Empty<System.Reflection.ParameterInfo>();
        var dummyAttrs = Enumerable.Empty<Attribute>();

        // 在 Mock 中傳入建構參數
        var mockTool = new Mock<ToolMetadataBase>(
            dummyName ,
            dummyMethod ,
            dummyParams ,
            (Func<object? , object? [ ]? , object?>)((obj , p) => null) , // FastInvoke
            (Func<object>)(() => new object()) , // InstanceFactory
            dummyAttrs
        );

        _mockRegistry
            .Setup(r => r.GetAllTools())
            .Returns(new List<ToolMetadataBase> { mockTool.Object });

        _mockConverter
            .Setup(c => c.ToToolDeclaration(It.IsAny<ToolMetadataBase>()))
            .Throws(new InvalidOperationException("Converter failed"));

        _mockScreenshotService
            .Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<System.Drawing.Imaging.ImageFormat>()))
            .Returns(new byte [ ] { 0x01 });

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.AutoExecuteAsync(gJob));
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldHandleScreenshotServiceReturningNull_Successfully()
    {
        Setup();
        // Arrange: 模擬截圖服務回傳空值（例如設備斷線時）
        var gJob = new GeminiJob { Prompt = "Task without screen" , UserTask = "Task" };

        _mockScreenshotService
            .Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<ImageFormat>()))
            .Returns((byte [ ])null); // 回傳 null

        _mockRegistry
            .Setup(r => r.GetAllTools())
            .Returns(new List<ToolMetadataBase>());

        // Act
        // 注意：這裡取決於 AddUserMessage 內部是否允許 null。如果內部會報錯，此測試應改為 Assert.Throws
        await _handler.AutoExecuteAsync(gJob);

        // Assert: 驗證流程是否能走到 SessionManager
        _mockSessionManager.Verify(s => s.ExecuteWithToolSupportAsync<TestProgress>(
            It.IsAny<GeminiGenerateRequest>() ,
            It.IsAny<string>() ,
            It.IsAny<AiExecutionSettings>() ,
            It.IsAny<CancellationToken>() ,
            It.IsAny<IProgress<TestProgress>>()
        ) , Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldHandleEmptyTools_Successfully()
    {
        // Arrange
        var gJob = new GeminiJob { Prompt = "Simple Task" , UserTask = "Task" };
        _mockRegistry.Setup(r => r.GetAllTools()).Returns(new List<ToolMetadataBase>());

        _mockScreenshotService
            .Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<ImageFormat>()))
            .Returns(new byte [ ] { 0x12 });

        // Act
        await _handler.AutoExecuteAsync(gJob);

        // Assert
        _mockSessionManager.Verify(s => s.ExecuteWithToolSupportAsync<TestProgress>(
            It.IsAny<GeminiGenerateRequest>() ,
            It.IsAny<string>() ,
            It.IsAny<AiExecutionSettings>() ,
            It.IsAny<CancellationToken>() ,
            It.IsAny<IProgress<TestProgress>>()
        ) , Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldUseUpdatedSettings_AfterSetExecutionSettingsIsCalled()
    {
        Setup();
        // Arrange
        var gJob = new GeminiJob { Prompt = "Test" , UserTask = "Task" };
        var newSettings = new AiExecutionSettings
        {
            ToolExecutionTimeout = TimeSpan.FromSeconds(99) // 特殊的時間
        };

        _mockScreenshotService
            .Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<ImageFormat>()))
            .Returns(new byte [ ] { 0x01 });

        // Act
        _handler.SetExecutionSettings(newSettings);
        await _handler.AutoExecuteAsync(gJob);

        // Assert: 驗證傳給 sessionManager 的 settings 物件是更新後的那一個
        _mockSessionManager.Verify(s => s.ExecuteWithToolSupportAsync<TestProgress>(
            It.IsAny<GeminiGenerateRequest>() ,
            It.IsAny<string>() ,
            It.Is<AiExecutionSettings>(settings => settings.ToolExecutionTimeout.TotalSeconds == 99) ,
            It.IsAny<CancellationToken>() ,
            It.IsAny<IProgress<TestProgress>>()
        ) , Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AutoExecuteAsync_ShouldThrowException_WhenPromptIsWhiteSpace(string invalidPrompt)
    {
        Setup();
        // Arrange
        var invalidJob = new GeminiJob
        {
            Prompt = invalidPrompt ,
            UserTask = "Valid Task"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.AutoExecuteAsync(invalidJob));
    }

    [Theory]
    [InlineData("This is a prompt")]
    [InlineData("WTF")]
    public async Task AutoExecuteAsync_ShouldNotThrowException_WhenPromptIsNotWhiteSpace(string validPrompt)
    {
        Setup();
        // Arrange
        var invalidJob = new GeminiJob
        {
            Prompt = validPrompt ,
            UserTask = "Valid Task"
        };

        // Act & Assert
        // await Assert.ThrowsAsync<ArgumentException>(() => _handler.AutoExecuteAsync(invalidJob));
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldHandleConcurrentRequests_Independently()
    {
        Setup();
        // Arrange
        var job1 = new GeminiJob { Prompt = "Prompt 1" , UserTask = "Task 1" };
        var job2 = new GeminiJob { Prompt = "Prompt 2" , UserTask = "Task 2" };

        // Act: 模擬併發執行
        var task1 = _handler.AutoExecuteAsync(job1);
        var task2 = _handler.AutoExecuteAsync(job2);

        await Task.WhenAll(task1 , task2);

        // Assert: 驗證 SessionManager 接收到的 Request 內容與對應的 Job 是一致的
        // 如果這個測試失敗，代表你的 DefaultRequest 物件被複用了，導致資料錯亂
        _mockSessionManager.Verify(s => s.ExecuteWithToolSupportAsync<TestProgress>(
            It.Is<GeminiGenerateRequest>(r => r.Prompt == "Prompt 1") , "Task 1" , It.IsAny<AiExecutionSettings>() , It.IsAny<CancellationToken>() , It.IsAny<IProgress<TestProgress>>()) , Times.Once);

        _mockSessionManager.Verify(s => s.ExecuteWithToolSupportAsync<TestProgress>(
            It.Is<GeminiGenerateRequest>(r => r.Prompt == "Prompt 2") , "Task 2" , It.IsAny<AiExecutionSettings>() , It.IsAny<CancellationToken>() , It.IsAny<IProgress<TestProgress>>()) , Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldPassProgressBarToSessionManager()
    {
        Setup();
        // Arrange
        var gJob = new GeminiJob { Prompt = "Test" , UserTask = "Task" };

        // Act
        await _handler.AutoExecuteAsync(gJob);

        // Assert
        _mockSessionManager.Verify(s => s.ExecuteWithToolSupportAsync<TestProgress>(
            It.IsAny<GeminiGenerateRequest>() ,
            It.IsAny<string>() ,
            It.IsAny<AiExecutionSettings>() ,
            It.IsAny<CancellationToken>() ,
            _mockProgress.Object // 關鍵：確保注入的 Progress 物件確實被傳下去了
        ) , Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ShouldOverwriteDataInStaticInstance()
    {
        Setup();
        // Arrange
        var job1 = new GeminiJob { Prompt = "Prompt A" , UserTask = "Task A" };
        var job2 = new GeminiJob { Prompt = "Prompt B" , UserTask = "Task B" };

        _mockScreenshotService.Setup(s => s.GetBytesOfCachedScreenshotBytes(It.IsAny<ImageFormat>()))
            .Returns(new byte [ ] { 0x01 });

        // Act
        await _handler.AutoExecuteAsync(job1);
        await _handler.AutoExecuteAsync(job2);

        // Assert: 驗證第二次執行時，傳給 SessionManager 的 Prompt 確實是第二次的資料
        _mockSessionManager.Verify(s => s.ExecuteWithToolSupportAsync<TestProgress>(
            It.Is<GeminiGenerateRequest>(r => r.Prompt == "Prompt B") ,
            "Task B" ,
            It.IsAny<AiExecutionSettings>() ,
            It.IsAny<CancellationToken>() ,
            It.IsAny<IProgress<TestProgress>>()
        ) , Times.Once);
    }

    [Fact]
    public async Task AutoExecuteAsync_ConcurrencyTest_ShouldEnsureDataConsistency()
    {
        Setup();

        // Arrange
        const int concurrentExecutionCount = 2;

        using var semaphore = new SemaphoreSlim(
            initialCount: 0,
            maxCount: concurrentExecutionCount);

        var capturedRequests =
            new System.Collections.Concurrent.ConcurrentBag<(string? Prompt, string UserTask)>();

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
                    capturedRequests.Add((request.Prompt, userTask));

                    await semaphore.WaitAsync(cancellationToken);

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
        var task1 = _handler.AutoExecuteAsync(job1);
        var task2 = _handler.AutoExecuteAsync(job2);

        semaphore.Release(concurrentExecutionCount);

        Func<Task> act = async () =>
            await Task.WhenAll(task1, task2);

        // Assert
        await act.Should().NotThrowAsync();

        capturedRequests.Should().BeEquivalentTo(
        [
            ("CRITICAL_A" , "TASK_A") ,
        ("CRITICAL_B" , "TASK_B")
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

}
