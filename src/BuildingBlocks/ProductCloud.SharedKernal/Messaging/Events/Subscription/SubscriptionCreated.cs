namespace ProductCloud.SharedKernal.Messaging.Events.Subscription
{
    public record SubscriptionCreated(
    Guid SubscriptionId,
    Guid TenantId,
    Guid PlanId,
    Guid ProductId,
    decimal Amount,
    string Currency,
    DateTime CreatedAt);
}
