using Microsoft.AspNetCore.Mvc;
using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.CreateProduct
{
    public static class ProductEndpoints
    {
        public static RouteGroupBuilder MapCreateProductEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/create", async ([FromBody] CreateProductCommand command, ICommandHandler<CreateProductCommand, Guid> handler) =>
            {
                var id = await handler.Handle(command);
                return Results.Created($"/products/{id}", id);
            });

            return group;
        }
    }

}
