namespace BillingService.Shared.Models
{
  public class StripeCheckoutRequest
  {
    public Guid TenantId { get; set; }
    public string CustomerEmail { get; set; } = default!;
    public string PriceId { get; set; } = default!;
    public string SuccessUrl { get; set; } = default!;
    public string CancelUrl { get; set; } = default!;
    public string ProductName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
  }
}
