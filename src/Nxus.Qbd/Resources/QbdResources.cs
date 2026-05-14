using Nxus.Qbd.Transport;

namespace Nxus.Qbd.Resources;

// ═══════════════════════════════════════════════════════════════════════
// Transactions
// ═══════════════════════════════════════════════════════════════════════

public sealed class ArRefundCreditCardsResource : VoidableResourceBase
{
    internal ArRefundCreditCardsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ar-refund-credit-cards";
    protected override string SingularPath => "/api/v1/ar-refund-credit-card/{id}";
}

public sealed class BillsResource : VoidableResourceBase
{
    internal BillsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/bills";
    protected override string SingularPath => "/api/v1/bill/{id}";
}

public sealed class BuildAssembliesResource : ResourceBase
{
    internal BuildAssembliesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/build-assemblies";
    protected override string SingularPath => "/api/v1/build-assembly/{id}";
}

public sealed class ChargesResource : VoidableResourceBase
{
    internal ChargesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/charges";
    protected override string SingularPath => "/api/v1/charge/{id}";
}

public sealed class CheckBillsResource : VoidableResourceBase
{
    internal CheckBillsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/check-bills";
    protected override string SingularPath => "/api/v1/check-bill/{id}";
}

public sealed class ChecksResource : VoidableResourceBase
{
    internal ChecksResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/checks";
    protected override string SingularPath => "/api/v1/check/{id}";
}

public sealed class CreditCardBillsResource : VoidableResourceBase
{
    internal CreditCardBillsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/credit-card-bills";
    protected override string SingularPath => "/api/v1/credit-card-bill/{id}";
}

public sealed class CreditCardChargesResource : VoidableResourceBase
{
    internal CreditCardChargesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/credit-card-charges";
    protected override string SingularPath => "/api/v1/credit-card-charge/{id}";
}

public sealed class CreditCardCreditsResource : VoidableResourceBase
{
    internal CreditCardCreditsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/credit-card-credits";
    protected override string SingularPath => "/api/v1/credit-card-credit/{id}";
}

public sealed class CreditMemosResource : VoidableResourceBase
{
    internal CreditMemosResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/credit-memos";
    protected override string SingularPath => "/api/v1/credit-memo/{id}";
}

public sealed class DepositsResource : VoidableResourceBase
{
    internal DepositsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/deposits";
    protected override string SingularPath => "/api/v1/deposit/{id}";
}

public sealed class EstimatesResource : ResourceBase
{
    internal EstimatesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/estimates";
    protected override string SingularPath => "/api/v1/estimate/{id}";
}

public sealed class InventoryAdjustmentsResource : VoidableResourceBase
{
    internal InventoryAdjustmentsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/inventory-adjustments";
    protected override string SingularPath => "/api/v1/inventory-adjustment/{id}";
}

public sealed class InvoicesResource : VoidableResourceBase
{
    internal InvoicesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/invoices";
    protected override string SingularPath => "/api/v1/invoice/{id}";
}

public sealed class ItemReceiptsResource : VoidableResourceBase
{
    internal ItemReceiptsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/item-receipts";
    protected override string SingularPath => "/api/v1/item-receipt/{id}";
}

public sealed class JournalEntriesResource : VoidableResourceBase
{
    internal JournalEntriesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/journal-entries";
    protected override string SingularPath => "/api/v1/journal-entry/{id}";
}

public sealed class PurchaseOrdersResource : ResourceBase
{
    internal PurchaseOrdersResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/purchase-orders";
    protected override string SingularPath => "/api/v1/purchase-order/{id}";
}

public sealed class ReceivePaymentsResource : ResourceBase
{
    internal ReceivePaymentsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/receive-payments";
    protected override string SingularPath => "/api/v1/receive-payment/{id}";
}

public sealed class SalesReceiptsResource : VoidableResourceBase
{
    internal SalesReceiptsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/sales-receipts";
    protected override string SingularPath => "/api/v1/sales-receipt/{id}";
}

public sealed class SalesTaxPaymentChecksResource : ResourceBase
{
    internal SalesTaxPaymentChecksResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/sales-tax-payment-checks";
    protected override string SingularPath => "/api/v1/sales-tax-payment-check/{id}";
}

public sealed class TimeTrackingsResource : ResourceBase
{
    internal TimeTrackingsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/time-tracking-activities";
    protected override string SingularPath => "/api/v1/time-tracking-activity/{id}";
}

public sealed class TransactionsResource : ResourceBase
{
    internal TransactionsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/transactions";
    protected override string SingularPath => "/api/v1/transaction/{id}";
}

public sealed class VendorCreditsResource : VoidableResourceBase
{
    internal VendorCreditsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/vendor-credits";
    protected override string SingularPath => "/api/v1/vendor-credit/{id}";
}

// ═══════════════════════════════════════════════════════════════════════
// Lists
// ═══════════════════════════════════════════════════════════════════════

public sealed class AccountsResource : ResourceBase
{
    internal AccountsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/accounts";
    protected override string SingularPath => "/api/v1/account/{id}";
}

public sealed class AccountTaxLineInfosResource : ResourceBase
{
    internal AccountTaxLineInfosResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/accounts-tax-line-info";
    protected override string SingularPath => "/api/v1/account-tax-line-info/{id}";
}

public sealed class BarCodesResource : ResourceBase
{
    internal BarCodesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/bar-codes";
    protected override string SingularPath => "/api/v1/bar-code/{id}";
}

public sealed class BillingRatesResource : ResourceBase
{
    internal BillingRatesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/billing-rates";
    protected override string SingularPath => "/api/v1/billing-rate/{id}";
}

public sealed class BillsToPayResource : ResourceBase
{
    internal BillsToPayResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/bills-to-pay";
    protected override string SingularPath => "/api/v1/bill-to-pay/{id}";
}

public sealed class ClassesResource : ResourceBase
{
    internal ClassesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/classes";
    protected override string SingularPath => "/api/v1/class/{id}";
}

public sealed class CurrenciesResource : ResourceBase
{
    internal CurrenciesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/currencies";
    protected override string SingularPath => "/api/v1/currency/{id}";
}

public sealed class CustomersResource : ResourceBase
{
    internal CustomersResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/customers";
    protected override string SingularPath => "/api/v1/customer/{id}";
}

public sealed class CustomerTypesResource : ResourceBase
{
    internal CustomerTypesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/customer-types";
    protected override string SingularPath => "/api/v1/customer-type/{id}";
}

public sealed class DateDrivenTermsResource : ResourceBase
{
    internal DateDrivenTermsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/date-driven-terms";
    protected override string SingularPath => "/api/v1/date-driven-term/{id}";
}

public sealed class EmployeesResource : ResourceBase
{
    internal EmployeesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/employees";
    protected override string SingularPath => "/api/v1/employee/{id}";
}

public sealed class InventorySitesResource : ResourceBase
{
    internal InventorySitesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/inventory-sites";
    protected override string SingularPath => "/api/v1/inventory-site/{id}";
}

public sealed class LeadsResource : ResourceBase
{
    internal LeadsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/leads";
    protected override string SingularPath => "/api/v1/lead/{id}";
}

public sealed class OtherNamesResource : ResourceBase
{
    internal OtherNamesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/other-names";
    protected override string SingularPath => "/api/v1/other-name/{id}";
}

public sealed class PaymentMethodsResource : ResourceBase
{
    internal PaymentMethodsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/payment-methods";
    protected override string SingularPath => "/api/v1/payment-method/{id}";
}

public sealed class PriceLevelsResource : ResourceBase
{
    internal PriceLevelsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/price-levels";
    protected override string SingularPath => "/api/v1/price-level/{id}";
}

public sealed class SalesTaxCodesResource : ResourceBase
{
    internal SalesTaxCodesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/sales-tax-codes";
    protected override string SingularPath => "/api/v1/sales-tax-code/{id}";
}

public sealed class ShipMethodsResource : ResourceBase
{
    internal ShipMethodsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/ship-methods";
    protected override string SingularPath => "/api/v1/ship-method/{id}";
}

public sealed class TermsResource : ResourceBase
{
    internal TermsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/terms";
    protected override string SingularPath => "/api/v1/term/{id}";
}

public sealed class UnitOfMeasureSetsResource : ResourceBase
{
    internal UnitOfMeasureSetsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/unit-of-measure-sets";
    protected override string SingularPath => "/api/v1/unit-of-measure-set/{id}";
}

public sealed class VendorsResource : ResourceBase
{
    internal VendorsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/vendors";
    protected override string SingularPath => "/api/v1/vendor/{id}";
}

public sealed class VendorTypesResource : ResourceBase
{
    internal VendorTypesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/vendor-types";
    protected override string SingularPath => "/api/v1/vendor-type/{id}";
}

// ═══════════════════════════════════════════════════════════════════════
// Items
// ═══════════════════════════════════════════════════════════════════════

public sealed class ItemsResource : ResourceBase
{
    internal ItemsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items";
    protected override string SingularPath => "/api/v1/item/{id}";
}

// QBD backend convention for items: /api/v1/items-{kind} for list,
// /api/v1/item-{kind}/{id} for individual CRUD.

public sealed class InventoryItemsResource : ResourceBase
{
    internal InventoryItemsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-inventory";
    protected override string SingularPath => "/api/v1/item-inventory/{id}";
}

public sealed class ItemDiscountsResource : ResourceBase
{
    internal ItemDiscountsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-discount";
    protected override string SingularPath => "/api/v1/item-discount/{id}";
}

public sealed class ItemFixedAssetsResource : ResourceBase
{
    internal ItemFixedAssetsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-fixed-asset";
    protected override string SingularPath => "/api/v1/item-fixed-asset/{id}";
}

public sealed class ItemGroupsResource : ResourceBase
{
    internal ItemGroupsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-group";
    protected override string SingularPath => "/api/v1/item-group/{id}";
}

public sealed class ItemInventoryAssembliesResource : ResourceBase
{
    internal ItemInventoryAssembliesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-inventory-assembly";
    protected override string SingularPath => "/api/v1/item-inventory-assembly/{id}";
}

public sealed class ItemNonInventoryResource : ResourceBase
{
    internal ItemNonInventoryResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-non-inventory";
    protected override string SingularPath => "/api/v1/item-non-inventory/{id}";
}

public sealed class ItemOtherChargesResource : ResourceBase
{
    internal ItemOtherChargesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-other-charge";
    protected override string SingularPath => "/api/v1/item-other-charge/{id}";
}

public sealed class ItemPaymentsResource : ResourceBase
{
    internal ItemPaymentsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-payment";
    protected override string SingularPath => "/api/v1/item-payment/{id}";
}

public sealed class ItemSalesTaxResource : ResourceBase
{
    internal ItemSalesTaxResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-sales-tax";
    protected override string SingularPath => "/api/v1/item-sales-tax/{id}";
}

public sealed class ItemSalesTaxGroupsResource : ResourceBase
{
    internal ItemSalesTaxGroupsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-sales-tax-group";
    protected override string SingularPath => "/api/v1/item-sales-tax-group/{id}";
}

public sealed class ServiceItemsResource : ResourceBase
{
    internal ServiceItemsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-service";
    protected override string SingularPath => "/api/v1/item-service/{id}";
}

public sealed class ItemSubtotalsResource : ResourceBase
{
    internal ItemSubtotalsResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/items-subtotal";
    protected override string SingularPath => "/api/v1/item-subtotal/{id}";
}

// ═══════════════════════════════════════════════════════════════════════
// Payroll
// ═══════════════════════════════════════════════════════════════════════

public sealed class PayrollItemNonWagesResource : ResourceBase
{
    internal PayrollItemNonWagesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/payroll-item-non-wages";
    protected override string SingularPath => "/api/v1/payroll-item-non-wage/{id}";
}

public sealed class PayrollItemWagesResource : ResourceBase
{
    internal PayrollItemWagesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/payroll-item-wages";
    protected override string SingularPath => "/api/v1/payroll-item-wage/{id}";
}

public sealed class WorkersCompCodesResource : ResourceBase
{
    internal WorkersCompCodesResource(NxusHttpTransport t) : base(t) { }
    protected override string ListPath => "/api/v1/workers-comp-codes";
    protected override string SingularPath => "/api/v1/workers-comp-code/{id}";
}
