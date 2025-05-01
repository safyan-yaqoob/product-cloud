using Shared.CQRS;

namespace BillingService.Features.GetBillingByTenant
{
  public record GetTenantBillingCommand(Guid TenantId): ICommand<GetBillingCommandResponse>;
}
