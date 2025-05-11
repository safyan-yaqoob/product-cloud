using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using ProductService.Entities;
using SharedKernal.Protos;

namespace ProductService.Grpc
{
    public class ProductPlanGrpc(ProductDbContext dbContext): PlanGrpc.PlanGrpcBase
    {
        public override async Task<PlanResponse> GetPlanDetails(GetPlanRequest request, ServerCallContext context)
        {
            var plan = await dbContext.Set<Plan>()
                .Where(e => e.Id == Guid.Parse(request.PlanId))
                .Include(p => p.Features)
                .Select(p => new PlanResponse
                {
                    PlanId = p.Id.ToString(),
                    Name = p.Name,
                    MonthlyPrice = p.MonthlyPrice.ToString(),
                    AnnaulPrice = p.AnnaulPrice.ToString(),
                    Currency = p.Currency,
                    Description = p.Description,
                }).FirstOrDefaultAsync();

            return plan;
        }
    }
}
