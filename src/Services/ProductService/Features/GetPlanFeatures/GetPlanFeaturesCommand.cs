using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.GetPlanFeatures;

public record GetPlanFeaturesCommand(Guid PlanId): ICommand<IEnumerable<PlanFeaturesCommandResponse>>;