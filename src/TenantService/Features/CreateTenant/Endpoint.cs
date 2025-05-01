using Microsoft.AspNetCore.Mvc;
using Shared.CQRS;

namespace TenantService.Features.CreateTenant
{
  public static class Endpoint
  {
    public static RouteGroupBuilder MapCreateTenant(this RouteGroupBuilder group)
    {
      group.MapPost("/", async ([FromBody] CreateTenantCommand request, ICommandHandler<CreateTenantCommand, Guid> handler, CancellationToken cancellationToken) =>
      {
        var id = await handler.Handle(request, cancellationToken);
        return Results.Created($"/tenants/{id}", new { id });
      })
      .WithName("CreateTenant")
      .WithSummary("Creates a new tenant");

      return group;
    }
  }
}
