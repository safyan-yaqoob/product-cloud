using Microsoft.EntityFrameworkCore;
using ProductCloud.SharedKernal.Caching;
using ProductCloud.SharedKernal.Common;
using ProductCloud.SharedKernal.CQRS;
using TenantService.Database;
using TenantService.Database.Entities;

namespace TenantService.Features.GetTenantById
{
	public class GetTenantByCommandIdHandler(TenantDbContext context, IAppCache appCache) : ICommandHandler<GetTenantByIdCommand, GetTenantByIdResponse>
	{
		public async Task<GetTenantByIdResponse> Handle(GetTenantByIdCommand command, CancellationToken cancellationToken = default)
		{
			var cacheKey = $"GetTenantById-{command.TenantId}";
			var cachedTenant = await appCache.GetAsync<GetTenantByIdResponse>(cacheKey);
			if (cachedTenant == null)
			{

				cachedTenant = await context.Set<Tenant>()
					.Where(t => t.Id == command.TenantId)
					.Select(t => new GetTenantByIdResponse
					{
						Id = t.Id,
						Name = t.Name,
						Orgnization = t.Orgnization,
						ContactEmail = t.ContactEmail,
						PlanType = t.PlanType,
						Industry = t.Industry,
						TimeZone = t.TimeZone,
						Logo = t.Logo,
						CreatedAt = t.CreatedAt
					}).FirstOrDefaultAsync(cancellationToken);
				
				if (cachedTenant == null)
				{
					throw new AppException(AppError.Validation(@$"Cannot found tenant with specified id {command.TenantId}"));
				}

				await appCache.SetAsync(cacheKey, cachedTenant, TimeSpan.FromMinutes(5));
				return cachedTenant;

			}
			return cachedTenant;
		}
	}
}
