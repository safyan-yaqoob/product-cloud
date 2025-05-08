using SharedKernal.CQRS;

namespace BillingService.Features.GetTenantPaymentMethods
{

  public record GetPaymentMethodCommand(Guid TenantId) : ICommand<GetPaymentMethodCommandResponse>;
}
