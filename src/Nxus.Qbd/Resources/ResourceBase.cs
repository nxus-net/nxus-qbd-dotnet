using System.Text.Json;
using Nxus.Qbd.Pagination;
using Nxus.Qbd.Transport;

namespace Nxus.Qbd.Resources;

/// <summary>
/// Base class for all resource namespaces. Provides CRUD helpers that
/// concrete resources compose via simple path configuration.
/// </summary>
public abstract class ResourceBase {
    private readonly NxusHttpTransport _transport;

    /// <summary>Plural path for list operations (e.g. <c>/api/v1/vendors</c>).</summary>
    protected abstract string ListPath { get; }

    /// <summary>Singular path template with <c>{id}</c> placeholder (e.g. <c>/api/v1/vendor/{id}</c>).</summary>
    protected abstract string SingularPath { get; }

    /// <summary>Path for create (POST). Defaults to singular path without <c>/{id}</c>.</summary>
    protected virtual string CreatePath => SingularPath.Replace("/{id}", "");

    internal ResourceBase(NxusHttpTransport transport) {
        _transport = transport;
    }

    private static RequestOptions? BuildCursorCloseOptions(RequestOptions? options) {
        if (options is null)
            return null;

        return new RequestOptions {
            Headers = options.Headers is null
                ? null
                : new Dictionary<string, string>(options.Headers),
            Timeout = options.Timeout,
        };
    }

    // ── List (paginated) ────────────────────────────────────────────────

    /// <summary>List resources with cursor-based pagination.</summary>
    public CursorPage<JsonElement> List(
        int? limit = null,
        string? cursor = null,
        IDictionary<string, string>? queryParams = null,
        RequestOptions? options = null) {
        var allParams = new Dictionary<string, string>(queryParams ?? new Dictionary<string, string>());
        if (limit.HasValue) allParams["limit"] = limit.Value.ToString();
        if (cursor is not null) allParams["cursor"] = cursor;

        var body = _transport.Request(HttpMethod.Get, ListPath, queryParams: allParams, options: options);
        var page = CursorPageBuilder.FromJson(body);

        // Attach sync fetcher for page navigation
        page.SyncFetcher = nextCursor => List(limit, nextCursor, queryParams, options);
        page.SyncCloser = nextCursor => _transport.Request(
            HttpMethod.Post,
            $"/api/v1/cursors/{Uri.EscapeDataString(nextCursor)}/close",
            options: BuildCursorCloseOptions(options));
        return page;
    }

    /// <summary>List resources with cursor-based pagination (async).</summary>
    public async Task<CursorPage<JsonElement>> ListAsync(
        int? limit = null,
        string? cursor = null,
        IDictionary<string, string>? queryParams = null,
        RequestOptions? options = null,
        CancellationToken ct = default) {
        var allParams = new Dictionary<string, string>(queryParams ?? new Dictionary<string, string>());
        if (limit.HasValue) allParams["limit"] = limit.Value.ToString();
        if (cursor is not null) allParams["cursor"] = cursor;

        var body = await _transport.RequestAsync(HttpMethod.Get, ListPath, queryParams: allParams, options: options, ct: ct).ConfigureAwait(false);
        var page = CursorPageBuilder.FromJson(body);

        // Attach async fetcher for page navigation
        page.AsyncFetcher = (nextCursor, innerCt) => ListAsync(limit, nextCursor, queryParams, options, innerCt);
        page.AsyncCloser = (nextCursor, innerCt) => _transport.RequestAsync(
            HttpMethod.Post,
            $"/api/v1/cursors/{Uri.EscapeDataString(nextCursor)}/close",
            options: BuildCursorCloseOptions(options),
            ct: innerCt);
        return page;
    }

    // ── Retrieve ────────────────────────────────────────────────────────

    /// <summary>Retrieve a single resource by ID.</summary>
    public JsonElement Retrieve(string id, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, SingularPath.Replace("{id}", id), options: options);

    /// <summary>Retrieve a single resource by ID (async).</summary>
    public Task<JsonElement> RetrieveAsync(string id, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, SingularPath.Replace("{id}", id), options: options, ct: ct);

    // ── Create ──────────────────────────────────────────────────────────

    /// <summary>Create a new resource.</summary>
    public JsonElement Create(object data, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Post, CreatePath, body: data, options: options);

    /// <summary>Create a new resource (async).</summary>
    public Task<JsonElement> CreateAsync(object data, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Post, CreatePath, body: data, options: options, ct: ct);

    // ── Update (POST to singular path — Nxus convention) ────────────────

    /// <summary>Update a resource by ID. Uses POST (not PATCH/PUT) per Nxus API convention.</summary>
    public JsonElement Update(string id, object data, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Post, SingularPath.Replace("{id}", id), body: data, options: options);

    /// <summary>Update a resource by ID (async).</summary>
    public Task<JsonElement> UpdateAsync(string id, object data, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Post, SingularPath.Replace("{id}", id), body: data, options: options, ct: ct);

    // ── Delete ──────────────────────────────────────────────────────────

    /// <summary>Delete a resource by ID.</summary>
    public JsonElement Delete(string id, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Delete, SingularPath.Replace("{id}", id), options: options);

    /// <summary>Delete a resource by ID (async).</summary>
    public Task<JsonElement> DeleteAsync(string id, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Delete, SingularPath.Replace("{id}", id), options: options, ct: ct);
}
