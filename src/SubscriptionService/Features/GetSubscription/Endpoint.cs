using Shared.CQRS;

namespace SubscriptionService.Features.GetSubscription
{
  public static class GetSubscriptionEndpoint
  {
    public static RouteGroupBuilder MapGetSubscription(this RouteGroupBuilder group)
    {
      group.MapGet("/{id}", async (Guid id, ICommandHandler<GetSubscriptionCommand, GetSubscriptionCommandResponse> handler) =>
      {
        var subscription = await handler.Handle(new GetSubscriptionCommand(id));

        if (subscription == null)
          return Results.NotFound();

        return Results.Ok(subscription);
      });

      return group;
    }
  }
}
