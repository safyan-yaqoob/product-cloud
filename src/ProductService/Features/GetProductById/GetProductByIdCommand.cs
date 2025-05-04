using Shared.CQRS;

namespace ProductService.Features.GetProductById
{
	public record GetProductByIdCommand(Guid ProductId): ICommand<GetProductByIdResponse>;
}
