using TenantService.Common;

namespace TenantService.Database.Entities
{
	public sealed class Tenant
	{
		public Guid Id { get; private set; } = Guid.CreateVersion7();
		public string Name { get; private set; }
		public string Orgnization { get; private set; }
		public string Logo { get; set; } = default!;
		public string TimeZone { get; set; }
		public string Industry { get; set; }
		public string ContactEmail { get; private set; }
		public int PlanType { get; private set; }
		public TenantStatus Status { get; private set; } = TenantStatus.Active;
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; private set; }
		public DateTime? PlanExpiresAt { get; private set; }
		public Guid UserId { get; set; }

		public static Tenant Create(string name, string orgnization, string contactEmail, int plan, string industry, string logo, string timeZone, Guid userId)
		{
			return new Tenant
			{
				Name = name,
				Orgnization = orgnization,
				ContactEmail = contactEmail,
				PlanType = plan,
				Status = TenantStatus.Active,
				Industry = industry,
				Logo = logo,
				TimeZone = timeZone,
				UserId = userId,
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
