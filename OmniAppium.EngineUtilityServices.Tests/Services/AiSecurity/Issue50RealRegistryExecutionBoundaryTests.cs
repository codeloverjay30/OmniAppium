using AiUtility.GeminiKits.Executor;
using AiUtility.GeminiKits.Registry;
using ExpressionTreeUtilityServices;
using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Planner;
using OmniAppiumDemo.DependencyInjection;
using ReflectionUtilityServices;
using TypeUtilityServices;

namespace OmniAppium.EngineUtilityServices.Tests.Services.AiSecurity;

public sealed class Issue50RealRegistryExecutionBoundaryTests
{
    [Fact]
    public async Task DirectThenProtected_RegisteredDirectTool_ShouldRejectBeforeFactoryInvocation()
    {
        var registry = CreateRegistry();
        int directFactoryCalls = 0;

        registry.RegisterAiClickTools(
            AiClickExecutionMode.Direct,
            isProduction: false,
            () =>
            {
                directFactoryCalls++;
                throw new InvalidOperationException("Direct device factory must not execute.");
            },
            () => throw new InvalidOperationException("Planner factory must not execute."));

        var executor = CreateExecutor(registry, AiClickExecutionMode.Protected, isProduction: true);
        Func<Task> act = async () => await executor.ExecuteAsync(
            nameof(ClickService.Click),
            new Dictionary<string, object>
            {
                ["rx"] = 10.0,
                ["ry"] = 20.0,
                ["usePreloadScreenSize"] = true
            },
            CancellationToken.None);

        (await act.Should().ThrowAsync<InvalidOperationException>())
            .WithMessage("*Protected policy*");
        directFactoryCalls.Should().Be(0);
    }

    [Fact]
    public async Task ProtectedThenDirect_RegisteredDirectTool_ShouldRejectBeforeFactoryInvocation()
    {
        var registry = CreateRegistry();
        int directFactoryCalls = 0;

        registry.RegisterAiClickTools(
            AiClickExecutionMode.Protected,
            isProduction: true,
            () => throw new InvalidOperationException("Direct factory must not execute."),
            () => throw new InvalidOperationException("Planner factory must not execute."));

        registry.Register<ClickService>(() =>
        {
            directFactoryCalls++;
            throw new InvalidOperationException("Direct device factory must not execute.");
        });

        var executor = CreateExecutor(registry, AiClickExecutionMode.Protected, isProduction: true);
        Func<Task> act = async () => await executor.ExecuteAsync(
            nameof(ClickService.Click),
            new Dictionary<string, object>
            {
                ["rx"] = 10.0,
                ["ry"] = 20.0,
                ["usePreloadScreenSize"] = true
            },
            CancellationToken.None);

        (await act.Should().ThrowAsync<InvalidOperationException>())
            .WithMessage("*Protected policy*");
        directFactoryCalls.Should().Be(0);
    }

    [Fact]
    public void DevelopmentDirect_RealRegistry_ShouldResolveAndAuthorizeDirectTool()
    {
        var registry = CreateRegistry();

        registry.RegisterAiClickTools(
            AiClickExecutionMode.Direct,
            isProduction: false,
            () => throw new InvalidOperationException("No Android device in this test."),
            () => throw new InvalidOperationException("Planner factory must not execute."));

        bool found = registry.TryGetTool(nameof(ClickService.Click), out var metadata);
        found.Should().BeTrue();
        metadata.Should().NotBeNull();
        metadata!.MethodInfo.DeclaringType.Should().Be(typeof(ClickService));

        var policy = GeminiClickExecutionPolicyFactory.Create(
            AiClickExecutionMode.Direct, isProduction: false);
        Action act = () => policy.EnsureAllowed(metadata.MethodInfo);
        act.Should().NotThrow();
    }

    private static GeminiToolRegistry CreateRegistry()
    {
        var expressionService = new ExpressionTreeUtilityService();
        var reflectionService = new ReflectionUtilityService(expressionService);
        return new GeminiToolRegistry(reflectionService);
    }

    private static GeminiToolExecutor CreateExecutor(
        GeminiToolRegistry registry,
        AiClickExecutionMode mode,
        bool isProduction)
    {
        return new GeminiToolExecutor(
            registry,
            new TypeUtilityService(),
            GeminiClickExecutionPolicyFactory.Create(mode, isProduction));
    }
}
