using ProductCloud.SharedKernal.CQRS;

namespace SubscriptionService.Features.GetSubscriptionsByTenant
{
    public static class GetSubscriptionsByTenantEndpoint
    {
        public static RouteGroupBuilder MapGetSubscriptionsByTenant(this RouteGroupBuilder group)
        {
            group.MapGet("/{tenantId}", async (Guid tenantId, ICommandHandler<GetSubscriptionsCommand, GetSubscriptionsCommandResponse> handler) =>
            {
                var subscription = await handler.Handle(new GetSubscriptionsCommand(tenantId));

                if (subscription == null)
                    return Results.NotFound();

                return Results.Ok(subscription);
            });

            return group;
        }
    }
}
