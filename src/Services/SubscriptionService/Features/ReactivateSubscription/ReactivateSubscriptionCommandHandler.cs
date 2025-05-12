using Microsoft.EntityFrameworkCore;
using ProductCloud.SharedKernal.Common;
using ProductCloud.SharedKernal.CQRS;
using SubscriptionService.Database;
using SubscriptionService.Entities;

namespace SubscriptionService.Features.ReactivateSubscription;

public class ReactivateSubscriptionCommandHandler(SubscriptionDbContext dbContext) : ICommandHandler<ReactivateSubscriptionCommand, Guid>
{
    public async Task<Guid> Handle(ReactivateSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.Set<Subscription>()
            .FirstOrDefaultAsync(e => e.Id == command.SubscriptionId && e.TenantId == command.TenantId,
                cancellationToken);

        if (subscription == null)
            throw new AppException(AppError.NotFound("Subscription not found."));
        
        subscription.IsActive = true;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return subscription.Id;
    }
}