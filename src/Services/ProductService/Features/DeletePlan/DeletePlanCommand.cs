using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.DeletePlan
{
	public record DeletePlanCommand(Guid PlanId) : ICommand<bool>;
}
