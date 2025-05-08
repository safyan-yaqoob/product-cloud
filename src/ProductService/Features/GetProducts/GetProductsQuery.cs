using SharedKernal.CQRS;

namespace ProductService.Features.GetProducts
{
  public record GetProductsQuery(string SearchText, int PageNumber, int PageSize) : ICommand<IEnumerable<GetProductsResponse>>;

}
