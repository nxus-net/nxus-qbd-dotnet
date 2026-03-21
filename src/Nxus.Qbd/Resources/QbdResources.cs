using Nxus.Qbd.Transport;

namespace Nxus.Qbd.Resources;

// ═══════════════════════════════════════════════════════════════════════
// Transactions
// ═══════════════════════════════════════════════════════════════════════

public sealed class ArRefundCreditCardsResource : ResourceBase {
    internal ArRefundCreditCardsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ARRefundCreditCards";
    protected override string SingularPath => "/api/v1/ARRefundCreditCard/{id}";
}

public sealed class BillsResource : ResourceBase {
    internal BillsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Bills";
    protected override string SingularPath => "/api/v1/Bill/{id}";
}

public sealed class BuildAssembliesResource : ResourceBase {
    internal BuildAssembliesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/BuildAssemblies";
    protected override string SingularPath => "/api/v1/BuildAssembly/{id}";
}

public sealed class ChargesResource : ResourceBase {
    internal ChargesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Charges";
    protected override string SingularPath => "/api/v1/Charge/{id}";
}

public sealed class CheckBillsResource : ResourceBase {
    internal CheckBillsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/CheckBills";
    protected override string SingularPath => "/api/v1/CheckBill/{id}";
}

public sealed class ChecksResource : ResourceBase {
    internal ChecksResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Checks";
    protected override string SingularPath => "/api/v1/Check/{id}";
}

public sealed class CreditCardBillsResource : ResourceBase {
    internal CreditCardBillsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/CreditCardBills";
    protected override string SingularPath => "/api/v1/CreditCardBill/{id}";
}

public sealed class CreditCardChargesResource : ResourceBase {
    internal CreditCardChargesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/CreditCardCharges";
    protected override string SingularPath => "/api/v1/CreditCardCharge/{id}";
}

public sealed class CreditCardCreditsResource : ResourceBase {
    internal CreditCardCreditsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/CreditCardCredits";
    protected override string SingularPath => "/api/v1/CreditCardCredit/{id}";
}

public sealed class CreditMemosResource : ResourceBase {
    internal CreditMemosResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/CreditMemos";
    protected override string SingularPath => "/api/v1/CreditMemo/{id}";
}

public sealed class DepositsResource : ResourceBase {
    internal DepositsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Deposits";
    protected override string SingularPath => "/api/v1/Deposit/{id}";
}

public sealed class EstimatesResource : ResourceBase {
    internal EstimatesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Estimates";
    protected override string SingularPath => "/api/v1/Estimate/{id}";
}

public sealed class InventoryAdjustmentsResource : ResourceBase {
    internal InventoryAdjustmentsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/InventoryAdjustments";
    protected override string SingularPath => "/api/v1/InventoryAdjustment/{id}";
}

public sealed class InvoicesResource : ResourceBase {
    internal InvoicesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Invoices";
    protected override string SingularPath => "/api/v1/Invoice/{id}";
}

public sealed class ItemReceiptsResource : ResourceBase {
    internal ItemReceiptsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemReceipts";
    protected override string SingularPath => "/api/v1/ItemReceipt/{id}";
}

public sealed class JournalEntriesResource : ResourceBase {
    internal JournalEntriesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/JournalEntries";
    protected override string SingularPath => "/api/v1/JournalEntry/{id}";
}

public sealed class PurchaseOrdersResource : ResourceBase {
    internal PurchaseOrdersResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/PurchaseOrders";
    protected override string SingularPath => "/api/v1/PurchaseOrder/{id}";
}

public sealed class ReceivePaymentsResource : ResourceBase {
    internal ReceivePaymentsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ReceivePayments";
    protected override string SingularPath => "/api/v1/ReceivePayment/{id}";
}

public sealed class SalesReceiptsResource : ResourceBase {
    internal SalesReceiptsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/SalesReceipts";
    protected override string SingularPath => "/api/v1/SalesReceipt/{id}";
}

public sealed class SalesTaxPaymentChecksResource : ResourceBase {
    internal SalesTaxPaymentChecksResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/SalesTaxPaymentChecks";
    protected override string SingularPath => "/api/v1/SalesTaxPaymentCheck/{id}";
}

public sealed class TimeTrackingsResource : ResourceBase {
    internal TimeTrackingsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/TimeTrackingActivities";
    protected override string SingularPath => "/api/v1/TimeTrackingActivity/{id}";
}

public sealed class TransactionsResource : ResourceBase {
    internal TransactionsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Transactions";
    protected override string SingularPath => "/api/v1/Transaction/{id}";
}

public sealed class VendorCreditsResource : ResourceBase {
    internal VendorCreditsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/VendorCredits";
    protected override string SingularPath => "/api/v1/VendorCredit/{id}";
}

// ═══════════════════════════════════════════════════════════════════════
// Lists
// ═══════════════════════════════════════════════════════════════════════

public sealed class AccountsResource : ResourceBase {
    internal AccountsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Accounts";
    protected override string SingularPath => "/api/v1/Account/{id}";
}

public sealed class AccountTaxLineInfosResource : ResourceBase {
    internal AccountTaxLineInfosResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/AccountsTaxLineInfo";
    protected override string SingularPath => "/api/v1/AccountTaxLineInfo/{id}";
}

public sealed class BarCodesResource : ResourceBase {
    internal BarCodesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/BarCodes";
    protected override string SingularPath => "/api/v1/BarCode/{id}";
}

public sealed class BillingRatesResource : ResourceBase {
    internal BillingRatesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/BillingRates";
    protected override string SingularPath => "/api/v1/BillingRate/{id}";
}

public sealed class BillsToPayResource : ResourceBase {
    internal BillsToPayResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/BillsToPay";
    protected override string SingularPath => "/api/v1/BillToPay/{id}";
}

public sealed class ClassesResource : ResourceBase {
    internal ClassesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Classes";
    protected override string SingularPath => "/api/v1/Class/{id}";
}

public sealed class CurrenciesResource : ResourceBase {
    internal CurrenciesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Currencies";
    protected override string SingularPath => "/api/v1/Currency/{id}";
}

public sealed class CustomersResource : ResourceBase {
    internal CustomersResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Customers";
    protected override string SingularPath => "/api/v1/Customer/{id}";
}

public sealed class CustomerTypesResource : ResourceBase {
    internal CustomerTypesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/CustomerTypes";
    protected override string SingularPath => "/api/v1/CustomerType/{id}";
}

public sealed class DateDrivenTermsResource : ResourceBase {
    internal DateDrivenTermsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/DateDrivenTerms";
    protected override string SingularPath => "/api/v1/DateDrivenTerm/{id}";
}

public sealed class EmployeesResource : ResourceBase {
    internal EmployeesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Employees";
    protected override string SingularPath => "/api/v1/Employee/{id}";
}

public sealed class InventorySitesResource : ResourceBase {
    internal InventorySitesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/InventorySites";
    protected override string SingularPath => "/api/v1/InventorySite/{id}";
}

public sealed class LeadsResource : ResourceBase {
    internal LeadsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Leads";
    protected override string SingularPath => "/api/v1/Lead/{id}";
}

public sealed class OtherNamesResource : ResourceBase {
    internal OtherNamesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/OtherNames";
    protected override string SingularPath => "/api/v1/OtherName/{id}";
}

public sealed class PaymentMethodsResource : ResourceBase {
    internal PaymentMethodsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/PaymentMethods";
    protected override string SingularPath => "/api/v1/PaymentMethod/{id}";
}

public sealed class PriceLevelsResource : ResourceBase {
    internal PriceLevelsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/PriceLevels";
    protected override string SingularPath => "/api/v1/PriceLevel/{id}";
}

public sealed class SalesTaxCodesResource : ResourceBase {
    internal SalesTaxCodesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/SalesTaxCodes";
    protected override string SingularPath => "/api/v1/SalesTaxCode/{id}";
}

public sealed class ShipMethodsResource : ResourceBase {
    internal ShipMethodsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ShipMethods";
    protected override string SingularPath => "/api/v1/ShipMethod/{id}";
}

public sealed class TermsResource : ResourceBase {
    internal TermsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Terms";
    protected override string SingularPath => "/api/v1/Term/{id}";
}

public sealed class UnitOfMeasureSetsResource : ResourceBase {
    internal UnitOfMeasureSetsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/UnitOfMeasureSets";
    protected override string SingularPath => "/api/v1/UnitOfMeasureSet/{id}";
}

public sealed class VendorsResource : ResourceBase {
    internal VendorsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Vendors";
    protected override string SingularPath => "/api/v1/Vendor/{id}";
}

public sealed class VendorTypesResource : ResourceBase {
    internal VendorTypesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/VendorTypes";
    protected override string SingularPath => "/api/v1/VendorType/{id}";
}

// ═══════════════════════════════════════════════════════════════════════
// Items
// ═══════════════════════════════════════════════════════════════════════

public sealed class ItemsResource : ResourceBase {
    internal ItemsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/Items";
    protected override string SingularPath => "/api/v1/Item/{id}";
}

public sealed class InventoryItemsResource : ResourceBase {
    internal InventoryItemsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsInventory";
    protected override string SingularPath => "/api/v1/ItemInventory/{id}";
}

public sealed class ItemDiscountsResource : ResourceBase {
    internal ItemDiscountsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsDiscount";
    protected override string SingularPath => "/api/v1/ItemDiscount/{id}";
}

public sealed class ItemFixedAssetsResource : ResourceBase {
    internal ItemFixedAssetsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsFixedAsset";
    protected override string SingularPath => "/api/v1/ItemFixedAsset/{id}";
}

public sealed class ItemGroupsResource : ResourceBase {
    internal ItemGroupsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsGroup";
    protected override string SingularPath => "/api/v1/ItemGroup/{id}";
}

public sealed class ItemInventoryAssembliesResource : ResourceBase {
    internal ItemInventoryAssembliesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsInventoryAssembly";
    protected override string SingularPath => "/api/v1/ItemInventoryAssembly/{id}";
}

public sealed class ItemNonInventoryResource : ResourceBase {
    internal ItemNonInventoryResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsNonInventory";
    protected override string SingularPath => "/api/v1/ItemNonInventory/{id}";
}

public sealed class ItemOtherChargesResource : ResourceBase {
    internal ItemOtherChargesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsOtherCharge";
    protected override string SingularPath => "/api/v1/ItemOtherCharge/{id}";
}

public sealed class ItemPaymentsResource : ResourceBase {
    internal ItemPaymentsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsPayment";
    protected override string SingularPath => "/api/v1/ItemPayment/{id}";
}

public sealed class ItemSalesTaxResource : ResourceBase {
    internal ItemSalesTaxResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsSalesTax";
    protected override string SingularPath => "/api/v1/ItemSalesTax/{id}";
}

public sealed class ItemSalesTaxGroupsResource : ResourceBase {
    internal ItemSalesTaxGroupsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsSalesTaxGroup";
    protected override string SingularPath => "/api/v1/ItemSalesTaxGroup/{id}";
}

public sealed class ServiceItemsResource : ResourceBase {
    internal ServiceItemsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsService";
    protected override string SingularPath => "/api/v1/ItemService/{id}";
}

public sealed class ItemSubtotalsResource : ResourceBase {
    internal ItemSubtotalsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ItemsSubtotal";
    protected override string SingularPath => "/api/v1/ItemSubtotal/{id}";
}

// ═══════════════════════════════════════════════════════════════════════
// Payroll
// ═══════════════════════════════════════════════════════════════════════

public sealed class PayrollItemNonWagesResource : ResourceBase {
    internal PayrollItemNonWagesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/PayrollItemNonWages";
    protected override string SingularPath => "/api/v1/PayrollItemNonWage/{id}";
}

public sealed class PayrollItemWagesResource : ResourceBase {
    internal PayrollItemWagesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/PayrollItemWages";
    protected override string SingularPath => "/api/v1/PayrollItemWage/{id}";
}

public sealed class WorkersCompCodesResource : ResourceBase {
    internal WorkersCompCodesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/WorkersCompCodes";
    protected override string SingularPath => "/api/v1/WorkersCompCode/{id}";
}
