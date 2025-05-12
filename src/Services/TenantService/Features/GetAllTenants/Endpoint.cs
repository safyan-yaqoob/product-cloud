using Microsoft.AspNetCore.Mvc;
using ProductCloud.SharedKernal.CQRS;

namespace TenantService.Features.GetAllTenants
{
	public static class Endpoint
	{
		public static RouteGroupBuilder MapGetAllTenant(this RouteGroupBuilder group)
		{
			group.MapPost("/get-all", async ([FromBody] GetAllTenantCommand command, 
				ICommandHandler<GetAllTenantCommand, IEnumerable<GetAllTenantResponse>> handler, CancellationToken cancellationToken) =>
			{
				var id = await handler.Handle(command, cancellationToken);
				return Results.Created($"/tenants/{id}", new { id });
			})
			.WithName("GetAllTenants")
			.WithSummary("Get all tenants");

			return group;
		}
	}
}