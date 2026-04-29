# Nxus.Qbd

Official .NET SDK for the [Nxus](https://nx-us.net/docs) QuickBooks Desktop API.

## Installation

```bash
dotnet add package Nxus.Qbd
```

## Quick Start

```csharp
using Nxus.Qbd;

using var client = new NxusClient(new NxusClientOptions {
    ApiKey = "sk_live_...",
    ConnectionId = "your-connection-id",
});

// List vendors
var page = await client.Vendors.ListAsync(limit: 50);
foreach (var vendor in page.Data) {
    Console.WriteLine(vendor.GetProperty("name").GetString());
}

// Retrieve a single customer
var customer = await client.Customers.RetrieveAsync("80000001-1234567890");

// Create an invoice
var invoice = await client.Invoices.CreateAsync(new {
    customerRefListId = "80000001-1234567890",
});
```

## Sync And Async APIs

The SDK supports both sync and async usage:

```csharp
// Sync
var vendors = client.Vendors.List(limit: 10);
foreach (var vendor in vendors.Data) {
    Console.WriteLine(vendor.GetProperty("name").GetString());
}

// Async
var asyncPage = await client.Vendors.ListAsync(limit: 10);
await foreach (var vendor in asyncPage) {
    Console.WriteLine(vendor.GetProperty("name").GetString());
}
```

## Environments

The SDK defaults to `https://api.nx-us.net/`.

For local development, use `Environment = NxusEnvironment.Development`. An explicit `BaseUrl` override still wins when you need a custom endpoint:

```csharp
using var client = new NxusClient(new NxusClientOptions {
    ApiKey = "sk_test_...",
    Environment = NxusEnvironment.Development,
    ConnectionId = "your-connection-id",
});
```

## Timeouts

The SDK defaults to a `30s` client timeout. You can override this globally or per request:

```csharp
using var client = new NxusClient(new NxusClientOptions {
    ApiKey = "sk_live_...",
    ConnectionId = "your-connection-id",
    Timeout = TimeSpan.FromSeconds(60),
});

var page = await client.Transactions.ListAsync(
    limit: 100,
    options: new RequestOptions {
        Timeout = TimeSpan.FromSeconds(45),
    });
```

List and report calls can also send a backend timeout hint without changing the local HTTP timeout:

```csharp
var page = await client.Customers.ListAsync(
    limit: 100,
    options: new RequestOptions {
        ServerTimeoutSeconds = 45,
    });
```

When `ServerTimeoutSeconds` is provided, the SDK sends it as the `X-Nxus-Timeout-Seconds` request header and preserves it across pagination calls.

## Connection Scoping

Every request requires a QuickBooks Desktop connection context. The easiest path is to set it once on the client:

```csharp
using var client = new NxusClient(new NxusClientOptions {
    ApiKey = "sk_live_...",
    ConnectionId = "your-connection-id",
});
```

You can also set it per request:

```csharp
var vendor = await client.Vendors.RetrieveAsync(
    "80000001-1234567890",
    new RequestOptions {
        ConnectionId = "your-connection-id",
    });
```

## Pagination

All list methods return a `CursorPage<T>` that supports both manual navigation and auto-pagination:

```csharp
// Auto-paginate through all records
var firstPage = await client.Customers.ListAsync(limit: 100);
await foreach (var customer in firstPage) {
    Console.WriteLine(customer.GetProperty("name").GetString());
}

// Manual page-by-page navigation
var page = await client.Customers.ListAsync(limit: 50);
while (page.HasNextPage()) {
    page = await page.GetNextPageAsync();
}
```

If you stop early with `break`, the SDK best-effort closes the backend cursor automatically so the QuickBooks Desktop lane is released quickly:

```csharp
var page = await client.Customers.ListAsync(limit: 100);
await foreach (var customer in page) {
    if (customer.GetProperty("name").GetString() == "Acme") {
        break;
    }
}
```

> [!IMPORTANT]
> If you are **testing** **pagination manually** by sending cursors yourself, note that the cursor session stays open for only about 10 seconds between pages. **If you do not request the next page within that window, the session is closed and you must restart the query from the beginning.**



## Examples

Runnable examples live in [`examples/Nxus.Qbd.Examples/`](examples/Nxus.Qbd.Examples/):

| Example | Description |
|---|---|
| [`BasicCrud.cs`](examples/Nxus.Qbd.Examples/BasicCrud.cs) | Create, retrieve, update, list, and delete a vendor |
| [`AutoPagination.cs`](examples/Nxus.Qbd.Examples/AutoPagination.cs) | Async auto-pagination, find-and-stop, and manual page navigation |

The example runner auto-loads a `.env` file from the repo root. Copy `.env.example` to `.env` and fill in your values:

```bash
copy .env.example .env
```

```ini
NXUS_API_KEY=sk_test_your_key_here
NXUS_CONNECTION_ID=your_connection_id_here
NXUS_ENVIRONMENT=development
```

Run an example:

```bash
dotnet run --project examples/Nxus.Qbd.Examples -- crud
dotnet run --project examples/Nxus.Qbd.Examples -- pagination
```

## Error Handling

Non-success responses throw `NxusApiException`:

```csharp
using Nxus.Qbd.Errors;

try {
    await client.Vendors.RetrieveAsync("non-existent-id");
} catch (NxusApiException ex) {
    Console.WriteLine(ex.Status);
    Console.WriteLine(ex.UserMessage);
    Console.WriteLine(ex.Code);
    Console.WriteLine(ex.RequestId);
}
```

## Resources

All QuickBooks Desktop resources are available as namespaced properties on `NxusClient`:

| Category | Resources |
|---|---|
| **Transactions** | `Invoices`, `Bills`, `Checks`, `Deposits`, `Estimates`, `CreditMemos`, `PurchaseOrders`, `SalesReceipts`, `JournalEntries`, `ReceivePayments`, `VendorCredits`, `CreditCardCharges`, `CreditCardBills`, `CreditCardCredits`, `Charges`, `BuildAssemblies`, `ArRefundCreditCards`, `SalesTaxPaymentChecks`, `ItemReceipts`, `CheckBills`, `TimeTrackings`, `Transactions` |
| **Lists** | `Accounts`, `Customers`, `Vendors`, `Employees`, `OtherNames`, `Currencies`, `Terms`, `DateDrivenTerms`, `PaymentMethods`, `ShipMethods`, `SalesTaxCodes`, `PriceLevels`, `Classes`, `CustomerTypes`, `VendorTypes`, `BillingRates`, `InventorySites`, `BarCodes`, `AccountTaxLineInfos`, `BillsToPay`, `UnitOfMeasureSets`, `Leads` |
| **Items** | `Items`, `InventoryItems`, `ItemDiscounts`, `ItemFixedAssets`, `ItemGroups`, `ItemInventoryAssemblies`, `ItemNonInventory`, `ItemOtherCharges`, `ItemPayments`, `ItemSalesTax`, `ItemSalesTaxGroups`, `ServiceItems`, `ItemSubtotals` |
| **Payroll** | `PayrollItemNonWages`, `PayrollItemWages`, `WorkersCompCodes` |
| **Reports** | `Reports` |
| **Platform** | `AuthSessions`, `Connections`, `SpecialItems` |

## License

MIT
