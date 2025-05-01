using BillingService.Database;
using BillingService.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.CQRS;

namespace BillingService.Features.Invoices
{
  public class GetInvoiceCommandHandler(BillingDbContext context)
      : ICommandHandler<GetInvoiceCommand, GetInvoiceResponse>
  {
    public async Task<GetInvoiceResponse> Handle(GetInvoiceCommand command, CancellationToken cancellationToken = default)
    {
      var invoice = await context.Set<Invoice>()
          .AsNoTracking()
          .FirstOrDefaultAsync(x => x.Id == command.InvoiceId, cancellationToken);

      if (invoice is null)
        throw new AppException(AppError.NotFound($"Invoice with ID {command.InvoiceId} not found."));

      return new GetInvoiceResponse
      {
        Id = invoice.Id,
        TenantId = invoice.TenantId,
        SubscriptionId = invoice.SubscriptionId,
        Amount = invoice.Amount,
        Currency = invoice.Currency,
        Status = invoice.Status,
        IssueDate = invoice.IssueDate,
        DueDate = invoice.DueDate,
        InvoiceNumber = invoice.InvoiceNumber,
        BillingTransactionId = invoice.BillingTransactionId,
      };
    }
  }
}
