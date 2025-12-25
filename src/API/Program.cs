using Application.Interfaces.Presistence;
using Application.Interfaces.Repository;
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
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    Log.Information("Starting PaySplit API...");

    builder.Host.UseSerilog();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<AppDbContext>((sp, options) =>
    {
        var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options
            .UseSqlServer(connectionString)
            .UseLoggerFactory(loggerFactory);

        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    });

    // Persistence and repositories
    builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<AppDbContext>());
    builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
    builder.Services.AddScoped<ITenantRepository, TenantRepository>();
    builder.Services.AddScoped<IMerchantRepository, MerchantRepository>();

    // Tenant handlers
    builder.Services.AddScoped<CreateTenantHandler>();
    builder.Services.AddScoped<ActivateCommandHandler>();
    builder.Services.AddScoped<DeactivateCommandHandler>();
    builder.Services.AddScoped<SuspendCommandHandler>();
    builder.Services.AddScoped<UpdateCommandHandler>();
    builder.Services.AddScoped<GetAllTenantHandler>();
    builder.Services.AddScoped<GetTenantByIdHandler>();

    // Merchant handlers
    builder.Services.AddScoped<CreateMerchantHandler>();
    builder.Services.AddScoped<UpdateMerchantHandler>();
    builder.Services.AddScoped<ActivateMerchantHandler>();
    builder.Services.AddScoped<DeactivateMerchantHandler>();
    builder.Services.AddScoped<SuspendMerchantHandler>();
    builder.Services.AddScoped<GetAllMerchantHandler>();
    builder.Services.AddScoped<GetMerchantByIdHandler>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
