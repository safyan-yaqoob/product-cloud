using Shared.CQRS;

namespace BillingService.Features.AddPaymentMethod
{
  public record AddPaymentMethodCommand(
      Guid TenantId,
      string PaymentMethodToken,
      string CardType,
      string LastFourDigits,
      DateTime ExpiryDate,
      string? BillingAddress,
      bool IsDefault
  ) : ICommand<Guid>;
}
