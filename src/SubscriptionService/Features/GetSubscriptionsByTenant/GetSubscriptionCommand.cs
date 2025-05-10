using SharedKernal.CQRS;

namespace SubscriptionService.Features.GetSubscriptionsByTenant
{
  public record GetSubscriptionsCommand(Guid TenantId): ICommand<GetSubscriptionsCommandResponse>;
}
