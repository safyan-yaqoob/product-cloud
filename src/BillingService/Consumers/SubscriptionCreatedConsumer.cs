using BillingService.Abstraction;
using BillingService.Database;
using BillingService.Entities;
using BillingService.Shared.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedKernal.Messaging.Events.Subscription;

namespace BillingService.Consumers
{
    public class SubscriptionCreatedConsumer(BillingDbContext dbContext,
        IStripeBillingService stripeBillingService) : IConsumer<SubscriptionCreated>
    {
        public async Task Consume(ConsumeContext<SubscriptionCreated> context)
        {
            var subscription = context.Message;
            var isActiveSession = await dbContext.Set<CheckoutSession>().AnyAsync(e => e.TenantId == subscription.TenantId && e.ExpiresAt <= DateTime.UtcNow);
            if (!isActiveSession)
            {
                var checkoutUrl = await stripeBillingService.CreateCheckoutSessionAsync(new StripeCheckoutRequest
                {
                    TenantId = subscription.TenantId,
                    Amount = subscription.Amount,
                    SuccessUrl = "command.SuccessUrl",
                    CancelUrl = "command.CancelUrl"
                });

                var checkoutSession = new CheckoutSession
                {
                    Id = Guid.CreateVersion7(),
                    SubscriptionId = subscription.SubscriptionId,
                    StripeSessionId = "",
                    ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                    Url = checkoutUrl
                };

                await dbContext.Set<CheckoutSession>().AddAsync(checkoutSession);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
