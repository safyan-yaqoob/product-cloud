using Shared.CQRS;

namespace SubscriptionService.Features.GetSubscription
{
  public record GetSubscriptionCommand(Guid SubscriptionId): ICommand<GetSubscriptionCommandResponse>;
}
