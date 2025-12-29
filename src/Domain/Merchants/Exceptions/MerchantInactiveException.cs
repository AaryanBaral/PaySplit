using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Merchants.Exceptions
{
    public sealed class MerchantInactiveException : DomainException
    {
        public MerchantInactiveException()
            : base("Merchant is already inactive.") { }
    }
}
