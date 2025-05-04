using ProductService.Features.CreatePlan;
using ProductService.Features.CreateProduct;
using ProductService.Features.DeletePlan;
using ProductService.Features.DeleteProduct;
using ProductService.Features.GetPlans;
using ProductService.Features.GetProductById;
using ProductService.Features.GetProducts;
using ProductService.Middleware;

namespace ProductService.Extensions
{
	public static class WebApplicationExtensions
	{
		public static WebApplication ConfigurePipeline(this WebApplication app, IConfiguration configuration)
		{
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseMiddleware<ExceptionHandlingMiddleware>();

			app.MapGroup("/api/products")
			   .MapCreateProductEndpoint()
			   .MapGetProductsEndpoint()
			   .MapGetProductByIdEndpoint()
			   .MapDeleteProductEndpoint();

			app.MapGroup("/api/plans")
			   .MapCreatePlanEndpoint()
			   .MapGetPlansEndpoint()
			   .MapDeletePlanEndpoint();

			return app;
		}
	}
}
