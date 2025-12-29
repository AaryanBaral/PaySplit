using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Ledgers.Exceptions
{
    public sealed class LedgerAmountInvalidException : DomainException
    {
        public LedgerAmountInvalidException(decimal amount)
            : base($"Ledger amount must be positive. Received: {amount}.") { }
    }
}
