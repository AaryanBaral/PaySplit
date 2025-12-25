using Serilog;

namespace PaySplit.API;

public static class MiddlewareExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();

        // app.UseAuthentication();
        // app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
