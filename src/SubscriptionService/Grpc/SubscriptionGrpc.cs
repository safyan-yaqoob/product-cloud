using System.Threading;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using SharedKernal.Common;
using SharedKernal.Protos;
using SubscriptionService.Database;
using SubscriptionService.Entities;

namespace SubscriptionService.Grpc
{
    public class SubscriptionGrpc(SubscriptionDbContext dbContext) : SharedKernal.Protos.SubscriptionGrpc.SubscriptionGrpcBase
    {
        public override async Task<SubscriptionResponse> GetActiveSubscription(GetSubscriptionRequest request, ServerCallContext context)
        {
            var subscription = await dbContext.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.Id == Guid.Parse(request.SubscriptionId));

            if (subscription is null)
                throw new AppException(AppError.NotFound("Subscription not found."));

            return new SubscriptionResponse
            {
                SubscriptionId = subscription.Id.ToString(),
                StartDate = subscription.StartDate.ToString("dd mm yyyy"),
                EndDate = subscription.EndDate?.ToString("dd mm yyyy"),
                PlanType = 1,
                Status = "",
                Ammount = "10.00"
            };
        }

    }
}
