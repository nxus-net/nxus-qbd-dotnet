namespace Nxus.Qbd;

/// <summary>
/// Configuration options for <see cref="NxusClient"/>.
/// </summary>
public sealed class NxusClientOptions {
    /// <summary>
    /// Your Nxus API key (<c>sk_live_...</c> or <c>sk_test_...</c>).
    /// </summary>
    public required string ApiKey { get; init; }

    /// <summary>
    /// Base URL for the API. Defaults to <c>https://api.nxus.app/</c>.
    /// </summary>
    public string BaseUrl { get; init; } = "https://api.nxus.app/";

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
}
