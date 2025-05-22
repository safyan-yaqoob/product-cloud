using AspNetCore.Serilog.RequestLoggingMiddleware;
using AuthService.Middleware;

namespace IdentityServer.Extensions
{
    public static class WebApplicationExtension
    {
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();           
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors();
            app.UseAuthentication();
            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseAuthorization();

            app.MapControllers();
            app.MapRazorPages();

            // Add default route
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/Index");
                return Task.CompletedTask;
            });

            return app;
        }
    }
}
