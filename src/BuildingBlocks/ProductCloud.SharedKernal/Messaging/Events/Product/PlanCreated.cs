namespace ProductCloud.SharedKernal.Messaging.Events.Product
{
    public record PlanCreated(
    Guid PlanId,
    decimal? Price,
    string[]? Features,
    DateTime CreatedAt);
}
