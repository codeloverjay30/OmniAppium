using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Mappers;
using AiUtility.GeminiKits.Services;
using AiUtility.GeminiUtilityServices.Services;
using AiUtility.ToolKits.Services;
using EnumUtilityServices;
using FluentAssertions;
using JsonUtilityServices;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OmniAppium.EngineUtilityService.Utilities;
using OmniAppium.EngineUtilityServices.Services.Observation;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppiumDemo.DependencyInjection;
using TypeUtilityServices;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests;

public sealed class GeminiJobHandlerDiCompositionTests
{
    [Fact]
    public void BuildServiceProvider_WhenGeminiHandlerDependenciesAreRegistered_ShouldResolveHandler()
    {
        // Arrange

        // Must invoke the same production registration method
        // used by OmniAppiumDemo/Program.cs.
        //
        // Example:
        // services.AddOmniAppiumDemoServices(...);

        var services = new ServiceCollection();

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

        var registry = new Mock<IGeminiToolRegistry>(
            MockBehavior.Strict);

        var sessionManager = new Mock<IGeminiSessionManager>(
            MockBehavior.Strict);

        var observationService =
            new Mock<IAndroidScreenObservationService>(
                MockBehavior.Strict);

        var plannerScopeFactory =
            new Mock<IOcrGroundedPlannerExecutionScopeFactory>(
                MockBehavior.Strict);

        // Use the actual converter construction from Program.cs.
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

        services.AddSingleton(
            observationService.Object);

        services.AddSingleton(
            plannerScopeFactory.Object);

        services.AddGeminiAutomation(
            aiExecutionSettings,
            registry.Object,
            converter,
            sessionManager.Object,
            progress);

        // Act
        Action act = () =>
        {
            using ServiceProvider provider =
                services.BuildServiceProvider(
                    new ServiceProviderOptions
                    {
                        ValidateOnBuild = true,
                        ValidateScopes = true
                    });

            IGeminiJobHandler handler =
                provider.GetRequiredService<IGeminiJobHandler>();

            handler.Should()
                .BeOfType<GeminiJobHandler<WorkflowProgress>>();
        };

        // Assert
        act.Should().NotThrow();
    }
}