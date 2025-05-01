using Shared.CQRS;

namespace ProductService.Features.CreateProduct
{
  public record CreateProductCommand(string Name, string Description) : ICommand<Guid>;

}
