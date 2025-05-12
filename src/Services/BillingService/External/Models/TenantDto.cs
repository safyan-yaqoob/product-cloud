using System;

namespace BillingService.External.Models
{
  public record TenantDto
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SubDomain { get; set; }
    public string ContactEmail { get; set; }
    public int PlantType { get; set; }
  }
}
