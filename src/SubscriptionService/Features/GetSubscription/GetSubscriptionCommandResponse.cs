namespace SubscriptionService.Features.GetSubscription
{
  public record GetSubscriptionCommandResponse
  {
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string PlanName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
  }
}
