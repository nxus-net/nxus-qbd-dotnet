using System.Net;
using System.Text;
using Nxus.Qbd;
using Nxus.Qbd.Resources;
using Xunit;

namespace Nxus.Qbd.Tests;

public class PaginationTests {
    private static readonly (string Name, Func<NxusClient, VoidableResourceBase> SelectResource, string Path)[] VoidableResourceCases = [
        ("ArRefundCreditCards", client => client.ArRefundCreditCards, "/api/v1/ar-refund-credit-card/txn_123/void"),
        ("Bills", client => client.Bills, "/api/v1/bill/txn_123/void"),
        ("Charges", client => client.Charges, "/api/v1/charge/txn_123/void"),
        ("CheckBills", client => client.CheckBills, "/api/v1/check-bill/txn_123/void"),
        ("Checks", client => client.Checks, "/api/v1/check/txn_123/void"),
        ("CreditCardBills", client => client.CreditCardBills, "/api/v1/credit-card-bill/txn_123/void"),
        ("CreditCardCharges", client => client.CreditCardCharges, "/api/v1/credit-card-charge/txn_123/void"),
        ("CreditCardCredits", client => client.CreditCardCredits, "/api/v1/credit-card-credit/txn_123/void"),
        ("CreditMemos", client => client.CreditMemos, "/api/v1/credit-memo/txn_123/void"),
        ("Deposits", client => client.Deposits, "/api/v1/deposit/txn_123/void"),
        ("InventoryAdjustments", client => client.InventoryAdjustments, "/api/v1/inventory-adjustment/txn_123/void"),
        ("Invoices", client => client.Invoices, "/api/v1/invoice/txn_123/void"),
        ("ItemReceipts", client => client.ItemReceipts, "/api/v1/item-receipt/txn_123/void"),
        ("JournalEntries", client => client.JournalEntries, "/api/v1/journal-entry/txn_123/void"),
        ("SalesReceipts", client => client.SalesReceipts, "/api/v1/sales-receipt/txn_123/void"),
        ("VendorCredits", client => client.VendorCredits, "/api/v1/vendor-credit/txn_123/void"),
    ];

    [Fact]
    public void ResolveBaseUrl_DefaultsToProduction() {
        Assert.Equal("https://api.nx-us.net/", SdkEnvironment.ResolveBaseUrl());
        Assert.Equal("https://api.nx-us.net/", SdkEnvironment.ResolveBaseUrl(environment: NxusEnvironment.Production));
        Assert.Equal("https://localhost:7242/", SdkEnvironment.ResolveBaseUrl(environment: NxusEnvironment.Development));
        Assert.Equal("https://custom.example.test/", SdkEnvironment.ResolveBaseUrl("https://custom.example.test/"));
    }

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

    [Fact]
    public void VoidableResources_PostToSingularVoidEndpoints() {
        using var handler = new RecordingHandler(
            VoidableResourceCases
                .Select(entry => Json(
                    $$"""{"id":"txn_123","objectType":"{{entry.Name}}","status":"voided","voided":true,"refNumber":"REF-123"}"""))
                .ToArray()
        );

        using var httpClient = new HttpClient(handler) {
            BaseAddress = new Uri("https://api.example.test/"),
            Timeout = TimeSpan.FromSeconds(30),
        };

        using var client = new NxusClient(new NxusClientOptions {
            ApiKey = "sk_test_123",
            HttpClient = httpClient,
        });

        foreach (var (name, selectResource, expectedPath) in VoidableResourceCases) {
            var result = selectResource(client).Void(
                "txn_123",
                new RequestOptions {
                    ConnectionId = "conn_123",
                    ServerTimeoutSeconds = 75,
                });

            Assert.Equal("txn_123", result.Id);
            Assert.Equal(name, result.ObjectType);
            Assert.Equal("voided", result.Status);
            Assert.True(result.Voided.GetValueOrDefault());
        }

        Assert.Collection(
            handler.Requests,
            VoidableResourceCases
                .Select<(string Name, Func<NxusClient, VoidableResourceBase> SelectResource, string Path), Action<HttpRequestMessage>>(entry => request => {
                    Assert.Equal(HttpMethod.Post, request.Method);
                    Assert.Equal($"https://api.example.test{entry.Path}", request.RequestUri?.ToString());
                    Assert.True(request.Headers.TryGetValues("X-Connection-Id", out var connectionIds));
                    Assert.Equal("conn_123", Assert.Single(connectionIds));
                    Assert.True(request.Headers.TryGetValues("X-Nxus-Timeout-Seconds", out var timeoutHints));
                    Assert.Equal("75", Assert.Single(timeoutHints));
                })
                .ToArray());

        Assert.IsNotAssignableFrom<VoidableResourceBase>(client.PurchaseOrders);
        Assert.IsNotAssignableFrom<VoidableResourceBase>(client.SalesTaxPaymentChecks);
        Assert.IsNotAssignableFrom<VoidableResourceBase>(client.TimeTrackings);
    }

    private static HttpResponseMessage Json(string body) =>
        new(HttpStatusCode.OK) {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };

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
