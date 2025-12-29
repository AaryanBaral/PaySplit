using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Tenants.Exceptions
{
    public sealed class TenantAlreadyInactiveException : DomainException
    {
        public TenantAlreadyInactiveException()
            : base("Tenant is already inactive.") { }
    }
}
