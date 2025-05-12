using Microsoft.AspNetCore.Mvc;
using ProductCloud.SharedKernal.CQRS;
using TenantService.Features.CreateTenant;

namespace TenantService.Features.GetTenantById
{
	public static class Endpoint
	{
		public static RouteGroupBuilder MapGetTenantById(this RouteGroupBuilder group)
		{
			group.MapPost("/{id}", async ([FromBody] GetTenantByIdCommand request,
			  ICommandHandler<GetTenantByIdCommand, GetTenantByIdResponse> handler, CancellationToken cancellationToken) =>
			{
				var id = await handler.Handle(request, cancellationToken);
				return Results.Created($"/tenants/{id}", new { id });
			})
			.WithName("GetTenant")
			.WithSummary("Get tenant by id");

			return group;
		}
	}
}
