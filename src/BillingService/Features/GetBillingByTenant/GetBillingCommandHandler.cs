using BillingService.Database;
using BillingService.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.CQRS;

namespace BillingService.Features.GetBillingByTenant
{
  public class GetBillingCommandHandler(BillingDbContext context)
      : ICommandHandler<GetTenantBillingCommand, GetBillingCommandResponse>
  {
    public async Task<GetBillingCommandResponse> Handle(GetTenantBillingCommand command, CancellationToken cancellationToken = default)
    {
      var tenantId = command.TenantId;

      var latestInvoice = await context.Set<Invoice>()
          .Where(i => i.TenantId == tenantId)
          .OrderByDescending(i => i.IssueDate)
          .FirstOrDefaultAsync(cancellationToken);

      var recentTransactions = await context.Set<Billing>()
          .Where(t => t.TenantId == tenantId)
          .OrderByDescending(t => t.CreatedAt)
          .Take(5)
          .ToListAsync(cancellationToken);

      if (latestInvoice == null && !recentTransactions.Any())
        throw new AppException(AppError.NotFound("No billing records found for this tenant."));

      return new GetBillingCommandResponse
      {
        LatestInvoice = latestInvoice is not null
              ? new InvoiceDto(
                  latestInvoice.Id,
                  latestInvoice.Amount,
                  latestInvoice.IssueDate,
                  latestInvoice.DueDate,
                  latestInvoice.Status.ToString(),
                  latestInvoice.InvoiceNumber)
              : null,

        RecentTransactions = recentTransactions.Select(tx => new BillingTransactionDto(
            tx.Id,
            tx.Amount,
            tx.Currency,
            tx.Status.ToString(),
            tx.CreatedAt,
            tx.PaidAt))
        .ToList()
      };
    }
  }
}
