namespace SharedKernal.Messaging.Events.Subscription
{
    public record SubscriptionPending(
        Guid SubscriptionId,
        Guid TenantId,
        Guid PlanId,
        Guid ProductId,
        decimal Amount,
        string Currency,
        DateTime CreatedAt);
}
