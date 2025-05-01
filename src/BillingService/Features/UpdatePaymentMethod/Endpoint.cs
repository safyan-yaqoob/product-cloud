using Microsoft.AspNetCore.Mvc;
using Shared.CQRS;

namespace BillingService.Features.UpdatePaymentMethod
{
  public static class UpdatePaymentMethodEndpoint
  {
    public static RouteGroupBuilder MapUpdatePaymentMethod(this RouteGroupBuilder group)
    {
      group.MapPut("/payment-methods", async ([FromBody] UpdatePaymentMethodCommand command, ICommandHandler<UpdatePaymentMethodCommand, Guid> handler) =>
      {
        var updatedId = await handler.Handle(command);
        return Results.Ok(new { id = updatedId });
      });

      return group;
    }
  }
}
