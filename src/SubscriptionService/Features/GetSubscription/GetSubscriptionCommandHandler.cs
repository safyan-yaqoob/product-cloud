using Microsoft.EntityFrameworkCore;
using SharedKernal.Common;
using SharedKernal.CQRS;
using SubscriptionService.Database;
using SubscriptionService.Entities;

namespace SubscriptionService.Features.GetSubscription
{
    public sealed class GetSubscriptionCommandHandler(SubscriptionDbContext context) : ICommandHandler<GetSubscriptionCommand, GetSubscriptionCommandResponse>
    {
        public async Task<GetSubscriptionCommandResponse> Handle(GetSubscriptionCommand command, CancellationToken cancellationToken = default)
        {
            var subscription = await context.Set<Subscription>()
                  .FirstOrDefaultAsync(s => s.Id == command.SubscriptionId, cancellationToken);

            if (subscription is null)
                throw new AppException(AppError.NotFound("Subscription not found."));

            return new GetSubscriptionCommandResponse
            {
                Id = subscription.Id,
                TenantId = subscription.TenantId,
                PlanName = subscription.PlanName,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                IsActive = subscription.IsActive
            };
        }
    }
}
