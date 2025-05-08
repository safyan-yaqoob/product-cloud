using SharedKernal.CQRS;

namespace BillingService.Features.StripeWebhook
{
  public record StripWebhookCommand(string Payload, string Signature): ICommand;
}
