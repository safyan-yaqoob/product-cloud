using BillingService.Features.AddPaymentMethod;
using BillingService.Features.GetBillingByTenant;
using BillingService.Features.GetInvoicesByTenant;
using BillingService.Features.GetTenantPaymentMethods;
using BillingService.Features.InitiateBilling;
using BillingService.Features.RefundSubscriptionPayment;
using BillingService.Features.StripeWebhook;
using BillingService.Features.UpdatePaymentMethod;
using BillingService.Middleware;

namespace BillingService.Extensions
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

            app.MapGroup("/billing")
              .MapGetBillingById()
              .MapGetInvoicesByTenant()
              .MapAddPaymentMethod()
              .MapUpdatePaymentMethod()
              .MapStripeWebhook()
              .MapGetActiveCheckoutSession()
              .MapGetTenantPaymentMethods()
              .MapRefundSubscriptionPayment();

            return app;
        }
    }
}
