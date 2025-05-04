using Shared.CQRS;

namespace ProductService.Features.CreateProduct
{
  public record CreateProductCommand(string Name, string Description, string UrlSlug, string productLogo) : ICommand<Guid>;
}
