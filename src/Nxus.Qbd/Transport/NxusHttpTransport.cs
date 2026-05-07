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
    private const int DefaultMaxRetries = 2;

    // 409 is intentionally omitted: the backend overloads it for both retryable
    // lock contention (`ObjectInUse`, `LockFailed`) and terminal business-rule
    // violations (`OutdatedEditSequence`, `NameNotUnique`, `TimeCreationMismatch`).
    // Without `x-should-retry` to disambiguate, retrying 409 blindly will burn
    // attempts on errors that need client-side action. Servers that emit the
    // header (`x-should-retry: true`) override this fallback and opt 409s in.
    private static readonly HashSet<int> RetryableStatuses = new() { 408, 429 };

    private readonly HttpClient _client;
    private readonly bool _ownsClient;
    private readonly string? _defaultConnectionId;
    private readonly TimeSpan _defaultTimeout;
    private readonly int? _defaultServerTimeoutSeconds;
    private readonly int _defaultMaxRetries;

    private static readonly JsonSerializerOptions JsonOptions = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    public NxusHttpTransport(NxusClientOptions options) {
        _defaultConnectionId = options.ConnectionId;
        _defaultTimeout = options.Timeout;
        _defaultServerTimeoutSeconds = options.ServerTimeoutSeconds;
        _defaultMaxRetries = Math.Max(0, options.MaxRetries);

        if (options.HttpClient is not null) {
            _client = options.HttpClient;
            _ownsClient = false;
        } else {
            _client = new HttpClient {
                BaseAddress = new Uri(SdkEnvironment.ResolveBaseUrl(options.BaseUrl, options.Environment).TrimEnd('/') + "/"),
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
        return await SendWithRetriesAsync(method, url, body, options, ct).ConfigureAwait(false);
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

    /// <summary>
    /// Overload that accepts repeatable query params (duplicate keys allowed).
    /// Use for ASP.NET <c>List&lt;T&gt;</c> [FromQuery] binding (e.g. ?accountTypes=Bank&amp;accountTypes=CreditCard).
    /// </summary>
    public async Task<JsonElement> RequestAsync(
        HttpMethod method,
        string path,
        object? body,
        IEnumerable<KeyValuePair<string, string>>? queryParams,
        RequestOptions? options = null,
        CancellationToken ct = default) {
        var url = BuildUrl(path, queryParams);
        return await SendWithRetriesAsync(method, url, body, options, ct).ConfigureAwait(false);
    }

    public JsonElement Request(
        HttpMethod method,
        string path,
        object? body,
        IEnumerable<KeyValuePair<string, string>>? queryParams,
        RequestOptions? options = null) {
        return RequestAsync(method, path, body, queryParams, options, CancellationToken.None)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private async Task<JsonElement> SendWithRetriesAsync(
        HttpMethod method,
        string url,
        object? body,
        RequestOptions? options,
        CancellationToken ct) {
        var maxRetries = Math.Max(0, options?.MaxRetries ?? _defaultMaxRetries);
        var attempt = 0;

        while (true) {
            using var request = BuildRequest(method, url, body, options);
            using var cts = options?.Timeout is not null
                ? CancellationTokenSource.CreateLinkedTokenSource(ct)
                : null;
            if (cts is not null)
                cts.CancelAfter(options!.Timeout!.Value);

            HttpResponseMessage response;
            try {
                response = await _client
                    .SendAsync(request, cts?.Token ?? ct)
                    .ConfigureAwait(false);
            } catch (OperationCanceledException) {
                throw;
            } catch (HttpRequestException) when (attempt < maxRetries) {
                await Task.Delay(ComputeRetryDelay(attempt), ct).ConfigureAwait(false);
                attempt++;
                continue;
            }

            using (response) {
                if (!response.IsSuccessStatusCode) {
                    if (attempt >= maxRetries || !ShouldRetry(response))
                        throw await NxusApiException.FromResponseAsync(response, ct).ConfigureAwait(false);

                    var delay = await ComputeRetryDelayAsync(attempt, response, ct).ConfigureAwait(false);
                    await Task.Delay(delay, ct).ConfigureAwait(false);
                    attempt++;
                    continue;
                }

                var responseText = await response.Content
                    .ReadAsStringAsync(ct)
                    .ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(responseText))
                    return default;

                return JsonSerializer.Deserialize<JsonElement>(responseText, JsonOptions);
            }
        }
    }

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string url,
        object? body,
        RequestOptions? options) {
        var request = new HttpRequestMessage(method, url);

        if (body is not null) {
            var json = JsonSerializer.Serialize(body, JsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        ApplyHeaders(request, options);
        return request;
    }

    private void ApplyHeaders(HttpRequestMessage request, RequestOptions? options) {
        var connectionId = options?.ConnectionId ?? _defaultConnectionId;
        if (connectionId is not null)
            request.Headers.TryAddWithoutValidation("X-Connection-Id", connectionId);

        var serverTimeoutSeconds = options?.ServerTimeoutSeconds ?? _defaultServerTimeoutSeconds;
        if (serverTimeoutSeconds is not null)
            request.Headers.TryAddWithoutValidation(
                "X-Nxus-Timeout-Seconds",
                serverTimeoutSeconds.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));

        if (options?.Headers is not null) {
            foreach (var (key, value) in options.Headers)
                request.Headers.TryAddWithoutValidation(key, value);
        }
    }

    private static bool ShouldRetry(HttpResponseMessage response) {
        var shouldRetry = ParseShouldRetry(response);
        if (shouldRetry is not null)
            return shouldRetry.Value;

        var status = (int)response.StatusCode;
        return RetryableStatuses.Contains(status) || status >= 500;
    }

    private static bool? ParseShouldRetry(HttpResponseMessage response) {
        if (!response.Headers.TryGetValues("X-Should-Retry", out var values))
            return null;

        var value = values.FirstOrDefault()?.Trim().ToLowerInvariant();
        return value switch {
            "1" or "true" or "yes" => true,
            "0" or "false" or "no" => false,
            _ => null,
        };
    }

    /// <summary>
    /// Network-error path: no response in hand, just back off exponentially.
    /// </summary>
    private static TimeSpan ComputeRetryDelay(int attempt) =>
        ExponentialBackoff(attempt);

    /// <summary>
    /// HTTP-error path: prefer <c>Retry-After</c> header, fall back to body
    /// <c>error.retryAfter</c> (seconds, integer) for proxies that strip
    /// hop-by-hop headers, and finally to exponential backoff.
    /// </summary>
    private static async Task<TimeSpan> ComputeRetryDelayAsync(
        int attempt,
        HttpResponseMessage response,
        CancellationToken ct) {
        if (response.Headers.RetryAfter?.Delta is { } delta)
            return Min(delta, TimeSpan.FromSeconds(8));

        if (response.Headers.RetryAfter?.Date is { } date) {
            var delay = date - DateTimeOffset.UtcNow;
            return Min(delay > TimeSpan.Zero ? delay : TimeSpan.Zero, TimeSpan.FromSeconds(8));
        }

        var bodySeconds = await TryReadBodyRetryAfterSecondsAsync(response, ct).ConfigureAwait(false);
        if (bodySeconds is { } seconds)
            return Min(TimeSpan.FromSeconds(seconds), TimeSpan.FromSeconds(8));

        return ExponentialBackoff(attempt);
    }

    private static TimeSpan ExponentialBackoff(int attempt) {
        var exponentialMs = Math.Min(500 * Math.Pow(2, attempt), 8_000);
        var jitterMs = Random.Shared.NextDouble() * exponentialMs * 0.5;
        return TimeSpan.FromMilliseconds(exponentialMs + jitterMs);
    }

    private static async Task<double?> TryReadBodyRetryAfterSecondsAsync(
        HttpResponseMessage response,
        CancellationToken ct) {
        var contentType = response.Content.Headers.ContentType?.MediaType;
        if (!string.Equals(contentType, "application/json", StringComparison.OrdinalIgnoreCase))
            return null;

        string body;
        try {
            body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        } catch {
            return null;
        }

        if (string.IsNullOrWhiteSpace(body))
            return null;

        try {
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                return null;

            // Shape per backend contract: `{ error: { retryAfter: <seconds> } }`.
            // Tolerate top-level `retryAfter` for older payloads.
            JsonElement candidate = default;
            var found = false;
            if (root.TryGetProperty("error", out var errorEl) &&
                errorEl.ValueKind == JsonValueKind.Object &&
                errorEl.TryGetProperty("retryAfter", out var fromError)) {
                candidate = fromError;
                found = true;
            } else if (root.TryGetProperty("retryAfter", out var fromRoot)) {
                candidate = fromRoot;
                found = true;
            }

            if (!found || candidate.ValueKind != JsonValueKind.Number)
                return null;

            if (!candidate.TryGetDouble(out var seconds) || seconds < 0)
                return null;

            return seconds;
        } catch (JsonException) {
            return null;
        }
    }

    private static TimeSpan Min(TimeSpan left, TimeSpan right) =>
        left <= right ? left : right;

    private static string BuildUrl(string path, IDictionary<string, string>? queryParams) {
        if (queryParams is null or { Count: 0 })
            return path;
        return BuildUrl(path, (IEnumerable<KeyValuePair<string, string>>)queryParams);
    }

    /// <summary>
    /// Build a URL with query string. Supports duplicate keys (repeated key=value pairs)
    /// which is what ASP.NET model binding for List&lt;T&gt; from [FromQuery] expects.
    /// </summary>
    private static string BuildUrl(string path, IEnumerable<KeyValuePair<string, string>>? queryParams) {
        if (queryParams is null) return path;

        var sb = new StringBuilder(path);
        var first = true;
        foreach (var (key, value) in queryParams) {
            sb.Append(first ? '?' : '&');
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
