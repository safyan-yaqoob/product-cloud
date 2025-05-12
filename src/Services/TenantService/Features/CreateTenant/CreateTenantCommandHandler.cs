using ProductCloud.SharedKernal.CQRS;
using System.Text.RegularExpressions;
using TenantService.Database;
using TenantService.Entities;

namespace TenantService.Features.CreateTenant
{
	public class CreateTenantCommandHandler(TenantDbContext context) : ICommandHandler<CreateTenantCommand, Guid>
	{
		public async Task<Guid> Handle(CreateTenantCommand command, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(command.Name))
				throw new ArgumentException("Tenant name must not be empty.", nameof(command.Name));

			if (string.IsNullOrWhiteSpace(command.Orgnization))
				throw new ArgumentException("Orgnization must not be empty.", nameof(command.Orgnization));

			if (!IsValidEmail(command.Email))
				throw new ArgumentException("Invalid contact email format.", nameof(command.Email));

			if (command.PlanType < 0)
				throw new ArgumentException("Invalid plan type.", nameof(command.PlanType));

			var tenant = Tenant.Create(command.Name,
			  command.Orgnization,
			  command.Email,
			  command.PlanType,
			  command.Industry,
			  command.Logo,
			  command.TimeZone,
			  command.UserId);

			await context.Set<Tenant>().AddAsync(tenant, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);

			return tenant.Id;
		}

		private bool IsValidEmail(string email)
		{
			return !string.IsNullOrWhiteSpace(email)
				&& Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
		}
	}
}
