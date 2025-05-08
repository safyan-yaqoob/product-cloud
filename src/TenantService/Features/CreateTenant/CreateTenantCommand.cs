using SharedKernal.CQRS;

namespace TenantService.Features.CreateTenant
{
	public record CreateTenantCommand : ICommand<Guid>
	{
		public string Name { get; set; }
		public string Orgnization { get; set; }
		public string Email { get; set; }
		public int PlanType { get; set; }
		public string Industry { get; set; }
		public string TimeZone { get; set; }
		public string Logo { get; set; }
		public Guid UserId { get; set; }
	}
}
