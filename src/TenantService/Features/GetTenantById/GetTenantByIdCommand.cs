using Shared.CQRS;

namespace TenantService.Features.GetTenantById
{
	public record GetTenantByIdCommand : ICommand<GetTenantByIdResponse>
	{
		public Guid TenantId { get; set; }
	}
}
