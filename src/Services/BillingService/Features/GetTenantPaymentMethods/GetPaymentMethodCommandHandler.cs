using BillingService.Database;
using BillingService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using ProductCloud.SharedKernal.Common;
using ProductCloud.SharedKernal.CQRS;

namespace BillingService.Features.GetTenantPaymentMethods
{
    public class GetPaymentMethodCommandHandler(BillingDbContext context)
       : ICommandHandler<GetPaymentMethodCommand, GetPaymentMethodCommandResponse>
    {
        public async Task<GetPaymentMethodCommandResponse> Handle(GetPaymentMethodCommand command, CancellationToken cancellationToken = default)
        {
            var methods = await context.Set<PaymentMethod>()
                .Where(pm => pm.TenantId == command.TenantId)
                .OrderByDescending(pm => pm.IsDefault)
                .ToListAsync(cancellationToken);

            if (!methods.Any())
                throw new AppException(AppError.NotFound("No payment methods found for this tenant."));

            var response = new GetPaymentMethodCommandResponse
            {
                Methods = methods.Select(pm => new PaymentMethodDto(
                    pm.Id,
                    pm.CardType,
                    pm.LastFourDigits,
                    pm.ExpiryDate,
                    pm.IsDefault,
                    pm.BillingAddress))
              .ToList()
            };

            return response;
        }
    }
}
