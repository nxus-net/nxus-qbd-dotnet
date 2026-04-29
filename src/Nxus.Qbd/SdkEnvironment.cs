namespace Nxus.Qbd;

/// <summary>
/// Environment and base URL helpers for the Nxus .NET SDK.
/// </summary>
public static class SdkEnvironment {
    public const string DefaultBaseUrl = "https://api.nx-us.net/";
    public const string LocalBaseUrl = "https://localhost:7242/";

    public static string ResolveBaseUrl(
        string? baseUrl = null,
        NxusEnvironment environment = NxusEnvironment.Production) {
        if (!string.IsNullOrWhiteSpace(baseUrl))
            return baseUrl;

        return environment == NxusEnvironment.Development
            ? LocalBaseUrl
            : DefaultBaseUrl;
    }
}
