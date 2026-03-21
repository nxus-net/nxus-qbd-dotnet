using System.Text.Json;
using Nxus.Qbd.Transport;

namespace Nxus.Qbd.Resources;

/// <summary>
/// Reports resource — each report endpoint is a GET with query parameters, no resource ID.
/// </summary>
public sealed class ReportsResource {
    private readonly NxusHttpTransport _transport;

    internal ReportsResource(NxusHttpTransport transport) {
        _transport = transport;
    }

    /// <summary>Retrieve an aging report (A/R or A/P aging).</summary>
    public JsonElement RetrieveAging(IDictionary<string, string> queryParams, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, "/api/v1/reports/aging", queryParams: queryParams, options: options);

    /// <summary>Retrieve an aging report (async).</summary>
    public Task<JsonElement> RetrieveAgingAsync(IDictionary<string, string> queryParams, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, "/api/v1/reports/aging", queryParams: queryParams, options: options, ct: ct);

    /// <summary>Retrieve a general detail report (e.g. General Ledger).</summary>
    public JsonElement RetrieveGeneralDetail(IDictionary<string, string> queryParams, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, "/api/v1/reports/general-detail", queryParams: queryParams, options: options);

    /// <summary>Retrieve a general detail report (async).</summary>
    public Task<JsonElement> RetrieveGeneralDetailAsync(IDictionary<string, string> queryParams, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, "/api/v1/reports/general-detail", queryParams: queryParams, options: options, ct: ct);

    /// <summary>Retrieve a general summary report (e.g. Profit &amp; Loss, Balance Sheet).</summary>
    public JsonElement RetrieveGeneralSummary(IDictionary<string, string> queryParams, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, "/api/v1/reports/general-summary", queryParams: queryParams, options: options);

    /// <summary>Retrieve a general summary report (async).</summary>
    public Task<JsonElement> RetrieveGeneralSummaryAsync(IDictionary<string, string> queryParams, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, "/api/v1/reports/general-summary", queryParams: queryParams, options: options, ct: ct);

    /// <summary>Retrieve a budget summary report.</summary>
    public JsonElement RetrieveBudgetSummary(IDictionary<string, string> queryParams, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, "/api/v1/reports/budget-summary", queryParams: queryParams, options: options);

    /// <summary>Retrieve a budget summary report (async).</summary>
    public Task<JsonElement> RetrieveBudgetSummaryAsync(IDictionary<string, string> queryParams, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, "/api/v1/reports/budget-summary", queryParams: queryParams, options: options, ct: ct);

    /// <summary>Retrieve a job report.</summary>
    public JsonElement RetrieveJob(IDictionary<string, string> queryParams, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, "/api/v1/reports/job", queryParams: queryParams, options: options);

    /// <summary>Retrieve a job report (async).</summary>
    public Task<JsonElement> RetrieveJobAsync(IDictionary<string, string> queryParams, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, "/api/v1/reports/job", queryParams: queryParams, options: options, ct: ct);

    /// <summary>Retrieve a time report.</summary>
    public JsonElement RetrieveTime(IDictionary<string, string> queryParams, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, "/api/v1/reports/time", queryParams: queryParams, options: options);

    /// <summary>Retrieve a time report (async).</summary>
    public Task<JsonElement> RetrieveTimeAsync(IDictionary<string, string> queryParams, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, "/api/v1/reports/time", queryParams: queryParams, options: options, ct: ct);

    /// <summary>Retrieve a custom detail report.</summary>
    public JsonElement RetrieveCustomDetail(IDictionary<string, string> queryParams, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, "/api/v1/reports/custom-detail", queryParams: queryParams, options: options);

    /// <summary>Retrieve a custom detail report (async).</summary>
    public Task<JsonElement> RetrieveCustomDetailAsync(IDictionary<string, string> queryParams, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, "/api/v1/reports/custom-detail", queryParams: queryParams, options: options, ct: ct);

    /// <summary>Retrieve a custom summary report.</summary>
    public JsonElement RetrieveCustomSummary(IDictionary<string, string> queryParams, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, "/api/v1/reports/custom-summary", queryParams: queryParams, options: options);

    /// <summary>Retrieve a custom summary report (async).</summary>
    public Task<JsonElement> RetrieveCustomSummaryAsync(IDictionary<string, string> queryParams, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, "/api/v1/reports/custom-summary", queryParams: queryParams, options: options, ct: ct);

    /// <summary>Retrieve a payroll detail report.</summary>
    public JsonElement RetrievePayrollDetail(IDictionary<string, string> queryParams, RequestOptions? options = null) =>
        _transport.Request(HttpMethod.Get, "/api/v1/reports/payroll-detail", queryParams: queryParams, options: options);

    /// <summary>Retrieve a payroll detail report (async).</summary>
    public Task<JsonElement> RetrievePayrollDetailAsync(IDictionary<string, string> queryParams, RequestOptions? options = null, CancellationToken ct = default) =>
        _transport.RequestAsync(HttpMethod.Get, "/api/v1/reports/payroll-detail", queryParams: queryParams, options: options, ct: ct);
}
