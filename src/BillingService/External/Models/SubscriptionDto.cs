namespace BillingService.External.Models
{
  public record SubscriptionDto
  {
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string PlanName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
    public string Currency { get; set; }
    public decimal Price { get; set; }
    public Guid InvoiceId { get; set; }
  }
}
