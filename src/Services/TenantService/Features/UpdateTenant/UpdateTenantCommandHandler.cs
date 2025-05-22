using Microsoft.EntityFrameworkCore;
using ProductCloud.SharedKernal.Common;
using ProductCloud.SharedKernal.CQRS;
using TenantService.Database;
using TenantService.Database.Entities;

namespace TenantService.Features.UpdateTenant;

public class UpdateTenantCommandHandler(TenantDbContext dbContext) : ICommandHandler<UpdateTenantCommand, Guid>
{
    public async Task<Guid> Handle(UpdateTenantCommand command, CancellationToken cancellationToken = default)
    {
        var tenant = await dbContext.Set<Tenant>().FirstOrDefaultAsync(e=>e.Id == command.Id, cancellationToken);
        if (tenant == null)
            throw new AppException(AppError.NotFound("Tenant not found."));

        tenant.Update(command.Name, tenant.ContactEmail, tenant.PlanType);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return tenant.Id;
    }
}