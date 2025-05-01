using Shared.CQRS;

namespace ProductService.Features.GetProducts
{
  public record GetProductsQuery : ICommand<IEnumerable<GetProductsResponse>>;

}
