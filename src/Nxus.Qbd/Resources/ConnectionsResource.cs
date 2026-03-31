using System.Text.Json;
using Nxus.Qbd.Transport;

namespace Nxus.Qbd.Resources;

/// <summary>
/// Connections resource with full CRUD plus custom auth-status endpoint.
/// </summary>
public sealed class ConnectionsResource : ResourceBase {
    private readonly NxusHttpTransport _http;

    internal ConnectionsResource(NxusHttpTransport t) : base(t) {
        _http = t;
    }

    protected override string ListPath => "/api/v1/connections";
    protected override string SingularPath => "/api/v1/connection/{id}";

    /// <summary>Check the authenticated status of a connection.</summary>
    public JsonElement RetrieveStatusAuthenticated(string connectionId, RequestOptions? options = null) =>
        _http.Request(HttpMethod.Get, $"/api/v1/connection/{connectionId}/status/authenticated", options: options);

    /// <summary>Check the authenticated status of a connection (async).</summary>
    public Task<JsonElement> RetrieveStatusAuthenticatedAsync(string connectionId, RequestOptions? options = null, CancellationToken ct = default) =>
        _http.RequestAsync(HttpMethod.Get, $"/api/v1/connection/{connectionId}/status/authenticated", options: options, ct: ct);
}

/// <summary>
/// Auth sessions resource — create and retrieve only.
/// </summary>
public sealed class AuthSessionsResource {
    private readonly NxusHttpTransport _transport;

    internal AuthSessionsResource(NxusHttpTransport transport) {
        _transport = transport;
    }

    /// <summary>Create a new auth session.</summary>
    public JsonElement Create(object data, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Post, "/api/v1/auth-session", body: data, options: options);

    /// <summary>Create a new auth session (async).</summary>
    public Task<JsonElement> CreateAsync(object data, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Post, "/api/v1/auth-session", body: data, options: options, ct: ct);

    /// <summary>Retrieve an auth session by token.</summary>
    public JsonElement Retrieve(string sessionToken, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, $"/api/v1/auth-session/{sessionToken}", options: options);

    /// <summary>Retrieve an auth session by token (async).</summary>
    public Task<JsonElement> RetrieveAsync(string sessionToken, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, $"/api/v1/auth-session/{sessionToken}", options: options, ct: ct);
}

/// <summary>
/// Special items resource — create only.
/// </summary>
public sealed class SpecialItemsResource {
    private readonly NxusHttpTransport _transport;

    internal SpecialItemsResource(NxusHttpTransport transport) {
        _transport = transport;
    }

    /// <summary>Create a special item.</summary>
    public JsonElement Create(object data, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Post, "/api/v1/special-item", body: data, options: options);

    /// <summary>Create a special item (async).</summary>
    public Task<JsonElement> CreateAsync(object data, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Post, "/api/v1/special-item", body: data, options: options, ct: ct);
}
