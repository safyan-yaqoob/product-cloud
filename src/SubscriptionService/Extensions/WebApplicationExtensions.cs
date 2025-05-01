using SubscriptionService.Features.CancelSubscription;
using SubscriptionService.Features.CreateSubscription;
using SubscriptionService.Features.GetSubscription;
using SubscriptionService.Features.UpdateSubscription;
using SubscriptionService.Middleware;

namespace SubscriptionService.Extensions
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

      app.MapGroup("/subscription")
        .MapCreateSubscription()
        .MapGetSubscription()
        .MapUpdateSubscriptionPlan()
        .MapCancelSubscription();

      return app;
    }
  }
}
