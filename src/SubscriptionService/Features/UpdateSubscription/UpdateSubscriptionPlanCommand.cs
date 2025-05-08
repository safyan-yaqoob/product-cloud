using SharedKernal.CQRS;

namespace SubscriptionService.Features.UpdateSubscription
{
  public record UpdateSubscriptionPlanCommand(Guid SubscriptionId, string NewPlanName): ICommand<Guid>;
}
