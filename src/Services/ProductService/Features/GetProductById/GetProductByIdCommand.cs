using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.GetProductById
{
	public record GetProductByIdCommand(Guid ProductId): ICommand<GetProductByIdResponse>;
}
