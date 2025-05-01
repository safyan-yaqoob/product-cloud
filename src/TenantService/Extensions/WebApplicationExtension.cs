using TenantService.Features.CreateTenant;
using TenantService.Features.GetTenantById;
using TenantService.Middleware;

namespace TenantService.Extensions
{
  public static class WebApplicationExtension
  {
    public static WebApplication ConfigurePipeline(this WebApplication app, IConfiguration configuration)
    {
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();
      app.UseMiddleware<ExceptionHandlingMiddleware>();

      app.MapGroup("/tenants")
         .MapCreateTenant()
         .MapGetTenantById();

      return app;
    }
  }
}
