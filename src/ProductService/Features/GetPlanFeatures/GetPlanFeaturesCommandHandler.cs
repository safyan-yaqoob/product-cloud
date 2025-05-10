using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using ProductService.Entities;
using SharedKernal.Common;
using SharedKernal.CQRS;

namespace ProductService.Features.GetPlanFeatures;

public class GetPlanFeaturesCommandHandler(ProductDbContext dbContext) : ICommandHandler<GetPlanFeaturesCommand, IEnumerable<PlanFeaturesCommandResponse>>
{
    public async Task<IEnumerable<PlanFeaturesCommandResponse>> Handle(GetPlanFeaturesCommand command, CancellationToken cancellationToken = default)
    {
        var plan = await dbContext.Set<Plan>().FirstOrDefaultAsync(e => e.Id == command.PlanId, cancellationToken);

        if (plan == null)
            throw new AppException(AppError.NotFound("Plan not found."));
        
        var planFeatures = await dbContext.Set<Feature>()
            .Where(e=>e.PlanId == command.PlanId)
            .Select(e=> new PlanFeaturesCommandResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
            }).ToListAsync(cancellationToken);

        return planFeatures;
    }
}