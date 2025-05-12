using ProductService.Features.GetPlans;

namespace ProductService.Features.GetProductById
{
	public record GetProductByIdResponse
	{
		public Guid Id { get; init; }
		public string Name { get; init; }
		public string Description { get; init; }
		public string Logo { get; init; }
		public string UrlSlug { get; set; }
		public IEnumerable<GetPlansResponse> Plans { get; set; }
	}
}
