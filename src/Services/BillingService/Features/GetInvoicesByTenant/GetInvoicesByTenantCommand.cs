using ProductCloud.SharedKernal.CQRS;

namespace BillingService.Features.GetInvoicesByTenant
{
  public record GetInvoicesByTenantCommand(Guid TenantId): ICommand<IEnumerable<GetInvoicesResponse>>;
}
