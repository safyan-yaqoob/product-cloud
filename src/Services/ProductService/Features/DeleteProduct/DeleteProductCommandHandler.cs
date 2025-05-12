using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using ProductService.Entities;
using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.DeleteProduct
{
	public class DeleteProductCommandHandler(ProductDbContext context) : ICommandHandler<DeleteProductCommand, bool>
	{
		public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken = default)
		{
			var product = await context.Set<Product>()
				.Include(p => p.Plans)
				.FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);

			if (product == null) return false;

			if (product.Plans.Any())
			{
				throw new InvalidOperationException("Cannot delete a product with associated plans.");
			}

			context.Set<Product>().Remove(product);

			await context.SaveChangesAsync(cancellationToken);

			return true;
		}
	}
}
