using System.Collections;
using System.Text.Json;

namespace Nxus.Qbd.Pagination;

/// <summary>
/// A single page of cursor-paginated results from the Nxus API.
/// Supports manual page navigation and automatic async enumeration across all pages.
/// </summary>
/// <typeparam name="T">The element type (typically <see cref="JsonElement"/> for untyped usage).</typeparam>
public sealed class CursorPage<T> : IEnumerable<T>, IAsyncEnumerable<T> {
    /// <summary>Items in this page.</summary>
    public IReadOnlyList<T> Data { get; }

    /// <summary>Whether additional pages exist beyond this one.</summary>
    public bool HasMore { get; }

    /// <summary>Opaque cursor to fetch the next page. <c>null</c> on the last page.</summary>
    public string? NextCursor { get; }

    /// <summary>Number of items in this page.</summary>
    public int Count { get; }

    /// <summary>The <c>limit</c> query parameter used for this request.</summary>
    public int? Limit { get; }

    /// <summary>1-based page number (informational).</summary>
    public int? Page { get; }

    /// <summary>Items remaining after this page.</summary>
    public int? RemainingCount { get; }

    /// <summary>Total items across all pages.</summary>
    public int? TotalCount { get; }

    internal Func<string, CancellationToken, Task<CursorPage<T>>>? AsyncFetcher { get; set; }
    internal Func<string, CursorPage<T>>? SyncFetcher { get; set; }
    internal Func<string, CancellationToken, Task>? AsyncCloser { get; set; }
    internal Action<string>? SyncCloser { get; set; }

    public CursorPage(
        IReadOnlyList<T> data,
        bool hasMore,
        string? nextCursor,
        int? count = null,
        int? limit = null,
        int? page = null,
        int? remainingCount = null,
        int? totalCount = null) {
        Data = data;
        HasMore = hasMore;
        NextCursor = nextCursor;
        Count = count ?? data.Count;
        Limit = limit;
        Page = page;
        RemainingCount = remainingCount;
        TotalCount = totalCount;
    }

    /// <summary>Whether there is another page to fetch.</summary>
    public bool HasNextPage() => HasMore && NextCursor is not null;

    /// <summary>Fetch the next page synchronously.</summary>
    /// <exception cref="PaginationException">No more pages or no sync fetcher attached.</exception>
    public CursorPage<T> GetNextPage() {
        if (!HasNextPage())
            throw new PaginationException("No additional pages are available.");
        if (SyncFetcher is null)
            throw new PaginationException("No synchronous fetcher is attached. Use GetNextPageAsync() instead.");
        return SyncFetcher(NextCursor!);
    }

    /// <summary>Fetch the next page asynchronously.</summary>
    /// <exception cref="PaginationException">No more pages or no async fetcher attached.</exception>
    public async Task<CursorPage<T>> GetNextPageAsync(CancellationToken ct = default) {
        if (!HasNextPage())
            throw new PaginationException("No additional pages are available.");
        if (AsyncFetcher is null)
            throw new PaginationException("No asynchronous fetcher is attached. Use GetNextPage() instead.");
        return await AsyncFetcher(NextCursor!, ct).ConfigureAwait(false);
    }

    // ── Sync enumeration (all pages) ────────────────────────────────────

    public IEnumerator<T> GetEnumerator() {
        var current = this;
        var completed = false;
        string? liveCursor = current.HasNextPage() ? current.NextCursor : null;

        try {
            while (true) {
                liveCursor = current.HasNextPage() ? current.NextCursor : null;
                foreach (var item in current.Data)
                    yield return item;
                if (!current.HasNextPage()) {
                    completed = true;
                    yield break;
                }
                current = current.GetNextPage();
            }
        } finally {
            if (!completed && liveCursor is not null && SyncCloser is not null) {
                try {
                    SyncCloser(liveCursor);
                } catch {
                    // Cursor close is best-effort cleanup and must never mask iteration flow.
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // ── Async enumeration (all pages) ───────────────────────────────────

    public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken ct = default) {
        var current = this;
        var completed = false;
        string? liveCursor = current.HasNextPage() ? current.NextCursor : null;

        try {
            while (true) {
                liveCursor = current.HasNextPage() ? current.NextCursor : null;
                foreach (var item in current.Data)
                    yield return item;
                if (!current.HasNextPage()) {
                    completed = true;
                    yield break;
                }
                current = await current.GetNextPageAsync(ct).ConfigureAwait(false);
            }
        } finally {
            if (!completed && liveCursor is not null && AsyncCloser is not null) {
                try {
                    await AsyncCloser(liveCursor, ct).ConfigureAwait(false);
                } catch {
                    // Cursor close is best-effort cleanup and must never mask iteration flow.
                }
            }
        }
    }

    public override string ToString() =>
        $"CursorPage(Count={Count}, HasMore={HasMore}, NextCursor={NextCursor})";
}
