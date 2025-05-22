using Microsoft.AspNetCore.Mvc;
using ProductCloud.SharedKernal.CQRS;

namespace BillingService.Features.GetActiveCheckoutSession
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
