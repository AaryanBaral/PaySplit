using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using PaySplit.Application;
using PaySplit.Infrastructure;

using Serilog;

namespace PaySplit.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PaySplit API",
                Version = "v1"
            });
        });

        services.AddApplication();
        services.AddInfrastructure(configuration);

        return services;
    }
}
