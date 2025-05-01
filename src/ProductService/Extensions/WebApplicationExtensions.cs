using ProductService.Features.CreatePlan;
using ProductService.Features.CreateProduct;
using ProductService.Features.GetPlans;
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
         .MapGetProductEndpoint();

      app.MapGroup("/api/plans")
         .MapCreatePlanEndpoint()
         .MapGetPlanEndpoint();

      return app;
    }
  }
}
