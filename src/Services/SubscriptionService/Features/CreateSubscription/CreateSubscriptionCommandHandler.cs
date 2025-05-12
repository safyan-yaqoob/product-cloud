using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductCloud.SharedKernal.Common;
using ProductCloud.SharedKernal.CQRS;
using ProductCloud.SharedKernal.Messaging.Events.Subscription;
using ProductCloud.SharedKernal.Protos;
using SubscriptionService.Database;
using SubscriptionService.Entities;

namespace SubscriptionService.Features.CreateSubscription
{
    public sealed class CreateSubscriptionCommandHandler(SubscriptionDbContext context, 
        PlanGrpc.PlanGrpcClient planGrpcClient,
        IPublishEndpoint publishEndpoint) : ICommandHandler<CreateSubscriptionCommand, Guid>
    {
        public async Task<Guid> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken = default)
        {
            var alreadyExists = await context.Set<Subscription>()
                .AnyAsync(s => s.TenantId == command.TenantId && s.IsActive, cancellationToken);

            if (alreadyExists)
                throw new AppException(AppError.Validation("Tenant already has an active subscription."));

            decimal amount;
            string currency;
            Subscription subscription;

            try
            {
                var planDetails = await planGrpcClient.GetPlanDetailsAsync(new GetPlanRequest { PlanId = command.PlanId.ToString() },
                    cancellationToken: cancellationToken);

                decimal monthly = decimal.Parse(planDetails.MonthlyPrice);
                decimal annualy = decimal.Parse(planDetails.AnnaulPrice);

                amount = monthly > 0 ? monthly : annualy;
                currency = planDetails.Currency;
            }
            catch (Exception)
            {
                // TO:DO implement fallback here
                throw new AppException(AppError.Internal("Grpc failed here"));
            }

            subscription = new Subscription
            {
                Id = Guid.CreateVersion7(),
                TenantId = command.TenantId,
                PlanId = command.PlanId,
                ProductId = command.ProductId,
                StartDate = DateTime.UtcNow,
                IsActive = true
            };

            await context.Set<Subscription>().AddAsync(subscription, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new SubscriptionCreated(subscription.Id,
                command.TenantId,
                subscription.PlanId,
                command.ProductId, amount, currency, DateTime.UtcNow));

            return subscription.Id;
        }
    }
}
