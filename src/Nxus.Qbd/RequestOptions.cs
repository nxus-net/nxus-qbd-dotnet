namespace Nxus.Qbd;

/// <summary>
/// Per-request overrides for connection ID, headers, and timeout.
/// </summary>
public sealed class RequestOptions {
    /// <summary>
    /// Connection ID (GUID or externalId) for data isolation.
    /// Sent as the <c>X-Connection-Id</c> header.
    /// </summary>
    public string? ConnectionId { get; init; }

    /// <summary>
    /// Additional headers for this request only.
    /// </summary>
    public IDictionary<string, string>? Headers { get; init; }

    /// <summary>
    /// Override the default timeout for this request.
    /// </summary>
    public TimeSpan? Timeout { get; init; }

    /// <summary>
    /// Creates a <see cref="RequestOptions"/> with just a connection ID.
    /// </summary>
    public static RequestOptions ForConnection(string connectionId) =>
        new() { ConnectionId = connectionId };
}
