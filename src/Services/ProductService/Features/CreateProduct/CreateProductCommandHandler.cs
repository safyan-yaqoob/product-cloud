using ProductService.Database;
using ProductCloud.SharedKernal.Common;
using ProductCloud.SharedKernal.CQRS;
using ProductService.Database.Entities;

namespace ProductService.Features.CreateProduct
{
    public class CreateProductCommandHandler(ProductDbContext context) : ICommandHandler<CreateProductCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                throw new AppException(AppError.Validation("Product name is required."));

            var product = new Product
            {
                Id = Guid.CreateVersion7(),
                Name = command.Name,
                Description = command.Description
            };

            context.Set<Product>().Add(product);
            await context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
