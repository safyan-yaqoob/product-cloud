using SharedKernal.CQRS;

namespace BillingService.Features.GetTenantPaymentMethods
{
  public class GetPaymentMethodCommandResponse
  {
    public List<PaymentMethodDto> Methods { get; set; } = [];
  }

  public record PaymentMethodDto(
      Guid Id,
      string CardType,
      string LastFourDigits,
      DateTime ExpiryDate,
      bool IsDefault,
      string? BillingAddress);
}
