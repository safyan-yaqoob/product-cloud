using Microsoft.AspNetCore.Mvc;
using SharedKernal.CQRS;

namespace BillingService.Features.InitiateBilling
{
    public static class GetActiveCheckoutSessionEndpoint
    {
        public static RouteGroupBuilder MapGetActiveCheckoutSession(this RouteGroupBuilder group)
        {
            group.MapGet("/checkout", async ([FromBody] GetActiveCheckoutSessionCommand command, ICommandHandler<GetActiveCheckoutSessionCommand, string> handler) =>
            {
                var result = await handler.Handle(command);
                return Results.Ok(result);
            });

            return group;
        }
    }
}
