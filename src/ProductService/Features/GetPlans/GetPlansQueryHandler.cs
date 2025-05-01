using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using ProductService.Entities;
using Shared.CQRS;

namespace ProductService.Features.GetPlans
{
  public class GetPlansQueryHandler(ProductDbContext context) : ICommandHandler<GetPlansQuery, IEnumerable<GetPlansResponse>>
  {
    public async Task<IEnumerable<GetPlansResponse>> Handle(GetPlansQuery command, CancellationToken cancellationToken = default)
    {
      return await context.Set<Plan>()
            .Select(p => new GetPlansResponse(p.Id, p.Name, p.Price, p.Currency, p.ProductId))
            .ToListAsync(cancellationToken);
    }
  }
}
