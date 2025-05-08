using Microsoft.AspNetCore.Mvc;
using SharedKernal.CQRS;

namespace BillingService.Features.InitiateBilling
{
    public static class InitiateBillingEndpoint
    {
        public static RouteGroupBuilder MapInitiateBilling(this RouteGroupBuilder group)
        {
            group.MapPost("/initiate-payment", async ([FromBody] InitiateBillingCommand command, ICommandHandler<InitiateBillingCommand, string> handler) =>
            {
                var result = await handler.Handle(command);
                return Results.Ok(result);
            });

            return group;
        }
    }
}
