using ProductService.Features.CreatePlan;
using ProductService.Features.CreateProduct;
using ProductService.Features.DeletePlan;
using ProductService.Features.DeleteProduct;
using ProductService.Features.GetPlanFeatures;
using ProductService.Features.GetPlans;
using ProductService.Features.GetProductById;
using ProductService.Features.GetProducts;
using ProductService.Features.UpdateProduct;
using ProductService.Middleware;
using ProductCloud.SharedKernal.Protos;

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
			   .MapDeleteProductEndpoint()
			   .MapUpdateProductEndpoint();

			app.MapGroup("/api/plans")
			   .MapCreatePlanEndpoint()
			   .MapGetPlansEndpoint()
			   .MapDeletePlanEndpoint()
			   .MapGetPlanFeaturesEndpoint();

			app.MapGrpcService<PlanGrpc.PlanGrpcBase>();

			return app;
		}
	}
}
