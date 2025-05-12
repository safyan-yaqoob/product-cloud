using ProductCloud.SharedKernal.CQRS;
using TenantService.Features.GetTenantById;

namespace TenantService.Features.GetTenantByUser
{
	public record GetTenantByUserCommand(Guid UserId): ICommand<GetTenantByIdResponse>;
}
