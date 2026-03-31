using Nxus.Qbd.Resources;
using Nxus.Qbd.Transport;

namespace Nxus.Qbd;

/// <summary>
/// The main entry point for the Nxus QuickBooks Desktop .NET SDK.
/// Provides typed resource namespaces for every QBD entity.
///
/// <example>
/// <code>
/// using var client = new NxusClient(new NxusClientOptions {
///     ApiKey = "sk_live_...",
///     ConnectionId = "your-connection-id"
/// });
///
/// var page = client.Vendors.List(limit: 50);
/// foreach (var vendor in page.Data)
///     Console.WriteLine(vendor.GetProperty("name").GetString());
/// </code>
/// </example>
/// </summary>
public sealed class NxusClient : IDisposable {
    private readonly NxusHttpTransport _transport;

    public NxusClient(NxusClientOptions options) {
        ArgumentNullException.ThrowIfNull(options);
        _transport = new NxusHttpTransport(options);

        // ── Transactions ────────────────────────────────────────────────
        ArRefundCreditCards = new ArRefundCreditCardsResource(_transport);
        Bills = new BillsResource(_transport);
        BuildAssemblies = new BuildAssembliesResource(_transport);
        Charges = new ChargesResource(_transport);
        CheckBills = new CheckBillsResource(_transport);
        Checks = new ChecksResource(_transport);
        CreditCardBills = new CreditCardBillsResource(_transport);
        CreditCardCharges = new CreditCardChargesResource(_transport);
        CreditCardCredits = new CreditCardCreditsResource(_transport);
        CreditMemos = new CreditMemosResource(_transport);
        Deposits = new DepositsResource(_transport);
        Estimates = new EstimatesResource(_transport);
        InventoryAdjustments = new InventoryAdjustmentsResource(_transport);
        Invoices = new InvoicesResource(_transport);
        ItemReceipts = new ItemReceiptsResource(_transport);
        JournalEntries = new JournalEntriesResource(_transport);
        PurchaseOrders = new PurchaseOrdersResource(_transport);
        ReceivePayments = new ReceivePaymentsResource(_transport);
        SalesReceipts = new SalesReceiptsResource(_transport);
        SalesTaxPaymentChecks = new SalesTaxPaymentChecksResource(_transport);
        TimeTrackings = new TimeTrackingsResource(_transport);
        Transactions = new TransactionsResource(_transport);
        VendorCredits = new VendorCreditsResource(_transport);

        // ── Lists ────────────────────────────────────────────────────────
        Accounts = new AccountsResource(_transport);
        AccountTaxLineInfos = new AccountTaxLineInfosResource(_transport);
        BarCodes = new BarCodesResource(_transport);
        BillingRates = new BillingRatesResource(_transport);
        BillsToPay = new BillsToPayResource(_transport);
        Classes = new ClassesResource(_transport);
        Currencies = new CurrenciesResource(_transport);
        Customers = new CustomersResource(_transport);
        CustomerTypes = new CustomerTypesResource(_transport);
        DateDrivenTerms = new DateDrivenTermsResource(_transport);
        Employees = new EmployeesResource(_transport);
        InventorySites = new InventorySitesResource(_transport);
        Leads = new LeadsResource(_transport);
        OtherNames = new OtherNamesResource(_transport);
        PaymentMethods = new PaymentMethodsResource(_transport);
        PriceLevels = new PriceLevelsResource(_transport);
        SalesTaxCodes = new SalesTaxCodesResource(_transport);
        ShipMethods = new ShipMethodsResource(_transport);
        Terms = new TermsResource(_transport);
        UnitOfMeasureSets = new UnitOfMeasureSetsResource(_transport);
        Vendors = new VendorsResource(_transport);
        VendorTypes = new VendorTypesResource(_transport);

        // ── Items ────────────────────────────────────────────────────────
        Items = new ItemsResource(_transport);
        InventoryItems = new InventoryItemsResource(_transport);
        ItemDiscounts = new ItemDiscountsResource(_transport);
        ItemFixedAssets = new ItemFixedAssetsResource(_transport);
        ItemGroups = new ItemGroupsResource(_transport);
        ItemInventoryAssemblies = new ItemInventoryAssembliesResource(_transport);
        ItemNonInventory = new ItemNonInventoryResource(_transport);
        ItemOtherCharges = new ItemOtherChargesResource(_transport);
        ItemPayments = new ItemPaymentsResource(_transport);
        ItemSalesTax = new ItemSalesTaxResource(_transport);
        ItemSalesTaxGroups = new ItemSalesTaxGroupsResource(_transport);
        ServiceItems = new ServiceItemsResource(_transport);
        ItemSubtotals = new ItemSubtotalsResource(_transport);

        // ── Payroll ──────────────────────────────────────────────────────
        PayrollItemNonWages = new PayrollItemNonWagesResource(_transport);
        PayrollItemWages = new PayrollItemWagesResource(_transport);
        WorkersCompCodes = new WorkersCompCodesResource(_transport);

        // ── Platform ─────────────────────────────────────────────────────
        Connections = new ConnectionsResource(_transport);
        AuthSessions = new AuthSessionsResource(_transport);
        SpecialItems = new SpecialItemsResource(_transport);
        Reports = new ReportsResource(_transport);
    }

    /// <summary>
    /// Convenience constructor — pass just an API key for production defaults.
    /// </summary>
    public NxusClient(string apiKey) : this(new NxusClientOptions { ApiKey = apiKey }) { }

    // ═══════════════════════════════════════════════════════════════════
    // Resource Namespace Properties
    // ═══════════════════════════════════════════════════════════════════

    // ── Transactions ────────────────────────────────────────────────────
    /// <summary>AR Refund Credit Cards — full CRUD.</summary>
    public ArRefundCreditCardsResource ArRefundCreditCards { get; }
    /// <summary>Bills — full CRUD.</summary>
    public BillsResource Bills { get; }
    /// <summary>Build Assemblies — full CRUD.</summary>
    public BuildAssembliesResource BuildAssemblies { get; }
    /// <summary>Charges — full CRUD.</summary>
    public ChargesResource Charges { get; }
    /// <summary>Check Bills — full CRUD.</summary>
    public CheckBillsResource CheckBills { get; }
    /// <summary>Checks — full CRUD.</summary>
    public ChecksResource Checks { get; }
    /// <summary>Credit Card Bills — full CRUD.</summary>
    public CreditCardBillsResource CreditCardBills { get; }
    /// <summary>Credit Card Charges — full CRUD.</summary>
    public CreditCardChargesResource CreditCardCharges { get; }
    /// <summary>Credit Card Credits — full CRUD.</summary>
    public CreditCardCreditsResource CreditCardCredits { get; }
    /// <summary>Credit Memos — full CRUD.</summary>
    public CreditMemosResource CreditMemos { get; }
    /// <summary>Deposits — full CRUD.</summary>
    public DepositsResource Deposits { get; }
    /// <summary>Estimates — full CRUD.</summary>
    public EstimatesResource Estimates { get; }
    /// <summary>Inventory Adjustments — full CRUD.</summary>
    public InventoryAdjustmentsResource InventoryAdjustments { get; }
    /// <summary>Invoices — full CRUD.</summary>
    public InvoicesResource Invoices { get; }
    /// <summary>Item Receipts — full CRUD.</summary>
    public ItemReceiptsResource ItemReceipts { get; }
    /// <summary>Journal Entries — full CRUD.</summary>
    public JournalEntriesResource JournalEntries { get; }
    /// <summary>Purchase Orders — full CRUD.</summary>
    public PurchaseOrdersResource PurchaseOrders { get; }
    /// <summary>Receive Payments — full CRUD.</summary>
    public ReceivePaymentsResource ReceivePayments { get; }
    /// <summary>Sales Receipts — full CRUD.</summary>
    public SalesReceiptsResource SalesReceipts { get; }
    /// <summary>Sales Tax Payment Checks — full CRUD.</summary>
    public SalesTaxPaymentChecksResource SalesTaxPaymentChecks { get; }
    /// <summary>Time Trackings — full CRUD.</summary>
    public TimeTrackingsResource TimeTrackings { get; }
    /// <summary>Transactions — list, retrieve, delete only.</summary>
    public TransactionsResource Transactions { get; }
    /// <summary>Vendor Credits — full CRUD.</summary>
    public VendorCreditsResource VendorCredits { get; }

    // ── Lists ────────────────────────────────────────────────────────────
    /// <summary>Accounts — full CRUD.</summary>
    public AccountsResource Accounts { get; }
    /// <summary>Account Tax Line Infos — read-only.</summary>
    public AccountTaxLineInfosResource AccountTaxLineInfos { get; }
    /// <summary>Bar Codes — list, retrieve, delete.</summary>
    public BarCodesResource BarCodes { get; }
    /// <summary>Billing Rates — list, retrieve, create, delete.</summary>
    public BillingRatesResource BillingRates { get; }
    /// <summary>Bills to Pay — list, retrieve only.</summary>
    public BillsToPayResource BillsToPay { get; }
    /// <summary>QBD Classes — full CRUD.</summary>
    public ClassesResource Classes { get; }
    /// <summary>Currencies — list, retrieve, create, update (no delete).</summary>
    public CurrenciesResource Currencies { get; }
    /// <summary>Customers — full CRUD.</summary>
    public CustomersResource Customers { get; }
    /// <summary>Customer Types — full CRUD.</summary>
    public CustomerTypesResource CustomerTypes { get; }
    /// <summary>Date-Driven Terms — full CRUD.</summary>
    public DateDrivenTermsResource DateDrivenTerms { get; }
    /// <summary>Employees — full CRUD.</summary>
    public EmployeesResource Employees { get; }
    /// <summary>Inventory Sites — full CRUD.</summary>
    public InventorySitesResource InventorySites { get; }
    /// <summary>Leads — full CRUD.</summary>
    public LeadsResource Leads { get; }
    /// <summary>Other Names — full CRUD.</summary>
    public OtherNamesResource OtherNames { get; }
    /// <summary>Payment Methods — full CRUD.</summary>
    public PaymentMethodsResource PaymentMethods { get; }
    /// <summary>Price Levels — full CRUD.</summary>
    public PriceLevelsResource PriceLevels { get; }
    /// <summary>Sales Tax Codes — full CRUD.</summary>
    public SalesTaxCodesResource SalesTaxCodes { get; }
    /// <summary>Ship Methods — full CRUD.</summary>
    public ShipMethodsResource ShipMethods { get; }
    /// <summary>Terms — full CRUD.</summary>
    public TermsResource Terms { get; }
    /// <summary>Unit of Measure Sets — list, retrieve, create.</summary>
    public UnitOfMeasureSetsResource UnitOfMeasureSets { get; }
    /// <summary>Vendors — full CRUD.</summary>
    public VendorsResource Vendors { get; }
    /// <summary>Vendor Types — list, retrieve, create, delete.</summary>
    public VendorTypesResource VendorTypes { get; }

    // ── Items ────────────────────────────────────────────────────────────
    /// <summary>Items — aggregate read-only view across all item types.</summary>
    public ItemsResource Items { get; }
    /// <summary>Inventory Items — full CRUD.</summary>
    public InventoryItemsResource InventoryItems { get; }
    /// <summary>Item Discounts — full CRUD.</summary>
    public ItemDiscountsResource ItemDiscounts { get; }
    /// <summary>Item Fixed Assets — full CRUD.</summary>
    public ItemFixedAssetsResource ItemFixedAssets { get; }
    /// <summary>Item Groups — full CRUD.</summary>
    public ItemGroupsResource ItemGroups { get; }
    /// <summary>Item Inventory Assemblies — full CRUD.</summary>
    public ItemInventoryAssembliesResource ItemInventoryAssemblies { get; }
    /// <summary>Item Non-Inventory — full CRUD.</summary>
    public ItemNonInventoryResource ItemNonInventory { get; }
    /// <summary>Item Other Charges — full CRUD.</summary>
    public ItemOtherChargesResource ItemOtherCharges { get; }
    /// <summary>Item Payments — full CRUD.</summary>
    public ItemPaymentsResource ItemPayments { get; }
    /// <summary>Item Sales Tax — full CRUD.</summary>
    public ItemSalesTaxResource ItemSalesTax { get; }
    /// <summary>Item Sales Tax Groups — full CRUD.</summary>
    public ItemSalesTaxGroupsResource ItemSalesTaxGroups { get; }
    /// <summary>Service Items — full CRUD.</summary>
    public ServiceItemsResource ServiceItems { get; }
    /// <summary>Item Subtotals — full CRUD.</summary>
    public ItemSubtotalsResource ItemSubtotals { get; }

    // ── Payroll ──────────────────────────────────────────────────────────
    /// <summary>Payroll Item Non-Wages — list, retrieve, delete.</summary>
    public PayrollItemNonWagesResource PayrollItemNonWages { get; }
    /// <summary>Payroll Item Wages — list, retrieve, create, delete.</summary>
    public PayrollItemWagesResource PayrollItemWages { get; }
    /// <summary>Workers Comp Codes — full CRUD.</summary>
    public WorkersCompCodesResource WorkersCompCodes { get; }

    // ── Platform ─────────────────────────────────────────────────────────
    /// <summary>Connections — full CRUD plus status check.</summary>
    public ConnectionsResource Connections { get; }
    /// <summary>Auth Sessions — create and retrieve.</summary>
    public AuthSessionsResource AuthSessions { get; }
    /// <summary>Special Items — create only.</summary>
    public SpecialItemsResource SpecialItems { get; }
    /// <summary>Reports — QuickBooks Desktop report endpoints.</summary>
    public ReportsResource Reports { get; }

    public void Dispose() => _transport.Dispose();
}
