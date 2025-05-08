using Microsoft.AspNetCore.Mvc;
using SharedKernal.CQRS;

namespace ProductService.Features.GetProducts
{
    public static class ProductEndpoints
    {
        public static RouteGroupBuilder MapGetProductsEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/get-all", async ([FromBody] GetProductsQuery query, ICommandHandler<GetProductsQuery, IEnumerable<GetProductsResponse>> handler) =>
            {
                var result = await handler.Handle(query);
                return Results.Ok(result);
            });

            return group;
        }
    }
}
