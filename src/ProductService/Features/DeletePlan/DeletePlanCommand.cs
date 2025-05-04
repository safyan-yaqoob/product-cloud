using Shared.CQRS;

namespace ProductService.Features.GetPlan
{
	public record DeletePlanCommand(Guid PlanId) : ICommand<bool>;
}
