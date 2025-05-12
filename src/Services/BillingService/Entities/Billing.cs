using BillingService.Common;

namespace BillingService.Entities
{
  public class Billing
  {
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid TenantId { get; set; }
    public Guid SubscriptionId { get; set; }
    public string PaymentGatewayTransactionId { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public BillingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }
    public string FailureReason { get; set; }
    public string PaymentIntentId { get; set; } //Strip
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
  }
}
