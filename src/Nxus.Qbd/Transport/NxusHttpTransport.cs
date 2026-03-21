using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Nxus.Qbd.Errors;

namespace Nxus.Qbd.Transport;

/// <summary>
/// Low-level HTTP transport wrapping <see cref="HttpClient"/>.
/// Handles serialization, header merging, and error mapping.
/// </summary>
internal sealed class NxusHttpTransport : IDisposable {
    private readonly HttpClient _client;
    private readonly bool _ownsClient;
    private readonly string? _defaultConnectionId;
    private readonly TimeSpan _defaultTimeout;

    private static readonly JsonSerializerOptions JsonOptions = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    public NxusHttpTransport(NxusClientOptions options) {
        _defaultConnectionId = options.ConnectionId;
        _defaultTimeout = options.Timeout;

        if (options.HttpClient is not null) {
            _client = options.HttpClient;
            _ownsClient = false;
        } else {
            _client = new HttpClient {
                BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/"),
                Timeout = options.Timeout,
            };
            _ownsClient = true;
        }

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", options.ApiKey);
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        if (options.DefaultHeaders is not null) {
            foreach (var (key, value) in options.DefaultHeaders)
                _client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
        }
    }

    // ── Public API ───────────────────────────────────────────────────────

    public async Task<JsonElement> RequestAsync(
        HttpMethod method,
        string path,
        object? body = null,
        IDictionary<string, string>? queryParams = null,
        RequestOptions? options = null,
        CancellationToken ct = default) {
        var url = BuildUrl(path, queryParams);
        using var request = new HttpRequestMessage(method, url);

        // Body
        if (body is not null) {
            var json = JsonSerializer.Serialize(body, JsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        // Per-request headers
        ApplyHeaders(request, options);

        // Timeout override
        using var cts = options?.Timeout is not null
            ? CancellationTokenSource.CreateLinkedTokenSource(ct)
            : null;
        if (cts is not null)
            cts.CancelAfter(options!.Timeout!.Value);

        var response = await _client
            .SendAsync(request, cts?.Token ?? ct)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw await NxusApiException.FromResponseAsync(response, ct).ConfigureAwait(false);

        var responseText = await response.Content
            .ReadAsStringAsync(ct)
            .ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(responseText))
            return default;

        return JsonSerializer.Deserialize<JsonElement>(responseText, JsonOptions);
    }

    public JsonElement Request(
        HttpMethod method,
        string path,
        object? body = null,
        IDictionary<string, string>? queryParams = null,
        RequestOptions? options = null) {
        // Sync-over-async — acceptable for an SDK where callers opt into sync explicitly.
        return RequestAsync(method, path, body, queryParams, options, CancellationToken.None)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private void ApplyHeaders(HttpRequestMessage request, RequestOptions? options) {
        var connectionId = options?.ConnectionId ?? _defaultConnectionId;
        if (connectionId is not null)
            request.Headers.TryAddWithoutValidation("X-Connection-Id", connectionId);

        if (options?.Headers is not null) {
            foreach (var (key, value) in options.Headers)
                request.Headers.TryAddWithoutValidation(key, value);
        }
    }

    private static string BuildUrl(string path, IDictionary<string, string>? queryParams) {
        if (queryParams is null or { Count: 0 })
            return path;

        var sb = new StringBuilder(path);
        sb.Append('?');
        var first = true;
        foreach (var (key, value) in queryParams) {
            if (!first) sb.Append('&');
            sb.Append(Uri.EscapeDataString(key));
            sb.Append('=');
            sb.Append(Uri.EscapeDataString(value));
            first = false;
        }
        return sb.ToString();
    }

    public void Dispose() {
        if (_ownsClient)
            _client.Dispose();
    }
}
