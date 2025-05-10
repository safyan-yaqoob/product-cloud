using Microsoft.AspNetCore.Mvc;
using SharedKernal.CQRS;

namespace SubscriptionService.Features.ReactivateSubscription;

public static class ReactivateSubscriptionEndpoint
{
    public static RouteGroupBuilder MapReactivateSubscription(this RouteGroupBuilder group)
    {
        group.MapPost("/reactivate-subscription", async ([FromBody] ReactivateSubscriptionCommand command, 
            ICommandHandler<ReactivateSubscriptionCommand, Guid> handler) =>
        {
            var result = await handler.Handle(command);
            return Results.Ok(result);
        });

        return group;
    }
}