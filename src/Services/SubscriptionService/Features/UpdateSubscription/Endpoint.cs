using Microsoft.AspNetCore.Mvc;
using ProductCloud.SharedKernal.CQRS;

namespace SubscriptionService.Features.UpdateSubscription
{
    public static class UpdateSubscriptionPlanEndpoint
    {
        public static RouteGroupBuilder MapUpdateSubscriptionPlan(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}/plan", async ([FromBody] UpdateSubscriptionPlanCommand command, ICommandHandler<UpdateSubscriptionPlanCommand, Guid> handle) =>
            {
                var updated = await handle.Handle(command);
                return Results.Ok(updated);
            });

            return group;
        }
    }
}
