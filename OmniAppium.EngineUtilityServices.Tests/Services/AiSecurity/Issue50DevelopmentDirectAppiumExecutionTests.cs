using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using AiUtility.GeminiKits.Executor;
using AiUtility.GeminiKits.Registry;
using CoordinateUtilityServices;
using ExpressionTreeUtilityServices;
using FluentAssertions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using Moq;
using OmniAppium.EngineUtilityService.Services.Screen;
using OmniAppium.EngineUtilityServices.Services.AiSecurity;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.ConfigUtilityServices.Models;
using OmniAppiumDemo.DependencyInjection;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using ReflectionUtilityServices;
using TypeUtilityServices;
using ByAccessibilityId = OmniAppium.ConfigUtilityServices.Models.ByAccessibilityId;

namespace OmniAppium.EngineUtilityServices.Tests.Services.AiSecurity;

public sealed class Issue50DevelopmentDirectAppiumExecutionTests
{
    [Fact]
    public async Task DevelopmentDirect_RealExecutor_ShouldPerformExactlyOneAppiumTap()
    {
        await using var server = new FakeAppiumServer();
        await server.StartAsync();

        var options = new AppiumOptions();
        options.PlatformName = "Android";
        options.AutomationName = "UiAutomator2";
        options.DeviceName = "Fake Android";

        using var driver = new AndroidDriver(server.Address, options, TimeSpan.FromSeconds(10));

        var loggerFactory = new Mock<ILoggerFactoryBaseUtilityService>(MockBehavior.Strict);
        loggerFactory.SetupGet(x => x.Logger).Returns(Mock.Of<ILogger>());

        var scaler = new Mock<IResolutionScaler>(MockBehavior.Strict);
        scaler.Setup(x => x.Transform(It.IsAny<CoordinateUtilityServices.Point>()))
            .Returns((CoordinateUtilityServices.Point point) => point);

        var screen = new TestScreenService(driver);

        var clickService = new ClickService(loggerFactory.Object, false)
        {
            ScreenService = screen,
            Scaler = scaler.Object
        };

        var reflection = new ReflectionUtilityService(new ExpressionTreeUtilityService());
        var registry = new GeminiToolRegistry(reflection);
        var factoryCalls = 0;

        registry.RegisterAiClickTools(
            AiClickExecutionMode.Direct,
            isProduction: false,
            () =>
            {
                factoryCalls++;
                return clickService;
            },
            () => throw new InvalidOperationException("Planner must not be resolved in Direct mode."));

        var executor = new GeminiToolExecutor(
            registry,
            new TypeUtilityService(),
            GeminiClickExecutionPolicyFactory.Create(AiClickExecutionMode.Direct, isProduction: false));

        Func<Task> act = async () => await executor.ExecuteAsync(
            nameof(ClickService.Click),
            new Dictionary<string, object>
            {
                ["target"] = new ByAccessibilityId
                {
                    Description = "Issue 50 development direct execution",
                    Value = "fake-target"
                }
            },
            TestContext.Current.CancellationToken);

        await act.Should().NotThrowAsync();

        factoryCalls.Should().Be(1);
        var actions = server.Actions;
        actions.Should().ContainSingle("Exactly one device input command is expected.");
        using var document = JsonDocument.Parse(actions.Single());
        var actionArray = document.RootElement.GetProperty("actions");
        actionArray.GetArrayLength().Should().Be(1);
        var steps = actionArray[0].GetProperty("actions");
        steps.GetArrayLength().Should().Be(3);
        var move = steps[0];
        move.GetProperty("type").GetString().Should().Be("pointerMove");
        move.GetProperty("x").GetInt32().Should().Be(584);
        move.GetProperty("y").GetInt32().Should().Be(884);
        steps[1].GetProperty("type").GetString().Should().Be("pointerDown");
        steps[2].GetProperty("type").GetString().Should().Be("pointerUp");
    }

    private sealed class TestScreenService : ScreenService
    {
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public TestScreenService(AndroidDriver driver)
        {
            Driver = driver;
        }
    }

    private sealed class FakeAppiumServer : IAsyncDisposable
    {
        private readonly HttpListener _listener = new();
        private readonly List<string> _actions = [];
        private readonly object _sync = new();
        private Task? _serveTask;
        private CancellationTokenSource? _cts;

        public Uri Address { get; private set; } = null!;

        public IReadOnlyList<string> Actions
        {
            get { lock (_sync) return _actions.ToArray(); }
        }

        public async Task StartAsync()
        {
            var portProbe = new TcpListener(IPAddress.Loopback, 0);
            portProbe.Start();
            var port = ((IPEndPoint)portProbe.LocalEndpoint).Port;
            portProbe.Stop();

            Address = new Uri($"http://127.0.0.1:{port}/");
            _listener.Prefixes.Add(Address.AbsoluteUri);
            _listener.Start();
            _cts = new CancellationTokenSource();
            _serveTask = ServeAsync(_cts.Token);
            await Task.CompletedTask;
        }

        private async Task ServeAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                HttpListenerContext context;
                try { context = await _listener.GetContextAsync().WaitAsync(cancellationToken); }
                catch (OperationCanceledException) { break; }
                catch (HttpListenerException) when (cancellationToken.IsCancellationRequested) { break; }

                try
                {
                    var path = context.Request.Url?.AbsolutePath ?? "";
                    if (path.EndsWith("/actions", StringComparison.Ordinal) &&
                        context.Request.HttpMethod == "POST")
                    {
                        using var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
                        var body = await reader.ReadToEndAsync(cancellationToken);
                        lock (_sync) _actions.Add(body);
                    }

                    object response = path switch
                    {
                        "/session" => new
                        {
                            value = new
                            {
                                sessionId = "issue50-fake-session",
                                capabilities = new { platformName = "Android" }
                            }
                        },
                        "/session/issue50-fake-session/window/rect" => new
                        {
                            value = new { x = 0, y = 0, width = 1080, height = 1920 }
                        },
                        "/status" => new { value = new { ready = true, message = "ready" } },
                        _ => new { value = (object?)null }
                    };

                    var json = JsonSerializer.Serialize(response);
                    var bytes = Encoding.UTF8.GetBytes(json);
                    context.Response.ContentType = "application/json; charset=utf-8";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = bytes.Length;
                    await context.Response.OutputStream.WriteAsync(bytes, cancellationToken);
                }
                catch (Exception) when (cancellationToken.IsCancellationRequested) { }
                finally { context.Response.Close(); }
            }
        }

        public async ValueTask DisposeAsync()
        {
            _cts?.Cancel();
            _listener.Stop();
            _listener.Close();
            if (_serveTask is not null)
            {
                try { await _serveTask; }
                catch (OperationCanceledException) { }
                catch (HttpListenerException) { }
            }
            _cts?.Dispose();
        }
    }
}