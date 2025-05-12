using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.GetPlanFeatures;


public static class ProductEndpoints
{
    public static RouteGroupBuilder MapGetPlanFeaturesEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}/features", async (Guid planId, ICommandHandler<GetPlanFeaturesCommand, IEnumerable<PlanFeaturesCommandResponse>> handler, CancellationToken cancellationToken) =>
        {
            var features = await handler.Handle(new GetPlanFeaturesCommand(planId), cancellationToken);
            return Results.Ok(features);
        });

        return group;
    }
}