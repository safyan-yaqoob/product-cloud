namespace ProductService.Features.GetPlans
{
	public record GetPlansResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Currency { get; set; }
		public Guid ProductId { get; set; }
		public int BillingCycle { get; set; }
		public string[] Features { get; set; }
	}
}
