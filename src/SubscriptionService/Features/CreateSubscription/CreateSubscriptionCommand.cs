using SharedKernal.CQRS;

namespace SubscriptionService.Features.CreateSubscription
{
  public record CreateSubscriptionCommand(Guid TenantId, Guid ProductId, Guid PlanId) : ICommand<Guid>;
}
