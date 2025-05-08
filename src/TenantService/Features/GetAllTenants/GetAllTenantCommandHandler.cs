using Microsoft.EntityFrameworkCore;
using SharedKernal.Caching;
using SharedKernal.CQRS;
using TenantService.Database;
using TenantService.Entities;

namespace TenantService.Features.GetAllTenants
{
	public class GetAllTenantCommandHandler(TenantDbContext context,
		IAppCache appCache) : ICommandHandler<GetAllTenantCommand, IEnumerable<GetAllTenantResponse>>
	{
		public async Task<IEnumerable<GetAllTenantResponse>> Handle(GetAllTenantCommand command, CancellationToken cancellationToken = default)
		{
			int skip = (command.PageNumber - 1) * command.PageSize;

			var cacheKey = $"GetAllTenants-{command.PageNumber}-{command.PageSize}-{command.SearchText}";
			var cachedTenants = await appCache.GetAsync<IEnumerable<GetAllTenantResponse>>(cacheKey);

			if (cachedTenants == null)
			{
				cachedTenants = await context.Set<Tenant>()
					.Where(t => string.IsNullOrEmpty(command.SearchText) || t.Name.Contains(command.SearchText))
					.OrderByDescending(t => t.CreatedAt)
					.Skip(skip)
					.Take(command.PageSize)
					.Select(t => new GetAllTenantResponse
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
					}).ToListAsync(cancellationToken);

				await appCache.SetAsync(cacheKey, cachedTenants, TimeSpan.FromMinutes(5));
				return cachedTenants;
			}

			return cachedTenants;
		}
	}
}
