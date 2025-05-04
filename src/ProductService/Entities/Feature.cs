namespace ProductService.Entities
{
	public class Feature
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;
		public Guid PlanId { get; set; }
		public Plan Plan { get; set; } = default!;
	}
}
