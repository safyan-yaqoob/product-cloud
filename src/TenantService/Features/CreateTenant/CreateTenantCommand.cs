using Shared.CQRS;

namespace TenantService.Features.CreateTenant
{
  public record CreateTenantCommand: ICommand<Guid>
  {
    public string Name { get; set; }
    public string SubDomain { get; set; }
    public string ContactEmail { get; set; }
    public int PlanType { get; set; }
  }
}
