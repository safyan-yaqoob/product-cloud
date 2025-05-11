namespace SharedKernal.Messaging.Events.Product
{
    public record PlanUpdated(
    Guid PlanId,
    decimal? NewPrice,
    string[]? NewFeatures,
    DateTime ModifiedAt);
}
