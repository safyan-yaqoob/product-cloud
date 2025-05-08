using SharedKernal.CQRS;

namespace ProductService.Features.DeleteProduct
{
	public static class Endpoint
	{
		public static RouteGroupBuilder MapDeleteProductEndpoint(this RouteGroupBuilder group)
		{
			group.MapDelete("/{productId}", async (Guid productId, ICommandHandler<DeleteProductCommand, bool> handler) =>
			{
				var result = await handler.Handle(new DeleteProductCommand(productId));
				return Results.Ok(result);
			});

			return group;
		}
	}
}
