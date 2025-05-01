using BillingService.Features.AddPaymentMethod;
using BillingService.Features.GetBillingByTenant;
using BillingService.Features.GetTenantPaymentMethods;
using BillingService.Features.InitiateBilling;
using BillingService.Features.Invoices;
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
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();
      app.UseMiddleware<ExceptionHandlingMiddleware>();

      app.MapGroup("/billing")
        .MapGetBillingById()
        .MapGetInvoiceById()
        .MapAddPaymentMethod()
        .MapUpdatePaymentMethod()
        .MapStripeWebhook()
        .MapInitiateBilling()
        .MapGetTenantPaymentMethods();

      return app;
    }
  }
}
