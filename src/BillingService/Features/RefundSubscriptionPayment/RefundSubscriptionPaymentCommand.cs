using SharedKernal.CQRS;

namespace BillingService.Features.RefundSubscriptionPayment;

public record RefundSubscriptionPaymentCommand : ICommand<bool>
{
    public Guid SubscriptionId { get; set; }
}