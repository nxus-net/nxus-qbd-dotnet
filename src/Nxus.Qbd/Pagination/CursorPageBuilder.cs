using System.Text.Json;

namespace Nxus.Qbd.Pagination;

/// <summary>
/// Builds <see cref="CursorPage{T}"/> instances from raw API JSON responses.
/// </summary>
internal static class CursorPageBuilder {
    /// <summary>
    /// Parse a JSON response body into a <see cref="CursorPage{T}"/>.
    /// Expects the standard Nxus paginated shape: <c>{ data, hasMore, nextCursor, count, ... }</c>.
    /// </summary>
    public static CursorPage<JsonElement> FromJson(JsonElement root) {
        var data = root.TryGetProperty("data", out var dataEl) && dataEl.ValueKind == JsonValueKind.Array
            ? dataEl.EnumerateArray().ToList()
            : new List<JsonElement>();

        var hasMore = root.TryGetProperty("hasMore", out var hm) &&
                      hm.ValueKind == JsonValueKind.True;

        string? nextCursor = root.TryGetProperty("nextCursor", out var nc) &&
                             nc.ValueKind == JsonValueKind.String
            ? nc.GetString()
            : null;

        int? count = GetOptionalInt(root, "count");
        int? limit = GetOptionalInt(root, "limit");
        int? page = GetOptionalInt(root, "page");
        int? remainingCount = GetOptionalInt(root, "remainingCount");
        int? totalCount = GetOptionalInt(root, "totalCount");

        return new CursorPage<JsonElement>(
            data: data,
            hasMore: hasMore,
            nextCursor: nextCursor,
            count: count,
            limit: limit,
            page: page,
            remainingCount: remainingCount,
            totalCount: totalCount);
    }

    private static int? GetOptionalInt(JsonElement el, string property) =>
        el.TryGetProperty(property, out var val) && val.ValueKind == JsonValueKind.Number
            ? val.GetInt32()
            : null;
}
