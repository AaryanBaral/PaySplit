using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Tenants.Exceptions
{
    public sealed class TenantAlreadySuspendedException : DomainException
    {
        public TenantAlreadySuspendedException()
            : base("Tenant is already suspended.") { }
    }
}
