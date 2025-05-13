namespace BillingService.Database.Entities
{
    public class CheckoutSession
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid SubscriptionId { get; set; }
        public string StripeSessionId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
