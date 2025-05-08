using SharedKernal.CQRS;

namespace BillingService.Features.GetBillingByTenant
{
  public record GetTenantBillingCommand(Guid TenantId): ICommand<GetBillingCommandResponse>;
}
