namespace Nxus.Qbd.Errors;

/// <summary>
/// Known error codes returned by the Nxus API.
/// </summary>
public static class NxusApiErrorCode {
    public const string RateLimitExceeded = "RATE_LIMIT_EXCEEDED";
    public const string ValidationError = "VALIDATION_ERROR";
    public const string QbdConnectionError = "QBD_CONNECTION_ERROR";
    public const string QbdStaleEditSequence = "QBD_STALE_EDIT_SEQUENCE";
    public const string AuthenticationFailed = "AUTHENTICATION_FAILED";
    public const string AuthorizationFailed = "AUTHORIZATION_FAILED";
    public const string NotFound = "NOT_FOUND";
    public const string InternalError = "INTERNAL_ERROR";
    public const string QbdIntegrationError = "QBD_INTEGRATION_ERROR";
    public const string Conflict = "CONFLICT";
}
