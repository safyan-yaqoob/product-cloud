using TenantService.Common;

namespace TenantService.Entities
{
  public sealed class Tenant
  {
    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public string Name { get; private set; }

    public string Subdomain { get; private set; }

    public string ContactEmail { get; private set; }

    public int PlanType { get; private set; }

    public TenantStatus Status { get; private set; } = TenantStatus.Active;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; private set; }
    public DateTime? PlanExpiresAt { get; private set; }
    
    public static Tenant Create(string name, string subdomain, string contactEmail, int plan)
    {
      return new Tenant
      {
        Name = name,
        Subdomain = subdomain,
        ContactEmail = contactEmail,
        PlanType = plan,
      };
    }

    public void Update(string name, string contactEmail, int plan)
    {
      Name = name;
      ContactEmail = contactEmail;
      PlanType = plan;
      UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(TenantStatus status)
    {
      Status = status;
      UpdatedAt = DateTime.UtcNow;
    }
  }
}
