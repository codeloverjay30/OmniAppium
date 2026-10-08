using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Mappers;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Services;
using AiUtility.ToolKits.Abstractions;
using AiUtility.ToolKits.Execution;
using AiUtility.ToolKits.Services;
using EnumUtilityServices;
using FluentAssertions;
using JsonUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using OmniAppium.EngineUtilityService.Services.Click;
using OmniAppium.EngineUtilityService.Services.Screenshots;
using OmniAppium.EngineUtilityService.Utilities;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppiumDemo.DependencyInjection;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using TypeUtilityServices;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests;

public sealed class ProductionDiCompositionTests
{
    [Fact]
    public void BuildServiceProvider_WithProductionServices_ShouldResolveFullGeminiDependencyGraph()
    {
        // Arrange: external boundary.
        var services = new ServiceCollection();

        var screenshotService =
            new Mock<IScreenshotService>(MockBehavior.Strict);

        services.AddSingleton<IScreenshotService>(
            screenshotService.Object);

        services.AddSingleton<IFileSystem>(
            new MockFileSystem());

        // Register the existing external Gemini and OCR dependencies
        // using the same instances and configuration as the passing
        // GeminiJobHandlerDiCompositionTests Arrange.
        //
        // Required here:
        //   AiExecutionSettings
        //   IGeminiToolRegistry
        //   GeminiToolConverter
        //   IGeminiSessionManager
        //   IProgress<WorkflowProgress>
        //   IOCRUtilityService
        //   ILoggerFactoryBaseUtilityService
        //
        // Also register the production Demo OCR and Gemini services
        // before BuildServiceProvider().

        // Arrange: Gemini execution settings.
        var aiExecutionSettings = new AiExecutionSettings
        {
            LastTokenCountNeededToBeKept = 5,
            MaxSteps = 20,
            Threshold =
                AiUtility.AiBaseUtilityServices.Consts.Constants
                    .ExecutionSettings.MAX_THRESHOLD,
            ToolExecutionTimeout = TimeSpan.FromMinutes(2),
            ForceSequentialToolExecution = true
        };

        // Arrange: external Gemini boundaries.
        var registry = new Mock<IGeminiToolRegistry>(
            MockBehavior.Strict);

        var sessionManager = new Mock<IGeminiSessionManager>(
            MockBehavior.Strict);
    
        // Arrange: external Appium click boundary.
        var clickService = new Mock<IClickService>(
            MockBehavior.Strict);
    
        var loggerFactoryService =
            new Mock<ILoggerFactoryBaseUtilityService>(
                MockBehavior.Strict);

        // Arrange: real Gemini tool converter dependencies.
        ITypeUtilityService typeUtilityService =
            new TypeUtilityService();

        IJsonUtilityService jsonUtilityService =
            new JsonUtilityService(typeUtilityService);

        IEnumUtilityService enumUtilityService =
            new EnumUtilityService();

        IAiParameterSchemaGenerator schemaGenerator =
            new GeminiSchemaGenerator(
                jsonUtilityService,
                typeUtilityService);

        IGeminiParameterPropertyMapper propertyMapper =
            new GeminiParameterPropertyMapper();

        var converter =
            new GeminiToolConverter(
                jsonUtilityService,
                enumUtilityService,
                schemaGenerator,
                propertyMapper);

        IProgress<WorkflowProgress> progress =
            new Progress<WorkflowProgress>();

        // Register external infrastructure dependencies.
        services.AddSingleton<ILoggerFactoryBaseUtilityService>(
            loggerFactoryService.Object);

        // Register real production OCR services.
        services.AddOcrUtilityServices();
        services.AddAndroidOcrServices();

        // Replace the real filesystem registration with a test filesystem.
        // This prevents diagnostic image operations from touching the host.
        services.Replace(
            ServiceDescriptor.Singleton<IFileSystem>(
                new MockFileSystem()));



        services.AddSingleton<IClickService>(
            clickService.Object);
    
        // Register the real production observation service.
        services.AddAndroidScreenObservation();

        // Register Gemini using the same production extension as Demo.
        services.AddGeminiAutomation(
            aiExecutionSettings,
            registry.Object,
            converter,
            sessionManager.Object,
            progress);
    

        // Arrange: real production grounding and planner services.
        services.AddSingleton<
            IOcrGroundedSnapshotFactory,
            OcrGroundedSnapshotFactory>();

        services.AddSingleton<
            IOcrGroundedSnapshotFreshnessGuard,
            OcrGroundedSnapshotFreshnessGuard>();

        services.AddSingleton<
            IAiToolExecutionStateAccessor<
                OcrGroundedPlannerExecutionState>,
            AiToolExecutionStateAccessor<
                OcrGroundedPlannerExecutionState>>();

        services.AddSingleton<
            IOcrGroundedPlannerExecutionScopeFactory,
            OcrGroundedPlannerExecutionScopeFactory>();

        // Act.
        using ServiceProvider provider =
            services.BuildServiceProvider(
                new ServiceProviderOptions
                {
                    ValidateOnBuild = true,
                    ValidateScopes = true
                });

        // Assert: real implementations must be resolved.
        provider.GetRequiredService<
                IAndroidScreenObservationService>()
            .Should()
            .BeOfType<AndroidScreenObservationService>();

        provider.GetRequiredService<
                IAndroidScreenOcrService>()
            .Should()
            .BeOfType<AndroidScreenOcrService>();

        provider.GetRequiredService<
                IOcrGroundedSnapshotFactory>()
            .Should()
            .BeOfType<OcrGroundedSnapshotFactory>();

        provider.GetRequiredService<
                IOcrGroundedSnapshotFreshnessGuard>()
            .Should()
            .BeOfType<OcrGroundedSnapshotFreshnessGuard>();

        provider.GetRequiredService<
                IOcrGroundedPlannerExecutionScopeFactory>()
            .Should()
            .BeOfType<OcrGroundedPlannerExecutionScopeFactory>();

        provider.GetRequiredService<IGeminiJobHandler>()
            .Should()
            .BeOfType<GeminiJobHandler<WorkflowProgress>>();
    }
}