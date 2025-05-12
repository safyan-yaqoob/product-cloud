using ProductCloud.SharedKernal.CQRS;

namespace BillingService.Features.InitiateBilling
{
  public record GetActiveCheckoutSessionCommand(Guid TenantId) :ICommand<string>;
}
