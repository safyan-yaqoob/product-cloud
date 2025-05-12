namespace ProductService.Features.GetProducts
{
	public record GetProductsResponse
	{
		public Guid Id { get; init; }
		public string Name { get; init; }
		public string Description { get; init; }
		public string Logo { get; init; }
		public string UrlSlug { get; set; }
	}
}
