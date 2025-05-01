using ProductService.Database;
using ProductService.Entities;
using Shared.Common;
using Shared.CQRS;

namespace ProductService.Features.CreatePlan
{
  public class CreatePlanCommandHandler(ProductDbContext context) : ICommandHandler<CreatePlanCommand, Guid>
  {
    public async Task<Guid> Handle(CreatePlanCommand command, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrWhiteSpace(command.Name))
        throw new AppException(AppError.Validation("Plan name is required."));

      if (command.Price <= 0)
        throw new AppException(AppError.Validation("Plan price must be greater than zero."));

      if (string.IsNullOrWhiteSpace(command.Currency))
        throw new AppException(AppError.Validation("Currency is required."));

      var plan = new Plan
      {
        Id = Guid.CreateVersion7(),
        Name = command.Name,
        Price = command.Price,
        Currency = command.Currency,
        ProductId = command.ProductId
      };

      context.Set<Plan>().Add(plan);

      await context.SaveChangesAsync(cancellationToken);
      return plan.Id;
    }
  }
}
