namespace ProductService.Entities
{
	public class Product
	{
		public Guid Id { get; set; } = Guid.CreateVersion7();
		public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;
		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public string Logo { get; set; }
		public string UrlSlug { get; set; }
		public ICollection<Plan> Plans { get; set; }
	}
}
