using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using ProductService.Entities;
using Shared.CQRS;

namespace ProductService.Features.GetProducts
{
  public class GetProductsQueryHandler(ProductDbContext context) : ICommandHandler<GetProductsQuery, IEnumerable<GetProductsResponse>>
  {
    public async Task<IEnumerable<GetProductsResponse>> Handle(GetProductsQuery command, CancellationToken cancellationToken = default)
    {
      return await context.Set<Product>()
            .Select(p => new GetProductsResponse(p.Id, p.Name, p.Description))
            .ToListAsync(cancellationToken);
    }
  }
}
