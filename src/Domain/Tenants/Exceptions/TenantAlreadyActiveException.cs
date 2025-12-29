using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Tenants.Exceptions
{
    public sealed class TenantAlreadyActiveException : DomainException
    {
        public TenantAlreadyActiveException()
            : base("Tenant is already active.") { }
    }
}
