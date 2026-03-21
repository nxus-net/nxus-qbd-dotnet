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
    public ArRefundCreditCardsResource ArRefundCreditCards { get; }
    public BillsResource Bills { get; }
    public BuildAssembliesResource BuildAssemblies { get; }
    public ChargesResource Charges { get; }
    public CheckBillsResource CheckBills { get; }
    public ChecksResource Checks { get; }
    public CreditCardBillsResource CreditCardBills { get; }
    public CreditCardChargesResource CreditCardCharges { get; }
    public CreditCardCreditsResource CreditCardCredits { get; }
    public CreditMemosResource CreditMemos { get; }
    public DepositsResource Deposits { get; }
    public EstimatesResource Estimates { get; }
    public InventoryAdjustmentsResource InventoryAdjustments { get; }
    public InvoicesResource Invoices { get; }
    public ItemReceiptsResource ItemReceipts { get; }
    public JournalEntriesResource JournalEntries { get; }
    public PurchaseOrdersResource PurchaseOrders { get; }
    public ReceivePaymentsResource ReceivePayments { get; }
    public SalesReceiptsResource SalesReceipts { get; }
    public SalesTaxPaymentChecksResource SalesTaxPaymentChecks { get; }
    public TimeTrackingsResource TimeTrackings { get; }
    public TransactionsResource Transactions { get; }
    public VendorCreditsResource VendorCredits { get; }

    // ── Lists ────────────────────────────────────────────────────────────
    public AccountsResource Accounts { get; }
    public AccountTaxLineInfosResource AccountTaxLineInfos { get; }
    public BarCodesResource BarCodes { get; }
    public BillingRatesResource BillingRates { get; }
    public BillsToPayResource BillsToPay { get; }
    public ClassesResource Classes { get; }
    public CurrenciesResource Currencies { get; }
    public CustomersResource Customers { get; }
    public CustomerTypesResource CustomerTypes { get; }
    public DateDrivenTermsResource DateDrivenTerms { get; }
    public EmployeesResource Employees { get; }
    public InventorySitesResource InventorySites { get; }
    public LeadsResource Leads { get; }
    public OtherNamesResource OtherNames { get; }
    public PaymentMethodsResource PaymentMethods { get; }
    public PriceLevelsResource PriceLevels { get; }
    public SalesTaxCodesResource SalesTaxCodes { get; }
    public ShipMethodsResource ShipMethods { get; }
    public TermsResource Terms { get; }
    public UnitOfMeasureSetsResource UnitOfMeasureSets { get; }
    public VendorsResource Vendors { get; }
    public VendorTypesResource VendorTypes { get; }

    // ── Items ────────────────────────────────────────────────────────────
    public ItemsResource Items { get; }
    public InventoryItemsResource InventoryItems { get; }
    public ItemDiscountsResource ItemDiscounts { get; }
    public ItemFixedAssetsResource ItemFixedAssets { get; }
    public ItemGroupsResource ItemGroups { get; }
    public ItemInventoryAssembliesResource ItemInventoryAssemblies { get; }
    public ItemNonInventoryResource ItemNonInventory { get; }
    public ItemOtherChargesResource ItemOtherCharges { get; }
    public ItemPaymentsResource ItemPayments { get; }
    public ItemSalesTaxResource ItemSalesTax { get; }
    public ItemSalesTaxGroupsResource ItemSalesTaxGroups { get; }
    public ServiceItemsResource ServiceItems { get; }
    public ItemSubtotalsResource ItemSubtotals { get; }

    // ── Payroll ──────────────────────────────────────────────────────────
    public PayrollItemNonWagesResource PayrollItemNonWages { get; }
    public PayrollItemWagesResource PayrollItemWages { get; }
    public WorkersCompCodesResource WorkersCompCodes { get; }

    // ── Platform ─────────────────────────────────────────────────────────
    public ConnectionsResource Connections { get; }
    public AuthSessionsResource AuthSessions { get; }
    public SpecialItemsResource SpecialItems { get; }
    public ReportsResource Reports { get; }

    public void Dispose() => _transport.Dispose();
}
