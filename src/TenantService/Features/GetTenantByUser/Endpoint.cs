using SharedKernal.CQRS;
using TenantService.Features.GetTenantById;

namespace TenantService.Features.GetTenantByUser
{
	public static class Endpoint
	{
		public static RouteGroupBuilder MapGetTenantByUser(this RouteGroupBuilder group)
		{
			group.MapGet("/user/{userid}", async (Guid userId, ICommandHandler<GetTenantByUserCommand, GetTenantByIdResponse> handler, CancellationToken cancellationToken) =>
			{
				var tenant = await handler.Handle(new GetTenantByUserCommand(userId), cancellationToken);

				if (tenant == null)
				{
					return Results.NotFound($"Tenant with user ID {userId} not found.");
				}

				return Results.Ok(tenant);
			})
			.WithName("GetTenantByUser")
			.WithSummary("Get tenant by user ID");

			return group;
		}
	}
}
