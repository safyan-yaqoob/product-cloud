using Microsoft.AspNetCore.Mvc;
using Shared.CQRS;

namespace BillingService.Features.StripeWebhook
{
  public static class StripeWebhookEndpoint
  {
    public static RouteGroupBuilder MapStripeWebhook(this RouteGroupBuilder group)
    {
      group.MapPost("/webhook", async (HttpRequest request, ICommandHandler<StripWebhookCommand> handler) =>
      {
        using var reader = new StreamReader(request.Body);
        var json = await reader.ReadToEndAsync();

        var stripeSignature = request.Headers["Stripe-Signature"];
        if (string.IsNullOrEmpty(stripeSignature))
          return Results.BadRequest("Missing Stripe signature header.");

        await handler.Handle(new StripWebhookCommand(json, stripeSignature));
        return Results.Ok();
      });

      return group;
    }
  }
}
