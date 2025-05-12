namespace ProductCloud.SharedKernal.Messaging.Events.Billing
{
    public record PaymentFailed(
    Guid SubscriptionId,
    string StripePaymentIntentId,
    DateTime FailedAt);
}
