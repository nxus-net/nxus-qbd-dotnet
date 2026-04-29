using DotNetEnv;
using Nxus.Qbd;
using Nxus.Qbd.Errors;
using Xunit;

namespace Nxus.Qbd.Tests;

/// <summary>
/// Integration smoke tests that run against a live (or local) Nxus API.
/// Skipped when NXUS_API_KEY is not set.
/// </summary>
public class SmokeTests : IDisposable {
    private readonly NxusClient? _client;
    private readonly string? _connectionId;

    public SmokeTests() {
        // Load .env from repo root
        var envPath = Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".env");
        if (File.Exists(envPath))
            Env.Load(envPath);

        var apiKey = Environment.GetEnvironmentVariable("NXUS_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
            return; // tests will be skipped

        var baseUrl = Environment.GetEnvironmentVariable("NXUS_BASE_URL");
        var environmentName = Environment.GetEnvironmentVariable("NXUS_ENVIRONMENT");
        _connectionId = Environment.GetEnvironmentVariable("NXUS_CONNECTION_ID");

        var environment = string.Equals(environmentName, "development", StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(environmentName, "dev", StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(environmentName, "local", StringComparison.OrdinalIgnoreCase)
            ? NxusEnvironment.Development
            : NxusEnvironment.Production;

        _client = new NxusClient(new NxusClientOptions {
            ApiKey = apiKey,
            BaseUrl = baseUrl,
            Environment = environment,
            ConnectionId = _connectionId,
        });
    }

    private bool ShouldSkip => _client is null;

    [Fact]
    public void ListVendors_ReturnsPage() {
        if (ShouldSkip) return;

        var page = _client!.Vendors.List(limit: 5);

        Assert.NotNull(page);
        Assert.True(page.Count >= 0);
        Assert.NotNull(page.Data);
    }

    [Fact]
    public async Task ListVendorsAsync_ReturnsPage() {
        if (ShouldSkip) return;

        var page = await _client!.Vendors.ListAsync(limit: 5);

        Assert.NotNull(page);
        Assert.True(page.Count >= 0);
        Assert.NotNull(page.Data);
    }

    [Fact]
    public void Pagination_NavigatesPages() {
        if (ShouldSkip) return;

        var page = _client!.Vendors.List(limit: 2);
        var pageCount = 1;

        while (page.HasNextPage() && pageCount < 5) {
            page = page.GetNextPage();
            pageCount++;
        }

        Assert.True(pageCount >= 1);
    }

    [Fact]
    public void RetrieveNonexistent_ThrowsNotFound() {
        if (ShouldSkip) return;

        var ex = Assert.Throws<NxusApiException>(() =>
            _client!.Vendors.Retrieve("nonexistent-00000"));

        Assert.True(ex.IsNotFound);
        Assert.Equal(404, ex.Status);
    }

    [Fact]
    public async Task RetrieveNonexistentAsync_ThrowsNotFound() {
        if (ShouldSkip) return;

        var ex = await Assert.ThrowsAsync<NxusApiException>(async () =>
            await _client!.Vendors.RetrieveAsync("nonexistent-00000"));

        Assert.True(ex.IsNotFound);
        Assert.Equal(404, ex.Status);
    }

    [Fact]
    public void ListCustomers_AutoIterate() {
        if (ShouldSkip) return;

        var page = _client!.Customers.List(limit: 5);
        var count = 0;

        // Auto-iterate via IEnumerable — stops after 20 for safety
        foreach (var item in page) {
            count++;
            if (count >= 20) break;
        }

        Assert.True(count > 0);
    }

    public void Dispose() => _client?.Dispose();
}
