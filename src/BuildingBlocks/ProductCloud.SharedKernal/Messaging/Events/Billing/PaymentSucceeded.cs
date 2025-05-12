namespace ProductCloud.SharedKernal.Messaging.Events.Billing
{
    public record PaymentSucceeded(
    Guid SubscriptionId,
    string StripePaymentIntentId,
    DateTime PaidAt);
}
