namespace Nxus.Qbd.Pagination;

/// <summary>
/// Thrown when pagination fails (no more pages, missing fetcher, malformed response).
/// </summary>
public class PaginationException : Exception {
    public PaginationException(string message) : base(message) { }
    public PaginationException(string message, Exception inner) : base(message, inner) { }
}
