using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using ProductService.Entities;
using ProductCloud.SharedKernal.Common;
using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.UpdateProduct;

public class UpdateProductCommandHandler(ProductDbContext dbContext) : ICommandHandler<UpdateProductCommand>
{
    public async Task Handle(UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = await dbContext.Set<Product>().FirstOrDefaultAsync(e=>e.Id == command.Id, cancellationToken);
        if (product == null)
            throw new AppException(AppError.NotFound("Product not found"));
        
        product.Name = command.Name;
        product.Description = command.Description;
        product.IsActive = true;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}