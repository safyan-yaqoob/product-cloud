using ProductService.Features.GetPlan;
using SharedKernal.CQRS;

namespace ProductService.Features.DeletePlan
{
	public static class Endpoint
	{
		public static RouteGroupBuilder MapDeletePlanEndpoint(this RouteGroupBuilder group)
		{
			group.MapDelete("/{planId}", async (Guid planId, ICommandHandler<DeletePlanCommand, bool> handler) =>
			{
				var result = await handler.Handle(new DeletePlanCommand(planId));
				return Results.Ok(result);
			});

			return group;
		}
	}
}
