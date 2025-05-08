using SharedKernal.CQRS;

namespace ProductService.Features.CreatePlan
{
  public record CreatePlanCommand(string Name, string Description, decimal mPrice, decimal aPrice, string Currency, int BillingCycle, string[] Features, Guid ProductId): ICommand<Guid>;
}