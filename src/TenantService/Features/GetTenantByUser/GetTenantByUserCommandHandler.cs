using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.CQRS;
using TenantService.Database;
using TenantService.Entities;
using TenantService.Features.GetTenantById;

namespace TenantService.Features.GetTenantByUser
{
	public sealed class GetTenantByUserCommandHandler(TenantDbContext context) : ICommandHandler<GetTenantByUserCommand, GetTenantByIdResponse>
	{
		public async Task<GetTenantByIdResponse> Handle(GetTenantByUserCommand command, CancellationToken cancellationToken = default)
		{
			var tenant = await context.Set<Tenant>().FirstOrDefaultAsync(e => e.UserId == command.UserId, cancellationToken: cancellationToken);

			if (tenant == null)
			{
				throw new AppException(AppError.Validation(@$"Cannot found tenant with specified id {command.UserId}"));
			}

			return new GetTenantByIdResponse
			{
				Id = tenant.Id,
				Name = tenant.Name,
				ContactEmail = tenant.ContactEmail,
				CreatedAt = tenant.CreatedAt,
				PlanType = tenant.PlanType,
				Orgnization = tenant.Orgnization,
				Logo = tenant.Logo,
				Industry = tenant.Industry,
			};
		}
	}
}
