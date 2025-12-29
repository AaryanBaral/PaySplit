using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Tenants.Exceptions
{
    public sealed class TenantUserAlreadyActiveException : DomainException
    {
        public TenantUserAlreadyActiveException()
            : base("Tenant user is already active.") { }
    }
}
