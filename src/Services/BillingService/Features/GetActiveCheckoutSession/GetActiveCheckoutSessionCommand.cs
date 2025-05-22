using ProductCloud.SharedKernal.CQRS;

namespace BillingService.Features.GetActiveCheckoutSession
{
  public record GetActiveCheckoutSessionCommand(Guid TenantId) :ICommand<string>;
}
