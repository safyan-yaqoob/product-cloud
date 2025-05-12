using BillingService.Common;
using BillingService.Entities;
using Microsoft.EntityFrameworkCore;
using ProductCloud.SharedKernal.CQRS;
using Stripe;
using BillingService.Database;
using Stripe.Checkout;
using ProductCloud.SharedKernal.Common;

namespace BillingService.Features.StripeWebhook
{
    public class StripeWebhookHandler(ILogger<StripeWebhookHandler> _logger,
      IConfiguration _config,
      BillingDbContext _context,
      IHttpContextAccessor httpContextAccessor) : ICommandHandler<StripWebhookCommand>
    {
        public async Task Handle(StripWebhookCommand command, CancellationToken cancellationToken = default)
        {
            var secret = _config["Stripe:WebhookSecret"];
            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    command.Payload,
                    command.Signature,
                    secret
                );
            }
            catch (StripeException e)
            {
                _logger.LogError("Error validating webhook signature: {0}", e.Message);
                throw new AppException(AppError.Internal(@$"Error validating webhook signature: {e.Message}"));
            }

            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    await HandleCheckoutSessionCompleted(stripeEvent);
                    break;
                default:
                    _logger.LogWarning("Unhandled event type: {0}", stripeEvent.Type);
                    throw new AppException(AppError.Internal(@$"Unhandled event type: {stripeEvent.Type}"));
            }
        }

        private async Task HandleCheckoutSessionCompleted(Event stripeEvent)
        {
            var session = stripeEvent.Data.Object as Session;
            if (session == null)
            {
                // Invalid event type
                throw new AppException(AppError.Internal("Invalid session data."));
            }

            var tenantId = Guid.Parse(session.ClientReferenceId); // set this to the tenantId
            var paymentStatus = session.PaymentStatus;

            if (paymentStatus == "paid")
            {
                var invoice = await _context.Set<Entities.Invoice>()
                    .Where(i => i.TenantId == tenantId && i.Status == InvoiceStatus.Pending)
                    .FirstOrDefaultAsync();

                if (invoice != null)
                {
                    invoice.Status = InvoiceStatus.Paid;
                    _context.Set<Entities.Invoice>().Update(invoice);
                }
                else
                {
                    invoice = new Entities.Invoice()
                    {
                        TenantId = Guid.Parse(session.ClientReferenceId), // Tenant ID from session
                        Amount = Convert.ToDecimal(session.AmountTotal / 100),  // Stripe provides amount in cents
                        InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}",
                        Status = InvoiceStatus.Paid,
                        IssueDate = DateTime.UtcNow,
                        DueDate = DateTime.UtcNow.AddDays(7)
                    };

                    await _context.Set<Entities.Invoice>().AddAsync(invoice);
                }

                var billingTransaction = await _context.Set<Billing>()
                    .Where(b => b.TenantId == tenantId && b.Status == BillingStatus.Pending)
                    .FirstOrDefaultAsync();

                if (billingTransaction != null)
                {
                    billingTransaction.Status = BillingStatus.Paid;
                    billingTransaction.PaymentIntentId = session.PaymentIntentId; // Link to Stripe's PaymentIntent
                    _context.Set<Billing>().Update(billingTransaction);
                }
                else
                {
                    var billing = new Billing
                    {
                        TenantId = Guid.Parse(session.ClientReferenceId),
                        Amount = Convert.ToDecimal(session.AmountTotal / 100),
                        Currency = session.Currency,
                        Status = BillingStatus.Paid,
                        CreatedAt = DateTime.UtcNow,
                        InvoiceId = invoice.Id
                    };

                    await _context.Set<Billing>().AddAsync(billing);
                }

                await _context.SaveChangesAsync();

                // trigger subscription activation here (if applicable)
                // Update subscription status if needed
            }

            else
            {
                var billingTransaction = await _context.Set<Billing>()
                    .Where(b => b.TenantId == tenantId && b.Status == BillingStatus.Pending)
                    .FirstOrDefaultAsync();

                if (billingTransaction != null)
                {
                    billingTransaction.Status = BillingStatus.Failed;
                    _context.Set<Billing>().Update(billingTransaction);
                    await _context.SaveChangesAsync();
                }

                // send an email to the user about the failure
            }
        }
    }
}
