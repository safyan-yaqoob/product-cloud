using Microsoft.AspNetCore.Mvc;
using SharedKernal.CQRS;

namespace ProductService.Features.UpdateProduct;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapUpdateProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapPatch("/update", async ([FromBody] UpdateProductCommand command, 
            ICommandHandler<UpdateProductCommand, Guid> handler, CancellationToken cancellationToken) =>
        {
            var id = await handler.Handle(command, cancellationToken);
            return Results.Ok(id);
        });

        return group;
    }
}