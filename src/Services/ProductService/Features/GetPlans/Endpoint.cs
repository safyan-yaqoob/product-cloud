using Microsoft.AspNetCore.Mvc;
using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.GetPlans
{
	public static class PlanEndpoints
	{
		public static RouteGroupBuilder MapGetPlansEndpoint(this RouteGroupBuilder group)
		{
			group.MapPost("/get-all", async ([FromBody] GetPlansQuery query, ICommandHandler<GetPlansQuery, IEnumerable<GetPlansResponse>> handler) =>
			{
				var result = await handler.Handle(query);
				return Results.Ok(result);
			});

			return group;
		}
	}
}
