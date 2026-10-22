using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using Moq;
using OmniAppium.EngineUtilityService.Services.Screen;
using OmniAppium.EngineUtilityServices.Services.Click;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Click;

public sealed class ClickServiceExceptionPropagationTests
{
    [Fact]
    public async Task TapAt_WhenAppiumRejectsAction_ShouldPropagateOriginalWebDriverException()
    {
        await using var server = new FakeAppiumServer(rejectActions: true);
        await server.StartAsync();

        using var driver = CreateDriver(server.Address);
        var service = CreateService(driver);

        Action act = () => service.TapAt(120, 240);

        act.Should().Throw<WebDriverException>()
            .WithMessage("*device dispatch rejected*");
        server.ActionRequests.Should().Be(1);
    }

    [Fact]
    public async Task TapAt_WhenAppiumRejectsAction_ShouldNotRetry()
    {
        await using var server = new FakeAppiumServer(rejectActions: true);
        await server.StartAsync();

        using var driver = CreateDriver(server.Address);
        var service = CreateService(driver);

        Action act = () => service.ClickAbsolute(120, 240);

        act.Should().Throw<WebDriverException>()
            .WithMessage("*device dispatch rejected*");
        server.ActionRequests.Should().Be(1);
    }

    [Fact]
    public async Task TapAt_WhenAppiumAcceptsAction_ShouldDispatchExactlyOnce()
    {
        await using var server = new FakeAppiumServer(rejectActions: false);
        await server.StartAsync();

        using var driver = CreateDriver(server.Address);
        var service = CreateService(driver);

        Action act = () => service.TapAt(120, 240);

        act.Should().NotThrow();
        server.ActionRequests.Should().Be(1);
    }

    private static AndroidDriver CreateDriver(Uri address)
    {
        var options = new AppiumOptions
        {
            PlatformName = "Android",
            AutomationName = "UiAutomator2",
            DeviceName = "Fake Android"
        };
        return new AndroidDriver(address, options, TimeSpan.FromSeconds(10));
    }

    private static ClickService CreateService(AndroidDriver driver)
    {
        var loggerFactory = new Mock<ILoggerFactoryBaseUtilityService>(MockBehavior.Strict);
        loggerFactory.SetupGet(x => x.Logger).Returns(Mock.Of<ILogger>());

        return new ClickService(loggerFactory.Object, false)
        {
            ScreenService = new TestScreenService(driver),
            Scaler = Mock.Of<CoordinateUtilityServices.IResolutionScaler>()
        };
    }

    private sealed class TestScreenService : ScreenService
    {
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public TestScreenService(AndroidDriver driver)
        {
            Driver = driver;
        }
    }

    private sealed class FakeAppiumServer(bool rejectActions) : IAsyncDisposable
    {
        private readonly HttpListener _listener = new();
        private CancellationTokenSource? _cancellation;
        private Task? _serverTask;
        private int _actionRequests;

        public Uri Address { get; private set; } = null!;

        public int ActionRequests => Volatile.Read(ref _actionRequests);

        public async Task StartAsync()
        {
            var probe = new TcpListener(IPAddress.Loopback, 0);
            probe.Start();
            var port = ((IPEndPoint)probe.LocalEndpoint).Port;
            probe.Stop();

            Address = new Uri($"http://127.0.0.1:{port}/");
            _listener.Prefixes.Add(Address.AbsoluteUri);
            _listener.Start();
            _cancellation = new CancellationTokenSource();
            _serverTask = ServeAsync(_cancellation.Token);
            await Task.CompletedTask;
        }

        private async Task ServeAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                HttpListenerContext context;
                try
                {
                    context = await _listener.GetContextAsync().WaitAsync(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (HttpListenerException) when (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    var path = context.Request.Url?.AbsolutePath ?? string.Empty;
                    var isAction = path.EndsWith("/actions", StringComparison.Ordinal)
                        && context.Request.HttpMethod == "POST";

                    if (isAction)
                    {
                        Interlocked.Increment(ref _actionRequests);
                        using var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
                        await reader.ReadToEndAsync(cancellationToken);
                    }

                    object response;
                    if (isAction && rejectActions)
                    {
                        context.Response.StatusCode = 500;
                        response = new
                        {
                            value = new
                            {
                                error = "unknown error",
                                message = "device dispatch rejected",
                                stacktrace = ""
                            }
                        };
                    }
                    else
                    {
                        response = path switch
                        {
                            "/session" => new
                            {
                                value = new
                                {
                                    sessionId = "click-propagation-session",
                                    capabilities = new { platformName = "Android" }
                                }
                            },
                            "/status" => new { value = new { ready = true, message = "ready" } },
                            _ => new { value = (object?)null }
                        };
                    }

                    var bytes = JsonSerializer.SerializeToUtf8Bytes(response);
                    context.Response.ContentType = "application/json; charset=utf-8";
                    context.Response.ContentLength64 = bytes.Length;
                    await context.Response.OutputStream.WriteAsync(bytes, cancellationToken);
                }
                catch (Exception) when (cancellationToken.IsCancellationRequested)
                {
                    // Shutdown may interrupt an in-flight response.
                }
                finally
                {
                    context.Response.Close();
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            _cancellation?.Cancel();
            _listener.Stop();
            _listener.Close();
            if (_serverTask is not null)
            {
                try
                {
                    await _serverTask;
                }
                catch (OperationCanceledException)
                {
                    // Expected during shutdown.
                }
                catch (HttpListenerException)
                {
                    // Listener has been stopped.
                }
            }

            _cancellation?.Dispose();
        }
    }
}
