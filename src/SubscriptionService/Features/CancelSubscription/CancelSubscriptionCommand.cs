using Shared.CQRS;

namespace SubscriptionService.Features.CancelSubscription
{
  public record CancelSubscriptionCommand(Guid SubscriptionId): ICommand;
}
