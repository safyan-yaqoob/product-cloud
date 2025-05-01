using Shared.CQRS;

namespace ProductService.Features.GetPlans
{
  public record GetPlansQuery : ICommand<IEnumerable<GetPlansResponse>>;
}
