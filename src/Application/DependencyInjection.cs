using Application.Merchants.Command.ActivateMerchant;
using Application.Merchants.Command.CreateMerchant;
using Application.Merchants.Command.DeactivateMerchant;
using Application.Merchants.Command.SuspendMerchant;
using Application.Merchants.Command.UpdateMerchant;
using Application.Merchants.Query.GetAllMerchant;
using Application.Merchants.Query.GetMerchantById;
using Application.Tenants.Command.ActivateTenant;
using Application.Tenants.Command.CreateTenant;
using Application.Tenants.Command.DeactivateTenant;
using Application.Tenants.Command.SuspendTenant;
using Application.Tenants.Command.UpdateTenant;
using Application.Tenants.Query.GetAllTenant;
using Application.Tenants.Query.GetTenantById;
using Microsoft.Extensions.DependencyInjection;

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
