using SharedKernal.CQRS;

namespace SubscriptionService.Features.ReactivateSubscription;

public class ReactivateSubscriptionCommand:ICommand<Guid>
{
    public Guid TenantId { get; set; }
    public Guid SubscriptionId { get; set; }
}