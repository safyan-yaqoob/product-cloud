using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Features.CreateProduct
{
  public record CreateProductCommand : ICommand<Guid>
  {
    public Guid TenantId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ProductLogo { get; set; }
  }
}
