using BillingService.Database;
using BillingService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using ProductCloud.SharedKernal.Common;
using ProductCloud.SharedKernal.CQRS;
namespace BillingService.Features.UpdatePaymentMethod
{
    public class UpdatePaymentMethodCommandHandler(BillingDbContext context)
        : ICommandHandler<UpdatePaymentMethodCommand, Guid>
    {
        public async Task<Guid> Handle(UpdatePaymentMethodCommand command, CancellationToken cancellationToken = default)
        {
            var paymentMethod = await context.Set<PaymentMethod>()
                .FirstOrDefaultAsync(pm => pm.Id == command.Id, cancellationToken);

            if (paymentMethod is null)
                throw new AppException(AppError.NotFound($"Payment method with ID {command.Id} not found."));

            if (command.ExpiryDate < DateTime.UtcNow.Date)
                throw new AppException(AppError.Validation("Expiry date must be in the future."));

            paymentMethod.CardType = command.CardType;
            paymentMethod.LastFourDigits = command.LastFourDigits;
            paymentMethod.ExpiryDate = command.ExpiryDate;
            paymentMethod.BillingAddress = command.BillingAddress;
            paymentMethod.IsDefault = command.IsDefault;

            await context.SaveChangesAsync(cancellationToken);

            return paymentMethod.Id;
        }
    }
}
