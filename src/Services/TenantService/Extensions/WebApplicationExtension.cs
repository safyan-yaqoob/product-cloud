using TenantService.Features.CreateTenant;
using TenantService.Features.GetAllTenants;
using TenantService.Features.GetTenantById;
using TenantService.Features.GetTenantByUser;
using TenantService.Features.UpdateTenant;
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

			app.MapGroup("/api/tenants")
				.RequireRateLimiting("TenantApi")
				.RequireAuthorization()
				.MapCreateTenant()
				.MapGetTenantById()
				.MapGetAllTenant()
				.MapGetTenantByUser()
				.MapUpdateTenant();

			return app;
		}
	}
}
