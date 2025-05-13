using BillingService.Shared.Common;

namespace BillingService.Database.Entities
{
  public class Invoice
  {
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid TenantId { get; set; }
    public Guid BillingTransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(30);
    public InvoiceStatus Status { get; set; }
    public string InvoiceNumber { get; set; } = default!;
    public Guid SubscriptionId { get; set; }
  }
}
