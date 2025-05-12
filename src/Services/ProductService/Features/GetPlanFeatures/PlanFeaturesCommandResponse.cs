using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.GetPlanFeatures;

public record PlanFeaturesCommandResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}