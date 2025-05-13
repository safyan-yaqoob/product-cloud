using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using ProductCloud.SharedKernal.CQRS;
using ProductService.Database.Entities;

namespace ProductService.Features.GetPlans
{
	public class GetPlansQueryHandler(ProductDbContext context) : ICommandHandler<GetPlansQuery, IEnumerable<GetPlansResponse>>
	{
		public async Task<IEnumerable<GetPlansResponse>> Handle(GetPlansQuery command, CancellationToken cancellationToken = default)
		{
			var plans = await context.Set<Plan>()
				.Include(p => p.Features)
				.Select(p => new GetPlansResponse
				{
					Id = p.Id,
					Name = p.Name,
					Price = p.MonthlyPrice > 0 ? p.MonthlyPrice : p.AnnaulPrice,
					Currency = p.Currency,
					ProductId = p.ProductId,
					BillingCycle = (int)p.BillingCycle,
					Features = p.Features.Select(f => f.Name).ToArray()
				})
				.ToListAsync(cancellationToken);

			return plans;
		}
	}
}
