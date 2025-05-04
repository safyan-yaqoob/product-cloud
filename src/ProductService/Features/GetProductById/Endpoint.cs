using Microsoft.AspNetCore.Mvc;
using Shared.CQRS;

namespace ProductService.Features.GetProductById
{
	public static class ProductEndpoints
	{
		public static RouteGroupBuilder MapGetProductByIdEndpoint(this RouteGroupBuilder group)
		{
			group.MapGet("/{id}", async (Guid id, ICommandHandler<GetProductByIdCommand, GetProductByIdResponse> handler) =>
			{
				var result = await handler.Handle(new GetProductByIdCommand(id));
				return Results.Ok(result);
			});

			return group;
		}
	}
}
