using Shared.CQRS;

namespace ProductService.Features.CreatePlan
{
  public record CreatePlanCommand(string Name, decimal Price, string Currency, Guid ProductId): ICommand<Guid>;
}
