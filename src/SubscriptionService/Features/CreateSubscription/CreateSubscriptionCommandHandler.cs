using Microsoft.EntityFrameworkCore;
using SharedKernal.Common;
using SharedKernal.CQRS;
using SubscriptionService.Database;
using SubscriptionService.Entities;

namespace SubscriptionService.Features.CreateSubscription
{
    public sealed class CreateSubscriptionCommandHandler(SubscriptionDbContext context) : ICommandHandler<CreateSubscriptionCommand, Guid>
    {
        public async Task<Guid> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command.PlanName))
                throw new AppException(AppError.Validation("Plan name is required."));

            var alreadyExists = await context.Set<Subscription>()
                .AnyAsync(s => s.TenantId == command.TenantId && s.IsActive, cancellationToken);

            if (alreadyExists)
                throw new AppException(AppError.Validation("Tenant already has an active subscription."));

            var subscription = new Subscription
            {
                Id = Guid.CreateVersion7(),
                TenantId = command.TenantId,
                PlanName = command.PlanName,
                StartDate = DateTime.UtcNow,
                IsActive = true
            };

            await context.Set<Subscription>().AddAsync(subscription, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return subscription.Id;
        }
    }
}
