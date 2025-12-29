using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Tenants.Exceptions
{
    public sealed class TenantUserNotActiveException : DomainException
    {
        public TenantUserNotActiveException()
            : base("Cannot change role of an inactive user.") { }
    }
}
