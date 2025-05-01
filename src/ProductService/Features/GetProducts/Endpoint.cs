using Microsoft.AspNetCore.Mvc;
using Shared.CQRS;

namespace ProductService.Features.GetProducts
{
  public static class ProductEndpoints
  {
    public static RouteGroupBuilder MapGetProductEndpoint(this RouteGroupBuilder group)
    {
      group.MapPost("/", async ([FromBody] GetProductsQuery query, ICommandHandler<GetProductsQuery, IEnumerable<GetProductsResponse>> handler) =>
      {
        var result = await handler.Handle(query);
        return Results.Ok(result);
      });

      return group;
    }
  }
}
