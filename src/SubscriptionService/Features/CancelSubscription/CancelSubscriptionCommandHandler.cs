using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.CQRS;
using SubscriptionService.Database;
using SubscriptionService.Entities;

namespace SubscriptionService.Features.CancelSubscription
{
  public sealed class CancelSubscriptionCommandHandler(SubscriptionDbContext context) : ICommandHandler<CancelSubscriptionCommand>
  {
    public async Task Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
      var subscription = await context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.Id == command.SubscriptionId, cancellationToken);

      if (subscription is null)
        throw new AppException(AppError.NotFound("Subscription not found."));

      if (!subscription.IsActive)
        throw new AppException(AppError.Validation("Subscription is already inactive."));

      subscription.IsActive = false;
      subscription.EndDate = DateTime.UtcNow;

      context.Set<Subscription>().Update(subscription);
      await context.SaveChangesAsync(cancellationToken);
    }
  }
}
