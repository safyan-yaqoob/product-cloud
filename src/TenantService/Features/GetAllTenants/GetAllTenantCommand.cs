using SharedKernal.CQRS;

namespace TenantService.Features.GetAllTenants
{
	public record GetAllTenantCommand : ICommand<IEnumerable<GetAllTenantResponse>>
	{
		public string SearchText { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}
}
