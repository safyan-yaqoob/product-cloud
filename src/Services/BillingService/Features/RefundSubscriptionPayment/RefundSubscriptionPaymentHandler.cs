using BillingService.Database;
using ProductCloud.SharedKernal.CQRS;

namespace BillingService.Features.RefundSubscriptionPayment;

public class RefundSubscriptionPaymentHandler(BillingDbContext dbContext): ICommandHandler<RefundSubscriptionPaymentCommand, bool>
{
    public Task<bool> Handle(RefundSubscriptionPaymentCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}