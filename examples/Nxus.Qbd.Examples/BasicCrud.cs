/// <summary>
/// BasicCrud — Full async CRUD lifecycle on the Vendors resource.
///
/// Demonstrates creating, retrieving, updating, listing, and deleting a vendor
/// using the flat API surface with async/await.
///
/// Usage:
///   Set NXUS_API_KEY and NXUS_CONNECTION_ID in environment (or .env file), then:
///   dotnet run --project examples/Nxus.Qbd.Examples -- crud
///
/// Optional env vars:
///   NXUS_BASE_URL         Defaults to https://api.nxus.app/
///   NXUS_DEV_MODE         Set to "true" to extend timeout for local dev
/// </summary>

using System.Text.Json;
using Nxus.Qbd;
using Nxus.Qbd.Errors;

namespace Nxus.Qbd.Examples;

public static class BasicCrud {
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
            // 1. Create a vendor with sample data
            // -----------------------------------------------------------------
            Console.WriteLine("--- Creating vendor ---");
            var uniqueSuffix = Guid.NewGuid().ToString("N")[..8];
            var created = await client.Vendors.CreateAsync(new {
                name = $"Acme Supplies {uniqueSuffix}",
                companyName = "Acme Supplies Inc.",
                phone = "555-0100",
                email = "accounts@acme-supplies.example.com",
            });

            var createdId = created.GetProperty("id").GetString()!;
            var createdName = created.GetProperty("name").GetString();
            Console.WriteLine($"Created vendor: {createdId} {createdName}");

            // -----------------------------------------------------------------
            // 2. Retrieve the vendor by ID
            // -----------------------------------------------------------------
            Console.WriteLine("\n--- Retrieving vendor ---");
            var fetched = await client.Vendors.RetrieveAsync(createdId);
            Console.WriteLine($"Retrieved: {fetched.GetProperty("name").GetString()} | Company: {fetched.GetProperty("companyName").GetString()}");

            // -----------------------------------------------------------------
            // 3. Update the vendor name
            // -----------------------------------------------------------------
            Console.WriteLine("\n--- Updating vendor ---");
            var revisionNumber = fetched.GetProperty("revisionNumber").GetString();
            var updated = await client.Vendors.UpdateAsync(createdId, new {
                name = $"Acme Supplies (Updated) {uniqueSuffix}",
                revisionNumber,  // required for optimistic concurrency
            });
            Console.WriteLine($"Updated name to: {updated.GetProperty("name").GetString()}");

            // -----------------------------------------------------------------
            // 4. List vendors (first page)
            // -----------------------------------------------------------------
            Console.WriteLine("\n--- Listing vendors (first page) ---");
            var page = await client.Vendors.ListAsync(limit: 5);
            Console.WriteLine($"Page contains {page.Count} vendors (totalCount: {page.TotalCount})");
            foreach (var vendor in page.Data) {
                Console.WriteLine($"  - {vendor.GetProperty("name").GetString()} ({vendor.GetProperty("id").GetString()})");
            }

            // -----------------------------------------------------------------
            // 5. Delete the vendor
            // -----------------------------------------------------------------
            Console.WriteLine("\n--- Deleting vendor ---");
            await client.Vendors.DeleteAsync(createdId);
            Console.WriteLine("Vendor deleted successfully.");

            Console.WriteLine("\nCRUD lifecycle complete.");
        } catch (NxusApiException ex) {
            Console.Error.WriteLine($"\nAPI Error [{ex.Status}]: {ex.UserMessage}");
            if (ex.Code is not null)
                Console.Error.WriteLine($"  Code: {ex.Code}");
            if (ex.RequestId is not null)
                Console.Error.WriteLine($"  Request ID: {ex.RequestId}");
            if (ex.ValidationErrors is not null) {
                Console.Error.WriteLine("  Validation errors:");
                foreach (var (field, messages) in ex.ValidationErrors) {
                    foreach (var msg in messages)
                        Console.Error.WriteLine($"    {field}: {msg}");
                }
            }
            Environment.Exit(1);
        }
    }
}
