namespace ProductService.Database.Entities
{
	public class Product
	{
		public List<Plan> _plans { get; set; } = [];
		public Guid Id { get; set; }
		public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;
		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public string Logo { get; set; }
		public string UrlSlug { get; set; }
		public IReadOnlyList<Plan> Plans => _plans.AsReadOnly();
		public Guid TenantId { get; set; }
	}
}
