namespace TenantService.Features.GetTenantById
{
  public record GetTenantByIdResponse
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SubDomain { get; set; }
    public string ContactEmail { get; set; }
    public int PlanType { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}
