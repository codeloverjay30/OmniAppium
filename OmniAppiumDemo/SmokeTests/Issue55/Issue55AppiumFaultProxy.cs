using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace OmniAppiumDemo.SmokeTests.Issue55;

/// <summary>
/// Forwards local Appium session traffic while rejecting W3C Actions requests.
/// Intended only for an explicitly opted-in diagnostic process.
/// </summary>
public sealed class Issue55AppiumFaultProxy : IAsyncDisposable
{
    private readonly ILogger<Issue55AppiumFaultProxy>? _logger;
    
    /// <summary>
    /// Gets the marker returned in the injected WebDriver error.
    /// </summary>
    public const string FailureMarker = "ISSUE55_DISPATCH_REJECTED";

    private readonly HttpListener _listener = new();
    private readonly HttpClient _client;
    private readonly Uri _upstream;
    private readonly CancellationTokenSource _stop = new();
    private Task? _loop;
    private string? _armedSession;
    private int _actions;
    private int _injected;
    private int _forwarded;
    private int _armed;
    private int _started;

    /// <summary>
    /// Creates a local-only proxy for a trusted loopback Appium endpoint.
    /// </summary>
    public Issue55AppiumFaultProxy(
        Uri upstream,
        ILogger<Issue55AppiumFaultProxy>? logger = null)
    {
        ArgumentNullException.ThrowIfNull(upstream);

        if (upstream.Scheme != Uri.UriSchemeHttp ||
            !IPAddress.TryParse(upstream.Host, out var ip) ||
            !IPAddress.IsLoopback(ip) ||
            upstream.Port <= 0 ||
            upstream.AbsolutePath != "/" ||
            !string.IsNullOrEmpty(upstream.Query) ||
            !string.IsNullOrEmpty(upstream.Fragment))
        {
            throw new ArgumentException(
                "Upstream must be an HTTP loopback origin with no base path or query.",
                nameof(upstream));
        }

        _logger = logger;
        _upstream = upstream;

        _client = new HttpClient(new SocketsHttpHandler
        {
            AllowAutoRedirect = false,
            UseProxy = false,
            AutomaticDecompression = DecompressionMethods.None
        })
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
    }


    /// <summary>
    /// Gets the proxy endpoint after it has started.
    /// </summary>
    public Uri? Endpoint { get; private set; }

    /// <summary>
    /// Gets whether the proxy is accepting requests.
    /// </summary>
    public bool IsRunning => Volatile.Read(ref _started) == 1 && !_stop.IsCancellationRequested;

    /// <summary>
    /// Gets the number of Actions requests received.
    /// </summary>
    public int ActionRequests => Volatile.Read(ref _actions);

    /// <summary>
    /// Gets the number of deliberately rejected Actions requests.
    /// </summary>
    public int InjectedFailures => Volatile.Read(ref _injected);

    /// <summary>
    /// Gets the number of Actions requests forwarded to Appium.
    /// This must remain zero for the lifetime of the proxy.
    /// </summary>
    public int ForwardedActionRequests => Volatile.Read(ref _forwarded);

    /// <summary>
    /// Starts the loopback listener before the AndroidDriver is created.
    /// </summary>
    public Task StartAsync()
    {
        if (Interlocked.CompareExchange(ref _started, 1, 0) != 0)
            throw new InvalidOperationException("The proxy has already started.");

        try
        {
            using var socket = new TcpListener(IPAddress.Loopback, 0);
            socket.Start();
            var port = ((IPEndPoint)socket.LocalEndpoint).Port;
            socket.Stop();

            Endpoint = new Uri($"http://127.0.0.1:{port}/");
            _listener.Prefixes.Add(Endpoint.AbsoluteUri);
            _listener.Start();
            _loop = AcceptLoopAsync(_stop.Token);
            return Task.CompletedTask;
        }
        catch
        {
            Volatile.Write(ref _started, 0);
            _listener.Close();
            throw;
        }
    }

    /// <summary>
    /// Arms exactly one expected session for the failure diagnostic.
    /// </summary>
    public void Arm(string sessionId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);
        if (!IsRunning || ActionRequests != 0 ||
            Interlocked.CompareExchange(ref _armed, 1, 0) != 0)
            throw new InvalidOperationException("The proxy cannot be armed in its current state.");

        _armedSession = sessionId;
    }

    /// <summary>
    /// Disables the active diagnostic. Actions remain blocked even after disarming.
    /// </summary>
    public void Disarm()
    {
        Volatile.Write(ref _armed, 0);
        _armedSession = null;
    }

    /// <summary>
    /// Stops the listener and releases the proxy resources.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        _stop.Cancel();
        if (_listener.IsListening)
            _listener.Stop();
        _listener.Close();
        if (_loop is not null)
        {
            try { await _loop.ConfigureAwait(false); }
            catch (HttpListenerException) { }
            catch (ObjectDisposedException) { }
            catch (OperationCanceledException) { }
        }

        _client.Dispose();
        _stop.Dispose();
    }

    /// <summary>
    /// Accepts incoming requests and processes them without blocking the accept loop.
    /// </summary>
    private async Task AcceptLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            HttpListenerContext context;
            try { context = await _listener.GetContextAsync().WaitAsync(token).ConfigureAwait(false); }
            catch (OperationCanceledException) { break; }
            catch (HttpListenerException) when (token.IsCancellationRequested) { break; }
            catch (ObjectDisposedException) when (token.IsCancellationRequested) { break; }

            // Await processing to preserve deterministic ordering and avoid orphaned tasks.
            await HandleAsync(context, token).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Handles an incoming request, rejecting any Actions traffic before forwarding.
    /// </summary>
    private async Task HandleAsync(HttpListenerContext context, CancellationToken token)
    {
        try
        {
            var request = context.Request;
            var segments = request.Url?.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments is null || segments.Length == 0 ||
                segments.Any(s => s is "." or "..") ||
                request.Url is null || !string.IsNullOrEmpty(request.Url.Query))
            {
                await WriteErrorAsync(context.Response, 400, "invalid argument",
                    "Unsupported request path.", token).ConfigureAwait(false);
                return;
            }

            // All Actions traffic is blocked, including unarmed, wrong-session and malformed methods.
            var actionsIndex = Array.FindIndex(segments, s => s.Equals("actions", StringComparison.OrdinalIgnoreCase));
            if (actionsIndex >= 0)
            {
                Interlocked.Increment(ref _actions);
                var valid = Volatile.Read(ref _armed) == 1 &&
                    request.HttpMethod == "POST" &&
                    actionsIndex == segments.Length - 1 &&
                    actionsIndex >= 2 &&
                    segments[actionsIndex - 2] == "session" &&
                    string.Equals(segments[actionsIndex - 1], _armedSession, StringComparison.Ordinal) &&
                    (actionsIndex == 2 ||
                     (actionsIndex == 4 && segments[0] == "wd" && segments[1] == "hub"));

                if (valid)
                {
                    Interlocked.Increment(ref _injected);
                    await WriteErrorAsync(context.Response, 500, "unknown error",
                        FailureMarker, token).ConfigureAwait(false);
                }
                else
                {
                    await WriteErrorAsync(context.Response, 403, "unknown error",
                        "Actions dispatch blocked by Issue55 proxy.", token).ConfigureAwait(false);
                }
                return;
            }

            // Explicitly disallow unrecognized endpoints and unsafe origins.
            if (!IsAllowedNonActionRequest(request.HttpMethod, segments))
            {
                _logger?.LogWarning(
                    "Issue55 Proxy rejected command: {HttpMethod} {RequestPath}",
                    request.HttpMethod,
                    FormatDiagnosticPath(segments));

                await WriteErrorAsync(
                    context.Response,
                    403,
                    "unknown command",
                    "Command is not allowlisted for this diagnostic.",
                    token).ConfigureAwait(false);

                return;
            }


            var relative = string.Join('/', segments);
            using var outgoing = new HttpRequestMessage(
                new HttpMethod(request.HttpMethod), new Uri(_upstream, relative));

            if (request.HasEntityBody)
            {
                using var memory = new MemoryStream();
                await request.InputStream.CopyToAsync(memory, token).ConfigureAwait(false);
                if (memory.Length > 1024 * 1024)
                {
                    await WriteErrorAsync(context.Response, 413, "invalid argument",
                        "Request body too large.", token).ConfigureAwait(false);
                    return;
                }
                outgoing.Content = new ByteArrayContent(memory.ToArray());
                if (!string.IsNullOrWhiteSpace(request.ContentType))
                    outgoing.Content.Headers.TryAddWithoutValidation("Content-Type", request.ContentType);
            }

            using var response = await _client.SendAsync(
                outgoing, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);

            context.Response.StatusCode = (int)response.StatusCode;
            context.Response.ContentType =
                response.Content.Headers.ContentType?.ToString() ?? "application/json";
            await response.Content.CopyToAsync(context.Response.OutputStream, token).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            try
            {
                if (context.Response.OutputStream.CanWrite)
                    await WriteErrorAsync(context.Response, 502, "unknown error",
                        "Issue55 proxy forwarding failure.", CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception) { /* Connection may already be closed; never forward as fallback. */ }
        }
        finally
        {
            try { context.Response.Close(); }
            catch (Exception) { }
        }
    }

    /// <summary>
    /// Determines which non-Action WebDriver commands may reach the real Appium server.
    /// </summary>
    private static bool IsAllowedNonActionRequest(string method, string[] segments)
    {
        var offset = segments.Length >= 2 && segments[0] == "wd" && segments[1] == "hub" ? 2 : 0;
        var remaining = segments.Length - offset;
        if (remaining == 1 && segments[offset] == "status" && method == "GET")
            return true;
        if (remaining == 1 && segments[offset] == "session" && method == "POST")
            return true;
        if (remaining < 2 || segments[offset] != "session" ||
            string.IsNullOrWhiteSpace(segments[offset + 1]))
            return false;
        if (remaining == 2)
            return method is "GET" or "DELETE";
        if (remaining == 4 && segments[offset + 2] == "window" &&
            segments[offset + 3] == "rect" && method == "GET")
            return true;
        if (remaining == 3 && segments[offset + 2] == "screenshot" && method == "GET")
            return true;

        // Allow app activation during real-driver initialization.
        if (method == "POST" &&
            remaining == 5 &&
            segments[offset + 2] == "appium" &&
            segments[offset + 3] == "app" &&
            segments[offset + 4] == "activate")
        {
            return true;
        }

        // Allow the observed Appium activation command during driver initialization.
        if (method == "POST" &&
            remaining == 5 &&
            segments[offset + 2] == "appium" &&
            segments[offset + 3] == "device" &&
            segments[offset + 4] == "activate_app")
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Writes a WebDriver-compatible error without disclosing internal exception details.
    /// </summary>
    private static async Task WriteErrorAsync(
        HttpListenerResponse response, int status, string error, string message, CancellationToken token)
    {
        response.StatusCode = status;
        response.ContentType = "application/json; charset=utf-8";
        var json = System.Text.Json.JsonSerializer.Serialize(new
        {
            value = new { error, message, stacktrace = "" }
        });
        var bytes = Encoding.UTF8.GetBytes(json);
        response.ContentLength64 = bytes.Length;
        await response.OutputStream.WriteAsync(bytes, token).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a bounded diagnostic path while masking WebDriver session identifiers.
    /// </summary>
    private static string FormatDiagnosticPath(string[] segments)
    {
        ArgumentNullException.ThrowIfNull(segments);

        int offset = segments.Length >= 2 &&
                     segments[0] == "wd" &&
                     segments[1] == "hub"
            ? 2
            : 0;

        const int maxSegmentLength = 64;
        const int maxPathLength = 256;

        var safeSegments = new string[segments.Length];

        Span<char> buffer = stackalloc char[maxSegmentLength];

        for (int i = 0; i < segments.Length; i++)
        {
            if (i == offset + 1 &&
                segments.Length > offset + 1 &&
                segments[offset] == "session")
            {
                safeSegments[i] = "{sessionId}";
                continue;
            }

            ReadOnlySpan<char> source = segments[i].AsSpan();

            int length = Math.Min(
                source.Length,
                maxSegmentLength);

            for (int j = 0; j < length; j++)
            {
                char c = source[j];

                buffer[j] =
                    char.IsAsciiLetterOrDigit(c) ||
                    c is '-' or '_' or '.'
                        ? c
                        : '_';
            }

            safeSegments[i] = new string(buffer[..length]);
        }

        string path = "/" + string.Join('/', safeSegments);

        return path.Length <= maxPathLength
            ? path
            : path[..maxPathLength];
    }
}
