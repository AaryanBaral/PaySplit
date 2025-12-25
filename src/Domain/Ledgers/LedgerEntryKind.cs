namespace PaySplit.Domain.Ledgers
{
    public enum LedgerEntryKind
    {
        MerchantCredit = 1,   // Merchant's balance increases (e.g., from a payment)
        MerchantDebit = 2,    // Merchant's balance decreases (e.g., payout)
        TenantCredit = 3,   // Tenant's revenue increases
        TenantDebit = 4     // Tenant's revenue decreases (e.g., refund, adjustment)
    }

}
