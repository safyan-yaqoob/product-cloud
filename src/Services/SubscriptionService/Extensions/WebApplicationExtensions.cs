using SubscriptionService.Features.CancelSubscription;
using SubscriptionService.Features.CreateSubscription;
using SubscriptionService.Features.GetSubscriptionsByTenant;
using SubscriptionService.Features.ReactivateSubscription;
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
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapGroup("/subscription")
              .MapCreateSubscription()
              .MapGetSubscriptionsByTenant()
              .MapUpdateSubscriptionPlan()
              .MapCancelSubscription()
              .MapReactivateSubscription();

            return app;
        }
    }
}
