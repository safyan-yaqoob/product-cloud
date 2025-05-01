using BillingService.Abstraction;
using BillingService.External;
using BillingService.Shared.Models;
using Shared.Common;
using Shared.CQRS;

namespace BillingService.Features.InitiateBilling
{
  public class InitiateBillingCommandHandler(
    SubscriptionServiceClient subscriptionServiceClient,
    TenantServiceClient tenantServiceClient,
    IStripeBillingService stripeBillingService
) : ICommandHandler<InitiateBillingCommand, string>
  {
    public async Task<string> Handle(InitiateBillingCommand command, CancellationToken cancellationToken = default)
    {
      var tenant = await tenantServiceClient.GetTenantAsync(command.TenantId, cancellationToken);
      if (tenant is null)
        throw new AppException(AppError.NotFound("Tenant not found."));

      var subscription = await subscriptionServiceClient.GetActiveSubscriptionAsync(command.TenantId, cancellationToken);
      if (subscription is null)
        throw new AppException(AppError.NotFound("No active subscription found."));

      var checkoutUrl = await stripeBillingService.CreateCheckoutSessionAsync(new StripeCheckoutRequest
      {
        TenantId = command.TenantId,
        CustomerEmail = tenant.ContactEmail,
        Amount = subscription.Price,
        SuccessUrl = "command.SuccessUrl",
        CancelUrl = "command.CancelUrl"
      });

      return checkoutUrl;
    }
  }
}
