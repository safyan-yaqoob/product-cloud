using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedKernal.Common;
using SharedKernal.CQRS;
using SubscriptionService.Database;
using SubscriptionService.Entities;

namespace SubscriptionService.Features.UpdateSubscription
{
    public sealed class UpdateSubscriptionCommandHandler(SubscriptionDbContext context) : ICommandHandler<UpdateSubscriptionPlanCommand, Guid>
    {
        public async Task<Guid> Handle([FromBody] UpdateSubscriptionPlanCommand command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command.NewPlanName))
                throw new AppException(AppError.Validation("New plan name cannot be empty."));

            var subscription = await context.Set<Subscription>()
                .FirstOrDefaultAsync(s => s.Id == command.SubscriptionId, cancellationToken);

            if (subscription is null)
                throw new AppException(AppError.NotFound("Subscription not found."));

            if (!subscription.IsActive)
                throw new AppException(AppError.Validation("Cannot change plan on an inactive subscription."));

            subscription.PlanName = command.NewPlanName;
            subscription.EndDate = null;
            subscription.StartDate = DateTime.UtcNow;

            context.Set<Subscription>().Update(subscription);
            await context.SaveChangesAsync(cancellationToken);

            return subscription.Id;
        }
    }
}
