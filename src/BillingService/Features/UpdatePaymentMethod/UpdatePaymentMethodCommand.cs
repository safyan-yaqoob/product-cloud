using SharedKernal.CQRS;

namespace BillingService.Features.UpdatePaymentMethod
{
  public record UpdatePaymentMethodCommand : ICommand<Guid>
  {
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid TenantId { get; set; }
    public string PaymentGatewayCustomerId { get; set; } = default!;
    public string CardType { get; set; } = default!;
    public string LastFourDigits { get; set; } = default!;
    public DateTime ExpiryDate { get; set; }
    public string BillingAddress { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDefault { get; set; } = false;
  }
}
