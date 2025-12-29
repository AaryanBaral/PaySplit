using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Tenants.Exceptions
{
    public sealed class TenantUserAlreadyInactiveException : DomainException
    {
        public TenantUserAlreadyInactiveException()
            : base("Tenant user is already inactive.") { }
    }
}
