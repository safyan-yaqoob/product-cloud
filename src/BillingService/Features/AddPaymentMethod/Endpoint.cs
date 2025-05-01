using Microsoft.AspNetCore.Mvc;
using Shared.CQRS;

namespace BillingService.Features.AddPaymentMethod
{
  public static class AddPaymentMethodEndpoint
  {
    public static RouteGroupBuilder MapAddPaymentMethod(this RouteGroupBuilder group)
    {
      group.MapPost("/payment-methods", async ([FromBody] AddPaymentMethodCommand command, ICommandHandler<AddPaymentMethodCommand, Guid> handler) =>
      {
        var id = await handler.Handle(command);
        return Results.Created($"/billing/payment-methods/{id}", new { id });
      });

      return group;
    }
  }
}
