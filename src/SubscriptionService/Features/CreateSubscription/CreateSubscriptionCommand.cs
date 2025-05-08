using SharedKernal.CQRS;

namespace SubscriptionService.Features.CreateSubscription
{
  public record CreateSubscriptionCommand(Guid TenantId, string PlanName) : ICommand<Guid>;
}
