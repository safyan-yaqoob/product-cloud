using Microsoft.AspNetCore.Mvc;
using ProductCloud.SharedKernal.CQRS;

namespace BillingService.Features.RefundSubscriptionPayment;

public static class RefundSubscriptionBillingEndpoint
{
    public static RouteGroupBuilder MapRefundSubscriptionPayment(this RouteGroupBuilder group)
    {
        group.MapPost("/refund", async ([FromBody] RefundSubscriptionPaymentCommand command, ICommandHandler<RefundSubscriptionPaymentCommand, bool> handler) =>
        {
            var result = await handler.Handle(command);
            return Results.Ok(result);
        });

        return group;
    }
}