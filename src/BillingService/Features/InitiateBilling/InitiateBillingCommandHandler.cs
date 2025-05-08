using BillingService.Abstraction;
using BillingService.Shared.Models;
using SharedKernal.Common;
using SharedKernal.CQRS;
using SharedKernal.Protos;

namespace BillingService.Features.InitiateBilling
{
    public class InitiateBillingCommandHandler(IStripeBillingService stripeBillingService,
        SubscriptionGrpc.SubscriptionGrpcClient subClient) : ICommandHandler<InitiateBillingCommand, string>
    {
        public async Task<string> Handle(InitiateBillingCommand command, CancellationToken cancellationToken = default)
        {
            //var tenant = await tenantServiceClient.GetTenantAsync(command.TenantId, cancellationToken);
            //if (tenant is null)
            //  throw new AppException(AppError.NotFound("Tenant not found."));

            var subscriptionQuery = new GetSubscriptionRequest
            {
                SubscriptionId = command.SubscriptionId.ToString(),
            };
            var subscription = subClient.GetActiveSubscription(subscriptionQuery);
            if (subscription is null)
                throw new AppException(AppError.NotFound("No subscription found."));

            var checkoutUrl = await stripeBillingService.CreateCheckoutSessionAsync(new StripeCheckoutRequest
            {
                TenantId = command.TenantId,
                CustomerEmail = "",
                Amount = 0,
                SuccessUrl = "command.SuccessUrl",
                CancelUrl = "command.CancelUrl"
            });

            return checkoutUrl;
        }
    }
}
