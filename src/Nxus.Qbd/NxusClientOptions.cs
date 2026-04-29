namespace Nxus.Qbd;

/// <summary>
/// Named SDK environments with stable default base URLs.
/// </summary>
public enum NxusEnvironment {
    Production,
    Development,
}

/// <summary>
/// Configuration options for <see cref="NxusClient"/>.
/// </summary>
public sealed class NxusClientOptions {
    /// <summary>
    /// Your Nxus API key (<c>sk_live_...</c> or <c>sk_test_...</c>).
    /// </summary>
    public required string ApiKey { get; init; }

    /// <summary>
    /// Explicit base URL override. When omitted, the SDK resolves the base URL
    /// from <see cref="Environment"/> and defaults to production.
    /// </summary>
    public string? BaseUrl { get; init; }

    /// <summary>
    /// Named environment shortcut. Use <see cref="NxusEnvironment.Development"/>
    /// for <c>https://localhost:7242/</c>. Production is the default.
    /// </summary>
    public NxusEnvironment Environment { get; init; } = NxusEnvironment.Production;

    /// <summary>
    /// Default request timeout. Defaults to 30 seconds.
    /// </summary>
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Optional default Connection ID header sent with every request.
    /// Can be overridden per-call via <see cref="RequestOptions.ConnectionId"/>.
    /// </summary>
    public string? ConnectionId { get; init; }

    /// <summary>
    /// Extra headers merged into every request.
    /// </summary>
    public IDictionary<string, string>? DefaultHeaders { get; init; }

    /// <summary>
    /// Optional <see cref="HttpClient"/> to use instead of creating one internally.
    /// When provided, the caller owns the lifetime — <see cref="NxusClient.Dispose"/> will not dispose it.
    /// </summary>
    public HttpClient? HttpClient { get; init; }

    /// <summary>
    /// Default value for the <c>X-Nxus-Timeout-Seconds</c> header sent on every
    /// request. Tells the server how long to wait for the queued QuickBooks
    /// Desktop job to complete before returning a 504. The server enforces
    /// operation-specific ceilings and may clamp this value based on deployment
    /// config. Current defaults are typically 120 seconds for CRUD and 90
    /// seconds for list/report operations. Leave null to let the server apply
    /// its own default.
    /// </summary>
    public int? ServerTimeoutSeconds { get; init; }
}
