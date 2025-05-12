using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using ProductService.Entities;
using ProductService.Features.GetPlans;
using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.GetProductById
{
	public record GetProductByIdCommandHandler(ProductDbContext context) : ICommandHandler<GetProductByIdCommand, GetProductByIdResponse>
	{
		public async Task<GetProductByIdResponse> Handle(GetProductByIdCommand command, CancellationToken cancellationToken = default)
		{
			var product = await context.Set<Product>()
				.Include(p => p.Plans)
					.ThenInclude(plan => plan.Features)
				.FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);

			if (product == null)
				throw new KeyNotFoundException($"Product with ID {command.ProductId} not found.");

			return new GetProductByIdResponse
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Logo = product.Logo,
				UrlSlug = product.UrlSlug,
				Plans = product.Plans.Select(plan => new GetPlansResponse
				{
					Id = plan.Id,
					Name = plan.Name,
					Price = plan.MonthlyPrice > 0 ? plan.MonthlyPrice : plan.AnnaulPrice,
					Currency = plan.Currency,
					ProductId = plan.ProductId,
					BillingCycle = (int)plan.BillingCycle,
					Features = plan.Features.Select(f => f.Name).ToArray()
				}).ToList()
			};
		}
	}
}
