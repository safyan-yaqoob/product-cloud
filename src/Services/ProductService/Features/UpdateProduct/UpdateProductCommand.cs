using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.UpdateProduct;

public class UpdateProductCommand : ICommand<Guid>
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ProductLogo { get; set; }
}