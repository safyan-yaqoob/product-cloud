using Microsoft.AspNetCore.Mvc;
using Shared.CQRS;

namespace ProductService.Features.GetPlans
{
  public static class PlanEndpoints
  {
    public static RouteGroupBuilder MapGetPlanEndpoint(this RouteGroupBuilder group)
    {
      group.MapGet("/", async ([FromBody] GetPlansQuery query, ICommandHandler<GetPlansQuery, IEnumerable<GetPlansResponse>> handler) =>
      {
        var result = await handler.Handle(query);
        return Results.Ok(result);
      });

      return group;
    }
  }
}
