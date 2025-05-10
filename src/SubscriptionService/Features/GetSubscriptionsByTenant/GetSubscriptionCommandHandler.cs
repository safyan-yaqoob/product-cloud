using Microsoft.EntityFrameworkCore;
using SharedKernal.Common;
using SharedKernal.CQRS;
using SubscriptionService.Database;
using SubscriptionService.Entities;

namespace SubscriptionService.Features.GetSubscriptionsByTenant
{
    public sealed class GetSubscriptionCommandHandler(SubscriptionDbContext context) : ICommandHandler<GetSubscriptionsCommand, GetSubscriptionsCommandResponse>
    {
        public async Task<GetSubscriptionsCommandResponse> Handle(GetSubscriptionsCommand command, CancellationToken cancellationToken = default)
        {
            var subscription = await context.Set<Subscription>()
                  .FirstOrDefaultAsync(s => s.TenantId == command.TenantId, cancellationToken);

            if (subscription is null)
                throw new AppException(AppError.NotFound("Subscription not found."));

            return new GetSubscriptionsCommandResponse
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
