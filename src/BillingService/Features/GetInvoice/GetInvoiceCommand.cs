using SharedKernal.CQRS;

namespace BillingService.Features.Invoices
{
  public record GetInvoiceCommand(Guid InvoiceId): ICommand<GetInvoiceResponse>;
}
