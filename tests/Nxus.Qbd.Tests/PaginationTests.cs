using System.Net;
using System.Text;
using Nxus.Qbd;
using Xunit;

namespace Nxus.Qbd.Tests;

public class PaginationTests {
    [Fact]
    public async Task AutoPagination_ClosesCursor_WhenEnumerationStopsEarly() {
        using var handler = new RecordingHandler(
            new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent(
                    """
                    {"data":[{"id":"vendor_1","name":"Vendor 1"}],"hasMore":true,"nextCursor":"cursor_2","count":1}
                    """,
                    Encoding.UTF8,
                    "application/json")
            },
            new HttpResponseMessage(HttpStatusCode.NoContent)
        );

        using var httpClient = new HttpClient(handler) {
            BaseAddress = new Uri("https://api.example.test/"),
            Timeout = TimeSpan.FromSeconds(30),
        };

        using var client = new NxusClient(new NxusClientOptions {
            ApiKey = "sk_test_123",
            HttpClient = httpClient,
        });

        var page = await client.Vendors.ListAsync(limit: 1);

        await foreach (var item in page) {
            Assert.Equal("vendor_1", item.GetProperty("id").GetString());
            break;
        }

        Assert.Collection(
            handler.Requests,
            request => {
                Assert.Equal(HttpMethod.Get, request.Method);
                Assert.Equal("https://api.example.test/api/v1/vendors?limit=1", request.RequestUri?.ToString());
            },
            request => {
                Assert.Equal(HttpMethod.Post, request.Method);
                Assert.Equal("https://api.example.test/api/v1/cursors/cursor_2/close", request.RequestUri?.ToString());
                Assert.False(request.Headers.Contains("X-Connection-Id"));
            });
    }

    private sealed class RecordingHandler : HttpMessageHandler, IDisposable {
        private readonly Queue<HttpResponseMessage> _responses;

        public RecordingHandler(params HttpResponseMessage[] responses) {
            _responses = new Queue<HttpResponseMessage>(responses);
        }

        public List<HttpRequestMessage> Requests { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            Requests.Add(CloneRequest(request));
            return Task.FromResult(_responses.Dequeue());
        }

        private static HttpRequestMessage CloneRequest(HttpRequestMessage request) {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);
            foreach (var header in request.Headers) {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            return clone;
        }

        public new void Dispose() {
            foreach (var response in _responses) {
                response.Dispose();
            }
            foreach (var request in Requests) {
                request.Dispose();
            }
            base.Dispose();
        }
    }
}
