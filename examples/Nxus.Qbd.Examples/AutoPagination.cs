/// <summary>
/// AutoPagination — Async auto-iteration and manual page-by-page navigation.
///
/// The SDK's CursorPage supports:
///   1. `await foreach (var item in ...)` — auto-fetches every page transparently.
///   2. `foreach (var item in ...)` — same, using the synchronous List() overload.
///   3. `page.GetNextPageAsync()` / `page.GetNextPage()` — manual page navigation.
///
/// A note on `break` and lane release:
///   When you `break` out of an `await foreach` (or `foreach`) loop, the SDK
///   quietly tells the backend you are done with the cursor (best-effort
///   `POST /api/v1/cursors/{cursor}/close`). The QuickBooks Desktop
///   connection lane is released within milliseconds — your *next* call on
///   the same connection does not have to wait for the silent-client timeout
///   to expire. There is nothing extra to do; just `break`.
///
/// Usage:
///   Set NXUS_API_KEY and NXUS_CONNECTION_ID in environment (or .env file), then:
///   dotnet run --project examples/Nxus.Qbd.Examples -- pagination
///
/// Optional env vars:
///   NXUS_BASE_URL         Defaults to https://api.nxus.app/
///   NXUS_DEV_MODE         Set to "true" to extend timeout for local dev
/// </summary>

using System.Text.Json;
using Nxus.Qbd;
using Nxus.Qbd.Errors;

namespace Nxus.Qbd.Examples;

public static class AutoPagination {
    public static async Task RunAsync() {
        var apiKey = Environment.GetEnvironmentVariable("NXUS_API_KEY");
        if (string.IsNullOrEmpty(apiKey)) {
            Console.Error.WriteLine("Error: NXUS_API_KEY environment variable is required.");
            Console.Error.WriteLine("  set NXUS_API_KEY=sk_test_...");
            Environment.Exit(1);
        }

        var connectionId = Environment.GetEnvironmentVariable("NXUS_CONNECTION_ID");
        if (string.IsNullOrEmpty(connectionId)) {
            Console.Error.WriteLine("Error: NXUS_CONNECTION_ID environment variable is required.");
            Console.Error.WriteLine("  Set it to the GUID (or externalId) of your QBD connection.");
            Environment.Exit(1);
        }

        var baseUrl = Environment.GetEnvironmentVariable("NXUS_BASE_URL") ?? "https://api.nxus.app/";
        var devMode = Environment.GetEnvironmentVariable("NXUS_DEV_MODE")?.ToLower() == "true";

        using var client = new NxusClient(new NxusClientOptions {
            ApiKey = apiKey,
            BaseUrl = baseUrl,
            ConnectionId = connectionId,
            Timeout = devMode ? TimeSpan.FromSeconds(60) : TimeSpan.FromSeconds(30),
        });

        try {
            // -----------------------------------------------------------------
            // Approach 1: Async auto-pagination with await foreach
            //
            // The simplest way to iterate through every record. The SDK
            // automatically fetches subsequent pages behind the scenes.
            // -----------------------------------------------------------------
            Console.WriteLine("=== Async Auto-Pagination (await foreach) ===\n");

            var count = 0;
            const int maxItems = 25; // cap for demo purposes

            var firstPage = await client.Customers.ListAsync(limit: 10);
            await foreach (var customer in firstPage) {
                count++;
                var name = customer.GetProperty("name").GetString();
                Console.WriteLine($"  {count}. {name} ({customer.GetProperty("id").GetString()})");

                if (count >= maxItems) {
                    Console.WriteLine($"  ... stopping after {maxItems} items for demo purposes.");
                    break;
                }
            }

            Console.WriteLine($"\nTotal items iterated: {count}");

            // -----------------------------------------------------------------
            // Approach 2: Find-and-stop
            //
            // The most common real-world pagination pattern: iterate until
            // you find what you need, then `break`. The SDK auto-iterator
            // handles cleanup — when the loop exits via `break` (instead of
            // running out of pages), the SDK fires a best-effort cursor-close
            // so the QBD lane is released within milliseconds. Your next API
            // call on this connection won't sit waiting for the silent-client
            // timeout.
            // -----------------------------------------------------------------
            Console.WriteLine("\n=== Find-and-stop ===\n");

            const string targetSubstring = "Store";
            string? matched = null;

            var findPage = await client.Customers.ListAsync(limit: 10);
            await foreach (var customer in findPage) {
                // TryGetProperty avoids throwing on records that don't have a top-level "name"
                if (customer.TryGetProperty("name", out var nameElement)) {
                    var name = nameElement.GetString() ?? string.Empty;
                    if (name.Contains(targetSubstring, StringComparison.OrdinalIgnoreCase)) {
                        matched = name;
                        break; // ← SDK auto-closes the cursor here
                    }
                }
            }

            if (matched is not null) {
                Console.WriteLine($"  Matched \"{matched}\" — stopped early.");
            } else {
                Console.WriteLine($"  No customer matched \"{targetSubstring}\".");
            }

            // -----------------------------------------------------------------
            // Approach 3: Manual page-by-page navigation (async)
            //
            // Await the list call to get a CursorPage, then use HasNextPage()
            // and GetNextPageAsync() to step through pages one at a time.
            // Useful when you need page-level metadata (totalCount, etc.).
            // -----------------------------------------------------------------
            Console.WriteLine("\n=== Manual Page-by-Page Navigation (async) ===\n");

            var page = await client.Customers.ListAsync(limit: 5);
            var pageNumber = 1;

            Console.WriteLine($"Page {pageNumber}: {page.Count} items (totalCount: {page.TotalCount})");
            foreach (var customer in page.Data) {
                Console.WriteLine($"  - {customer.GetProperty("name").GetString()}");
            }

            while (page.HasNextPage() && pageNumber < 3) {
                page = await page.GetNextPageAsync();
                pageNumber++;

                Console.WriteLine($"\nPage {pageNumber}: {page.Count} items");
                foreach (var customer in page.Data) {
                    Console.WriteLine($"  - {customer.GetProperty("name").GetString()}");
                }
            }

            if (!page.HasNextPage()) {
                Console.WriteLine("\nReached the last page.");
            } else {
                Console.WriteLine("\n... stopping after 3 pages for demo purposes.");
            }
        } catch (NxusApiException ex) {
            Console.Error.WriteLine($"\nAPI Error [{ex.Status}]: {ex.UserMessage}");
            if (ex.Code is not null)
                Console.Error.WriteLine($"  Code: {ex.Code}");
            if (ex.RequestId is not null)
                Console.Error.WriteLine($"  Request ID: {ex.RequestId}");
            Environment.Exit(1);
        }
    }
}
