using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Merchants.Exceptions
{
    public sealed class MerchantAlreadyActiveException : DomainException
    {
        public MerchantAlreadyActiveException()
            : base("Merchant is already active.") { }
    }
}
