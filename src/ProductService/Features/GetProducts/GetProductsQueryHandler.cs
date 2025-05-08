using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using ProductService.Entities;
using SharedKernal.CQRS;

namespace ProductService.Features.GetProducts
{
	public class GetProductsQueryHandler(ProductDbContext context) : ICommandHandler<GetProductsQuery, IEnumerable<GetProductsResponse>>
	{
		public async Task<IEnumerable<GetProductsResponse>> Handle(GetProductsQuery command, CancellationToken cancellationToken = default)
		{
			int skip = (command.PageNumber - 1) * command.PageSize;

			return await context.Set<Product>()
				.OrderBy(p => p.Name)
				.Skip(skip)
				.Take(command.PageSize)
				.Select(p => new GetProductsResponse
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					Logo = p.Logo,
					UrlSlug = p.UrlSlug
				})
				.ToListAsync(cancellationToken);
		}
	}
}
