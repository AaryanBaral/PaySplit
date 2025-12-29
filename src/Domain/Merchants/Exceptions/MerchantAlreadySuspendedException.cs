using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Merchants.Exceptions
{
    public sealed class MerchantAlreadySuspendedException : DomainException
    {
        public MerchantAlreadySuspendedException()
            : base("Merchant is already suspended.") { }
    }
}
