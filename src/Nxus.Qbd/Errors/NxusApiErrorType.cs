namespace Nxus.Qbd.Errors;

/// <summary>
/// Broad error categories returned by the Nxus API.
/// </summary>
public static class NxusApiErrorType {
    public const string Authentication = "AUTHENTICATION_ERROR_TYPE";
    public const string Validation = "VALIDATION_ERROR_TYPE";
    public const string NotFound = "NOT_FOUND_ERROR_TYPE";
    public const string RateLimit = "RATE_LIMIT_ERROR_TYPE";
    public const string Integration = "INTEGRATION_ERROR_TYPE";
    public const string Api = "API_ERROR_TYPE";
}
