using BillingService.Database;
using BillingService.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernal.Common;
using SharedKernal.CQRS;

namespace BillingService.Features.InitiateBilling
{
    public class GetActiveCheckoutSessionCommandHandler(BillingDbContext dbContext) : ICommandHandler<GetActiveCheckoutSessionCommand, string>
    {
        public async Task<string> Handle(GetActiveCheckoutSessionCommand command, CancellationToken cancellationToken = default)
        {
            var activeCheckoutSession = await dbContext.Set<CheckoutSession>().FirstOrDefaultAsync(e => e.TenantId == command.TenantId && e.ExpiresAt <= DateTime.UtcNow);
            if (activeCheckoutSession == null)
            {
                throw new AppException(AppError.Internal("No active checkout session is found!"));
            }

            return activeCheckoutSession.Url;
        }
    }
}
