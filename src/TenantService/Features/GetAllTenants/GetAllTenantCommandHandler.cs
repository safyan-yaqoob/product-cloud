using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using TenantService.Database;
using TenantService.Entities;

namespace TenantService.Features.GetAllTenants
{
	public class GetAllTenantCommandHandler(TenantDbContext context) : ICommandHandler<GetAllTenantCommand, IEnumerable<GetAllTenantResponse>>
	{
		public async Task<IEnumerable<GetAllTenantResponse>> Handle(GetAllTenantCommand command, CancellationToken cancellationToken = default)
		{
			int skip = (command.PageNumber - 1) * command.PageSize;

			var tenants = await context.Set<Tenant>()
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

			return tenants;
		}
	}
}
