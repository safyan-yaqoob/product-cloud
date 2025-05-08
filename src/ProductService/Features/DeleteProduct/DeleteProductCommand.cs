using SharedKernal.CQRS;

namespace ProductService.Features.DeleteProduct
{
	public record DeleteProductCommand(Guid ProductId) : ICommand<bool>;
}
