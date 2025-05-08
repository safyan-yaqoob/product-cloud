using BillingService.Database;
using BillingService.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernal.Common;
using SharedKernal.CQRS;

namespace BillingService.Features.AddPaymentMethod
{
  public class AddPaymentMethodCommandHandler(BillingDbContext context)
      : ICommandHandler<AddPaymentMethodCommand, Guid>
  {
    public async Task<Guid> Handle(AddPaymentMethodCommand command, CancellationToken cancellationToken)
    {
      if (string.IsNullOrWhiteSpace(command.PaymentMethodToken))
        throw new AppException(AppError.Validation("Payment method token is required."));

      var exists = await context.Set<PaymentMethod>()
          .AnyAsync(pm =>
              pm.TenantId == command.TenantId &&
              pm.PaymentGatewayCustomerId == command.PaymentMethodToken,
              cancellationToken);

      if (exists)
        throw new AppException(AppError.Validation("Payment method already exists for this tenant."));

      if (command.IsDefault)
      {
        var currentDefaults = await context.Set<PaymentMethod>()
            .Where(pm => pm.TenantId == command.TenantId && pm.IsDefault)
            .ToListAsync(cancellationToken);

        foreach (var current in currentDefaults)
          current.IsDefault = false;
      }

      var newPaymentMethod = new PaymentMethod
      {
        TenantId = command.TenantId,
        PaymentGatewayCustomerId = command.PaymentMethodToken,
        CardType = command.CardType,
        LastFourDigits = command.LastFourDigits,
        ExpiryDate = command.ExpiryDate,
        BillingAddress = command.BillingAddress,
        IsDefault = command.IsDefault
      };

      context.Set<PaymentMethod>().Add(newPaymentMethod);
      await context.SaveChangesAsync(cancellationToken);

      return newPaymentMethod.Id;
    }
  }
}
