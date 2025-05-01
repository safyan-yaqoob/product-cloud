using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.CQRS;
using TenantService.Database;
using TenantService.Entities;

namespace TenantService.Features.GetTenantById
{
  public class GetTenantByCommandIdHandler(TenantDbContext context) : ICommandHandler<GetTenantByIdCommand, GetTenantByIdResponse>
  {
    public async Task<GetTenantByIdResponse> Handle(GetTenantByIdCommand command, CancellationToken cancellationToken = default)
    {
      var tenant = await context.Set<Tenant>().FirstOrDefaultAsync(e => e.Id == command.TenantId);

      if (tenant == null)
      {
        throw new AppException(AppError.Validation(@$"Cannot found tenant with specified id {command.TenantId}"));
      }

      return new GetTenantByIdResponse
      {
        Id = tenant.Id,
        Name = tenant.Name,
        ContactEmail = tenant.ContactEmail,
        CreatedAt = tenant.CreatedAt,
        PlanType = tenant.PlanType,
        SubDomain = tenant.Subdomain
      };
    }
  }
}
