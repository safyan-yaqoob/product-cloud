namespace SharedKernal.Messaging.Events.Subscription
{
    public record SubscriptionCancelled(
    Guid SubscriptionId,
    Guid TenantId,
    Guid PlanId,
        Guid ProductId,
    DateTime CancelledAt);
}
