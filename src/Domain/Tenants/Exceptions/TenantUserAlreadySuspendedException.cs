using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Tenants.Exceptions
{
    public sealed class TenantUserAlreadySuspendedException : DomainException
    {
        public TenantUserAlreadySuspendedException()
            : base("Tenant user is already suspended.") { }
    }
}
