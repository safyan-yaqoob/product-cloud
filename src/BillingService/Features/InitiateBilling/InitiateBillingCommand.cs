using Shared.CQRS;

namespace BillingService.Features.InitiateBilling
{
  public record InitiateBillingCommand(Guid TenantId, Guid SubscriptionId, decimal Amount) :ICommand<string>;
}
