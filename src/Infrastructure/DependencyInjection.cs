
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Queries;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Infrastructure.Persistence;
using PaySplit.Infrastructure.Persistence.Queries;
using PaySplit.Infrastructure.Persistence.Repositories;

namespace PaySplit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options
                .UseNpgsql(connectionString)
                .UseLoggerFactory(loggerFactory);

            if (IsDevelopmentEnvironment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IMerchantRepository, MerchantRepository>();
        services.AddScoped<IPayoutRepository, PayoutRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ILedgerEntryRepository, LedgerEntryRepository>();
        services.AddScoped<IMerchantBalanceQuery, MerchantBalanceQuery>();

        return services;
    }

    private static bool IsDevelopmentEnvironment()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase);
    }
}
