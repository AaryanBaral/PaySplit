using Microsoft.Extensions.DependencyInjection;

using PaySplit.Application.Merchants.Command.ActivateMerchant;
using PaySplit.Application.Merchants.Command.CreateMerchant;
using PaySplit.Application.Merchants.Command.DeactivateMerchant;
using PaySplit.Application.Merchants.Command.SuspendMerchant;
using PaySplit.Application.Merchants.Command.UpdateMerchant;
using PaySplit.Application.Merchants.Query.GetAllMerchant;
using PaySplit.Application.Merchants.Query.GetMerchantById;
using PaySplit.Application.Tenants.Command.ActivateTenant;
using PaySplit.Application.Tenants.Command.CreateTenant;
using PaySplit.Application.Tenants.Command.DeactivateTenant;
using PaySplit.Application.Tenants.Command.SuspendTenant;
using PaySplit.Application.Tenants.Command.UpdateTenant;
using PaySplit.Application.Tenants.Query.GetAllTenant;
using PaySplit.Application.Tenants.Query.GetTenantById;

namespace PaySplit.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Tenant handlers
        services.AddScoped<CreateTenantHandler>();
        services.AddScoped<ActivateCommandHandler>();
        services.AddScoped<DeactivateCommandHandler>();
        services.AddScoped<SuspendCommandHandler>();
        services.AddScoped<UpdateCommandHandler>();
        services.AddScoped<GetAllTenantHandler>();
        services.AddScoped<GetTenantByIdHandler>();

        // Merchant handlers
        services.AddScoped<CreateMerchantHandler>();
        services.AddScoped<UpdateMerchantHandler>();
        services.AddScoped<ActivateMerchantHandler>();
        services.AddScoped<DeactivateMerchantHandler>();
        services.AddScoped<SuspendMerchantHandler>();
        services.AddScoped<GetAllMerchantHandler>();
        services.AddScoped<GetMerchantByIdHandler>();

        return services;
    }
}
