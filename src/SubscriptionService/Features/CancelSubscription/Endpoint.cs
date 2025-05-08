using Microsoft.AspNetCore.Mvc;
using SharedKernal.CQRS;

namespace SubscriptionService.Features.CancelSubscription
{
    public static class CancelSubscriptionEndpoint
    {
        public static RouteGroupBuilder MapCancelSubscription(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id}", async (Guid id, [FromServices] ICommandHandler<CancelSubscriptionCommand> handler) =>
            {
                await handler.Handle(new CancelSubscriptionCommand(id));
                return Results.NoContent();
            });

            return group;
        }
    }
}
