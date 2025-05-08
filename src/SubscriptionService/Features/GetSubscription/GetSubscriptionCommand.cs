using SharedKernal.CQRS;

namespace SubscriptionService.Features.GetSubscription
{
  public record GetSubscriptionCommand(Guid SubscriptionId): ICommand<GetSubscriptionCommandResponse>;
}
