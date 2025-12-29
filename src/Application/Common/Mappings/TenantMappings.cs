using PaySplit.Application.Tenants.Command.ActivateTenant;
using PaySplit.Application.Tenants.Command.CreateTenant;
using PaySplit.Application.Tenants.Command.DeactivateTenant;
using PaySplit.Application.Tenants.Command.SuspendTenant;
using PaySplit.Application.Tenants.Command.UpdateTenant;
using PaySplit.Application.Tenants.Query.GetAllTenant;
using PaySplit.Application.Tenants.Query.GetTenantById;
using PaySplit.Domain.Tenants;

namespace PaySplit.Application.Common.Mappings
{
    public static class TenantMappings
    {
        public static CreateTenantResult ToCreateTenantResult(this Tenant tenant) =>
            new(tenant.Id, tenant.Status.ToString());

        public static RenameTenantResult ToRenameTenantResult(this Tenant tenant) =>
            new(tenant.Id, tenant.Name, tenant.Status.ToString());

        public static ActivateTenantResult ToActivateTenantResult(this Tenant tenant) =>
            new(tenant.Id, tenant.Status.ToString());

        public static DeactivateTenantResult ToDeactivateTenantResult(this Tenant tenant) =>
            new(tenant.Id, tenant.Status.ToString());

        public static SuspendTenantResult ToSuspendTenantResult(this Tenant tenant) =>
            new(tenant.Id, tenant.Status.ToString());

        public static GetAllTenantDto ToGetAllTenantDto(this Tenant tenant) =>
            new(
                tenant.Id,
                tenant.Name,
                tenant.Status.ToString(),
                tenant.CreatedAtUtc
            );

        public static GetTenantByIdDto ToGetTenantByIdDto(this Tenant tenant) =>
            new(
                tenant.Id,
                tenant.Name,
                tenant.Status.ToString(),
                tenant.CreatedAtUtc,
                tenant.DeactivatedAtUtc,
                tenant.SuspendedAtUtc
            );
    }
}
