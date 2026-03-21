using System.Net;
using System.Text.Json;

namespace Nxus.Qbd.Errors;

/// <summary>
/// Exception thrown when the Nxus API returns a non-2xx response.
/// Normalizes both <c>StandardErrorResponse</c> and <c>ProblemDetails</c> shapes
/// into a single exception with typed properties and boolean helpers.
/// </summary>
public class NxusApiException : Exception {
    /// <summary>HTTP status code.</summary>
    public int Status { get; }

    /// <summary>Machine-readable error code (e.g. <c>RATE_LIMIT_EXCEEDED</c>).</summary>
    public string? Code { get; }

    /// <summary>Broad error category (e.g. <c>AUTHENTICATION_ERROR_TYPE</c>).</summary>
    public string? ErrorType { get; }

    /// <summary>Human-readable message suitable for display to end users.</summary>
    public string UserMessage { get; }

    /// <summary>Server-assigned request ID for support inquiries.</summary>
    public string? RequestId { get; }

    /// <summary>QuickBooks Desktop integration error code, if applicable.</summary>
    public string? IntegrationCode { get; }

    /// <summary>Per-field validation errors (field name → messages), if applicable.</summary>
    public IReadOnlyDictionary<string, string[]>? ValidationErrors { get; }

    /// <summary>The raw JSON body deserialized as a <see cref="JsonElement"/>, if available.</summary>
    public JsonElement? RawBody { get; }

    public NxusApiException(
        string message,
        int status,
        string userMessage,
        string? code = null,
        string? errorType = null,
        string? requestId = null,
        string? integrationCode = null,
        IReadOnlyDictionary<string, string[]>? validationErrors = null,
        JsonElement? rawBody = null)
        : base(message) {
        Status = status;
        UserMessage = userMessage;
        Code = code;
        ErrorType = errorType;
        RequestId = requestId;
        IntegrationCode = integrationCode;
        ValidationErrors = validationErrors;
        RawBody = rawBody;
    }

    // ── Boolean helpers ──────────────────────────────────────────────────

    /// <summary>Whether this is a rate-limit error (429).</summary>
    public bool IsRateLimited => Status == 429 || Code == NxusApiErrorCode.RateLimitExceeded;

    /// <summary>Whether this is an authentication/authorization error (401 or 403).</summary>
    public bool IsAuthError => Status is 401 or 403 || ErrorType == NxusApiErrorType.Authentication;

    /// <summary>Whether this is a validation error with per-field details.</summary>
    public bool IsValidationError => ValidationErrors is not null;

    /// <summary>Whether this is a not-found error (404).</summary>
    public bool IsNotFound => Status == 404 || ErrorType == NxusApiErrorType.NotFound;

    /// <summary>Whether this error originated from QuickBooks Desktop.</summary>
    public bool IsIntegrationError => IntegrationCode is not null;

    /// <summary>Whether this is a stale edit sequence conflict (409).</summary>
    public bool IsConflict => Status == 409 || Code == NxusApiErrorCode.QbdStaleEditSequence;

    // ── Factory methods ──────────────────────────────────────────────────

    /// <summary>
    /// Creates a <see cref="NxusApiException"/> from an <see cref="HttpResponseMessage"/>.
    /// </summary>
    public static async Task<NxusApiException> FromResponseAsync(
        HttpResponseMessage response,
        CancellationToken ct = default) {
        var status = (int)response.StatusCode;
        string? bodyText = null;

        try {
            bodyText = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        } catch {
            // If we can't read the body, fall through to generic error
        }

        if (string.IsNullOrWhiteSpace(bodyText)) {
            return new NxusApiException(
                $"HTTP {status}",
                status,
                userMessage: $"Request failed with status {status}.");
        }

        JsonElement root;
        try {
            root = JsonSerializer.Deserialize<JsonElement>(bodyText);
        } catch {
            return new NxusApiException(
                bodyText,
                status,
                userMessage: bodyText);
        }

        return FromJsonElement(root, status);
    }

    /// <summary>
    /// Creates a <see cref="NxusApiException"/> from a parsed JSON body.
    /// </summary>
    public static NxusApiException FromJsonElement(JsonElement root, int status) {
        // StandardErrorResponse: { "error": { "message": ..., ... } }
        if (root.TryGetProperty("error", out var errObj) &&
            errObj.ValueKind == JsonValueKind.Object &&
            errObj.TryGetProperty("message", out _)) {
            return new NxusApiException(
                message: GetString(errObj, "message") ?? $"HTTP {status}",
                status: GetInt(errObj, "httpStatusCode") ?? status,
                userMessage: GetString(errObj, "userFacingMessage")
                             ?? GetString(errObj, "message")
                             ?? $"Request failed with status {status}.",
                code: GetString(errObj, "code"),
                errorType: GetString(errObj, "type"),
                requestId: GetString(errObj, "requestId"),
                integrationCode: GetString(errObj, "integrationCode"),
                rawBody: root);
        }

        // ProblemDetails: { "title": ..., "detail": ..., "status": ..., "errors": ... }
        if (root.TryGetProperty("status", out _) &&
            (root.TryGetProperty("title", out _) || root.TryGetProperty("detail", out _))) {
            var message = GetString(root, "detail")
                          ?? GetString(root, "title")
                          ?? "Validation failed.";

            Dictionary<string, string[]>? validationErrors = null;
            if (root.TryGetProperty("errors", out var errorsEl) &&
                errorsEl.ValueKind == JsonValueKind.Object) {
                validationErrors = new Dictionary<string, string[]>();
                foreach (var prop in errorsEl.EnumerateObject()) {
                    if (prop.Value.ValueKind == JsonValueKind.Array) {
                        validationErrors[prop.Name] = prop.Value
                            .EnumerateArray()
                            .Select(e => e.GetString() ?? "")
                            .ToArray();
                    }
                }
            }

            return new NxusApiException(
                message: message,
                status: GetInt(root, "status") ?? 422,
                userMessage: GetString(root, "detail")
                             ?? GetString(root, "title")
                             ?? "Please check your input and try again.",
                code: NxusApiErrorCode.ValidationError,
                errorType: NxusApiErrorType.Validation,
                validationErrors: validationErrors,
                rawBody: root);
        }

        // Fallback
        var fallbackMsg = GetString(root, "message")
                          ?? GetString(root, "detail")
                          ?? GetString(root, "title")
                          ?? "An unexpected error occurred.";

        return new NxusApiException(
            message: fallbackMsg,
            status: GetInt(root, "status") ?? GetInt(root, "statusCode") ?? status,
            userMessage: fallbackMsg,
            rawBody: root);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static string? GetString(JsonElement el, string property) =>
        el.TryGetProperty(property, out var val) && val.ValueKind == JsonValueKind.String
            ? val.GetString()
            : null;

    private static int? GetInt(JsonElement el, string property) =>
        el.TryGetProperty(property, out var val) && val.ValueKind == JsonValueKind.Number
            ? val.GetInt32()
            : null;
}
