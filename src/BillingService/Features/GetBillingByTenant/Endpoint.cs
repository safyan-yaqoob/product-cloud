using Shared.CQRS;

namespace BillingService.Features.GetBillingByTenant
{
  public static class GetBillingByIdEndpoint
  {
    public static RouteGroupBuilder MapGetBillingById(this RouteGroupBuilder group)
    {
      group.MapGet("/{tenantId}", async (Guid tenantId, ICommandHandler<GetTenantBillingCommand, GetBillingCommandResponse> handler) =>
      {
        var result = await handler.Handle(new GetTenantBillingCommand(tenantId));
        return Results.Ok(result);
      });

      return group;
    }
  }
}
