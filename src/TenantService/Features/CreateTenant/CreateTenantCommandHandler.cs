using Shared.CQRS;
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

      if (string.IsNullOrWhiteSpace(command.SubDomain))
        throw new ArgumentException("Subdomain must not be empty.", nameof(command.SubDomain));

      if (!IsValidEmail(command.ContactEmail))
        throw new ArgumentException("Invalid contact email format.", nameof(command.ContactEmail));

      if (command.PlanType < 0)
        throw new ArgumentException("Invalid plan type.", nameof(command.PlanType));

      var tenant = Tenant.Create(command.Name,
        command.SubDomain,
        command.ContactEmail,
        command.PlanType);

      await context.AddAsync(tenant, cancellationToken);

      return tenant.Id;
    }

    private bool IsValidEmail(string email)
    {
      return !string.IsNullOrWhiteSpace(email)
          && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
  }
}
