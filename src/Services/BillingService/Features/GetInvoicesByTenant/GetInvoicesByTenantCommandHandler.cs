using BillingService.Database;
using BillingService.Entities;
using Microsoft.EntityFrameworkCore;
using ProductCloud.SharedKernal.CQRS;

namespace BillingService.Features.GetInvoicesByTenant
{
    public class GetInvoicesByTenantCommandHandler(BillingDbContext context)
        : ICommandHandler<GetInvoicesByTenantCommand, IEnumerable<GetInvoicesResponse>>
    {
        public async Task<IEnumerable<GetInvoicesResponse>> Handle(GetInvoicesByTenantCommand command, CancellationToken cancellationToken = default)
        {
            var invoices = await context.Set<Invoice>()
                .AsNoTracking()
                .Where(x => x.TenantId == command.TenantId)
                .ToListAsync(cancellationToken);

            return invoices.Select(e => new GetInvoicesResponse
            {
                Id = e.Id,
                InvoiceNumber = e.InvoiceNumber,
                Amount = e.Amount,
                Currency = e.Currency,
                BillingTransactionId = e.BillingTransactionId,
                DueDate = e.DueDate,
                IssueDate = e.IssueDate,
                SubscriptionId = e.SubscriptionId,
                Status = e.Status,
                TenantId = e.TenantId
            });
        }
    }
}
