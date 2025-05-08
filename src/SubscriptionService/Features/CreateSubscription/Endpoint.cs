using Microsoft.AspNetCore.Mvc;
using SharedKernal.CQRS;

namespace SubscriptionService.Features.CreateSubscription
{
  public static class CreateSubscriptionEndpoint
  {
    public static RouteGroupBuilder MapCreateSubscription(this RouteGroupBuilder group)
    {
      group.MapPost("/", async ([FromBody] CreateSubscriptionCommand command, ICommandHandler<CreateSubscriptionCommand, Guid> handler, CancellationToken cancellationToken) =>
      {
        var id = await handler.Handle(command);
        return Results.Created($"/subscriptions/{id}", id);
      });

      return group;
    }
  }
}
