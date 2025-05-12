namespace TenantService.Features.GetAllTenants
{
	public record GetAllTenantResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Orgnization { get; set; }
		public string ContactEmail { get; set; }
		public int PlanType { get; set; }
		public string Industry { get; set; }
		public string TimeZone { get; set; }
		public string Logo { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
