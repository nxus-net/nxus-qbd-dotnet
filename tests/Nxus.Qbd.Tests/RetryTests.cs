using System.Net;
using System.Text;
using Nxus.Qbd.Errors;
using Xunit;

namespace Nxus.Qbd.Tests;

public class RetryTests {
    [Fact]
    public void RetriesRetryableStatus() {
        var (client, handler) = CreateClient(
            Error(HttpStatusCode.ServiceUnavailable, ("Retry-After", "0")),
            Json(HttpStatusCode.OK, """{"id":"conn_123"}"""));
        using (client) {
            var result = client.Connections.Retrieve("conn_123");

            Assert.Equal("conn_123", result.GetProperty("id").GetString());
            Assert.Equal(2, handler.CallCount);
        }
    }

    [Fact]
    public void HonorsXShouldRetryTrue() {
        var (client, handler) = CreateClient(
            Error(HttpStatusCode.BadRequest, ("X-Should-Retry", "true"), ("Retry-After", "0")),
            Json(HttpStatusCode.OK, """{"id":"conn_123"}"""));
        using (client) {
            var result = client.Connections.Retrieve("conn_123");

            Assert.Equal("conn_123", result.GetProperty("id").GetString());
            Assert.Equal(2, handler.CallCount);
        }
    }

    [Fact]
    public void HonorsXShouldRetryFalse() {
        var (client, handler) = CreateClient(
            Error(HttpStatusCode.ServiceUnavailable, ("X-Should-Retry", "false")));
        using (client) {
            var ex = Assert.Throws<NxusApiException>(() => client.Connections.Retrieve("conn_123"));

            Assert.Equal(503, ex.Status);
            Assert.Equal(1, handler.CallCount);
        }
    }

    [Fact]
    public void DoesNotRetry409ByDefault() {
        // 409 is overloaded server-side: lock contention is transient, but
        // OutdatedEditSequence / NameNotUnique are terminal. Without
        // X-Should-Retry, the safe default is to surface to the caller.
        var (client, handler) = CreateClient(
            Error(HttpStatusCode.Conflict));
        using (client) {
            var ex = Assert.Throws<NxusApiException>(() => client.Connections.Retrieve("conn_123"));

            Assert.Equal(409, ex.Status);
            Assert.Equal(1, handler.CallCount);
        }
    }

    [Fact]
    public void Retries409WhenXShouldRetryTrue() {
        var (client, handler) = CreateClient(
            Error(HttpStatusCode.Conflict, ("X-Should-Retry", "true"), ("Retry-After", "0")),
            Json(HttpStatusCode.OK, """{"id":"conn_123"}"""));
        using (client) {
            var result = client.Connections.Retrieve("conn_123");

            Assert.Equal("conn_123", result.GetProperty("id").GetString());
            Assert.Equal(2, handler.CallCount);
        }
    }

    [Fact]
    public void UsesBodyRetryAfterWhenHeaderMissing() {
        // Backend body shape (RateLimitingExtensions returns this on 429).
        var rateLimitedBody =
            """{"error":{"message":"rate limited","code":"RATE_LIMIT_EXCEEDED","httpStatusCode":429,"retryAfter":0}}""";
        var rateLimitedResponse = new HttpResponseMessage(HttpStatusCode.TooManyRequests) {
            Content = new StringContent(rateLimitedBody, Encoding.UTF8, "application/json"),
        };
        var (client, handler) = CreateClient(
            rateLimitedResponse,
            Json(HttpStatusCode.OK, """{"id":"conn_123"}"""));
        using (client) {
            var result = client.Connections.Retrieve("conn_123");

            Assert.Equal("conn_123", result.GetProperty("id").GetString());
            Assert.Equal(2, handler.CallCount);
        }
    }

    [Fact]
    public void PerRequestMaxRetriesOverridesClientDefault() {
        var (client, handler) = CreateClient(
            new[] {
                Error(HttpStatusCode.ServiceUnavailable, ("Retry-After", "0")),
                Json(HttpStatusCode.OK, """{"id":"conn_123"}"""),
            },
            maxRetries: 0);
        using (client) {
            var result = client.Connections.Retrieve(
                "conn_123",
                new RequestOptions { MaxRetries = 1 });

            Assert.Equal("conn_123", result.GetProperty("id").GetString());
            Assert.Equal(2, handler.CallCount);
        }
    }

    private static (NxusClient Client, QueueHandler Handler) CreateClient(params HttpResponseMessage[] responses) =>
        CreateClient(responses, maxRetries: 2);

    private static (NxusClient Client, QueueHandler Handler) CreateClient(
        IEnumerable<HttpResponseMessage> responses,
        int maxRetries) {
        var handler = new QueueHandler(responses);
        var httpClient = new HttpClient(handler) {
            BaseAddress = new Uri("https://api.example.test/"),
        };

        var client = new NxusClient(new NxusClientOptions {
            ApiKey = "sk_test",
            HttpClient = httpClient,
            MaxRetries = maxRetries,
        });
        return (client, handler);
    }

    private static HttpResponseMessage Json(HttpStatusCode status, string body) =>
        new(status) {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };

    private static HttpResponseMessage Error(
        HttpStatusCode status,
        params (string Key, string Value)[] headers) {
        var response = Json(
            status,
            "{\"error\":{\"message\":\"try again\",\"httpStatusCode\":" + (int)status + "}}");
        foreach (var (key, value) in headers)
            response.Headers.TryAddWithoutValidation(key, value);
        return response;
    }

    private sealed class QueueHandler : HttpMessageHandler {
        private readonly Queue<HttpResponseMessage> _responses;

        public QueueHandler(IEnumerable<HttpResponseMessage> responses) {
            _responses = new Queue<HttpResponseMessage>(responses);
        }

        public int CallCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) {
            CallCount++;
            if (_responses.Count == 0)
                throw new InvalidOperationException("No response queued for test HTTP call.");
            return Task.FromResult(_responses.Dequeue());
        }
    }
}
