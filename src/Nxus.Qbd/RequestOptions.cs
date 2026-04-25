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
    /// Override for the <c>X-Nxus-Timeout-Seconds</c> header on this request.
    /// Tells the server how long to wait for the queued QuickBooks Desktop job
    /// to complete before returning a 504. The server enforces operation-
    /// specific ceilings and may clamp this value based on deployment config.
    /// Current defaults are typically 120 seconds for CRUD and 90 seconds for
    /// list/report operations. Omit to use the value configured on
    /// <see cref="NxusClientOptions.ServerTimeoutSeconds"/>, or the server
    /// default if neither is set.
    /// </summary>
    public int? ServerTimeoutSeconds { get; init; }

    /// <summary>
    /// Creates a <see cref="RequestOptions"/> with just a connection ID.
    /// </summary>
    public static RequestOptions ForConnection(string connectionId) =>
        new() { ConnectionId = connectionId };
}
