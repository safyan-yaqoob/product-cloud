using ProductService.Common;

namespace ProductService.Entities
{
	public class Plan
	{
		public Guid Id { get; set; } = Guid.CreateVersion7();
		public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;
		public decimal MonthlyPrice { get; set; }
		public decimal AnnaulPrice { get; set; }
		public string Currency { get; set; } = "USD";
		public BillingCycle BillingCycle { get; set; }
		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public Guid ProductId { get; set; }
		public Product Product { get; set; } = default!;
		public ICollection<Feature> Features { get; set; } = new List<Feature>();
	}

}
