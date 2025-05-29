using ProductService.Database;
using ProductCloud.SharedKernal.Common;
using ProductCloud.SharedKernal.CQRS;
using ProductService.Database.Entities;

namespace ProductService.Features.CreatePlan
{
    public class CreatePlanCommandHandler(ProductDbContext context) : ICommandHandler<CreatePlanCommand, Guid>
    {
        public async Task<Guid> Handle(CreatePlanCommand command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                throw new AppException(AppError.Validation("Plan name is required."));

            if (command.mPrice <= 0 && command.aPrice <= 0)
                throw new AppException(AppError.Validation("At least one price (monthly or annual) must be greater than zero."));

            if (string.IsNullOrWhiteSpace(command.Currency))
                throw new AppException(AppError.Validation("Currency is required."));

            var plan = new Plan
            {
                Id = Guid.CreateVersion7(),
                Name = command.Name,
                Description = command.Description,
                MonthlyPrice = command.mPrice,
                AnnaulPrice = command.aPrice,
                Currency = command.Currency,
                BillingCycle = (Common.BillingCycle)command.BillingCycle,
                ProductId = command.ProductId
            };

            var features = command.Features
                .Select(featureName => new Feature
                {
                    Id = Guid.CreateVersion7(),
                    Name = featureName,
                    PlanId = plan.Id
                })
                .ToList();

            context.Set<Plan>().Add(plan);
            context.Set<Feature>().AddRange(features);

            await context.SaveChangesAsync(cancellationToken);
            return plan.Id;
        }
    }
}
