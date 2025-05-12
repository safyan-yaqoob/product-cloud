using ProductCloud.SharedKernal.CQRS;

namespace TenantService.Features.UpdateTenant;

public record UpdateTenantCommand: ICommand<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Orgnization { get; set; }
    public string Email { get; set; }
    public int PlanType { get; set; }
    public string Industry { get; set; }
    public string TimeZone { get; set; }
    public string Logo { get; set; }
}