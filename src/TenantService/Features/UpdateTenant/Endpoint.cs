using Microsoft.AspNetCore.Mvc;
using SharedKernal.CQRS;

namespace TenantService.Features.UpdateTenant;

public static class Endpoint
{
    public static RouteGroupBuilder MapUpdateTenant(this RouteGroupBuilder group)
    {
        group.MapPatch("/update", async ([FromBody] UpdateTenantCommand request, ICommandHandler<UpdateTenantCommand, Guid> handler, CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(request, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("UpdateTenant")
            .WithSummary("Update existing tenant");

        return group;
    }
}