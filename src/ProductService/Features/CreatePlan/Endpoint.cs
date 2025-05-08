using Microsoft.AspNetCore.Mvc;
using SharedKernal.CQRS;

namespace ProductService.Features.CreatePlan
{
	public static class PlanEndpoints
	{
		public static RouteGroupBuilder MapCreatePlanEndpoint(this RouteGroupBuilder group)
		{
			group.MapPost("/create", async ([FromBody] CreatePlanCommand command, ICommandHandler<CreatePlanCommand, Guid> handler) =>
			{
				var id = await handler.Handle(command);
				return Results.Created($"/create/{id}", id);
			});

			return group;
		}
	}
}
